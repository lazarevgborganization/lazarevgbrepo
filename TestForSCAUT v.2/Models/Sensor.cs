using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForSCAUT_v_2.Models
{
    /// <summary>
    /// Сенсоры, принадлежащие терминалу
    /// </summary>
    public class Sensors
    {
        public Sensors(string type, string value)
        {
            Type = type;
            Value = value;
        }

        public string Type { get; set; }
        public string Value { get; set; }
    }
}
