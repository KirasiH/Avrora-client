using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avrora.Core.JsonClassesContainers
{
    class UserSettingsJSONContainer
    {
        public Dictionary<string, UserSettingsContainer> Settings { get; set; }
    }
}
