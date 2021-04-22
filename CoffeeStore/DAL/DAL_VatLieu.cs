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
    public class DAL_VatLieu : DBConnect
    {
        public DataTable SelectAllVatLieu()
        {
            try
            {
                string sql = $"select MaVatLieu, TenVatLieu, DonViTinh from VatLieu";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                DataTable dsMon = new DataTable();
                da.Fill(dsMon);
                return dsMon;
            }
            catch (Exception)
            {
            };
            return null;
        }

    }

}
