using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
   public class PostTypeDto
    {

        public long Id { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
        public long? Price { get; set; }
        public bool? IsFree { get; set; }
        public string ApiUrl { get; set; }
        public string Icon { get; set; }
        public int? Rkey { get; set; }
    }
}
