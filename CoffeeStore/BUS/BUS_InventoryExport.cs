using CoffeeStore.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.BUS
{
    public class BUS_InventoryExport
    {
        DAL_InventoryExport export = new DAL_InventoryExport();
        public DataTable  SelectAll()
        {
            return export.SelectAllExport();
        }
        public string Create(String name, String date)
        {
            return export.Create(name, date);
        }
        public bool Delete(String id)
        {
            return export.Delete(id);
        }
        public DataTable SelectDetail(String id)
        {
            return export.SelectDetail(id);
        }
    }
}
