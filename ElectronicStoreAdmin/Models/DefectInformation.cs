using System;
using System.Collections.Generic;

namespace ElectronicStoreAdmin.Models
{
    public partial class DefectInformation
    {
        public int? IdDefectInformation { get; set; }
        public string? DescriptionDefectInformation { get; set; } = null!;
        public int? OrderId { get; set; }
    }
}
