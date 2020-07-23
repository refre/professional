using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ista.Utilities
{
    public static class YesNoToBoolExtension
    {
        public static bool YesNotoBool(this string source)
        {
            if (source.ToUpper() == "Y" || source.ToUpper() == "Yes")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool YesNotoBool(this char source)
        {
            if (char.ToUpper(source) == 'Y')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
