using System;
using System.Collections.Generic;

#nullable disable

namespace Publisher.Entities
{
    public partial class UserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Eamil { get; set; }
        public string UserCode { get; set; }
    }
}
