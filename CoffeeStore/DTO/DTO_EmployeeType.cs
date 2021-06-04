using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DTO
{
    class DTO_EmployeeType
    {
        #region Atrributes
        private string EmployeeTypeID;
        private string EmployeeTypeName;
        #endregion

        #region Method
        DTO_EmployeeType()
        {
            EmployeeTypeID = "";
            EmployeeTypeName = "";
        }
        DTO_EmployeeType(string ID, string name)
        {
            EmployeeTypeID = ID;
            EmployeeTypeName = name;
        }
        #region GetSetMethod
        string GetEmployeeID()
        {
            return EmployeeTypeID;
        }
        string GetEmployeeTypeName()
        {
            return EmployeeTypeName;
        }
        void SetEmployeeTypeID(string newTypeId)
        {
            EmployeeTypeID = newTypeId;
        }
        void SetEmployeeTypeName(string newName)
        {
            EmployeeTypeName = newName;
        }
        #endregion
        #endregion
    }
}
