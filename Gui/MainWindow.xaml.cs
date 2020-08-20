using DataAccess;

using Entities;

using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using System.Windows;


namespace Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window
    {
     
        private readonly ViewModel viewModel;
        private readonly Repository repository;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new ViewModel();
            DataContext = viewModel;

            repository = new Repository();
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


        private async void NewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();

            try
            {
                int.TryParse(comboBoxEmployeeID.Text, out int employeeID);
                int.TryParse(comboBoxShipVia.Text, out int shipVia);
                decimal.TryParse(textBoxFreight.Text, out decimal freight);
                Order order = await Task.Run(() => new Order(1, comboBoxCustomerID.Text, employeeID, datePickerOrderDate.SelectedDate.Value.Date,
                    datePickerOrderDate.SelectedDate.Value.Date, datePickerOrderDate.SelectedDate.Value.Date, shipVia, freight,
                    textBoxShipName.Text, textBoxShipAddress.Text, textBoxShipCity.Text, textBoxShipRegion.Text, textBoxShipPostalCode.Text,
                    textBoxShipCountry.Text, orderDetails));

                Repository repository = new Repository();
                await Task.Run(() => repository.AddOrder(order));
                await Task.Run(() => viewModel.Orders.Add(order));
            }
            catch(Exception ex)
            {
                await Task.Run(() => MessageBox.Show(ex.Message));
            }
        }

        private async Task GetComboBoxItemsASync()
        {
            for(int i = 0; i < viewModel.Orders.Count; i++)
            {
                await Task.Run(() => comboBoxOrderID.Items.Add(viewModel.Orders[i].OrderID));
            }
            for(int i = 0; i < viewModel.Orders.Count; i++)
            {
                await Task.Run(() => comboBoxCustomerID.Items.Add(viewModel.Orders[i].CustomerID));
            }
            for(int i = 0; i < 10; i++)
            {
                await Task.Run(() => comboBoxEmployeeID.Items.Add(i));
            }
            for(int i = 200; i < 212; i++)
            {
                await Task.Run(() => comboBoxEmployeeID.Items.Add(i));
            }
            for(int i = 0; i < 4; i++)
            {
                await Task.Run(() => comboBoxShipVia.Items.Add(i));
            }
        }
        private async void EditOrderButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => AllowEditOrder());
        }


    }
}