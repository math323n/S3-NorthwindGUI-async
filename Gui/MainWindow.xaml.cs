using DataAccess;
using Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window
    {
        #region fields
        protected ViewModel viewModel;
        protected Repository repository;
        #endregion

        #region Constructor for MainWindow
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Allow to Edit properties inside of window
        /// </summary>
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
        /// <summary>
        /// Disallow to Edit properties inside of window
        /// </summary>
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

        /// <summary>
        /// Get comboBox items async
        /// </summary>
        /// <returns></returns>
        private async Task GetComboBoxItemsASync()
        {
            Dispatcher.Invoke(new Action(async delegate
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
            }), DispatcherPriority.Normal);
        }
        #endregion

        #region EventHandlers
        /// <summary>
        /// Button EventHandler for adding a new item to DataBase
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Button EventHandler for editing an existing item in DataBase
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void EditOrderButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => AllowEditOrder());
        }

        /// <summary>
        /// EventHandler for when Window is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            // viewmodel
            viewModel = new ViewModel();

            // Assign the viewModel to DataContext
            DataContext = viewModel;

            // Start repository
            repository = new Repository();
           await repository.InitRepository();

            /* ViewModel method for getting data, will link to GetAllOrdersAsync, 
             * then retrieves all orders async all the way.*/
            await viewModel.InitializeAsync();

            // Disable all textbox' for editing
            DisAllowEditing();
        }
        #endregion
    }
}