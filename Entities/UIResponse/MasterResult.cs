
using System;
using System.Collections.Generic;

namespace Entities.UIResponse
{
    public class MasterResult<T>
    {
        public int ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public long TotalCount { get; set; }
        public List<T> ObjList { get; set; }
    }
}