using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class FamousComments
    {
        public long Id { get; set; }
        public string ProfilePic { get; set; }
        public string Name { get; set; }
        public string Post { get; set; }
        public string Comment { get; set; }
        public string CommentPic { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }
    }
}
