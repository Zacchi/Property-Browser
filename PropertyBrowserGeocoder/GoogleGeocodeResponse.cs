using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBrowserGeocoder
{
    public class GoogleGeocodeResponse
    {
        public List<GoogleGeocodeResult> Results { get; set; }
        public string Status { get; set; }
    }

    public class GoogleGeocodeResult
    {
        public GoogleGeocodeGeometry Geometry { get; set; }
    }

    public class GoogleGeocodeGeometry
    {
        public GoogleGeocodeLocation Location { get; set; }
    }

    public class GoogleGeocodeLocation
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

}
