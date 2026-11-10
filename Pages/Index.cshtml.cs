using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GuestBook.Web.Models;
using GuestBook.Web.Services;

namespace GuestBook.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SqlStorageService _sql;
        private readonly BlobStorageService _photos;

        [BindProperty]
        public string GuestName { get; set; } = string.Empty;

        [BindProperty]
        public string Message { get; set; } = string.Empty;

        [BindProperty]
        public IFormFile? Photo { get; set; }

        public IEnumerable<GuestBookEntry> Entries { get; set; } = new List<GuestBookEntry>();

        public IndexModel(SqlStorageService sql, BlobStorageService photos)
        {
            _sql = sql;
            _photos = photos;
        }

        public async Task OnGetAsync()
        {
            Entries = await _sql.GetTodayEntriesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string photoUrl = "/images/default.png";
            if (Photo != null && Photo.Length > 0)
            {
                photoUrl = await _photos.UploadPhotoAsync(Photo);
            }

            var entry = new GuestBookEntry
            {
                GuestName = GuestName,
                Message = Message,
                PhotoUrl = photoUrl,
                CreatedAt = DateTime.UtcNow
            };

            await _sql.AddEntryAsync(entry);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _sql.DeleteEntryAsync(id);
            return RedirectToPage();
        }
    }
}