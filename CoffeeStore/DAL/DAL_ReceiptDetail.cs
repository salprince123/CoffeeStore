using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DAL
{
    class DAL_ReceiptDetail : DBConnect
    {
        public bool CreateReceiptDetail(DTO_ReceiptDetail newReceiptDetail)
        {
            //insert SQLite 
            string sql = $"INSERT INTO ReceiptDetail (ReceiptID, BeverageID, Amount, Price) VALUES ('{newReceiptDetail.ReceiptID}', '{newReceiptDetail.BeverageID}', {newReceiptDetail.Amount}, {newReceiptDetail.Price})";
            SQLiteCommand insert = new SQLiteCommand(sql, getConnection().OpenAndReturn());
            try
            {
                insert.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
