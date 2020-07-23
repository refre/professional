using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ista.Utilities
{
    /// <summary>
    /// This class makes the comparaison between 2 property data
    /// </summary>
    public class PropertyDeviceComparer
    {
        /// <summary>
        /// Gets the PropertyDeviceCompared list.
        /// </summary>
        public List<PropertyDeviceExtended> PropertyDeviceCompared { get; private set; }
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public PropertyDeviceComparer(List<PropertyDevice> first,List<PropertyDevice> second)
        {
            List<PropertyDeviceExtended> firstExtended  = GenerateExtended(first);
            List<PropertyDeviceExtended> secondExtended = GenerateExtended(second);

            var query = firstExtended.Select(x => x.UniqueID).FullOuterJoin(secondExtended.Select(ye => ye.UniqueID), x => x, x => x, (x1, x2) => (x1 ?? "False") + (x2 ?? "False"));
            List<PropertyDeviceExtended> lastExtendedList = new List<PropertyDeviceExtended>();
            PropertyDeviceExtended lastExtended;
            foreach (var item in query)
            {
                lastExtended         = new PropertyDeviceExtended();

                char[] sep   = {'-'};
                string[] result = item.Split(sep, StringSplitOptions.RemoveEmptyEntries);

                string firstId  = result[0];
                string secondId = result[1];

                bool isFirstPresent  = result[0] != "False";
                bool isSecondPresent = result[1] != "False";
               
                if (isFirstPresent && isSecondPresent)
                {
                    lastExtended = firstExtended.Where(x => x.UniqueID.Split(sep, StringSplitOptions.RemoveEmptyEntries)[0] == firstId).First();
                    lastExtended.IsInListOne = true;
                    lastExtended.IsInListTwo = true;
                    lastExtendedList.Add(lastExtended);
                }
                else if (isFirstPresent)
                {
                    var test = firstExtended.Select(x => x.UniqueID).ToList();

                    lastExtended = firstExtended.Where(x => x.UniqueID.Split(sep, StringSplitOptions.RemoveEmptyEntries)[0] == firstId).First();
                    lastExtended.IsInListOne = true;
                    lastExtended.IsInListTwo = false;
                    lastExtendedList.Add(lastExtended);
                }
                else if (isSecondPresent)
                {
                    lastExtended = secondExtended.Where(x => x.UniqueID.Split(sep, StringSplitOptions.RemoveEmptyEntries)[0] == secondId).First();
                    lastExtended.IsInListOne = false;
                    lastExtended.IsInListTwo = true;
                    lastExtendedList.Add(lastExtended);
                }
                else
                {
                    throw new InvalidOperationException("Impossible case");
                }
                PropertyDeviceCompared = lastExtendedList;
            }
        }
        /// <summary>
        /// Generates the extended value for the property list.
        /// </summary>
        /// <param name="propdevList">Current property</param>
        /// <returns>Extended list of property.</returns>
        private List<PropertyDeviceExtended> GenerateExtended(List<PropertyDevice> propdevList)
        {
            PropertyDeviceExtended propdExt;
            List<PropertyDeviceExtended> propExtended = new List<PropertyDeviceExtended>();

            foreach (PropertyDevice propDev in propdevList)
            {
                propdExt = new PropertyDeviceExtended();
                string uniqueID = string.Concat("-",((int)propDev.Article).ToString("00"), propDev.Property, propDev.SerialNum.ToString("000000000"),"-");
                propdExt.Property = propDev.Property;
                propdExt.ArticleNum = propDev.ArticleNum;
                propdExt.SerialNum = propDev.SerialNum;
                propdExt.UniqueID = uniqueID;

                propExtended.Add(propdExt);
            }
            return propExtended;
        }
    }
}
