using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunCalculatorTester
{
    class Program
    {
        static void Main(string[] args)
        {
            for(int i = 0; i < 365; i++)
            {
                var date = (new DateTime(2016, 1, 1, 15, 0, 0)).AddDays(i);
                var position = SunCalculator.CalculateSunPosition(date, 41.3275, 19.8187);
                Console.WriteLine("Date: {0} - Altitude: {1} - Azimuth: {2}", date.ToString("yyyy-MM-dd hh:mm"), Math.Round(position.Altitude, 2), Math.Round(position.Azimuth, 2));
            }

            Console.Read();
        }
    }
}
