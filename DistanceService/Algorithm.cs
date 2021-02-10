using System;

namespace DistanceService
{
    public static class Algorithm
    {
        private const int MaxIterations = 150;
        
        /// <summary>
        /// Calculates distance between two points on elliptical datum.
        /// This method uses Vincenty formula: https://en.wikipedia.org/wiki/Vincenty%27s_formulae
        /// </summary>
        /// <param name="v1">First point.</param>
        /// <param name="v2">Second point.</param>
        /// <returns>The great-circle distance between given points.</returns>
        public static double CalculateDistance(double[] v1, double[] v2)
        {
            var a = 6378137.0;
            var b = 6356752.3142451793;
            var f = 1 / 298.257223563;

            var lat1 = v1[1] * Math.PI / 180;
            var lat2 = v2[1] * Math.PI / 180;

            var u1 = Math.Atan((1 - f) * Math.Tan(lat1));
            var u2 = Math.Atan((1 - f) * Math.Tan(lat2));

            var dLon = (v1[0] - v2[0]) * Math.PI / 180;

            var tolerance = 1 / 10e7;

            var lambda = dLon;

            double cos2Alpha = 0;
            double sinSig = 0;
            double cos2SigM = 0;
            double cosSig = 0;
            double sig = 0;
            for (var i = 0; i < MaxIterations; i++)
            {
                var cosU1 = Math.Cos(u1);
                var cosU2 = Math.Cos(u2);
                var sinU1 = Math.Sin(u1);
                var sinU2 = Math.Sin(u2);
                var cosLambda = Math.Cos(lambda);
                var sinLambda = Math.Sin(lambda);

                sinSig = Math.Sqrt(
                    Math.Pow(cosU2 * sinLambda, 2) + Math.Pow((cosU1 * sinU2) - (sinU1 * cosU2 * cosLambda), 2));

                cosSig = (sinU1 * sinU2) + (cosU1 * cosU2 * cosLambda);
                sig = Math.Atan2(sinSig, cosSig);

                var sinAlpha = (cosU1 * cosU2 * sinLambda) / sinSig;
                cos2Alpha = 1 - (sinAlpha * sinAlpha);

                if (cos2Alpha == 0)
                {
                    cos2SigM = cosSig;
                }
                else
                {
                    cos2SigM = cosSig - ((2 * sinU1 * sinU2) / cos2Alpha);
                }

                var c = f / 16 * cos2Alpha * (4 + (f * (4 - (3 * cos2Alpha))));

                var prevLambda = lambda;
                lambda = dLon + ((1 - c) * f * sinAlpha *
                                 (sig + (c * sinSig * (cos2SigM + (c * cosSig * (-1 + (2 * cos2SigM)))))));
                if (Math.Abs(lambda - prevLambda) < tolerance)
                {
                    break;
                }
            }

            var uSq = cos2Alpha * (((a * a) - (b * b)) / (b * b));
            var aCap = 1 + (uSq / 16384 * (4096 + (uSq * (-768 + (uSq * (320 - (175 * uSq)))))));
            var bCap = uSq / 1024 * (256 + (uSq * (-128 + (uSq * (74 - (47 * uSq))))));

            var dSig = bCap * sinSig * (cos2SigM + (0.25 * bCap * ((cosSig * (-1 + (2 * cos2SigM))) -
                                                                   (bCap / 6 * cos2SigM * (-3 + (4 * sinSig * sinSig)) *
                                                                    (-3 + (4 * cos2SigM))))));
            var s = b * aCap * (sig - dSig);

            return s;
        }
    }
}