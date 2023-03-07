using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avrora.Core;

namespace Avrora
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            //Core.Core.Start();

            App app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
