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
    class DAL_InventoryExport : DBConnect
    {
        public DataTable SelectAllExport()
        {
            try
            {
                string sql = $"select employeename, exportid, exportdate from InventoryExport Imp Join Employees employ on employ.EmployeeID=Imp.EmployeeID";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                DataTable listExport = new DataTable();
                da.Fill(listExport);
                return listExport;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception AT" + e.ToString());
                return new DataTable();
            };

        }
        /*public void Create(String employID)
        {
            //create auto increase ID
            //Get max MaterialID
            String id = "";
            string tempSQL = "SELECT importID FROM InventoryImport order by importID desc LIMIT 1 ";
            SQLiteDataAdapter da = new SQLiteDataAdapter(tempSQL, getConnection());
            DataTable maxId = new DataTable();
            da.Fill(maxId);
            foreach (DataRow row in maxId.Rows)
            {
                id = row["MaterialID"].ToString();
            }
            //auto increase ID
            string newID = "Imp" +
                (Convert.ToInt32(id.Replace("Mater", "")) + 1)
                    .ToString()
                    .PadLeft(7, '0');
            //insert SQLite 
            string sql = $"insert into InventoryImport('ImportID','EmployeeID','ImportDate') VALUES ('{newID}','{employID}','{DateTime.Now.ToString()}');";
            SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                insert.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool Delete(String id)
        {
            string sql = $"delete from inventoryImport where ImportID='{id}'";
            SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                return insert.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Update(String id, String employID, String date)
        {
            string sql = $"update inventoryImport set employeeID='{employID}', ImportDate = '{date}'  where importID='{id}'";
            SQLiteCommand update = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                return update.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }*/
        public DataTable SelectDetail(String id)
        {
            try
            {
                string sql = $"select materialname as 'Tên',amount as 'Số lượng',description as 'Mô tả' from InventoryExportDetail detail Join Material mater on detail.MaterialID= mater.MaterialID where exportID='{id}'";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                DataTable listImport = new DataTable();
                da.Fill(listImport);
                return listImport;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception AT" + e.ToString());
                return new DataTable();
            };
        }
    }
}
