using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GuestBook.Web.Services;

namespace GuestBook.Web.Pages
{
    public class GalleryModel : PageModel
    {
        private readonly BlobStorageService _blob;
        public List<string> BlobUrls { get; set; } = new();

        public GalleryModel(BlobStorageService blob)
        {
            _blob = blob;
        }

        public void OnGet()
        {
            BlobUrls = _blob.GetAllBlobUrls();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string url)
        {
            await _blob.DeleteBlobAsync(url);
            return RedirectToPage();
        }
    }
}