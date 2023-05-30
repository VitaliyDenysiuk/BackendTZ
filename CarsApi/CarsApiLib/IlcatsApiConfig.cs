using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApiLib
{
    public class IlcatsApiConfig
    {
        public static string BaseUrl { get; } = "https://www.ilcats.ru/";
        public static long TimeStamp => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}
