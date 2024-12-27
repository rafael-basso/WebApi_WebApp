using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserAPI.DB;
using UserFRONT.Services;

namespace UserFRONT.Pages
{
    public class IndexModel : PageModel
    {
        private readonly GetUsersService _service;
        public List<User> Users { get; set; } = [];

        public IndexModel(GetUsersService service)
        {
            _service = service;
        }

        public async Task OnGetAsync()
        {
            Users = await _service.GetUsersAsync();
        }
    }
}
