using Game.Base.Domain.Security;
using System.Collections.Generic;

namespace Game.Base.Domain.Customers
{
    /// <summary>
    /// Represents a customer role
    /// </summary>
    public partial class CustomerRole : BaseEntity
    {
        /// <summary>
        /// Gets or sets the customer role name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer role is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer role is system
        /// </summary>
        public bool IsSystemRole { get; set; }

        /// <summary>
        /// Gets or sets the customer role system name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customers must change passwords after a specified time
        /// </summary>
        public bool EnablePasswordLifetime { get; set; }


        private ICollection<CustomerRolePermissionRecordMapping> _customerRolePermissionRecordMapping;
        public virtual ICollection<CustomerRolePermissionRecordMapping> CustomerRolePermissionRecordMapping
        {
            get { return _customerRolePermissionRecordMapping ?? (_customerRolePermissionRecordMapping = new List<CustomerRolePermissionRecordMapping>()); }
            protected set { _customerRolePermissionRecordMapping = value; }
        }

        private ICollection<CustomerCustomerRoleMapping> _customerCustomerRoleMapping;
        public virtual ICollection<CustomerCustomerRoleMapping> CustomerCustomerRoleMapping
        {
            get { return _customerCustomerRoleMapping ?? (_customerCustomerRoleMapping = new List<CustomerCustomerRoleMapping>()); }
            protected set { _customerCustomerRoleMapping = value; }
        }
    }
}