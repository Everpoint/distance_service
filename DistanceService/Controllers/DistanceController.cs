using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace DistanceService.Controllers
{
    
    [Route("distance")]
    public class DistanceController : ControllerBase
    {
        private int _requestCounter;
        
        /// <summary>
        /// Calculates geodesic distance between two points on WGS84 datum.
        /// </summary>
        /// <example>
        ///     https://localhost:5001/distance?point1=0,0&point2=1,1
        /// </example>
        /// <param name="point1">First point in longitude,latitude format.</param>
        /// <param name="point2">Second point in longitude,latitude format.</param>
        /// <returns>Distance between two points in meters.</returns>
        [HttpGet]
        public ActionResult<double> CalculateDistance(string point1, string point2)
        {
            _requestCounter++;
            
            var parsed1 = ParsePoint(point1);
            var parsed2 = ParsePoint(point2);
            
            return Ok(Algorithm.CalculateDistance(parsed1, parsed2));
        }

        /// <summary>
        /// Returns the number of times the distance calculation was called since the server was started.
        /// </summary>
        /// <returns>Request count.</returns>
        [HttpGet("request_count")]
        public ActionResult<int> RequestCount()
        {
            return Ok(_requestCounter);
        }

        private static double[] ParsePoint(string str)
        {
            return str.Split(",").Select(double.Parse).ToArray();
        }
    }
}