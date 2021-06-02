using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DAL
{
    public class DAL_EmployeeType : DBConnect
    {
        public DataTable GetEmployeeTypes()
        {
            DataTable empTypes = new DataTable();
            try
            {
                string sql = $"select * from EmployeeType";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(empTypes);
            }
            catch
            {

            }
            return empTypes;
        }    
    }
}
