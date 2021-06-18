﻿using CoffeeStore.DAL;
using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.BUS
{
    class BUS_Employees
    {
        DAL_Employees dalEmp = new DAL_Employees();
        public string GetPasswordByID(string ID)
        {
            return dalEmp.GetPasswordByID(ID);
        }

        public DataTable GetActiveEmployees()
        {
            return dalEmp.GetActiveEmployees();
        }
        
        public bool CreateEmployee(DTO_Employees newEmp)
        {
            return dalEmp.CreateEmployee(newEmp);
        }    

        public bool EditEmployee(DTO_Employees editEmp)
        {
            return dalEmp.EditEmployee(editEmp);
        }

        public bool EditPassword(string id, string newPass)
        {
            return dalEmp.EditPassword(id, newPass);
        }    

        public int DeleteEmployee(string deleteEmpID)
        {
            return dalEmp.Delete(deleteEmpID);
        }

        public string GetEmpTypeByID(string id)
        {
            return dalEmp.GetEmpTypeByID(id);
        }

        public string GetEmpNameByID(string id)
        {
            return dalEmp.GetEmpNameByID(id);
        }

        public DTO_Employees GetEmpByID(string id)
        {
            return dalEmp.GetEmpByID(id);
        }
    }
}
