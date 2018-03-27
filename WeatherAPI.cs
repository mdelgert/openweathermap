using System;
using System.Threading;
using System.Net;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap
{
    public class WeatherApi
    {
        public void Run(WeatherModel wm)
        {
            var timer = new System.Timers.Timer {Interval = wm.Interval};
            timer.Elapsed += TimerEvent;
            timer.Start();
        }

        private static void TimerEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            var wapi = new WeatherApi();
            wapi.LogMsgToFile("Timer Event");
            //Console.WriteLine("Timer Event");
        }

        public string Check(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                var response = request.GetResponse();

                using (var responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                var errorResponse = ex.Response;
                using (var responseStream = errorResponse.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    var errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }

        }

        public WeatherModel GetModel()
        {
            var wm = new WeatherModel
            {
                ApiKey = GetKeyValue("ApiKey"),
                Url = GetKeyValue("Url"),
                CountryCode = GetKeyValue("CountryCode"),
                ZipCode = int.Parse(GetKeyValue("ZipCode")),
                Units = GetKeyValue("Units"),
                Mode = GetKeyValue("Mode"),
                Interval = int.Parse(GetKeyValue("Interval"))
            };

            return wm;
        }

        private static string GetKeyValue(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            return value;
        }

        public string GetRequestUrl(WeatherModel wm)
        {
            var requestUrl = $@"http://{wm.Url}?mode={wm.Mode}&units={wm.Units}&zip={wm.ZipCode},{wm.CountryCode}&appid={wm.ApiKey}";
            return requestUrl;
        }

        public void LogResponse(string response)
        {
            
        }

        public void LogMsgToFile(string msg)
        {
            using (var sw = File.AppendText(GetKeyValue("Log")))
            {
                try
                {
                    var logLine = $"{DateTime.Now:G}: {msg}";
                    sw.WriteLine(logLine);
                    Console.WriteLine(logLine);
                }
                finally
                {
                    sw.Close();
                }
            } 
            
        }

    }
}

//https://blogs.msdn.microsoft.com/csharpfaq/2006/03/27/how-can-i-easily-log-a-message-to-a-file-for-debugging-purposes/
//http://api.openweathermap.org/data/2.5/weather?mode=xml&zip=44312,us&appid=f4f384540e308bdfc88f463e7d8df69d
//http://api.openweathermap.org/data/2.5/weather?zip=44312,us&appid=f4f384540e308bdfc88f463e7d8df69d
//http://api.openweathermap.org/data/2.5/weather?mode=xml&units=imperial&zip=44312,us&appid=f4f384540e308bdfc88f463e7d8df69d
//https://stackoverflow.com/questions/6169288/execute-specified-function-every-x-seconds
//http://json2csharp.com/
//https://stackoverflow.com/questions/8270464/best-way-to-call-a-json-webservice-from-a-net-console
// Returns JSON string