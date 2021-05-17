using CoffeeStore.BUS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CoffeeStore.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
            txtBoxPassword.Password = "12345";
        }

        public bool CheckPassword()
        {
            string id = txtBoxAccount.Text;
            string pass = txtBoxPassword.Password;
            BUS_Employees busEmp = new BUS_Employees();
            string truePass = busEmp.GetPasswordByID(id);
            if (truePass == "")
            {
                //show validate can't find account
                return false;
            }    
                
            if (truePass != pass)
            {
                //show validate false account or password
                return false;
            }
            
            return true;
        }    
    }
}