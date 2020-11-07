using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Users
    {
        public Users()
        {
            Customer = new HashSet<Customer>();
            Employee = new HashSet<Employee>();
            Seller = new HashSet<Seller>();
            UserActivation = new HashSet<UserActivation>();
            UserRole = new HashSet<UserRole>();
        }

        public long Id { get; set; }
        public string Username { get; set; }
        public string Hpassword { get; set; }
        public string Email { get; set; }
        public long? Mobile { get; set; }
        public string FullName { get; set; }
        public long? ExpDate { get; set; }
        public long? FinalStatusId { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual ICollection<Customer> Customer { get; set; }
        public virtual ICollection<Employee> Employee { get; set; }
        public virtual ICollection<Seller> Seller { get; set; }
        public virtual ICollection<UserActivation> UserActivation { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
