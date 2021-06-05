using CoffeeStore.BUS;
using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.Data;
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
using static CoffeeStore.Account.GroupAccountList;
using static CoffeeStore.Account.PopupAddGroupAccount;

namespace CoffeeStore.Account
{
    /// <summary>
    /// Interaction logic for PopupEditGroupAccount.xaml
    /// </summary>
    public partial class PopupEditGroupAccount : UserControl
    {
        List<AccessPermissionName> dgUnselectedList;
        List<AccessPermissionName> dgSelectedList;
        AccessPermissionName seletedItemLeft;
        AccessPermissionName seletedItemRight;
        DataTable AccPersData;
        GroupAccountInfo editGrAccInfo;
        public PopupEditGroupAccount()
        {
            InitializeComponent();
        }

        public PopupEditGroupAccount(GroupAccountInfo grAccInfo)
        {
            InitializeComponent();
            LoadData(grAccInfo);
        }    

        public void LoadData(GroupAccountInfo grAccInfo)
        {
            BUS_AccessPermission busAccPer = new BUS_AccessPermission();
            AccPersData = busAccPer.GetAccessPermission();
            dgUnselectedList = new List<AccessPermissionName>();
            dgSelectedList = new List<AccessPermissionName>();
            foreach (DataRow row in AccPersData.Rows)
            {
                string name = row["AccessPermissionName"].ToString();
                dgUnselectedList.Add(new AccessPermissionName(name));
            }

            editGrAccInfo = grAccInfo;
            tbName.Text = editGrAccInfo.name;
            this.dgUnselected.ItemsSource = dgUnselectedList;
            this.dgSelected.ItemsSource = dgSelectedList;

            #region change list item
            if (grAccInfo.cashier == true)
            {
                dgSelectedList.Add(new AccessPermissionName(Constants.cashier));
                AccessPermissionName item = dgUnselectedList.Find(x => x.name == Constants.cashier);
                dgUnselectedList.Remove(item);
            }

            if (grAccInfo.changeAccount == true)
            {
                dgSelectedList.Add(new AccessPermissionName(Constants.changeAccount));
                AccessPermissionName item = dgUnselectedList.Find(x => x.name == Constants.changeAccount);
                dgUnselectedList.Remove(item);
            }

            if (grAccInfo.importInventory == true)
            {
                dgSelectedList.Add(new AccessPermissionName(Constants.importInventory));
                AccessPermissionName item = dgUnselectedList.Find(x => x.name == Constants.importInventory);
                dgUnselectedList.Remove(item);
            }

            if (grAccInfo.exportInventory == true)
            {
                dgSelectedList.Add(new AccessPermissionName(Constants.exportInventory));
                AccessPermissionName item = dgUnselectedList.Find(x => x.name == Constants.exportInventory);
                dgUnselectedList.Remove(item);
            }
            #endregion

            this.dgUnselected.Items.Refresh();
            this.dgSelected.Items.Refresh();
        }

        private void btnSelectedToRight_Click(object sender, RoutedEventArgs e)
        {
            dgSelectedList.Add(seletedItemLeft);
            dgUnselectedList.Remove(seletedItemLeft);
            this.dgUnselected.Items.Refresh();
            this.dgSelected.Items.Refresh();
        }

        private void btnAllToRight_Click(object sender, RoutedEventArgs e)
        {
            foreach (AccessPermissionName item in dgUnselectedList)
            {
                dgSelectedList.Add(item);
            }
            dgUnselectedList.Clear();
            this.dgSelected.Items.Refresh();
            this.dgUnselected.Items.Refresh();
        }

        private void btnSelectedToLeft_Click(object sender, RoutedEventArgs e)
        {
            dgUnselectedList.Add(seletedItemRight);
            dgSelectedList.Remove(seletedItemRight);
            this.dgSelected.Items.Refresh();
            this.dgUnselected.Items.Refresh();
        }

        private void btnAllToLeft_Click(object sender, RoutedEventArgs e)
        {
            foreach (AccessPermissionName item in dgSelectedList)
            {
                dgUnselectedList.Add(item);
            }
            dgSelectedList.Clear();
            this.dgSelected.Items.Refresh();
            this.dgUnselected.Items.Refresh();
        }

        private void dgUnselected_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgUnselected.SelectedItems.Count > 0)
            {
                seletedItemLeft = (AccessPermissionName)dgUnselected.SelectedItems[0];
            }
        }

        private void dgSelected_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgSelected.SelectedItems.Count > 0)
            {
                seletedItemRight = (AccessPermissionName)dgSelected.SelectedItems[0];
            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (tbName.Text == "")
            {
                // Name of Employee Type is empty
                return;
            }

            BUS_EmployeeType busEmpType = new BUS_EmployeeType();
            string editID = busEmpType.GetIDByName(editGrAccInfo.name);
            bool result = busEmpType.EditEmployeeType(new DTO_EmployeeType(editID, editGrAccInfo.name));
            if (!result)
            {
                MessageBox.Show($"Đã xảy ra lỗi do tên loại tài khoản trong quá trình sửa loại tài khoản {editGrAccInfo.name}.");
            }    
            BUS_AccessPermissionGroup busAccPerGr = new BUS_AccessPermissionGroup();
            #region Get List Permission
            List<AccessPermissionName> oldPerList = new List<AccessPermissionName>();

            if (editGrAccInfo.cashier)
                oldPerList.Add(new AccessPermissionName(Constants.cashier));

            if (editGrAccInfo.changeAccount)
                oldPerList.Add(new AccessPermissionName(Constants.changeAccount));

            if (editGrAccInfo.importInventory)
                oldPerList.Add(new AccessPermissionName(Constants.importInventory));

            if (editGrAccInfo.exportInventory)
                oldPerList.Add(new AccessPermissionName(Constants.exportInventory));

            #endregion
            // per deleted
            foreach(AccessPermissionName name in oldPerList)
            {
                if (!dgSelectedList.Contains(name))
                {
                    foreach (DataRow row in AccPersData.Rows)
                    {
                        if (name.name == row[1].ToString())
                        {
                            result = busAccPerGr.DeleteAccessPermissionGroup(new DTO_AccessPermissionGroup(row[0].ToString(), editID));
                            if (!result)
                            {
                                MessageBox.Show($"Đã xảy ra lỗi do quyền bị xóa đi trong quá trình sửa loại tài khoản {editGrAccInfo.name}.");
                            }
                            break;
                        }
                    }
                }    
            }

            // per added
            foreach (AccessPermissionName item in dgSelectedList)
            {
                if (!oldPerList.Contains(item))
                {
                    foreach (DataRow row in AccPersData.Rows)
                    {
                        if (item.name == row[1].ToString())
                        {
                            result = busAccPerGr.CreateAccessPermissionGroup(new DTO_AccessPermissionGroup(row[0].ToString(), editID));
                            if (!result)
                            {
                                MessageBox.Show($"Đã xảy ra lỗi do quyền được thêm vào trong quá trình sửa loại tài khoản {editGrAccInfo.name}.");
                            }
                            break;
                        }
                    }
                }
            }

            MessageBox.Show($"Đã sửa thành công loại tài khoản {editGrAccInfo.name}.");
            Window.GetWindow(this).Close();
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
