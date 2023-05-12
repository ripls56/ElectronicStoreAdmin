using System;
using System.Collections.Generic;

namespace ElectronicStoreAdmin.Models
{
    public partial class ClientPromocode
    {
        public int? IdClientPromocode { get; set; }
        public int? PromocodeId { get; set; }
        public int? ClientId { get; set; }
    }
}
