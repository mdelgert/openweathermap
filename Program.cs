/*
File: Program.cs
Author: Matthew David Elgert
Date: 3/27/2018
https://github.com/mdelgert/openweathermap
*/

namespace OpenWeatherMap
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var wapi = new WeatherApi();
            wapi.Run();
        }
    }
}
