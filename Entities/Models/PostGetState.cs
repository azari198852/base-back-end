using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class PostGetState
    {
        public long Id { get; set; }
        public int? Code { get; set; }
        public string Name { get; set; }
    }
}
