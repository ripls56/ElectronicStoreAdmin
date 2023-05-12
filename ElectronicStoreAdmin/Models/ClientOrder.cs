using System;
using System.Collections.Generic;

namespace ElectronicStoreAdmin.Models
{
    public partial class ClientOrder
    {
        public string? НомерЗаказа { get; set; } = null!;
        public string? ЛогинКлиента { get; set; } = null!;
        public string? Товар { get; set; } = null!;
    }
}
