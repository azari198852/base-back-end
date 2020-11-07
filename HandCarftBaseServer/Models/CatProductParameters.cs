using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class CatProductParameters
    {
        public CatProductParameters()
        {
            ProductCatProductParameters = new HashSet<ProductCatProductParameters>();
        }

        public long Id { get; set; }
        public long? ParametersId { get; set; }
        public long? CatProductId { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual CatProduct CatProduct { get; set; }
        public virtual Parameters Parameters { get; set; }
        public virtual ICollection<ProductCatProductParameters> ProductCatProductParameters { get; set; }
    }
}
