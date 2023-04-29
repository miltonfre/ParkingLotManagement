using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using MediatR;
using ParkingLotManagement.Domain.DTOs;
using ParkingLotManagement.Application.Interfaces;
using ParkingLotManagement.Application.DTOs;
using ParkingLotManagement.Application.Validators;
using System.ComponentModel.DataAnnotations;

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
        public async Task<ActionResult> OnPostCarOutAsync(string tagNumber)
        {
            try
            {
                var parkedDTO = new InOutParkingDTO() { TagNumber = tagNumber };
                var operationResult = await _parkingServices.Update(parkedDTO);
                if (operationResult.IsValid)
                {
                    return new EmptyResult();
                }
                return BadRequest("An error has occurred");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("An error has occurred");
            }
        }

        public async Task<ActionResult> OnPostCarInAsync(string tagNumber)
        {
            try
            {
                var parkedDTO = new InOutParkingDTO() { TagNumber = tagNumber };
                var operationResult = await _parkingServices.Add(parkedDTO);
                if (operationResult.IsValid)
                {
                    return new EmptyResult();
                }
                return BadRequest("An error has occurred");
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

        #region ExtendedMethods
        public double ElapsedTime(DateTime dateTime)
        {
            TimeSpan totalHoursParked = DateTime.Now - dateTime;
            return Math.Ceiling(totalHoursParked.TotalMinutes);
        }
        #endregion
    }
}