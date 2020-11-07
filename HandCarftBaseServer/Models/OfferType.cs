using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class OfferType
    {
        public OfferType()
        {
            Offer = new HashSet<Offer>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long? Rkey { get; set; }
        public bool? PublicOffer { get; set; }
        public bool? CustomerOffer { get; set; }
        public bool? ProductOffer { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual ICollection<Offer> Offer { get; set; }
    }
}
