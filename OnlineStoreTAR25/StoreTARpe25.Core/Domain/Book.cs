using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.Core.Domain
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublishedYear { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; } //lisasin need igaksjuhuks
        public DateTime ModifiedAt { get; set; } //lisasin need igaksjuhuks
        public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}
