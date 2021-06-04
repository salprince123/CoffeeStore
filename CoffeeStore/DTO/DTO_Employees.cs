using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DTO
{
    class DTO_Employees
    {
        #region Atrributes
        private string EmployeeID;
        private string EmployeeName;
        private string EmployeeTypeID;
        private string Password;
        #endregion

        #region Method
        DTO_Employees()
        {
            EmployeeID = "";
            EmployeeName = "";
            EmployeeTypeID = "";
            Password = "";
        }
        DTO_Employees(string ID, string name, string typeId, string pass)
        {
            EmployeeID = ID;
            EmployeeName = name;
            EmployeeTypeID = typeId;
            Password = pass;
        }
        #region GetSetMethod
        string GetEmployeeID()
        {
            return EmployeeID;
        }    
        string GetEmployeeName()
        {
            return EmployeeName;
        }    
        string GetEmployeeTypeID()
        {
            return EmployeeTypeID;
        }
        string GetEmployeePassword()
        {
            return Password;
        }
        void SetEmployeeName(string newName)
        {
            EmployeeName = newName;
        }
        void SetEmployeeTypeID(string newTypeId)
        {
            EmployeeTypeID = newTypeId;
        }
        void SetPassword(string newPass)
        {
            Password = newPass;
        }
        #endregion
        #endregion
    }
}