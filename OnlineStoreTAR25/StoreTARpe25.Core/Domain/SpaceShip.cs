using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTARpe25.Core.Domain
{
    public class SpaceShip
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Classification { get; set; } = string.Empty;
        public DateTime? BuildDate { get; set; }
        public int? Crew { get; set; }
        public int? EnginePower { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

    }
}
