/*
File: WeatherModel.cs
Author: Matthew David Elgert
Date: 3/27/2018
https://github.com/mdelgert/openweathermap
*/

namespace OpenWeatherMap
{
    public class WeatherModel
    {
        public string ApiKey { get; set; }
        public string Url { get; set; }
        public int ZipCode { get; set; }
        public string CountryCode { get; set; }
        public string Units { get; set; }
        public string Mode { get; set; }
        public int Interval { get; set; }
    }
}