using CoffeeStore.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.BUS
{
    public class BUS_Material
    {
        DAL_Material dalVL = new DAL_Material();
        public DataTable selectAll()
        {
            return dalVL.SelectAllMaterial();
        }
        public void Create (String name, String unit)
        {
            dalVL.Create(name, unit);
        }
        public bool Delete (String id)
        {
            return dalVL.Delete(id);
        }
        public bool Update(String id, String name, String unit)
        {
            return dalVL.Update(id,name,unit);
        }
    }
}
