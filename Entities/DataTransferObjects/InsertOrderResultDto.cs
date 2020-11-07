using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
   public class InsertOrderResultDto
    {
        public long? CustomerOrderId { get; set; }
        public long? OrderNo { get; set; }
        public long? PostPrice { get; set; }
        public bool RedirectToBank { get; set; }
        public string BankUrl { get; set; }


    }
}
