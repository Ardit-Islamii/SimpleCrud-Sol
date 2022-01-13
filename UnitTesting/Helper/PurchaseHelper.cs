using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace UnitTesting.Helper
{
    public static class PurchaseHelper
    {

        public static Purchase PurchaseData()
        {
            Purchase purchase = new Purchase()
            {
                Id = Guid.NewGuid(),
                ItemId = Guid.NewGuid(),
                Amount = 5
            };
            return purchase;
        }
    }
}
