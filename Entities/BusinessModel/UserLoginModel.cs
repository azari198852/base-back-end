using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.BusinessModel
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "آدرس ایمیل الزامی می باشد")]
        public string Username { get; set; }
        [Required(ErrorMessage = "رمز عبور الزامی می باشد")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
      
    }
}
