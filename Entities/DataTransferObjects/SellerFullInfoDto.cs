using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Entities.DataTransferObjects
{
   public class SellerFullInfoDto
    {
        public long SellerId { get; set; }
        public long? RealOrLegal { get; set; }
        public string Name { get; set; }
        public string Fname { get; set; }
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
        public DateTime? RegisterDate { get; set; }
        public List<SellerAddressDto> AddressList { get; set; }
        public List<SellerDocumentDto> DocumentList { get; set; }
    }
}
