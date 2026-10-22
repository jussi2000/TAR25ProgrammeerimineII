namespace ShopTARpe25.Models.Library
{
    public class LibraryDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublishedYear { get; set; }
        public bool IsAvailable { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public List<imageViewModel> Images { get; set; } = new List<imageViewModel>();
    }
}