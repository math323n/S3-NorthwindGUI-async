using DataAccess;

using Entities;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Gui
{
    public class ViewModel : INotifyPropertyChanged
    {

        protected Repository repository;

        protected ObservableCollection<Order> orders;
        protected Order selectedOrder;
        protected OrderDetail selectedOrderDetail;

        public ViewModel()
        {
            repository = new Repository();

            InitializeAsync();

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

                    NotifyPropertyChanged();
                }
            }
        }

        public virtual ObservableCollection<OrderDetail> OrderDetails { get; set; }

        public virtual Order SelectedOrder
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


        public virtual async void InitializeAsync()
        {
            // Initialize ObservableCollections
            Orders = new ObservableCollection<Order>(await repository.GetAllOrdersAsync());

        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    #endregion

}
