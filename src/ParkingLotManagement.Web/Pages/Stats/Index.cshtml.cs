using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ParkingLotManagement.Web.Pages
{
    public class StatsIndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public StatsIndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}