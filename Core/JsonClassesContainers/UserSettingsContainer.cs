using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avrora.Core.JsonClassesContainers
{
    public class UserSettingsContainer
    {
        public string name { get; set; } = "";
        public string nickname { get; set; } = "";
        public string first_key { get; set; } = ""; 
        public string second_key { get; set; } = "";
    }

}
