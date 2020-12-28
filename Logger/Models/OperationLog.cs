using System;
using System.Collections.Generic;

#nullable disable

namespace Logger.Models
{
    public partial class OperationLog
    {
        public long Id { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public string MethodName { get; set; }
        public string ServiceName { get; set; }
        public string Parameters { get; set; }
        public string Answer { get; set; }
        public string ExecuteTime { get; set; }
    }
}
