/*
File: WeatherApi.cs
Author: Matthew David Elgert
Date: 3/27/2018
https://github.com/mdelgert/openweathermap
*/

using System;
using System.Net;
using System.IO;
using System.Configuration;
using System.Text;

namespace OpenWeatherMap
{
    public class WeatherApi
    {
        #region Public
        public void Run()
        {
            var wm = GetModel();
            var timer = new System.Timers.Timer { Interval = wm.Interval };
            timer.Elapsed += (sender, e) => TimerEvent(wm);
            SetupConsole();
            LogMessage($@"Next WeatherMap Call in {wm.Interval} milliseconds.");
            timer.Start();
            Console.ReadKey();
        }
        #endregion

        #region Private Methods
        private static void SetupConsole()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($@"#####################################################");
            Console.WriteLine($@"OpenWeatherApi: Version 1.0");
            Console.WriteLine($@"Author: Matthew Elgert");
            Console.WriteLine($@"https://github.com/mdelgert/openweathermap");
            Console.WriteLine($@"#####################################################");
            Console.WriteLine($@"Press any key to exit.");
        }

        private static void TimerEvent(WeatherModel wm)
        {
            CheckWeatherMap(GetRequestUrl(wm));
            LogMessage($@"Next WeatherMap Call in {wm.Interval} milliseconds.");
        }

        private static void CheckWeatherMap(string requestUrl)
        {
            LogMessage($@"ApiRequest={requestUrl}");

            var request = (HttpWebRequest)WebRequest.Create(requestUrl);
            try
            {
                var response = request.GetResponse();
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        var reader = new StreamReader(responseStream, Encoding.UTF8);
                        LogMessage($@"Success: WeatherApi received response.");
                        LogResponse(reader.ReadToEnd());
                    }
                    else
                    {
                        LogMessage($@"Error: WeatherApi received null response.");
                    }
                }
            }
            catch (WebException ex)
            {
                var errorResponse = ex.Response;
                using (var responseStream = errorResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        var reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                        var errorText = reader.ReadToEnd();
                        LogMessage($@"Error: WeatherApi failed response with error={errorText}");
                    }
                }
            }
        }

        private static WeatherModel GetModel()
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

        private static string GetRequestUrl(WeatherModel wm)
        {
            var requestUrl = $@"http://{wm.Url}?mode={wm.Mode}&units={wm.Units}&zip={wm.ZipCode},{wm.CountryCode}&appid={wm.ApiKey}";
            return requestUrl;
        }

        private static void LogResponse(string response)
        {
            var fileName = $@"Response{DateTime.Now:yyyyMMddHHmmss}.xml";
            using (var sw = new StreamWriter(fileName))
            {
                sw.WriteLine(response);
                LogMessage($@"Successfully save file {fileName}");
            }
        }

        private static void LogMessage(string message)
        {
            using (var fa = File.AppendText(GetKeyValue("Log")))
            {
                var logLine = $"{DateTime.Now:G}: {message}";
                fa.WriteLine(logLine);
                fa.Close();
                Console.WriteLine(logLine);
            } 
        }
        #endregion
    }
}
