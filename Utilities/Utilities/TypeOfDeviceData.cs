using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using System.IO;

namespace ista.Utilities
{
    /// <summary>
    /// Enum that allow the selection of devices.
    /// </summary>
    public enum TypeOfDevice
    {
        Doprimo,
        Water,
        Pulsonic,
        Optosonic,
        Fumonic,
        PowerSonic,
        Repeater,
        Unknow
    }

    /// <summary>
    /// This class that manages the type of data.
    /// </summary>
    public /*static*/ class TypeOfDeviceData
    {
        private Stream _stream;
        public TypeOfDeviceData()
        {
            string fileName = "DeviceType.xml";
            var assembly = Assembly.GetExecutingAssembly();
            _stream = assembly.GetManifestResourceStream(this.GetType(), fileName);
            try
            {
                if (_stream == null)
                {
                    throw new FileNotFoundException("Couldnot find embedded mappings resource file.", fileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Find type of device by an article number. Be careful!! this method is slow because it opens the xml file!
        /// </summary>
        /// <param name="articleNumber">Article number</param>
        /// <returns>Type of device.</returns>
        public TypeOfDevice CurentDevice(int articleNumber)
        {
            TypeOfDevice currentDevice = TypeOfDevice.Unknow;
            try
            {
                XDocument devices = XDocument.Load(_stream);
                var query = from top in devices.Descendants("Device")
                            from art in top.Elements("Article")
                            select new
                            {
                                Name = top.Attribute("name").Value.ToString(),
                                Article = int.Parse(art.Value)
                            };

                string deviceName = query.Single(x => x.Article == articleNumber).Name;
                switch (deviceName)
                {
                    case "Doprimo":
                        currentDevice = TypeOfDevice.Doprimo;
                        break;
                    case "Water":
                        currentDevice = TypeOfDevice.Water;
                        break;
                    case "Pulsonic":
                        currentDevice = TypeOfDevice.Pulsonic;
                        break;
                    case "Optosonic":
                        currentDevice = TypeOfDevice.Optosonic;
                        break;
                    case "PowerSonic":
                        currentDevice = TypeOfDevice.PowerSonic;
                        break;
                    case "Repeater":
                        currentDevice = TypeOfDevice.Repeater;
                        break;
                    default:
                        currentDevice = TypeOfDevice.Unknow;
                        break;
                }
            }
            catch (Exception ex)
            {


            }

            return currentDevice;
        }
        /// <summary>
        /// Create a dictionnary with the article number and the type of device.
        /// </summary>
        /// <returns>Dictionnary with the type of device.</returns>
        public /*static*/ Dictionary<int, TypeOfDevice> DeviceDictionnay()
        {
            Dictionary<int, TypeOfDevice> deviceDectionnary = new Dictionary<int, TypeOfDevice>();
            TypeOfDevice currentDevice = TypeOfDevice.Unknow;

            XDocument devices = XDocument.Load(_stream);
            var query = from top in devices.Descendants("Device")
                        from art in top.Elements("Article")
                        select new
                        {
                            Name = top.Attribute("name").Value.ToString(),
                            Article = int.Parse(art.Value)
                        };

            foreach (var item in query)
            {
                switch (item.Name)
                {
                    case "Doprimo":
                        currentDevice = TypeOfDevice.Doprimo;
                        break;
                    case "Water":
                        currentDevice = TypeOfDevice.Water;
                        break;
                    case "Pulsonic":
                        currentDevice = TypeOfDevice.Pulsonic;
                        break;
                    case "Optosonic":
                        currentDevice = TypeOfDevice.Optosonic;
                        break;
                    case "PowerSonic":
                        currentDevice = TypeOfDevice.PowerSonic;
                        break;
                    case "Repeater":
                        currentDevice = TypeOfDevice.Repeater;
                        break;
                    default:
                        currentDevice = TypeOfDevice.Unknow;
                        break;
                }
                deviceDectionnary.Add(item.Article, currentDevice);
            }
            return deviceDectionnary;
        }
    }
}
