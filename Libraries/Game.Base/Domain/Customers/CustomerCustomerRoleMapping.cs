using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Base.Domain.Customers
{
    public class CustomerCustomerRoleMapping
    {
        public CustomerCustomerRoleMapping() { }

        public CustomerCustomerRoleMapping(int customerId,int customerRoleId)
        {
            this.CustomerId = customerId;
            this.CustomerRoleId = customerRoleId;
        }
        public int CustomerId { get; set; }

        public int CustomerRoleId { get; set; }

        public Customer Customer { get; set; }

        public CustomerRole CustomerRole { get; set; }
    }
}
