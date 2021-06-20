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
    class DAL_AccessPermission : DBConnect
    {
        public DataTable GetAccessInfo(int limit, int offset)
        {
            DataTable accessData = new DataTable(); //datatable from database
            DataTable perListData = new DataTable();
            DataTable accessInfo = new DataTable(); //datatable for showwing
            DataColumn dtColumn;
            try
            {
                // create columns of datatable for show
                string sql = $"select AccessPermissionName from AccessPermission";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(perListData);

                // create rows of datatable for show
                sql = $"select * from EmployeeType limit {limit} offset {offset}";
                da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(accessInfo);

                for (int i = 0; i < perListData.Rows.Count; i++)
                {
                    dtColumn = new DataColumn();
                    dtColumn.DataType = Type.GetType("System.String");
                    dtColumn.Caption = dtColumn.ColumnName = perListData.Rows[i].ItemArray[0].ToString();
                    accessInfo.Columns.Add(dtColumn);
                }

                for (int i = 0; i < accessInfo.Rows.Count; i++)
                    for (int j = 0; j < perListData.Rows.Count; j++)
                        accessInfo.Rows[i][perListData.Rows[j].ItemArray[0].ToString()] = "0";

                sql = $"select EmployeeType.EmployeeTypeName, AccessPermission.AccessPermissionName from EmployeeType join AccessPermission on EmployeeType.EmployeeTypeID = AccessPermissionGroup.EmployeeTypeID join AccessPermissionGroup on AccessPermissionGroup.AccessPermissionID = AccessPermission.AccessPermissionID";
                da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(accessData);

                foreach (DataRow row in accessData.Rows)
                {
                    for (int i = 0; i < accessInfo.Rows.Count; i++)
                    {
                        if (row.ItemArray[0].ToString() == accessInfo.Rows[i].ItemArray[1].ToString())
                        {
                            accessInfo.Rows[i][columnName: row.ItemArray[1].ToString()] = "1";
                            break;
                        }
                    }
                }
            }
            catch
            {

            }
            return accessInfo;
        }

        public DataTable GetAccessPermissions()
        {
            DataTable accPers = new DataTable();

            try
            {
                string sql = $"select * from AccessPermission";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(accPers);
            }
            catch
            {

            }

            return accPers;
        }    

        public string GetIDByName(string name)
        {
            string id = "";
            DataTable empTypeName = new DataTable();
            try
            {
                string sql = $"select AccessPermissionID from AccessPermission where AccessPermissionName = '{name}'";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(empTypeName);
                id = empTypeName.Rows[0].ItemArray[0].ToString();
            }
            catch
            {

            }
            return id;
        }    
    }
}
