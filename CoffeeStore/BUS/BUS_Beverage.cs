using CoffeeStore.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeStore.DTO;
namespace CoffeeStore.BUS
{
    public class BUS_Beverage
    {
        DAL_Beverage dalBeverage = new DAL_Beverage();
        public DataTable getAllBeverage()
        {
            return dalBeverage.getAllBeverage();
        }

        public DataTable getTop5()
        {
            return dalBeverage.getTop5();
        }
        public DataTable findBeverage(string type, string name)
        {
            return dalBeverage.findBeverage(type, name);
        }
        public int createNewBevverage(DTO_Beverage beverage)
        {
            return dalBeverage.createNewBeverage(beverage);
        }
        public int editBevverage(DTO_Beverage beverage)
        {
            return dalBeverage.editBeverage(beverage);
        }
        public int deleteBevverage(string ID)
        {
            return dalBeverage.deleteBeverage(ID);
        }
        public string createID()
        {
            return dalBeverage.createID();
        }
        public List<String> getBeverageType()
        {
            return dalBeverage.GetBeverageType();
        }
        public DataTable GetBeverageTypeInfo()
        {
            return dalBeverage.GetBeverageTypeInfo();
        }

        public string getBeverageTypeID(string beveragename)
        {
            return dalBeverage.getBeverageTypeID(beveragename);
        }

        public bool ChangeIsOutOfStockValue(string id, bool IsOutOfStock)
        {
            return dalBeverage.ChangeIsOutOfStockValue(id, IsOutOfStock);
        }

        public DataTable GetBeverageOrderBySellAmount(DateTime startDate, DateTime endDate)
        {
            return dalBeverage.GetBeverageOrderBySellAmount(startDate, endDate);
        }

        public DataTable GetBeverageOrderBySellIncome(DateTime startDate, DateTime endDate)
        {
            return dalBeverage.GetBeverageOrderBySellIncome(startDate, endDate);
        }
    }
}
