using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class CustomerOrderPaymentDto
    {
        public long Id { get; set; }
        public long? PaymentPrice { get; set; }
        public string TrackingCode { get; set; }
        public string TransactionDate { get; set; }
        public long? TransactionPrice { get; set; }
        public long? SystemTraceNo { get; set; }
        public string ResNum { get; set; }
        public long? TerminalNo { get; set; }
        public string RefNum { get; set; }
        public string CardPan { get; set; }
        public string TraceNo { get; set; }
        public string FinalStatus { get; set; }
    }
}
