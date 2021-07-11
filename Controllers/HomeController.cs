using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using app.Models;
using app.Services;

namespace app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index([FromServices] InfluxDBService service)
        {
            var results = await service.QueryAsync(async query =>
            {
                var flux = "from(bucket:\"test-bucket\") " +
                           "|> range(start: 0) " +
                           "|> filter(fn: (r) => " +
                           "r._measurement == \"altitude\" and " +
                           "r._value > 3500)";
                           var tables = await query.QueryAsync(flux, "organization");
                return tables.SelectMany(table =>
                    table.Records.Select(record =>
                        new AltitudeModel
                        {
                            Time = record.GetTime().ToString(),
                            Altitude = int.Parse(record.GetValue().ToString())
                        }));
            });

            return View(results);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
