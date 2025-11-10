using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21_10_25
{
    public class Waiter
    {
        public int Id { get; set; }
        public List<Order> OpenOrders;
        public List<Order> CloseOrders;

    }
}
