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

        protected Repository repository;

        protected ObservableCollection<Order> orders;
        public ViewModel()
        {
            repository = new Repository();

            InitializeAsync();

        }

        public virtual async void InitializeAsync()
        {
            // Initialize ObservableCollections
            Orders = new ObservableCollection<Order>(await repository.GetAllOrdersASync());
  
        }

        public ObservableCollection<Order> Orders { get { return orders; } set { orders = value; } }


        public ObservableCollection<OrderDetail> OrderDetails { get; set; }

        public Order SelectedOrder { get; set; }
        public OrderDetail SelectedOrderDetail { get; set; }
    }
}