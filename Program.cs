using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avrora
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            App app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
