﻿using DataAccess;
using Entities;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Gui
{
    public class ViewModel: INotifyPropertyChanged
    {
        #region fields
        protected Repository<NorthwindContext> repository;
        protected ObservableCollection<Orders> orders;
        protected Order selectedOrder;
        protected OrderDetail selectedOrderDetail;
        #endregion

        #region Constructor for ViewModel
        public ViewModel()
        {
            
        }
        #endregion

        #region Properties
        public virtual ObservableCollection<Orders> Orders
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
        #endregion

        #region Methods
        public virtual async Task InitializeAsync()
        {

            List<Orders> orders = (List<Orders>)repository.GetAll();
            // Initialize ObservableCollections
            Orders = new ObservableCollection<Orders>(orders);
        }

        // Represents the method that will handle the System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        // event raised when a property is changed on a component.      
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Report the changes happening to class properties
        /// Only need implementation on ViewModel
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));   
        }
        #endregion
    }
}