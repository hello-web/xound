using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunCalculatorTester
{
    public class AeronauticalPosition
    {
        public double Altitude { get; set; }
        public double Azimuth { get; set; }

        public AeronauticalPosition(double altitude, double azimuth)
        {
            this.Altitude = altitude;
            this.Azimuth = azimuth;
        }
    }
}
