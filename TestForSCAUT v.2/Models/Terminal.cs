using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForSCAUT_v_2.Models
{
    public class Terminal
    {
        public Terminal(string protocol, string serialID, int amountOfSensors)
        {
            Protocol = protocol;
            SerialID = serialID;
            AmountOfSensors = amountOfSensors;
        }
        public string Protocol { get; set; }
        public string SerialID { get; set; }
        public int AmountOfSensors { get; set; }
        public ObservableCollection <Sensors> Sensor { get; set; }    //TODO

    }
}
