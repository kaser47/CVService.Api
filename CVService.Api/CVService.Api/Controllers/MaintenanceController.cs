#if DEBUG

using System.Threading.Tasks;
using CVService.Api.CommonLayer;
using CVService.Api.DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CVService.Api.Controllers
{
    [Route("Maintenance")]
    public class MaintenanceController : Controller
    {
        private readonly ApiContext _context;

        public MaintenanceController(ApiContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Wipes all the data from database (Used for testing)
        /// </summary>
        [Route("WipeData")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> WipeData()
        {
            _context.Database.EnsureDeleted();
            return Ok();
        }

        /// <summary>
        /// Creates and seeds data into the database (Used for testing)
        /// </summary>
        [Route("CreateData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> CreateData()
        {
            _context.Database.EnsureCreated();
            SeedData.AddTestData(_context);
            return Ok();
        }

    }
}

#endif