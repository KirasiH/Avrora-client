﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avrora.Core.JsonClassesContainers
{
    public class ApplicationSettingsContainer
    {
        public string? actualURIServer { get; set; }

        public List<string>? listServer{ get; set; }
    }
}
