using DataAccess;

using Entities;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Gui
{
    public class ViewModel
    {


        #region Constructor for ViewModel
        public ViewModel()
        {
            NorthwindContext context = new NorthwindContext();
            OrderRepository repository = new OrderRepository(context);
            IEnumerable<Orders> orders = repository.GetAll();
            Orders = new ObservableCollection<Orders>(orders);
           
        }
        #endregion

        #region Properties
        public virtual ObservableCollection<Orders> Orders
        {
            get; set;

        }

        public virtual ObservableCollection<OrderDetails> OrderDetails { get; set; }

        public virtual Orders SelectedOrder
        {
            get; set;
        }
        public ICollection<OrderDetails> SelectedOrderDetail { get; set; }
 
    }
}
#endregion
