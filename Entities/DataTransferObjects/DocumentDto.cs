using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
  public  class DocumentDto
    {
        public long Id { get; set; }
        public long? CatDocumentId { get; set; }
        public string CatDocumentName { get; set; }
        public string Title { get; set; }
        public bool? IsRequired { get; set; }
    }
}
