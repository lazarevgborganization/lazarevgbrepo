using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForSCAUT_v_2.Models
{
    internal class TerminalDataSource
    {
        private static ObservableCollection<Terminal> readTerminal = new ObservableCollection<Terminal>();

        /// <summary>
        /// Позволяет пользователю выбрать файл расширения .xml
        /// </summary>
        /// <returns></returns>
        public string Load()
        {
            OpenFileDialog ofDialog = new OpenFileDialog();
            ofDialog.Filter = "XmlDocument|.*xml";
            ofDialog.Multiselect = true;
            ofDialog.CheckFileExists = true;

            Nullable<bool> result = ofDialog.ShowDialog();

            if(result == true)
            {
                return ofDialog.FileName;
             }
            return null;        
        }

        /// <summary>
        /// Читает файл, имя которого передается параметром, и записывает в коллекцию протокол, серийный номер
        /// для каждого терминала, а также тип и значение для каждого сенсора в терминале.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ObservableCollection<Terminal> ReadFile(string fileName)
        {
            try
            {
                var document = System.Xml.Linq.XDocument.Load(fileName);

                var terminals = from term in document.Descendants("terminal")
                                select new
                                {
                                    protocol = (string)term.Attribute("protocol") ?? String.Empty,
                                    serialId = (string)term.Attribute("serialId") ?? String.Empty
                                };

                int amountOfSensors = document.Descendants("sensor").Count();

                var sensors = from sens in document.Descendants("sensor")
                              select new
                              {
                                  type = (string)sens.Attribute("type") ?? String.Empty,
                                  value = (string)sens.Attribute("value") ?? String.Empty
                              };
                
                foreach (var t in terminals)
                {
                        readTerminal.Add(new Terminal(t.protocol, t.serialId, amountOfSensors));
                }
     //TODO: заполнение сенсоров

                return readTerminal;
            }
            catch(ArgumentNullException ex)
            {
                ex = new ArgumentNullException(fileName, "DataSource file could be not found!");
                throw ex;
            }
        }
    }
}
