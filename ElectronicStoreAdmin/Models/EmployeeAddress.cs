using System;
using System.Collections.Generic;

namespace ElectronicStoreAdmin.Models
{
    public partial class EmployeeAddress
    {
        public int? IdEmployeeAddresses { get; set; }
        public int? EmployeeId { get; set; }
        public int? StoreAddressesId { get; set; }
    }
}
