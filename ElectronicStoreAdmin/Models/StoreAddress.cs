﻿using System;
using System.Collections.Generic;

namespace ElectronicStoreAdmin.Models
{
    public partial class StoreAddress
    {

        public int? IdStoreAddresses { get; set; }
        public string? StoreAddres { get; set; } = null!;
        public string? Postcode { get; set; } = null!;

    }
}
