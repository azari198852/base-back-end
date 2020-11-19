using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
   public class CustomerProfileDto
    {

        public string Name { get; set; }
        public string Fname { get; set; }
        public long? MelliCode { get; set; }
        public long? Mobile { get; set; }
        public DateTime? Bdate { get; set; }
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
        public string ProfileImageHurl { get; set; }
        public long? LocationId { get; set; }
        public long? FinalStatusId { get; set; }
        public bool? SendNews { get; set; }
        public bool? HaveMobileApp { get; set; }
        public string MobileAppVersion { get; set; }
        public long? MobileAppTypeId { get; set; }
        public long? CustomerClubCode { get; set; }
        public long? WalletFinalPrice { get; set; }
        public long? CustomerFinalScore { get; set; }
        public long? WorkId { get; set; }
        public long? PresentationCode { get; set; }
        public string Password { get; set; }

    }
}
