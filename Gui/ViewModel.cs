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
        protected Order selectedOrder;
        public ViewModel()
        {
            repository = new Repository();

            InitializeAsync();

        }

        public virtual async void InitializeAsync()
        {
            // Initialize ObservableCollections
            Orders = new ObservableCollection<Order>(await repository.GetAllOrdersAsync());

        }

        public virtual ObservableCollection<Order> Orders
        {
            get
            {
                return orders;
            }
            set
            {
                if(orders != value)
                {
                    orders = value;
                }
            }
        }

        public virtual ObservableCollection<OrderDetail> OrderDetails { get; set; }

        public virtual Order SelectedOrder { get; set; }
        public virtual OrderDetail SelectedOrderDetail { get; set; }
    }
}