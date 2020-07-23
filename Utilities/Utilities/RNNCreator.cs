using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ista.Utilities
{
    /// <summary>
    /// This class is used in order to create an RNN for a given property.
    /// </summary>
    public class RNNCreator
    {
        #region Property
        /// <summary>
        /// Gets the value of the check digit.
        /// </summary>
        public int CheckDigit { get; private set; }
        /// <summary>
        /// Get the value of the property number .
        /// </summary>
        public string PropertyNumber { get; private set; }
        #endregion Property

        /// <summary>
        /// This constructor is the base of the creation of an RNN.
        /// The two type is available, the old RNN calculation: Groupe immeuble + 0000
        /// And the new version: 0000+Groupe Immeuble.
        /// </summary>
        /// <param name="propertyNumber">Groupe Immeuble</param>
        public RNNCreator(string propertyNumber)
        {
            PropertyNumber = propertyNumber;
        }
        /// <summary>
        /// This private method calcultates the check digit value.
        /// </summary>
        /// <returns>Check digit value.</returns>
        private int CheckDigitCal(string RNNData)
        {
            if (RNNData.Length != 10)
            {
                throw new Exception("Property number must be composed by 10 value.");
            }
            char[] propertyData = RNNData.ToCharArray();
            int x10 = int.Parse(propertyData[0].ToString());
            int x9 = int.Parse(propertyData[1].ToString());
            int x8 = int.Parse(propertyData[2].ToString());
            int x7 = int.Parse(propertyData[3].ToString());
            int x6 = int.Parse(propertyData[4].ToString());
            int x5 = int.Parse(propertyData[5].ToString());
            int x4 = int.Parse(propertyData[6].ToString());
            int x3 = int.Parse(propertyData[7].ToString());
            int x2 = int.Parse(propertyData[8].ToString());
            int x1 = int.Parse(propertyData[9].ToString());

            int smalltotal = 4*x10+3*x9+2*x8+7*x7+6*x6+5*x5+4*x4+3*x3+2*x2+x1;
            int secondTotal = smalltotal % 10;
            int total = 9 - secondTotal;

            return total;
        }
        /// <summary>
        /// This method calculate the RNN number via the old way (Groupe immeuble+0000).
        /// </summary>
        /// <returns>String that represent the RNN.</returns>
        public string RNNOld()
        {
            int checkDigit = CheckDigitCal(PropertyNumber + "0000");
            CheckDigit = checkDigit;
            string RNN = string.Concat(PropertyNumber, "0000", checkDigit);
            return RNN;
        }
        /// <summary>
        /// This method calculate the RNN number via the old way (Groupe immeuble+0000).
        /// </summary>
        /// <returns>Long number that represent the RNN.</returns>
        public long OldRnnInNumber()
        {
            return long.Parse(RNNOld());
        }
        /// <summary>
        /// This method calculate the RNN number via the new way for any new PDA buidling (0000+Groupe immeuble).
        /// </summary>
        /// <returns>String that represent the RNN for new buildings.</returns>
        public string NewRnnPDAString()
        {
            int checkDigit = CheckDigitCal("0000" + PropertyNumber);
            CheckDigit = checkDigit;
            string RNN = string.Concat("0000", PropertyNumber, checkDigit);
            return RNN;
        }
        /// <summary>
        /// This method calculate the RNN number via the new way for any new PDA buidling (0000+Groupe immeuble).
        /// </summary>
        /// <returns>Long that represent the RNN for new buildings.</returns>
        public long NewRnnPDANumber()
        {
            return long.Parse(NewRnnPDAString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> NewRnnAMMstring()
        {
            //List<string> ammList = new List<string>();
            //int total = 33;
            //int last = 0;
            //do
            //{
            //    int checkDigit = CheckDigitCal("00" + PropertyNumber + last.ToString("00"));
            //    string RNN = string.Concat("00", PropertyNumber, last.ToString("00"), checkDigit);
            //    ammList.Add(RNN);
            //    last++;
            //} while (last < total);

            //return ammList;
            return NewRnnAMMstring(32);
        }
        public List<string> NewRnnAMMstring(short totalSGW)
        {
            List<string> ammList = new List<string>();
            int total = totalSGW;
            int last = 0;
            do
            {
                int checkDigit = CheckDigitCal("00" + PropertyNumber + last.ToString("00"));
                string RNN = string.Concat("00", PropertyNumber, last.ToString("00"), checkDigit);
                ammList.Add(RNN);
                last++;
            } while (last < total);

            return ammList;
        }
        public List<long> NewRnnAMMNumber()
        {
            List<long> ammList = new List<long>();
            foreach (var item in NewRnnAMMstring())
            {
                ammList.Add(long.Parse(item));
            }
            return ammList;
        }
        public List<long> NewRnnAMMNumber(short totalSGW)
        {
            List<long> ammList = new List<long>();
            foreach (var item in NewRnnAMMstring(totalSGW))
            {
                ammList.Add(long.Parse(item));
            }
            return ammList;
        }
        /// <summary>
        /// This method calculate the RNN number via the new way (0000+Groupe immeuble).
        /// </summary>
        /// <returns>String that represent the RNN.</returns>
        //public string RNNAMM()
        //{
        //    int checkDigit = CheckDigitCal("0000"+PropertyNumber);
        //    CheckDigit = checkDigit;
        //    string RNN = string.Concat("0000",PropertyNumber , checkDigit);
        //    return RNN;
        //}
        public string CalculateRNN(string rNNComputed)
        {
            int checkDigit = CheckDigitCal(rNNComputed);
            string RNN = string.Concat(rNNComputed, checkDigit);
            return RNN;
        }

    }
}
