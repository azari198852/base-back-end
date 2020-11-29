using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class SellerRegisterDto
    {
        public long? RealOrLegal { get; set; }
        public string Name { get; set; }
        public string Fname { get; set; }
        public string PassWord { get; set; }
        public long? MelliCode { get; set; }
        public int? Gender { get; set; }
        public string IdentityNo { get; set; }
        public long? Tel { get; set; }
        public long? Mobile { get; set; }
        public long? SecondMobile { get; set; }
        public string ShabaNo { get; set; }
        public DateTime Bdate { get; set; }
        public string Email { get; set; }
        public bool? HaveMobileApp { get; set; }
        public string MobileAppVersion { get; set; }
        public long? MobileAppTypeId { get; set; }
        public SellerAddressDto Address { get; set; }
    }
}
