using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Base.Domain.Customers
{
    public class CustomerCustomerRoleMapping:BaseEntity
    {
        public CustomerCustomerRoleMapping() { }

        public CustomerCustomerRoleMapping(int customerId,int customerRoleId)
        {
            this.CustomerId = customerId;
            this.CustomerRoleId = customerRoleId;
        }
        public int CustomerId { get; set; }

        public int CustomerRoleId { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual CustomerRole CustomerRole { get; set; }
    }
}
