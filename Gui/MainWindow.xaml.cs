using DataAccess;
using Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window
    {

       
        ViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new ViewModel();
            DataContext = viewModel;
            GetComboBoxItems();
            DisAllowEditing();
        }
        
        private void AllowEditOrder()
        {
            
            comboBoxOrderID.IsReadOnly = false;
            comboBoxCustomerID.IsReadOnly = false;
            comboBoxEmployeeID.IsReadOnly = false;
            datePickerOrderDate.IsEnabled = true;
            datePickerRequiredDate.IsEnabled = true;
            datePickerShippedDate.IsEnabled = true;
            comboBoxShipVia.IsReadOnly = false;
            textBoxFreight.IsReadOnly = false;
            textBoxShipName.IsReadOnly = false;
            textBoxShipAddress.IsReadOnly = false;
            textBoxShipCity.IsReadOnly = false;
            textBoxShipRegion.IsReadOnly = false;
            textBoxShipPostalCode.IsReadOnly = false;
            textBoxShipCountry.IsReadOnly = false;
        }
        private void DisAllowEditing()
        {

            comboBoxOrderID.IsReadOnly = true;
            comboBoxCustomerID.IsReadOnly = true;
            comboBoxEmployeeID.IsReadOnly = true;
            datePickerOrderDate.IsEnabled = false;
            datePickerRequiredDate.IsEnabled = false;
            datePickerShippedDate.IsEnabled = false;
            comboBoxShipVia.IsReadOnly = true;
            textBoxFreight.IsReadOnly = true;
            textBoxShipName.IsReadOnly = true;
            textBoxShipAddress.IsReadOnly = true;
            textBoxShipCity.IsReadOnly = true;
            textBoxShipRegion.IsReadOnly = true;
            textBoxShipPostalCode.IsReadOnly = true;
            textBoxShipCountry.IsReadOnly = true;
        }


        private void NewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            try
            {
                int.TryParse(comboBoxEmployeeID.Text, out int employeeID);
                int.TryParse(comboBoxShipVia.Text, out int shipVia);
                decimal.TryParse(textBoxFreight.Text, out decimal freight);
                Order order = new Order(1, comboBoxCustomerID.Text, employeeID, datePickerOrderDate.SelectedDate.Value.Date,
                    datePickerOrderDate.SelectedDate.Value.Date, datePickerOrderDate.SelectedDate.Value.Date, shipVia, freight,
                    textBoxShipName.Text, textBoxShipAddress.Text, textBoxShipCity.Text, textBoxShipRegion.Text, textBoxShipPostalCode.Text,
                    textBoxShipCountry.Text, orderDetails);

                Repository repository = new Repository();
                repository.AddOrder(order);
                viewModel.Orders.Add(order);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetComboBoxItems()
        {
            for(int i = 0; i < viewModel.Orders.Count; i++)
            {
                comboBoxOrderID.Items.Add(viewModel.Orders[i].OrderID);
            }
            for(int i = 0; i < viewModel.Orders.Count; i++)
            {
                comboBoxCustomerID.Items.Add(viewModel.Orders[i].CustomerID);
            }
            for(int i = 0; i < 10; i++)
            {
                comboBoxEmployeeID.Items.Add(i);
            }
            for(int i = 200; i < 212; i++)
            {
                comboBoxEmployeeID.Items.Add(i);
            }
            for(int i = 0; i < 4; i++)
            {
                comboBoxShipVia.Items.Add(i);
            }
        }
        private void EditOrderButton_Click(object sender, RoutedEventArgs e)
        {
            AllowEditOrder();
        }

     
    }
}