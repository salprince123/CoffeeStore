using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DTO
{
    class DTO_AccessPermissionGroup
    {
        #region Atrributes
        private string AccessPermissionID;
        private string EmployeeTypeID;
        #endregion

        #region Method
        DTO_AccessPermissionGroup()
        {
            AccessPermissionID = "";
            EmployeeTypeID = "";
        }
        DTO_AccessPermissionGroup(string APID, string ETID)
        {
            AccessPermissionID = APID;
            EmployeeTypeID = ETID;
        }
        #region GetSetMethod
        string GetEmployeeID()
        {
            return AccessPermissionID;
        }
        string GetAccessPermissionName()
        {
            return EmployeeTypeID;
        }
        void SetAccessPermissionID(string newAPID)
        {
            AccessPermissionID = newAPID;
        }
        void SetAccessPermissionName(string newETID)
        {
            EmployeeTypeID = newETID;
        }
        #endregion
        #endregion
    }
}
