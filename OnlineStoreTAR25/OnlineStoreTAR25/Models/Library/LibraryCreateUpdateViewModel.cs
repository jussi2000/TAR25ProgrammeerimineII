using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ShopTARpe25.Models.Library
{
    public class LibraryCreateUpdateViewModel
    {
        public Guid Id { get; set; }

        //error message on selleks et kui sisestaja ei sisesta vajalikke andmeid, siis projekt kuvab error message'i
        //ja required on selleks, et sisestaja sisestaks kõik vajalikud andmed, et projekt töötaks

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Published year is required")]
        [Range(1, 9999, ErrorMessage = "Enter a valid year")]
        public int? PublishedYear { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public List<IFormFile>? Files { get; set; }
        public List<imageViewModel> Images { get; set; } = new List<imageViewModel>();
    }
}