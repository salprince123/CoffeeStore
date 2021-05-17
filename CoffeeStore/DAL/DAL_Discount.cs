using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeStore.DTO;
namespace CoffeeStore.DAL
{
    class DAL_Discount: DBConnect
    {
        public DataTable getAllDiscount()
        {
            /*string sql = $"select * from discount order by enddate DESC";
            SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;*/
            return null;
        }

        public DataTable findDiscount(string startdate, string enddate)
        {
            string sql = $"select * from discount where startdate>=" + startdate + " and enddate<=" + enddate;
            SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public int createNewDiscount(DTO_Discount dTO_Discount)
        {
            int result = 0;
            string sql = $"Insert into Discount (DiscountID, DiscountName, StartDate, EndDate, DiscountValue, Description) values ('" + dTO_Discount.DiscountID + "','" + dTO_Discount.DiscountName + "','" + dTO_Discount.StartDate + "','" + dTO_Discount.EndDate + "'," + dTO_Discount.DiscountValue + ",'" + dTO_Discount.Description + "');";
            try
            {
                SQLiteCommand sqlite = getConnection().CreateCommand();
                sqlite.CommandText = sql;
                sqlite.Connection.Open();
                result = sqlite.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(sql);
            }
            return result;
        }

        public int editDiscount(DTO_Discount dTO_Discount)
        {
            string sql = $"Update Discount set Discountname = "+dTO_Discount.DiscountName + ", StartDate = " + dTO_Discount.StartDate + ", EndDate = " + dTO_Discount.EndDate + ", DiscountValue = " + dTO_Discount.DiscountValue + ", Description = " + dTO_Discount.Description + " Where DiscountID = "+dTO_Discount.DiscountID;
            SQLiteCommand sqlite = getConnection().CreateCommand();
            sqlite.CommandText = sql;
            return sqlite.ExecuteNonQuery();
        }

        public int deleteDiscount(string discountID)
        {
            string sql = $"Delete from Discount where DiscountID = "+discountID;
            SQLiteCommand sqlite = getConnection().CreateCommand();
            sqlite.CommandText = sql;
            sqlite.Connection.Open();
            return sqlite.ExecuteNonQuery();
        }
    }
}
