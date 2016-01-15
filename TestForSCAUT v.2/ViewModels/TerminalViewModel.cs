using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForSCAUT_v_2.ViewModels
{
    public class TerminalViewModel : NotifyPropertyChanged
    {
        private Models.Terminal _terminal;

        public TerminalViewModel(Models.Terminal terminal)
        {
            _terminal = terminal;
        }
        
        public string Protocol
        {
            get { return _terminal.Protocol; }
        }

        public string SerialID
        {
            get { return _terminal.SerialID; }
        }

        public int AmountOfSensors
        {
            get { return _terminal.AmountOfSensors; }
        }

        public ObservableCollection<Models.Sensors> Sensor
        {
            get { return _terminal.Sensor; }
        }
    }
}
