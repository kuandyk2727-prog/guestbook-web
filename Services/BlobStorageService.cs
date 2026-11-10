namespace GuestBook.Web.Services
{
    public class BlobStorageService
    {
        private readonly string _folder;
        private const string UrlPrefix = "/guestbookpics/";

        public BlobStorageService(IWebHostEnvironment env)
        {
            _folder = Path.Combine(env.WebRootPath, "guestbookpics");
            Directory.CreateDirectory(_folder);
        }

        public async Task<string> UploadPhotoAsync(IFormFile file)
        {
            string blobName = $"photo_{Guid.NewGuid()}.jpg";
            var path = Path.Combine(_folder, blobName);
            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            return UrlPrefix + blobName;
        }

        public List<string> GetAllBlobUrls()
        {
            return Directory.GetFiles(_folder)
                .Select(f => UrlPrefix + Path.GetFileName(f))
                .ToList();
        }

        public Task DeleteBlobAsync(string blobUrl)
        {
            var blobName = Path.GetFileName(blobUrl);
            var path = Path.Combine(_folder, blobName);
            if (File.Exists(path)) File.Delete(path);
            return Task.CompletedTask;
        }
    }
}