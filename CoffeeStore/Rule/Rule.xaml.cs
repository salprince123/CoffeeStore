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

namespace CoffeeStore.Rule
{
    /// <summary>
    /// Interaction logic for Rule.xaml
    /// </summary>
    public partial class Rule : UserControl
    {
        MainWindow _context;
        public Rule(MainWindow mainWindow)
        {
            InitializeComponent();
            _context = mainWindow;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var screen = new RuleEdit(_context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
        }
    }
}
