using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BPExporter
{
    class Settings
    {
        public static string DatabasePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "data.db");
        public static LookupService CitySrv = new LookupService("GeoIPCity.dat", LookupService.GEOIP_MEMORY_CACHE);
        public static LookupService IspSrv = new LookupService("GeoIPISP.dat", LookupService.GEOIP_MEMORY_CACHE);
    }
}
