using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
   public class SellerRegisterDto
    {
        [Required]
        public string Hpassword { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public long? Mobile { get; set; }

    }
}
