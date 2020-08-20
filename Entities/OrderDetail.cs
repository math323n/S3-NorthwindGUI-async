using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace Entities
{
    public class OrderDetail
    {
        #region Fields
        protected int orderID;
        protected int productID;
        protected decimal unitPrice;
        protected short quantity;
        protected float discount;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for OrderDetail
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="productID"></param>
        /// <param name="unitPrice"></param>
        /// <param name="quantity"></param>
        /// <param name="discount"></param>
        public OrderDetail(int orderID, int productID, decimal unitPrice, short quantity, float discount)
        {
            OrderID = orderID;
            ProductID = productID;
            UnitPrice = unitPrice;
            Quantity = quantity;
            Discount = discount;
        }
        #endregion

        #region
        public virtual int OrderID
        {
            get
            {
                return orderID;
            }
            set
            {
                orderID = value;
            }
        }
        public virtual int ProductID
        {
            get
            {
                return productID;
            }

            set
            {
                if(value != productID)
                {
                    productID = value;
                }
            }
        }

        public virtual decimal UnitPrice
        {
            get
            {
                return unitPrice;
            }
            set
            {
                unitPrice = value;
            }
        }

        public virtual short Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
               
                if(value != quantity)
                {
                    quantity = value;
                }
            }
        }

        public virtual float Discount
        {
            get
            {
                return discount;
            }
            set
            {
                discount = value;
            }
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"ID: {orderID}\nUnitprice: {unitPrice}\nQuantity: {quantity}\nDiscount: {discount}";
        }
        #endregion
    }
}