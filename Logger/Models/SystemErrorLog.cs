using System;
using System.Collections.Generic;

#nullable disable

namespace Logger.Models
{
    public partial class SystemErrorLog
    {
        public long Id { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public string ExceptionStr { get; set; }
        public string InnerClassName { get; set; }
        public string InnerMethodName { get; set; }
        public string InnerParameters { get; set; }
        public string ServiceName { get; set; }
        public string ServiceMethodName { get; set; }
        public string ServiceParameters { get; set; }
        public int? InnerLineNumber { get; set; }
    }
}
