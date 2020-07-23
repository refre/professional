using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace ista.Utilities
{
    public class FindComissioningFile
    {
        #region Variable
        /// <summary>
        /// Private variable: check whether the device was found.
        /// </summary>
        private const string _baseDirectory = @"\\DEPT\PDA\";
        #endregion
        /// <summary>
        /// Get whether the device was found or not.
        /// </summary>
        public bool IsDeviceFound           { get; private set; }
        /// <summary>
        /// Gets the Article Number.
        /// </summary>
        public int ArticleNumber            { get; private set; }
        /// <summary>
        /// Gets the serial number.
        /// </summary>
        public int SerialNumber             { get; private set; }
        /// <summary>
        /// Gets the year of installation.
        /// </summary>
        public int YearOfInstallation       { get; private set; }
        /// <summary>
        /// Gets the Floor number.
        /// </summary>
        public int Floor                    { get; private set; }
        /// <summary>
        /// Gets the radio network number.
        /// </summary>
        public long RNN                     { get; private set; }
        /// <summary>
        /// Gets the appartement number (could be also a letter).
        /// </summary>
        public string NumAppart             { get; private set; }
        /// <summary>
        /// Gets or set the Type Of Floor.
        /// </summary>
        public string TypeOfFloor           { get; private set; }
        /// <summary>
        /// Gets or sets the full path of the xml file where the device is.
        /// </summary>
        public string FullPath              { get; private set; }
        /// <summary>
        /// Gets the property number.
        /// </summary>
        public string PropertyNumber        { get; private set; }
        /// <summary>
        /// Gets the PDA number
        /// </summary>
        public string PDAInstallationNumBer { get; private set; }
        /// <summary>
        /// Gets the end of billing date.
        /// </summary>
        public DateTime EndOfBillingDate    { get; private set; }
        /// <summary>
        /// Gets the start of counting date.
        /// </summary>
        public DateTime StartOfCountingdate { get; private set; }
        /// <summary>
        /// Gets the installation date.
        /// </summary>
        public DateTime InstallationDate    { get; private set; }
        /// <summary>
        /// Gets 
        /// </summary>
        public bool ResetFlag { get;private set; }
        /// <summary>
        /// Gets the list of data found.
        /// </summary>
        public List<CommissionElement> Elements { get; private set; }
        //public FindComissioningFile(string propertyNumber, int article, int serial, string[] directories)
        //{
        //    SerialNumber   = serial;
        //    ArticleNumber  = article;
        //    PropertyNumber = propertyNumber;
        //    Elements = new List<CommissionElement>();
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error while searching file", ex);
        //    }
        //}
        /// <summary>
        /// Find the comissioning file.
        /// </summary>
        /// <param name="propertyNumber">Property number of where the device is.</param>
        /// <param name="article">Article number of the device.</param>
        /// <param name="serial">Serial number of the device.</param>
        /// <param name="basicDirectory">Basic derictory to search. (Use other constructor to use defaut directory).</param>
        public FindComissioningFile(string propertyNumber, int article, int serial, string basicDirectory)
        {
            PropertyNumber = propertyNumber;
            ArticleNumber  = article;
            SerialNumber   = serial;
            Elements       = new List<CommissionElement>();

            try
            {
                IEnumerable<string> installDirectories = Directory.GetDirectories(basicDirectory);

                foreach (var currentDirectory in installDirectories)
                {
                    DirectoryInfo currentInfo = new DirectoryInfo(currentDirectory);
                    string directoryName      = currentInfo.Name;
                    int directoryVal          = 0;

                    if (!int.TryParse(directoryName, out directoryVal))
                    {
                        continue;
                    }

                    IEnumerable<string> install = Directory.GetDirectories(currentDirectory).Where(s => s.Contains(propertyNumber));
                    foreach (var intribe in install)
                    {
                        string[] installIntribe = Directory.GetDirectories(intribe, "inbetriebnahme", SearchOption.AllDirectories);

                        foreach (var intribefilesDirectory in installIntribe)
                        {
                            int installationYear = 0;
                            string[] dataFromPath = intribefilesDirectory.Split('\\');

                            string year = dataFromPath[5];
                            int.TryParse(year, out installationYear);
                            YearOfInstallation    = installationYear;
                            PDAInstallationNumBer = dataFromPath[7];

                            string[] files = Directory.GetFiles(intribefilesDirectory, "*.xml", SearchOption.AllDirectories);

                            foreach (var file in files)
                            {
                                long rnn = 0;
                                string[] dataFromFile = file.Split('\\');
                                string[] extractDate = dataFromFile[dataFromFile.Length - 1].Split('_');
                                string one = extractDate[0];
                                if (!long.TryParse(extractDate[0], out rnn))
                                {
                                    continue;
                                }
                                
                                if (FindSerial(file, serial, article))
                                { 
                                    //FullPath = intribefilesDirectory;
                                    FullPath = file;
                                    //TO DO tryparse de la date


                                    InstallationDate = new DateTime(int.Parse(extractDate[3]), int.Parse(extractDate[2]), int.Parse(extractDate[1]), int.Parse(extractDate[4]), int.Parse(extractDate[5]), int.Parse(extractDate[6]));

                                    CommissionElement element     = new CommissionElement();
                                    element.ArticleNumber         = article;
                                    element.SerialNumber          = serial;
                                    element.YearOfInstallation    = YearOfInstallation;
                                    element.Floor                 = Floor;
                                    element.NumAppart             = NumAppart;
                                    element.TypeOfFloor           = TypeOfFloor;
                                    element.FullPath              = FullPath;
                                    element.PropertyNumber        = propertyNumber;
                                    element.EndOfBillingDate      = EndOfBillingDate;
                                    element.StartOfCountingdate   = StartOfCountingdate;
                                    element.RNN                   = RNN;                
                                    element.InstallationDate      = InstallationDate;
                                    element.PDAInstallationNumBer = PDAInstallationNumBer;

                                    Elements.Add(element);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} - {1} - {2}",propertyNumber, article, serial);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw new Exception("Error while searching file", ex);
            }
        }
        /// <summary>
        /// Find the comissioning file.
        /// </summary>
        /// <param name="propertyNumber">Property number of where the device is.</param>
        /// <param name="article">Article number of the device.</param>
        /// <param name="serial">Serial number of the device.</param>
        public FindComissioningFile(string propertyNumber, int article, int serial)
            : this(propertyNumber,article,serial,_baseDirectory)
        { }
        /// <summary>
        /// Find the comissioning file.
        /// </summary>
        /// <param name="serial">Serial number of the device.</param>
        /// <param name="basicDirectory">Basic derictory to search. (Use other constructor to use defaut directory).</param>
        public FindComissioningFile(int serial, string basicDirectory)
        {
            DateTime start = DateTime.Now;
            SerialNumber   = serial;
            Elements       = new List<CommissionElement>();
            try
            {
                IEnumerable<string> installDirectories = Directory.GetDirectories(basicDirectory);
                foreach (var currentDirectory in installDirectories)
                {
                    DirectoryInfo currentInfo = new DirectoryInfo(currentDirectory);
                    string directoryName = currentInfo.Name;


                    int directoryVal = 0;
                    if (!int.TryParse(directoryName, out directoryVal))
                    {
                        continue;
                    }

                    //Find whether the folder is a placement folder.. those folders always begin by a number.
                    if (char.IsDigit(directoryName[0]))
                    {
                        IEnumerable<string> install = Directory.GetDirectories(currentDirectory); /*.Where(s => s.Contains(propertyNumber));*/
                        foreach (var intribe in install)
                        {
                            string[] installIntribe = Directory.GetDirectories(intribe, "inbetriebnahme", SearchOption.AllDirectories);

                            foreach (var intribefilesDirectory in installIntribe)
                            {
                                int installationYear = 0;
                                string[] dataFromPath = intribefilesDirectory.Split('\\');

                                string year = dataFromPath[5];
                                int.TryParse(year, out installationYear);
                                YearOfInstallation = installationYear;

                                PDAInstallationNumBer = dataFromPath[7];

                                string[] files = Directory.GetFiles(intribefilesDirectory, "*.xml", SearchOption.AllDirectories);

                                foreach (var file in files)
                                {
                                    long rnn = 0;
                                    string[] dataFromFile = file.Split('\\');
                                    string[] extractDate = dataFromFile[dataFromFile.Length-1].Split('_');
                                    string one = extractDate[0];
                                    if (!long.TryParse(extractDate[0], out rnn))
                                    {
                                        continue;
                                    }
                                    Console.WriteLine(file);
                                    if (!TryLoadXmlFile(file))
                                    {
                                        Console.WriteLine("error in xml file.");
                                        continue;
                                    }

                                    if (FindSerial(file, serial))
                                    {
                                        //FullPath = intribefilesDirectory;
                                        FullPath = file;
                                        InstallationDate = new DateTime(int.Parse(extractDate[3]), int.Parse(extractDate[2]), int.Parse(extractDate[1]), int.Parse(extractDate[4]), int.Parse(extractDate[5]), int.Parse(extractDate[6]));

                                        CommissionElement element     = new CommissionElement();
                                        element.ArticleNumber         = ArticleNumber;
                                        element.SerialNumber          = serial;
                                        element.YearOfInstallation    = YearOfInstallation;
                                        element.Floor                 = Floor;
                                        element.NumAppart             = NumAppart;
                                        element.TypeOfFloor           = TypeOfFloor;
                                        element.FullPath              = FullPath;
                                        element.PropertyNumber        = PropertyNumber;
                                        element.EndOfBillingDate      = EndOfBillingDate;
                                        element.StartOfCountingdate   = StartOfCountingdate;
                                        element.RNN                   = RNN;
                                        element.InstallationDate      = InstallationDate;
                                        element.PDAInstallationNumBer = PDAInstallationNumBer;

                                        Elements.Add(element);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            DateTime fin = DateTime.Now;
            TimeSpan totalTime = fin - start;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serial"></param>
        public FindComissioningFile(int serial)
            : this(serial, _baseDirectory)
        { }
        private bool TryLoadXmlFile(string path)
        {
            try
            {
                XDocument.Load(path);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Error in file: {0}",path);
                return false;
            }
        }
        /// <summary>
        /// Find the serial number of the selected device.
        /// </summary>
        /// <param name="path">Path of the file that place the building.</param>
        /// <param name="serial">Serial number of the device.</param>
        /// <param name="article">Article number of the device.</param>
        /// <returns>True whether the serial has been found</returns>
        private bool FindSerial(string path, int serial,int article)
        {
            bool isSerialfound=false;

            try
            {
                if ((article == 19410) | (article == 19411) | (article == 90000) | (article == 90001))
                {
                    //Water meter
                    // In order to avoid craches, we pass the data in a foreach loop, the we can also threat them (cast,...)
                    var query = from data in XDocument.Load(path).Descendants("WZM")
                                where ((int.Parse(data.Element("W0").Value) == serial) && (int.Parse(data.Element("W1").Value) == article))
                                select data;
                    if (query.Any())
                    {
                        isSerialfound = true;
                        IsDeviceFound = true;
                        foreach (var item in query)
                        {
                            EndOfBillingDate = DateTime.Parse(item.Element("W3").Value.ToString());
                            StartOfCountingdate = new DateTime(1970, 1, 1);
                            RNN = long.Parse(item.Element("W2").Value);
                            Floor = int.Parse(item.Element("W4").Value);
                            TypeOfFloor = item.Element("W5").Value.ToString();
                            NumAppart = item.Element("W6").Value.ToString();
                        }
                    }
                }
                else if ((article == 19450) | (article == 19451) | (article == 90002) | (article == 59121))
                {
                    //Integrator
                    // In order to avoid craches, we pass the data in a foreach loop, the we can also threat them (cast,...)
                    var query = from data in XDocument.Load(path).Descendants("OPM")
                                where ((int.Parse(data.Element("O0").Value) == serial) && (int.Parse(data.Element("O1").Value) == article))
                                select data;
                    if (query.Any())
                    {
                        isSerialfound = true;
                        IsDeviceFound = true;
                        foreach (var item in query)
                        {
                            EndOfBillingDate = DateTime.Parse(item.Element("O3").Value.ToString());
                            StartOfCountingdate = new DateTime(1970, 1, 1);
                            RNN = long.Parse(item.Element("O2").Value);
                            Floor = int.Parse(item.Element("O4").Value);
                            TypeOfFloor = item.Element("O5").Value.ToString();
                            NumAppart = item.Element("O6").Value.ToString();
                        }
                    }
                }
                else if ((article == 19414) | (article == 19415) | (article == 19419) | (article == 90100) | (article == 90200) | (article == 90300) |(article == 19320))
                {
                    //pulsonic
                    // In order to avoid craches, we pass the data in a foreach loop, the we can also threat them (cast,...)
                    var query = from data in XDocument.Load(path).Descendants("Puls")
                                where ((int.Parse(data.Element("P0").Value) == serial) && (int.Parse(data.Element("P1").Value) == article))
                                select data;
                    if (query.Any())
                    {
                        isSerialfound = true;
                        IsDeviceFound = true;
                        foreach (var item in query)
                        {
                            EndOfBillingDate = DateTime.Parse(item.Element("P3").Value.ToString());
                            StartOfCountingdate = new DateTime(1970, 1, 1);
                            RNN = long.Parse(item.Element("P2").Value);
                            Floor = int.Parse(item.Element("P4").Value);
                            TypeOfFloor = item.Element("P5").Value.ToString();
                            NumAppart = item.Element("P6").Value.ToString();
                        }
                    }
                }
                else if ((article == 11070) | (article == 11079) | (article == 11080) | (article == 11089)
                    | (article == 11090) | (article == 11091) | (article == 11098) | (article == 11099) | (article == 11150)
                    | (article == 11151) | (article == 11160) | (article == 11161) | (article == 11190) | (article == 11199) | (article == 11490) | (article == 11499))
                {
                    // In order to avoid craches, we pass the data in a foreach loop, the we can also threat them (cast,...)
                    var query = from data in XDocument.Load(path).Descendants("HKV")
                                where ((int.Parse(data.Element("H0").Value) == serial) && (int.Parse(data.Element("H1").Value) == article))
                                select data;
                    if (query.Any())
                    {
                        isSerialfound = true;
                        IsDeviceFound = true;
                        foreach (var item in query)
                        {
                            EndOfBillingDate = DateTime.Parse(item.Element("H3").Value.ToString());
                            StartOfCountingdate = DateTime.Parse(item.Element("H7").Value.ToString());
                            RNN = long.Parse(item.Element("H2").Value);
                            Floor = int.Parse(item.Element("H4").Value);
                            TypeOfFloor = item.Element("H5").Value.ToString();
                            NumAppart = item.Element("H6").Value.ToString();
                            ResetFlag = item.Element("H13").Value.ToString().Trim() == "1";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error during Xml reading", ex);
            }
            return isSerialfound;
        }
        /// <summary>
        /// Find the serial number of the selected device.
        /// </summary>
        /// <param name="path">Path of the file that place the building.</param>
        /// <param name="serial">serial number.</param>
        /// <returns>true whether the serial has been found.</returns>
        private bool FindSerial(string path, int serial)
        {
            bool isSerialfound = false;
            try
            {
                //Water meter
                // In order to avoid craches, we pass the data in a foreach loop, the we can also threat them (cast,...)
                var queryWater = from data in XDocument.Load(path).Descendants("WZM")
                            where ((int.Parse(data.Element("W0").Value) == serial))
                            select data;
                if (queryWater.Any())
                {
                    isSerialfound = true;
                    IsDeviceFound = true;
                    foreach (var item in queryWater)
                    {
                        EndOfBillingDate = DateTime.Parse(item.Element("W3").Value.ToString());
                        StartOfCountingdate = new DateTime(1970, 1, 1);
                        RNN = long.Parse(item.Element("W2").Value);
                        Floor = int.Parse(item.Element("W4").Value);
                        TypeOfFloor = item.Element("W5").Value.ToString();
                        NumAppart = item.Element("W6").Value.ToString();
                        ArticleNumber = int.Parse(item.Element("W1").Value);
                    }

                    var queryProp = from data in XDocument.Load(path).Descendants("Property")
                                    select data;
                    if (queryProp.Any())
                    {
                        foreach (var item in queryProp)
                        {
                            if (item.Element("p0") != null)
                            {
                                PropertyNumber = item.Element("p0").Value.ToString().Substring(0, 6);
                            }
                            else if (item.Element("P0") != null)
                            {
                                PropertyNumber = item.Element("P0").Value.ToString().Substring(0, 6);
                            }
                        }
                    }
                }
               
                //Integrator
                // In order to avoid craches, we pass the data in a foreach loop, the we can also threat them (cast,...)
                var queryIntegrator = from data in XDocument.Load(path).Descendants("OPM")
                            where ((int.Parse(data.Element("O0").Value) == serial))
                            select data;
                if (queryIntegrator.Any())
                {
                    Console.WriteLine("Found Integrator: " + path);
                    isSerialfound = true;
                    IsDeviceFound = true;
                    foreach (var item in queryIntegrator)
                    {
                        EndOfBillingDate = DateTime.Parse(item.Element("O3").Value.ToString());
                        StartOfCountingdate = new DateTime(1970, 1, 1);
                        RNN = long.Parse(item.Element("O2").Value);
                        Floor = int.Parse(item.Element("O4").Value);
                        TypeOfFloor = item.Element("O5").Value.ToString();
                        NumAppart = item.Element("O6").Value.ToString();
                        ArticleNumber = int.Parse(item.Element("O1").Value);
                    }

                    var queryProp = from data in XDocument.Load(path).Descendants("Property")
                                    select data;
                    if (queryProp.Any())
                    {
                        foreach (var item in queryProp)
                        {
                            if (item.Element("p0") != null)
                            {
                                PropertyNumber = item.Element("p0").Value.ToString().Substring(0, 6);
                            }
                            else if (item.Element("P0") != null)
                            {
                                PropertyNumber = item.Element("P0").Value.ToString().Substring(0, 6);
                            }
                        }
                    }
                }
                
                //pulsonic
                // In order to avoid craches, we pass the data in a foreach loop, the we can also threat them (cast,...)
                var queryPulsonic = from data in XDocument.Load(path).Descendants("Puls")
                            where ((int.Parse(data.Element("P0").Value) == serial))
                            select data;
                if (queryPulsonic.Any())
                {
                    Console.WriteLine("Found Pulsonic: " + path);
                    isSerialfound = true;
                    IsDeviceFound = true;
                    foreach (var item in queryPulsonic)
                    {
                        EndOfBillingDate = DateTime.Parse(item.Element("P3").Value.ToString());
                        StartOfCountingdate = new DateTime(1970, 1, 1);
                        RNN = long.Parse(item.Element("P2").Value);
                        Floor = int.Parse(item.Element("P4").Value);
                        TypeOfFloor = item.Element("P5").Value.ToString();
                        NumAppart = item.Element("P6").Value.ToString();
                        ArticleNumber = int.Parse(item.Element("P1").Value);
                    }
                    var queryProp = from data in XDocument.Load(path).Descendants("Property")
                                    select data;
                    if (queryProp.Any())
                    {
                        foreach (var item in queryProp)
                        {
                            if (item.Element("p0") != null)
                            {
                                PropertyNumber = item.Element("p0").Value.ToString().Substring(0, 6);
                            }
                            else if (item.Element("P0") != null)
                            {
                                PropertyNumber = item.Element("P0").Value.ToString().Substring(0, 6);
                            }
                        }
                    }
                }
                
                // In order to avoid craches, we pass the data in a foreach loop, the we can also threat them (cast,...)
                var queryHCA = from data in XDocument.Load(path).Descendants("HKV")
                            where ((int.Parse(data.Element("H0").Value) == serial))
                            select data;
                if (queryHCA.Any())
                {
                    Console.WriteLine("Found HCA: " + path);
                    isSerialfound = true;
                    IsDeviceFound = true;
                    foreach (var item in queryHCA)
                    {
                        EndOfBillingDate = DateTime.Parse(item.Element("H3").Value.ToString());
                        StartOfCountingdate = DateTime.Parse(item.Element("H7").Value.ToString());
                        RNN = long.Parse(item.Element("H2").Value);
                        Floor = int.Parse(item.Element("H4").Value);
                        TypeOfFloor = item.Element("H5").Value.ToString();
                        NumAppart = item.Element("H6").Value.ToString();
                        ArticleNumber = int.Parse(item.Element("H1").Value);
                    }
                    var queryProp = from data in XDocument.Load(path).Descendants("Property")
                                    select data;
                    if (queryProp.Any())
                    {
                        foreach (var item in queryProp)
                        {
                            if (item.Element("p0") != null)
                            {
                                PropertyNumber = item.Element("p0").Value.ToString().Substring(0, 6);
                            }
                            else if (item.Element("P0") != null)
                            {
                                PropertyNumber = item.Element("P0").Value.ToString().Substring(0, 6);
                            }
                        }
                    }
                }
            }
            
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error during Xml reading", ex);
            }
            return isSerialfound;
        }
    }
}
