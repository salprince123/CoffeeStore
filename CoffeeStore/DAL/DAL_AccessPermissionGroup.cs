using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DAL
{
    class DAL_AccessPermissionGroup : DBConnect
    {
        public bool CreateAccessPermissionGroup(DTO_AccessPermissionGroup newAccPerGr)
        {
            //insert SQLite 
            string sql = $"insert into AccessPermissionGroup('EmployeeTypeID','AccessPermissionID') VALUES ('{newAccPerGr.EmployeeTypeID}','{newAccPerGr.AccessPermissionID}')";
            SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                insert.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
