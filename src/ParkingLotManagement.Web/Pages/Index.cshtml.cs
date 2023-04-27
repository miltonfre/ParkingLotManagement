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

        //[BindProperty]
        //public Parking Parking { get; set; }
        private readonly IParkingServices _parkingServices;
        [BindProperty]
        public IReadOnlyList<ParkedCarDTO> ParkedCars { get; set; }
        [BindProperty]
        public InOutParkingDTO parkedDTO { get; set; }
        public IndexModel(ILogger<IndexModel> logger, IParkingServices parkingServices)
        {
            _parkingServices = parkingServices;
               _logger = logger;
            parkedDTO = null ?? new InOutParkingDTO();
        }

        public async void OnGet()
        {
            ParkedCars=await _parkingServices.GetAllAsync();
        }

        //Task<ActionResult<
        public async void OnPostCarOutAsync()
        {
            await _parkingServices.Update(parkedDTO);
        }

        public async void OnPostCarInAsync()
        {
          
            await _parkingServices.Add(parkedDTO);
            ParkedCars = await _parkingServices.GetAllAsync();
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