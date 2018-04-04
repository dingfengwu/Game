using Domain.Domain.Security;
using Game.Base.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Base.Domain.Security
{
    public class CustomerRolePermissionRecordMapping
    {
        public int CustomerRoleId { get; set; }

        public int PermissionRecordId { get; set; }

        public CustomerRole CustomerRole { get; set; }

        public PermissionRecord PermissionRecord { get; set; }
    }
}
