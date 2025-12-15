using System;
using System.Collections.Generic;
using System.Text;

namespace StonksAppWithLogs.Core.DTO
{
    public class Orders
    {
        public List<BuyOrderResponse> BuyOrders { get; set; } = new List<BuyOrderResponse>();
        public List<SellOrderResponse> SellOrders { get; set; } = new List<SellOrderResponse>();

    }
}
