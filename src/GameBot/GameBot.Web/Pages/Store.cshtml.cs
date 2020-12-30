using System.Threading.Tasks;
using GameBot.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameBot.Web.Pages
{
    public class StoreModel : PageModel
    {
        private readonly IServiceBusHandler _serviceBus;

        public StoreModel(IServiceBusHandler serviceBus) {
            _serviceBus = serviceBus;
        }

        public void OnGet()
        {
        }

        [BindProperty]
        public Order Order { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Order.User = "Bob"; // Pretend this is from the user's session

            await _serviceBus.SendOrderToQueue(Order);

            return RedirectToPage("./Index");
        }
    }
}
