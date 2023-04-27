using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using MediatR;
using ParkingLotManagement.Domain.DTOs;
using ParkingLotManagement.Application.Interfaces;
using ParkingLotManagement.Application.Services;
using ParkingLotManagement.Application.DTOs;

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
        public string tagNumber { get; set; }
        [BindProperty]
        public string HourlyFee { get; set; }
        [BindProperty]
        public string CapacitySpots { get; set; }

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

        //Task<ActionResult<
        public async void OnPostCarOutAsync()
        {
            await _parkingServices.Update(parkedDTO);
        }

        public async Task<IActionResult> OnPostCarInAsync()
        {
            parkedDTO.TagNumber = tagNumber;
              await _parkingServices.Add(parkedDTO);
            ParkedCars = await _parkingServices.GetAllAsync();
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
    }
}