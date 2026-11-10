namespace GuestBook.Web.Models
{
    public class GuestBookEntry
    {
        public int Id { get; set; }

        public string GuestName { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string PhotoUrl { get; set; } = string.Empty;

        public string ThumbnailUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}