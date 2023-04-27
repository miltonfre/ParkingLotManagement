using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ParkingLotManagement.Web.Pages
{
    public class StatsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public StatsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}