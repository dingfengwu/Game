using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Base.Domain.Customers
{
    public class Customer:BaseEntity
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public Customer()
        {
            this.CustomerGuid = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the customer GUID
        /// </summary>
        public Guid CustomerGuid { get; set; }

        /// <summary>
        /// 管理者对客户的评价
        /// </summary>
        public string AdminComment { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Username { get; set; }
        
        public bool RequireReLogin { get; set; }

        public bool Active { get; set; }

        public bool Deleted { get; set; }

        public bool IsSystemAccount { get; set; }

        public string SystemName { get; set; }

        public int FailedLoginAttempts { get; set; }

        public string EmailToRevalidate { get; set; }

        public string LastIpAddress { get; set; }

        public DateTime? LastLoginDateUtc { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime LastActivityDateUtc { get; set; }

        public DateTime? CannotLoginUntilDateUtc { get; set; }
        

        private ICollection<CustomerCustomerRoleMapping> _customerCustomerRoleMapping;
        public virtual ICollection<CustomerCustomerRoleMapping> CustomerCustomerRoleMapping
        {
            get { return _customerCustomerRoleMapping ?? (_customerCustomerRoleMapping = new HashSet<CustomerCustomerRoleMapping>()); }
            protected set { _customerCustomerRoleMapping = value; }
        }
    }
}
