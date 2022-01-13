using System;

namespace Models
{
    public class Purchase
    {
        /// <summary>
        /// The id of the purchase
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// The itemId that was purchased
        /// </summary>
        public Guid ItemId { get; set; }
        /// <summary>
        /// The item details
        /// </summary>
        public Item Item { get; set; }
        /// <summary>
        /// The amount of items that was purchased
        /// </summary>
        public int Amount { get; set; }
    }
}
