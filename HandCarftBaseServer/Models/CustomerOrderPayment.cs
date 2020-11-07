using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class CustomerOrderPayment
    {
        public long Id { get; set; }
        public long? CustomerOrderId { get; set; }
        public long? PaymentDate { get; set; }
        public long? PaymentPrice { get; set; }
        public string TrackingCode { get; set; }
        public long? TransactionDate { get; set; }
        public long? TransactionPrice { get; set; }
        public long? SystemTraceNo { get; set; }
        public string OrderNo { get; set; }
        public string ResNum { get; set; }
        public long? TerminalNo { get; set; }
        public string RefNum { get; set; }
        public string CardPan { get; set; }
        public string TraceNo { get; set; }
        public long? FinalStatusId { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual CustomerOrder CustomerOrder { get; set; }
        public virtual Status FinalStatus { get; set; }
    }
}
