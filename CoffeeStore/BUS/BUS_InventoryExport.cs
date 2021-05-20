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

    }
}
