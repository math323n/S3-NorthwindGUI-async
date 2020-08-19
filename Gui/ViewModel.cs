using DataAccess;

using Entities;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui
{
    public class ViewModel
    {
        Repository repository = new Repository();

        public async void ViewModelObj()
        {


            List<Order> orders = await Task.Run(() => repository.GetAllOrdersASync());
            List<OrderDetail> orderDetails = repository.GetAllOrderDetails();


            OrderDetails = new ObservableCollection<OrderDetail>(orderDetails);
            Orders = new ObservableCollection<Order>(orders);

        }

        public ObservableCollection<Order> Orders { get; set; }


        public ObservableCollection<OrderDetail> OrderDetails { get; set; }

        public Order SelectedOrder { get; set; }
        public OrderDetail SelectedOrderDetail { get; set; }
    }
}