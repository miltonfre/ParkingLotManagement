using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using MediatR;
using ParkingLotManagement.Domain.DTOs;
using ParkingLotManagement.Application.Interfaces;
using ParkingLotManagement.Application.Services;
using ParkingLotManagement.Application.DTOs;
using ParkingLotManagement.Application.Validators;
using ParkingLotManagement.Core.Entities;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace ParkingLotManagement.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly IParkingServices _parkingServices;
        private readonly ICustomConfigureServices _customConfigure;
        private InOutParkingDTO parkedDTO { get; set; }

        [BindProperty]
        public IReadOnlyList<ParkedCarDTO> ParkedCars { get; set; }
        [BindProperty]
        [Required]
        public string tagNumber { get; set; }
        [BindProperty]
        public string? HourlyFee { get; set; }
        [BindProperty]
        public string? CapacitySpots { get; set; }
        [BindProperty]
        public OperationResult ResultOperation { get; set; }=new OperationResult();

        public IndexModel(ILogger<IndexModel> logger, IParkingServices parkingServices, ICustomConfigureServices customConfigure)
        {
            _parkingServices = parkingServices;
            _customConfigure = customConfigure; 
               _logger = logger;
            parkedDTO = null ?? new InOutParkingDTO();
        }

        public async Task<IActionResult> OnGet()
        {
            ParkedCars=await _parkingServices.GetAllAsync();
            HourlyFee=_customConfigure.HourlyFee();
            CapacitySpots=_customConfigure.CapacitySpots();
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
                    await OnGet();
                }
            }
            catch (Exception ex)
            {
                ResultOperation.IsValid= false;
                ResultOperation.Message = "An error has occurred";
                _logger.LogError(ex, ex.Message);
            }
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
                    await OnGet();
                }
            }
            catch (Exception ex)
            {
                ResultOperation.IsValid = false;
                ResultOperation.Message = "An error has occurred";
                _logger.LogError(ex, ex.Message);
            }
          
              
           
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }      
            return RedirectToPage("./Index");
        }

        public double ElapsedTime(DateTime dateTime)
        {
            TimeSpan totalHoursParked = DateTime.Now - dateTime;
            return Math.Ceiling(totalHoursParked.TotalMinutes);
        }
    }
}