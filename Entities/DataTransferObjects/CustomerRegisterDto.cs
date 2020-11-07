using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
   public class CustomerRegisterDto
    {

        [Required]
        public string Hpassword { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public long? Mobile { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
