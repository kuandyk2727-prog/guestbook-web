namespace GuestBook.Web.Services
{
    public class PhotoStorageService
    {
        private readonly string _folder;
        private const string FolderName = "uploads";

        public PhotoStorageService(IWebHostEnvironment env)
        {
            _folder = Path.Combine(env.WebRootPath, FolderName);
            Directory.CreateDirectory(_folder);
        }

        public async Task<string> SaveAsync(IFormFile file)
        {
            string name = $"photo_{Guid.NewGuid()}.jpg";
            var path = Path.Combine(_folder, name);
            using var fs = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fs);
            return $"/{FolderName}/{name}";
        }

        public List<string> GetAllUrls()
        {
            return Directory.GetFiles(_folder)
                .Select(f => $"/{FolderName}/{Path.GetFileName(f)}")
                .ToList();
        }

        public void Delete(string url)
        {
            var name = Path.GetFileName(Uri.UnescapeDataString(url));
            var path = Path.Combine(_folder, name);
            if (File.Exists(path)) File.Delete(path);
        }
    }
}