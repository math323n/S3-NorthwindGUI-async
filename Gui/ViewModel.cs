using DataAccess;

using Entities;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Gui
{
    public class ViewModel: INotifyPropertyChanged
    {
        protected Repository<Orders> orderRepository;
        protected ObservableCollection<Orders> orders;
        protected ObservableCollection<OrderDetails> orderDetails;
        protected Orders selectedOrder;
        protected OrderDetail selectedOrderDetail;

        protected NorthwindContext context;


        #region Constructor for ViewModel
        public ViewModel()
        {


        }
        #endregion


        public virtual async Task InitializeAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    RepositoryFactory<OrderRepository, Orders> factory = RepositoryFactory<OrderRepository, Orders>.GetInstance();
                    OrderRepository repo = factory.Create();

                    IEnumerable<Orders> orders = repo.GetAll();





                    // Initialize ObservableCollections
                    Orders = new ObservableCollection<Orders>(orders);
                    //OrderDetails = new ObservableCollection<OrderDetails>(orderDetails);

                });
            }
            catch(Exception)
            {
                throw;
            }
        }

        #region Properties
        public ObservableCollection<Orders> Orders
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

                    NotifyPropertyChanged();
                }
            }
        }

        public ObservableCollection<OrderDetails> OrderDetails
        {
            get
            {
                return orderDetails;
            }
            set
            {
                if(orderDetails != value)
                {
                    orderDetails = value;

                    NotifyPropertyChanged();
                }
            }

        }



        public virtual Orders SelectedOrder
        {
            get
            {
                return selectedOrder;
            }
            set
            {
                if(selectedOrder != value)
                {
                    selectedOrder = value;

                    NotifyPropertyChanged();
                }
            }
        }
        public ICollection<OrderDetails> SelectedOrderDetails { get; set; }
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
#endregion
