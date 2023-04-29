using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


using ParkingLotManagement.Application.Interfaces;
using ParkingLotManagement.Application.DTOs;
namespace ParkingLotManagement.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IParkingServices _parkingServices;
        private readonly IStatsService _statsServices;
        private readonly ICustomConfigureServices _customConfigure;



        public IndexModel(ILogger<IndexModel> logger, IParkingServices parkingServices, ICustomConfigureServices customConfigure, IStatsService statsServices)
        {
            _statsServices = statsServices;
            _parkingServices = parkingServices;
            _customConfigure = customConfigure;
            _logger = logger;
        }

        #region PostMethods
        public async Task<ActionResult> OnGetCarInAsync(string tagNumber, InOutParkingDTO tdo)
        {
            try
            {
                if (tagNumber == null )
                {
                    return BadRequest("please fill out the field");
                }
                var parkedDTO = new InOutParkingDTO() { TagNumber = tagNumber };
                var operationResult = await _parkingServices.Add(parkedDTO);
                if (operationResult.IsValid)
                {
                    return new JsonResult(operationResult.Message);
                }
                return BadRequest(operationResult.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("An error has occurred");
            }
        }
        public async Task<ActionResult> OnGetCarOutAsync(string tagNumber)
        {
            try
            {
                if (tagNumber == null)
                {
                    return BadRequest("please fill out the field");
                }
                var parkedDTO = new InOutParkingDTO() { TagNumber = tagNumber };
                var operationResult = await _parkingServices.Update(parkedDTO);
                if (operationResult.IsValid)
                {
                    return new JsonResult(operationResult.Message);
                }
                return BadRequest(operationResult.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("An error has occurred");
            }
        }
        # endregion PostMethods

        #region GetMethods
        public async Task<IActionResult> OnGet()
        {
            return Page();
        }

       
        public async Task<IActionResult> OnGetCurrentParkedCarsAsync()
        {
            try
            {
                var parkedCars = await _parkingServices.GetAllAsync();
                return new JsonResult(new { data = parkedCars });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("An error has occurred");
            }
        }
        public IActionResult OnGetHourlyFeeAsync()
        {
            try
            {
                var hourlyFee = _customConfigure.HourlyFee();
                return new JsonResult(hourlyFee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("An error has occurred");
            }
        }
        public IActionResult OnGetCapacitySpotsAsync()
        {
            try
            {
                var capacitySpots = _customConfigure.CapacitySpots(); ;
                return new JsonResult(capacitySpots);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("An error has occurred");
            }
        }
        public async Task<IActionResult> OnGetTotalRevenueTodayAsync()
        {
            try
            {
                var totalRevenueToday = await _statsServices.TotalRevenueToday();
                return new JsonResult(totalRevenueToday);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("An error has occurred");
            }
        }
        public async Task<IActionResult> OnGetAverageCarsPerDayAsync()
        {
            try
            {
                var averageCarsPerDay = await _statsServices.AverageCarsPerDay();
                return new JsonResult(averageCarsPerDay);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("An error has occurred");
            }
        }
        public async Task<IActionResult> OnGetAverageRevenuePerDayAsync()
        {
            try
            {
                var averageRevenuePerDay = await _statsServices.AverageRevenuePerDay();
                return new JsonResult(averageRevenuePerDay);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("An error has occurred");
            }
        }

        public async Task<IActionResult> OnGetAvailableSpotsAsync()
        {
            try
            {
                var parkedCars = await _parkingServices.GetAllAsync();
                var capacitySpots = _customConfigure.CapacitySpots();
                return new JsonResult(capacitySpots- parkedCars.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("An error has occurred");
            }
        }
        #endregion GetMethods

    }
}