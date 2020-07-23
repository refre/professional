using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ista.Utilities
{
    public class PropertyDeviceExtended:PropertyDevice
    {
        public string UniqueID  { get; set; }
        public bool IsInListOne { get; set; }
        public bool IsInListTwo { get; set; }
    }
}
