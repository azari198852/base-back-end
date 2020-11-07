using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
   public class LoginRegisterDto
    {
        public long UserId { get; set; }
        public bool IsExist { get; set; }
    }
}
