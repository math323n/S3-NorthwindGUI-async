using Entities;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class OrderRepository: Repository<Orders>
    {
        public OrderRepository(NorthwindContext context) : base(context)
        {

        }
        public override IEnumerable<Orders> GetAll()
        {
            IEnumerable<Orders> orders = context.Orders.Include("OrderDetails");
            return orders;
        }
    }
}
