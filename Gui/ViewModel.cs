using DataAccess;
using Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Gui
{
   public class ViewModel
    {

        public ViewModel()
        {
            Repository repository = new Repository();

            List<Order> orders = repository.GetAllOrders();
            List<OrderDetail> orderDetails = repository.GetAllOrderDetails();

            Orders = new ObservableCollection<Order>(orders);
            OrderDetails = new ObservableCollection<OrderDetail>(orderDetails);

        }

        public ObservableCollection<Order> Orders { get; set; }
        public ObservableCollection<OrderDetail> OrderDetails { get; set; }

        public Order SelectedOrder { get; set; }
        public OrderDetail SelectedOrderDetail { get; set; }
    }
}