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
        private InOutParkingDTO parkedDTO { get; set; }

        [BindProperty]
        public IReadOnlyList<ParkedCarDTO> parkedCars { get; set; }
        [BindProperty]
        [Required]
        public string tagNumber { get; set; }
        [BindProperty]
        public decimal hourlyFee { get; set; }
        [BindProperty]
        public int capacitySpots { get; set; }
        [BindProperty]
        public decimal averageRevenuePerDay { get; set; }
        [BindProperty]
        public decimal averageCarsPerDay { get; set; }
        [BindProperty]
        public decimal totalRevenueToday { get; set; }

        [BindProperty]
        public OperationResult ResultOperation { get; set; }=new OperationResult();

        public IndexModel(ILogger<IndexModel> logger, IParkingServices parkingServices, ICustomConfigureServices customConfigure, IStatsService statsServices)
        {
            _statsServices = statsServices;
            _parkingServices = parkingServices;
            _customConfigure = customConfigure;
            _logger = logger;
            parkedDTO = null ?? new InOutParkingDTO();
        }

        public async Task<IActionResult> OnGet()
        {
            parkedCars=await _parkingServices.GetAllAsync();
            hourlyFee=_customConfigure.HourlyFee();
            capacitySpots=_customConfigure.CapacitySpots();
            averageRevenuePerDay =  await _statsServices.AverageRevenuePerDay();
            averageCarsPerDay =     await _statsServices.AverageCarsPerDay();
            totalRevenueToday =     await _statsServices.TotalRevenueToday();

            return Page();
        }

        public async Task<IActionResult> OnPostCarOutAsync()
        {
            try
            {
                parkedDTO.TagNumber = tagNumber;
                ResultOperation = await _parkingServices.Update(parkedDTO);
                if (ResultOperation.IsValid)
                {
                    tagNumber = "";
                }
            }
            catch (Exception ex)
            {
                ResultOperation.IsValid= false;
                ResultOperation.Message = "An error has occurred";
                _logger.LogError(ex, ex.Message);
            }
            await OnGet();
            return Page();
        }

        public async Task<IActionResult> OnPostCarInAsync()
        {
            try
            {
                parkedDTO.TagNumber = tagNumber;
                ResultOperation = await _parkingServices.Add(parkedDTO);
                if (ResultOperation.IsValid)
                {
                    tagNumber = "";
                }
            }
            catch (Exception ex)
            {
                ResultOperation.IsValid = false;
                ResultOperation.Message = "An error has occurred";
                _logger.LogError(ex, ex.Message);
            }
            await OnGet();
            return Page();
        }

       
        public double ElapsedTime(DateTime dateTime)
        {
            TimeSpan totalHoursParked = DateTime.Now - dateTime;
            return Math.Ceiling(totalHoursParked.TotalMinutes);
        }
    }
}