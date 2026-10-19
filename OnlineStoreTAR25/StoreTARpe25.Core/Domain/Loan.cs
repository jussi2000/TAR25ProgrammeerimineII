using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.Core.Domain
{
    public class Loan
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; } = null!;
        public int ReaderId { get; set; }
        public virtual Reader Reader { get; set; } = null!;
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; } 
    }
}
