using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class Employee
    {
        public long Id { get; set; }
        public long? UserId { get; set; }

        public virtual Users User { get; set; }
    }
}
