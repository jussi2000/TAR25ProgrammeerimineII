namespace ShopTARpe25.Models.Library
{
    public class LibraryIndexViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublishedYear { get; set; }
        public bool IsAvailable { get; set; }
        public string? BorrowedBy { get; set; }
    }
}