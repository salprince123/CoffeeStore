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
    class DAL_InventoryExportDetail : DBConnect
    {
        public DataTable SelectAllExportDetail()
        {
            try
            {
                string sql = $"select * from InventoryExportDetail";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                DataTable listDetail = new DataTable();
                da.Fill(listDetail);
                return listDetail;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception AT" + e.ToString());
                return new DataTable();
            };
        }
        public DataTable SelectAllExportDetailGroupByName()
        {
            try
            {
                string sql = $" select materialname as 'Tên', sum(amount) as 'Số lượng' " +
                            $"from InventoryExportDetail detail Join Material mater " +
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
    }
}
