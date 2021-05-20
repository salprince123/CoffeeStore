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
    public class DAL_Beverage : DBConnect
    {
        public DataTable getAllBeverage()
        {
            try
            {
                string sql = $"Select * From BeverageName";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                DataTable dsMon = new DataTable();
                da.Fill(dsMon);
                return dsMon;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            };
            return null;
        }       
        public int createNewBeverage(DTO_Beverage beverage)
        {
            int rs = 0;
            return rs;
        }
        public int deleteBeverage(string id)
        {
            int rs = 0;
            return rs;
        }
        public int editBeverage(DTO_Beverage beverage)
        {
            int rs = 0;
            return rs;
        }
        public string createID()
        {
            string ID = "B";
            return ID;
        }
    }
}
