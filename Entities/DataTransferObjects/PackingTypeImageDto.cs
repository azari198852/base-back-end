using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
  public class PackingTypeImageDto
    {
        public long Id { get; set; }
        public long? PackingTypeId { get; set; }
        public string Title { get; set; }
        public string Decription { get; set; }
        public string ImageFileUrl { get; set; }
    }
}
