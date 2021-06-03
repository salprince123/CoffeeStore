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
        public DataTable GetAccessInfo()
        {
            DataTable accessData = new DataTable(); //datatable from database
            DataTable perListData = new DataTable();
            DataTable empTypeListData = new DataTable();
            DataTable accessInfo = new DataTable(); //datatable for showwing
            DataColumn dtColumn;
            DataRow dtRow;
            try
            {
                // create columns of datatable for show
                string sql = $"select AccessPermissionName from AccessPermission";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(perListData);

                //Create first Column in datatable for showwing
                dtColumn = new DataColumn();
                dtColumn.DataType = Type.GetType("System.String");
                dtColumn.ColumnName = "EmployeeType";
                dtColumn.Caption = "Type Name";
                accessInfo.Columns.Add(dtColumn);

                for (int i = 0; i < perListData.Rows.Count; i++)
                {
                    dtColumn = new DataColumn();
                    dtColumn.DataType = Type.GetType("System.String");
                    dtColumn.Caption = dtColumn.ColumnName = perListData.Rows[i].ItemArray[0].ToString();
                    accessInfo.Columns.Add(dtColumn);
                }

                // create rows of datatable for show
                sql = $"select EmployeeTypeName from EmployeeType";
                da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(empTypeListData);

                for (int i = 0; i < empTypeListData.Rows.Count; i++)
                {
                    dtRow = accessInfo.NewRow();
                    dtRow[0] = empTypeListData.Rows[i].ItemArray[0].ToString();
                    for (int j = 1; j <= perListData.Rows.Count; j++)
                        dtRow[perListData.Rows[j - 1].ItemArray[0].ToString()] = "0";
                    accessInfo.Rows.Add(dtRow);
                }

                sql = $"select EmployeeType.EmployeeTypeName, AccessPermission.AccessPermissionName from EmployeeType join AccessPermission on EmployeeType.EmployeeTypeID = AccessPermissionGroup.EmployeeTypeID join AccessPermissionGroup on AccessPermissionGroup.AccessPermissionID = AccessPermission.AccessPermissionID";
                da = new SQLiteDataAdapter(sql, getConnection());
                da.Fill(accessData);

                foreach (DataRow row in accessData.Rows)
                {
                    for (int i = 0; i < empTypeListData.Rows.Count; i++)
                    {
                        if (row.ItemArray[0].ToString() == empTypeListData.Rows[i].ItemArray[0].ToString())
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
    }
}
