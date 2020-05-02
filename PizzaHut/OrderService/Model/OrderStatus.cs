using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Model
{
    public enum OrderStatus
    {
        AwaitingConfirmation,
        Paid,
        OnWay,
        Cancelled
    }
}
