using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GuestBook.Web.Models;
using GuestBook.Web.Services;

namespace GuestBook.Web.Pages
{
    public class AllEntriesModel : PageModel
    {
        private readonly SqlStorageService _sql;

        public List<GuestBookEntry> Entries { get; set; } = new();

        public AllEntriesModel(SqlStorageService sql)
        {
            _sql = sql;
        }

        public async Task OnGetAsync()
        {
            Entries = await _sql.GetAllEntriesAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _sql.DeleteEntryAsync(id);
            return RedirectToPage();
        }
    }
}