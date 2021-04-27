using CoffeeStore.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.BUS
{
    public class BUS_InventoryImport
    {
        DAL_InventoryImport import = new DAL_InventoryImport();
        public DataTable selectAll()
        {
            return import.SelectAllImport();
        }
        public void Create(String employID)
        {
            import.Create(employID);
        }
        public bool Delete(String id)
        {
            return import.Delete(id);
        }
        public bool Update(String id, String employId, String date)
        {
            return import.Update(id, employId, date);
        }
    }
}
