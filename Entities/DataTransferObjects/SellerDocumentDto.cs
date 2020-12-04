using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class SellerDocumentDto
    {
        public long Id { get; set; }
        public long? DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string FileUrl { get; set; }
        public string FianlStatus { get; set; }
        public string AdminDescription { get; set; }
    }
}
