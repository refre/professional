using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ista.Utilities
{
    public class CommissionElement
    {
        /// <summary>
        /// Gets the Article Number.
        /// </summary>
        public int ArticleNumber { get; set; }
        /// <summary>
        /// Gets the serial number.
        /// </summary>
        public int SerialNumber { get; set; }
        /// <summary>
        /// Gets the year of installation.
        /// </summary>
        public int YearOfInstallation { get; set; }
        /// <summary>
        /// Gets the Floor number.
        /// </summary>
        public int Floor { get; set; }
        /// <summary>
        /// Gets the radio network number.
        /// </summary>
        public long RNN { get; set; }
        /// <summary>
        /// Gets the appartement number (could be also a letter).
        /// </summary>
        public string NumAppart { get; set; }
        /// <summary>
        /// Gets or set the Type Of Floor.
        /// </summary>
        public string TypeOfFloor { get; set; }
        /// <summary>
        /// Gets or sets the full path of the xml file where the device is.
        /// </summary>
        public string FullPath { get; set; }
        /// <summary>
        /// Gets the property number.
        /// </summary>
        public string PropertyNumber { get; set; }
        /// <summary>
        /// Gets the PDA number
        /// </summary>
        public string PDAInstallationNumBer { get; set; }
        /// <summary>
        /// Gets the end of billing date.
        /// </summary>
        public DateTime EndOfBillingDate { get; set; }
        /// <summary>
        /// Gets the start of counting date.
        /// </summary>
        public DateTime StartOfCountingdate { get; set; }
        /// <summary>
        /// Gets the installation date.
        /// </summary>
        public DateTime InstallationDate { get; set; }
    }
}
