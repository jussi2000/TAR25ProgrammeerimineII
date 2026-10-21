using System;
using System.Collections.Generic;
using System.Text;


namespace ShopTARpe25.Core.Domain
{
    public class FileToApi
    {
        public Guid Id { get; set; }
        public string ExistingFilePath { get; set; } = string.Empty;
        public Guid? BookId { get; set; }
    }
}
