using System;

namespace XamFormsTestApp.Data.Model
{
    public class OrderDetails
    {
        public string OrdId { get; set; }
        public string OrdName { get; set; }
        public string Status { get; set; }
        public DateTime LastUpdated { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Cargo { get; set; }
        public Single Speed { get; set; }
    }
}
