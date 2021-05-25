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
            string sql = $"select DiscountID as 'Mã giảm giá', DiscountName as 'Tên ưu đãi', StartDate as 'Ngày bắt đầu', EndDate as 'Ngày kết thúc', DiscountValue as 'Mức ưu đãi (%)'  from discount order by enddate DESC";
            SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable findDiscount(string startdate, string enddate)
        {
            string sql = $"select DiscountID as 'Mã giảm giá', DiscountName as 'Tên ưu đãi', DiscountValue as 'Mức ưu đãi (%)', StartDate as 'Ngày bắt đầu', EndDate as 'Ngày kết thúc' from discount where startdate>=" + startdate + " and enddate<=" + enddate;
            SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public int createNewDiscount(DTO_Discount dTO_Discount)
        {
            int result = 0;
            string sql = $"Insert into Discount (DiscountID, DiscountName, StartDate, EndDate, DiscountValue, Description) values ('" + createID() + "','" + dTO_Discount.DiscountName + "','" + dTO_Discount.StartDate + "','" + dTO_Discount.EndDate + "'," + dTO_Discount.DiscountValue + ",'" + dTO_Discount.Description + "');";
            try
            {
                SQLiteCommand sqlite = getConnection().CreateCommand();
                sqlite.CommandText = sql;
                sqlite.Connection.Open();
                result = sqlite.ExecuteNonQuery();
            }
            catch (Exception )
            {
                Console.WriteLine(sql);
            }
            return result;
        }
        public string createID()
        {
            string ID = "DC";
            string sql = "Select DiscountID from Discount";
            int count = 0;
            int max = 0;
            SQLiteCommand sqlite = new SQLiteCommand(sql, getConnection());
            sqlite.Connection.Open();
            SQLiteDataReader reader = sqlite.ExecuteReader();
            while (reader.Read())
                {
                    count = Int32.Parse(reader["DiscountID"].ToString().Remove(0, 2));
                if (count > max)
                    max = count;
                };
            max++;
            ID = ID + max.ToString();
            return ID;
        }
        public int editDiscount(DTO_Discount dTO_Discount)
        {
            string sql = $"Update Discount set Discountname = '"+dTO_Discount.DiscountName + "', StartDate = '" + dTO_Discount.StartDate + "', EndDate = '" + dTO_Discount.EndDate + "', DiscountValue = " + dTO_Discount.DiscountValue + ", Description = '" + dTO_Discount.Description + "' Where DiscountID = '"+dTO_Discount.DiscountID+"'";
            int rs = 0;
            try
            {
                SQLiteCommand sqlite = getConnection().CreateCommand();
                sqlite.CommandText = sql;
                sqlite.Connection.Open();
                rs = sqlite.ExecuteNonQuery();
            }
            catch(Exception)
 //(Discount &Menu: binding data for dataGrid)
            {
                Console.WriteLine(sql);
            }
            return rs;
        }

        public int deleteDiscount(string discountID)
        {
            string sql = $"Delete from Discount where DiscountID = '"+discountID+"'";
            SQLiteCommand sqlite = getConnection().CreateCommand();
            sqlite.CommandText = sql;
            sqlite.Connection.Open();
            return sqlite.ExecuteNonQuery();
        }
    }
}
