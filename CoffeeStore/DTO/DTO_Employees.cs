using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DTO
{
    public class DTO_Employees
    {
        #region Atrributes
        private string EmployeeID;
        private string EmployeeName;
        private string EmployeeTypeName;
        private string Password;
        #endregion

        #region Method
        public DTO_Employees()
        {
            EmployeeID = "";
            EmployeeName = "";
            EmployeeTypeName = "";
            Password = "";
        }
        public DTO_Employees(string ID, string name, string typeName, string pass)
        {
            EmployeeID = ID;
            EmployeeName = name;
            EmployeeTypeName = typeName;
            Password = pass;
        }
        #region GetSetMethod
        public string GetEmployeeID()
        {
            return EmployeeID;
        }
        public string GetEmployeeName()
        {
            return EmployeeName;
        }
        public string GetEmployeeTypeName()
        {
            return EmployeeTypeName;
        }
        public string GetEmployeePassword()
        {
            return Password;
        }
        public void SetEmployeeName(string newName)
        {
            EmployeeName = newName;
        }
        public void SetEmployeeTypeName(string newTypeName)
        {
            EmployeeTypeName = newTypeName;
        }
        public void SetPassword(string newPass)
        {
            Password = newPass;
        }
        #endregion
        #endregion
    }
}