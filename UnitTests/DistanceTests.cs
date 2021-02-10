using System;
using System.Security.Cryptography;
using DistanceService;
using Xunit;

namespace UnitTests
{
    public class DistanceTests
    {
        [Theory]
        [InlineData(new[] {5.714722222222222, 50.06638888888889}, new[] {3.0700000000000003, 58.64388888888889}, 969932.9876)]
        [InlineData(new[] {0.0, 0.0}, new[] {179.5, 0.5}, 19936288.579)]
        public void DistanceIsCalculatedCorrectly(double[] p1, double[] p2, double expected)
        {
            var d = Algorithm.CalculateDistance(p1, p2);
            Assert.InRange(d, expected - 1, expected + 1);
        }
    }
}