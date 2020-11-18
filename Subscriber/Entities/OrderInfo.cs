using System;
using System.Collections.Generic;

#nullable disable

namespace Subscriber.Entities
{
    public partial class OrderInfo
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string UserCode { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
