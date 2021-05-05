using System.Windows.Controls;

namespace CoffeeStore.ViewModel
{
    public class SubItem
    {
        public SubItem(string name,  int uid,UserControl screen = null)
        {
            Name = name;
            Screen = screen;
            Uid = uid;
        }
        public string Name { get; private set; }
        public UserControl Screen { get; private set; }
        public int Uid { get; set; }
    }
}