using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DTO
{
    class DTO_AccessPermission
    {
        #region Atrributes
        private string AccessPermissionID;
        private string AccessPermissionName;
        #endregion

        #region Method
        DTO_AccessPermission()
        {
            AccessPermissionID = "";
            AccessPermissionName = "";
        }
        DTO_AccessPermission(string ID, string name)
        {
            AccessPermissionID = ID;
            AccessPermissionName = name;
        }
        #region GetSetMethod
        string GetEmployeeID()
        {
            return AccessPermissionID;
        }
        string GetAccessPermissionName()
        {
            return AccessPermissionName;
        }
        void SetAccessPermissionID(string newTypeId)
        {
            AccessPermissionID = newTypeId;
        }
        void SetAccessPermissionName(string newName)
        {
            AccessPermissionName = newName;
        }
        #endregion
        #endregion
    }
}
