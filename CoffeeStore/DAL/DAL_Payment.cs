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
    public class DAL_Payment : DBConnect
    {
        public DataTable getAllPayment()
        {
            string sql = $"Select PaymentID, Time, EmployeeName, TotalAmount From Employees e, PaymentVoucher pv Where pv.EmployeeID=e.EmployeeID";
            SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        public DataTable findPaymentbyID(string ID)
        {
            string sql ="";
            if (ID.Length > 0)
                sql = $"Select PaymentID, Time, EmployeeName, TotalAmount, Description From Employees e, PaymentVoucher pv Where pv.EmployeeID=e.EmployeeID and paymentID='" + ID + "'";
            else
                sql = $"Select PaymentID, Time, EmployeeName, TotalAmount, Description From Employees e, PaymentVoucher pv Where pv.EmployeeID=e.EmployeeID";
            SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        public int createNewPayment(DTO_Payment Payment)
        {
            int rs = 0;
            string sql = $"Insert into PaymentVoucher values ('" + Payment.PaymentID + "'," + Payment.TotalAmount + ",'" + Payment.Description + "','" + Payment.EmployeeID + "','" + Payment.Time + "')";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, getConnection());
                command.Connection.Open();
                rs = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return rs;
        }
        public int deletePayment(string id)
        {
            int rs = 0;
            string sql = $"Delete From PaymentVoucher Where PaymentID='" + id + "'";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, getConnection());
                command.Connection.Open();
                rs = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return rs;
        }
        public int editPayment(DTO_Payment Payment)
        {
            int rs = 0;
            Console.WriteLine(Payment.PaymentID);
            string sql = $"Update PaymentVoucher set PaymentID='" + Payment.PaymentID + "', EmployeeID='" + Payment.EmployeeID + "', TotalAmount=" + Payment.TotalAmount + ",Time='" + Payment.Time + "',Description='" + Payment.Description + "' Where PaymentID='" + Payment.PaymentID + "'";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, getConnection());
                command.Connection.Open();
                rs = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return rs;
        }
        public string createID()
        {
            string ID = "P";
            int count = 0;
            int max = 0;
            string sql = "Select PaymentID from PaymentVoucher";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, getConnection());
                command.Connection.Open();
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    count = Int16.Parse(reader["PaymentID"].ToString().Remove(0, 1));
                    if (count > max)
                        max = count;
                }
                max++;
                ID += max.ToString();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
            return ID;
        }
        public List<String> GetEmployee()
        {
            List<String> Employee = new List<string>();
            string sql = $"Select EmployeeName From Employees";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, getConnection());
                command.Connection.Open();
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Employee.Add(reader["EmployeeName"].ToString());
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
            return Employee;
        }
        public string getEmployeeID(string employeename)
        {
            string EmployeeID = "";
            string sql = $"Select EmployeeID from Employees where EmployeeName='" + employeename + "'";
            try
            {
                SQLiteCommand command = new SQLiteCommand(sql, getConnection());
                command.Connection.Open();
                SQLiteDataReader value = command.ExecuteReader();
                while (value.Read())
                    EmployeeID = value["EmployeeID"].ToString();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
            Console.WriteLine("ID get: " + EmployeeID);
            return EmployeeID;
        }

        public DataTable findPayment(string type, string name)
        {
            try
            {
                string sql = "";
                if (type.Length != 0 && name.Length == 0)
                    sql = $"Select PaymentID, PaymentVoucher, EmployeeName, Price From PaymentVoucher BN, Employee BT Where BN.EmployeeID=BT.EmployeeID and (BT.EmployeeName='" + type + "')";
                else if (type.Length != 0 && name.Length != 0)
                    sql = $"Select PaymentID, PaymentVoucher, EmployeeName, Price From PaymentVoucher BN, Employee BT Where BN.EmployeeID=BT.EmployeeID and (BT.EmployeeName='" + type + "' and BN.PaymentVoucher like '%" + name + "%')";
                else if (type.Length == 0 && name.Length != 0)
                    sql = $"Select PaymentID, PaymentVoucher, EmployeeName, Price From PaymentVoucher BN, Employee BT Where BN.EmployeeID=BT.EmployeeID and ( BN.PaymentVoucher like '%" + name + "%')";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                DataTable PaymentList = new DataTable();
                da.Fill(PaymentList);
                return PaymentList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            };
            return null;
        }

        public DataTable GetTotalAmountByYear(int year)
        {
            DataTable data = new DataTable();
            string sql = $"select substr(Time, 4, 2) as Month, cast(sum(TotalAmount) as int) as Total from PaymentVoucher where Time like '%/%/{year} %' group by Month";
            try
            {
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(data);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return data;
        }

        public DataTable GetTotalAmountByMonth(int month, int year)
        {
            DataTable data = new DataTable();
            string sql = $"select substr(Time, 1, 2) as Day, cast(sum(TotalAmount) as int) as Total from PaymentVoucher where Time like '%/{month.ToString().PadLeft(2, '0')}/{year} %' group by Day";
            try
            {
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(data);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
