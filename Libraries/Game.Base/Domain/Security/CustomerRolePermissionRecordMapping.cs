using Game.Base.Domain.Customers;

namespace Game.Base.Domain.Security
{
    public class CustomerRolePermissionRecordMapping
    {
        public CustomerRolePermissionRecordMapping() { }

        public CustomerRolePermissionRecordMapping(int customerRoleId,int permissionRecordId)
        {
            this.CustomerRoleId = customerRoleId;
            this.PermissionRecordId = permissionRecordId;
        }

        public int CustomerRoleId { get; set; }

        public int PermissionRecordId { get; set; }

        public CustomerRole CustomerRole { get; set; }

        public PermissionRecord PermissionRecord { get; set; }
    }
}
