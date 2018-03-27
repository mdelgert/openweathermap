using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap
{
    class Program
    {
        static void Main(string[] args)
        {

            var wapi = new WeatherApi();
            var wm = wapi.GetModel();
            var request = wapi.GetRequestUrl(wm);
            var response = wapi.Check(request);

            wapi.LogMsgToFile("OpenWeatherMap Begin:");
            wapi.LogMsgToFile(response);
            wapi.Run(wm);
            wapi.LogMsgToFile("OpenWeatherMap End:");
            Console.ReadKey();

        }
    }
}
