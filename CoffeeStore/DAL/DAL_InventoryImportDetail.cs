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
    class DAL_InventoryImportDetail : DBConnect
    {
        public void delete(String importID)
        {    
            String sql = $"delete from InventoryImportDetail where importID='{importID}'";
            SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                insert.ExecuteNonQuery();
            }
            catch (Exception)
            {
            }
        }
        public void ImportList(List<String> sqlList)
        {
            foreach(String s in sqlList)
            {
                try
                {
                    SQLiteCommand insert = new SQLiteCommand(s, getConnection().OpenAndReturn());
                    insert.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            
            
        }
        public DataTable SelectAllImportDetail()
        {
            try
            {
                string sql = $"select * from InventoryImportDetail";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                DataTable listImportDetail = new DataTable();
                da.Fill(listImportDetail);
                return listImportDetail;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception AT" + e.ToString());
                return new DataTable();
            };

        }
        public DataTable SelectAllImportDetailGroupByName()
        {
            try
            {
                string sql = $" select isUse ,materialname as 'Tên',unit as 'Đơn vị tính', sum(amount) as 'Số lượng' " +
                            $"from InventoryImportDetail detail Join Material mater " +
                            $"on detail.MaterialID= mater.MaterialID " +
                            $"group by materialname";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                DataTable listImportDetail = new DataTable();
                da.Fill(listImportDetail);
                return listImportDetail;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception AT" + e.ToString());
                return new DataTable();
            };

        }
        public DataTable FindWithKeyWord(String key)
        {
            try
            {
                string sql = $" select materialname as 'Tên',unit as 'Đơn vị tính', sum(amount) as 'Số lượng' " +
                            $"from InventoryImportDetail detail Join Material mater " +
                            $"on detail.MaterialID= mater.MaterialID where materialName like '%{key}%' or unit like '%{key}%' " +
                            $"group by materialname ";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                DataTable listImportDetail = new DataTable();
                da.Fill(listImportDetail);
                return listImportDetail;
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
        }
        public DataTable SelectDetail(String id)
        {
            try
            {
                string sql = $"select materialname as 'Tên',amount as 'Số lượng',price as 'Đơn giá',unit as 'Đơn vị tính' from InventoryImportDetail detail Join Material mater on detail.MaterialID= mater.MaterialID  where importID='{id}'";
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
        }*/
    }
}
