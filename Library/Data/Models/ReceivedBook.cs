using System;

namespace Library.Data.Models {
    public class ReceivedBook {
        public Book     Book          { get; set; }
        public User     User          { get; set; }
        public DateTime ReceivingDate { get; set; }
        public DateTime DeliveryDate  { get; set; }
    }
}