using DistanceService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace UnitTests
{
    public class ControllerTests
    {
        [Fact]
        public void RequestCounter()
        {
            var controller = new DistanceController();
            const int requestCount = 20;
            for (var i = 0; i < requestCount; i++)
            {
                controller.CalculateDistance("0, 0", "10, 10");
            }

            var result = (OkObjectResult)controller.RequestCount().Result;
            Assert.Equal(requestCount, result.Value);
        }
    }
}