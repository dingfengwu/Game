using Game.Base;
using Game.Base.Domain.Customers;
using Game.Base.Domain.Security;
using System.Collections.Generic;

namespace Domain.Domain.Security
{
    /// <summary>
    /// Represents a permission record
    /// </summary>
    public partial class PermissionRecord : BaseEntity
    {
        /// <summary>
        /// Gets or sets the permission name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the permission system name
        /// </summary>
        public string SystemName { get; set; }
        
        /// <summary>
        /// Gets or sets the permission category
        /// </summary>
        public string Category { get; set; }

        
        private ICollection<CustomerRolePermissionRecordMapping> _customerRolePermissionRecordMapping;
        public virtual ICollection<CustomerRolePermissionRecordMapping> CustomerRolePermissionRecordMapping
        {
            get { return _customerRolePermissionRecordMapping ?? (_customerRolePermissionRecordMapping = new List<CustomerRolePermissionRecordMapping>()); }
            protected set { _customerRolePermissionRecordMapping = value; }
        }   
    }
}
