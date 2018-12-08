using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Net;
using System.Xml;

namespace WelcomeLibrary.UF
{
    public class googlegeolocalization
    {

        /// Resolve addresses into latitude/longitude coordinates using Google MAP API webservices
    }
    public static class Geocoder
    {

        public static double GetDistanceTo(double lat1, double lng1, double lat2, double lng2)
        {
            double d = lat1 * 0.017453292519943295;
            double num3 = lng1 * 0.017453292519943295;
            double num4 = lat2 * 0.017453292519943295;
            double num5 = lng2 * 0.017453292519943295;
            double num6 = num5 - num3;
            double num7 = num4 - d;
            double num8 = Math.Pow(Math.Sin(num7 / 2.0), 2.0) + ((Math.Cos(d) * Math.Cos(num4)) * Math.Pow(Math.Sin(num6 / 2.0), 2.0));
            double num9 = 2.0 * Math.Atan2(Math.Sqrt(num8), Math.Sqrt(1.0 - num8));
            return (6376500.0 * num9);
        }


        /// <summary>
        /// restituisce la distanza in metri tra due coordinate senza usare servizi esterni
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <returns></returns>
        public static double? CalculateDistanceBetweenpoints(double lat1, double lon1, double lat2, double lon2)
        {
            double? distance = null;
            try
            {
                var sCoord = new GeoCoordinate(lat1, lon1);
                var eCoord = new GeoCoordinate(lat2, lon2);

                distance = sCoord.GetDistanceTo(eCoord);
            }
            catch {

            };
            return distance;
        }




        public static List<Geolocation?> ReverseAddress(decimal lat, decimal lon, bool sensor = false)
        {
            string xmlString = "";

            //REVERSE GEOCODING V3
            //http://maps.googleapis.com/maps/api/geocode/json?latlng=40.714224,-73.961452&sensor=true_or_false
            string url = "http://maps.googleapis.com/maps/api/geocode/xml?key=" + WelcomeLibrary.UF.ConfigManagement.ReadKey("GoogleMapsKey") + "&latlng={0},{1}&sensor=";
            if (sensor)
                url += "true";
            else
                url += "false";
            url = String.Format(url, lat.ToString().Replace(",", "."), lon.ToString().Replace(",", "."));
            // XmlNode coords = null;

            List<Geolocation?> gllist = new List<Geolocation?>();
            Geolocation? gl = null;
            try
            {
                xmlString = GetUrl(url);
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(xmlString);
                //XmlNamespaceManager xnm = new XmlNamespaceManager(xd.NameTable);
                Dictionary<string, string> datilocalita = null;

                foreach (XmlNode xn in xd.GetElementsByTagName("result"))
                {
                    datilocalita = new Dictionary<string, string>();
                    string indirizzocompleto = xn.SelectSingleNode("formatted_address").InnerText;
                    datilocalita.Add("formatted_address", indirizzocompleto);
                    foreach (XmlNode _node in xn.SelectNodes("address_component"))
                    {
                        //foreach (XmlNode _subnode in _node.ChildNodes)
                        //{
                        XmlNode nodetype = _node.SelectSingleNode("type");
                        XmlNode nodelong_name = _node.SelectSingleNode("long_name");
                        //XmlNode nodeshort_name = _subnode.SelectSingleNode("short_name");
                        datilocalita.Add(nodetype.InnerText, nodelong_name.InnerText);
                        //}
                    }
                    gl = new Geolocation(lat, lon, xmlString, datilocalita);
                    if (gl != null)
                        gllist.Add(gl);
                }

            }
            catch { }
            return gllist;

        }

        // private static string _GoogleMapsKey = ConfigurationManager.AppSettings["GoogleMapsKey"];
        /// Google.com Geocoder
        /// Url request to
        /// http://maps.google.com/maps/geo?q=your address&output=xml&key=xxxxxxxxxxxxxx
        public static List<Geolocation?> ResolveAddress(string query, bool sensor = false)
        {
            //if (string.IsNullOrEmpty(_GoogleMapsKey))
            //    _GoogleMapsKey = ConfigurationManager.AppSettings["GoogleMapsKey"];

            //https://developers.google.com/maps/documentation/geocoding/?hl=it#Limits
            //http://maps.googleapis.com/maps/api/geocode/output?parameters
            //https://maps.googleapis.com/maps/api/geocode/output?parameters
            //            where output may be either of the following values:
            //             •json (recommended) indicates output in JavaScript Object Notation (JSON)
            //                  •xml indicates output as XML

            //            •address — The address that you want to geocode. 
            //     or
            //latlng — The textual latitude/longitude value for which you wish to obtain the closest, human-readable address. See Reverse Geocoding for more information. 
            //     or
            //components — A component filter for which you wish to obtain a geocode. See Component Filtering for more information. The components filter will also be accepted as an optional parameter if an address is provided. 
            //•sensor — Indicates whether or not the geocoding request comes from a device with a location sensor. This value must be either true or false.
            //Maps API for Business users must include valid client and signature parameters with their Geocoding requests. Please refer to Maps API for Business Web Services for more information.
            //https://developers.google.com/maps/documentation/business/webservices?hl=it

            //Optional parameters
            //•bounds — The bounding box of the viewport within which to bias geocode results more prominently. This parameter will only influence, not fully restrict, results from the geocoder. (For more information see Viewport Biasing below.)
            //•language — The language in which to return results. See the list of supported domain languages. Note that we often update supported languages so this list may not be exhaustive. If language is not supplied, the geocoder will attempt to use the native language of the domain from which the request is sent wherever possible.
            //•region — The region code, specified as a ccTLD ("top-level domain") two-character value. This parameter will only influence, not fully restrict, results from the geocoder. (For more information see Region Biasing below.)
            //•components — The component filters, separated by a pipe (|). Each component filter consists of a component:value pair and will fully restrict the results from the geocoder. For more information see Component Filtering, below.
            //REVERSE GEOCODING V3
            //http://maps.googleapis.com/maps/api/geocode/json?latlng=40.714224,-73.961452&sensor=true_or_false

            //string url = "http://maps.google.com/maps/geo?q={0}&output=xml&key=" + _GoogleMapsKey;

            //GEOCODING V3


            string url = "http://maps.googleapis.com/maps/api/geocode/xml?key=" + WelcomeLibrary.UF.ConfigManagement.ReadKey("GoogleMapsKey") + "&address={0}&sensor=";
            if (sensor)
                url += "true";
            else
                url += "false";
            url = String.Format(url, query.Replace(" ", "+"));
            XmlNodeList coords = null;
            try
            {
                string xmlString = GetUrl(url);
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(xmlString);
                XmlNamespaceManager xnm = new XmlNamespaceManager(xd.NameTable);
                coords = xd.GetElementsByTagName("location"); //PRENDO SOLO IL PRIMO RISULTATO IN BASE AL TESTO PASSATO ( MA POTERI AVERNE DI PIU' quondi dovrei darli tutti!!)
            }
            catch { }
            Geolocation? gl = null;
            List<Geolocation?> gllist = new List<Geolocation?>();
            if (coords != null && coords.Count > 0)
            {
                foreach (XmlNode _node in coords)
                {
                    if (_node.ChildNodes != null && _node.ChildNodes.Count >= 2)
                    {
                        gl = new Geolocation(Convert.ToDecimal(_node.ChildNodes[0].InnerText.Replace(".", ",").ToString()), Convert.ToDecimal(_node.ChildNodes[1].InnerText.Replace(".", ",").ToString()));
                        if (gl != null)
                            gllist.Add(gl);
                    }
                }
            }
            return gllist;
        }


        public static List<Geolocation?> ResolveAddress(string address, string city, string state, string postcode, string country, bool sensor = false)
        {
            return ResolveAddress(address + "," + city + "," + state + "," + postcode + " " + country, sensor);
        }

        /// <summary>
        /// Retrieve a Url via WebClient
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string GetUrl(string url)
        {
            string result = string.Empty;
            System.Net.WebClient Client = new WebClient();
            using (Stream strm = Client.OpenRead(url))
            {
                StreamReader sr = new StreamReader(strm);
                result = sr.ReadToEnd();
            }
            return result;
        }
    }

    public struct Geolocation
    {
        public decimal Lat;
        public decimal Lon;
        public string ReturnXml;
        public Dictionary<string, string> Datilocalita;

        public Geolocation(decimal lat, decimal lon, string returnXml = "", Dictionary<string, string> datilocalita = null)
        {
            Lat = lat;
            Lon = lon;
            ReturnXml = returnXml;
            Datilocalita = datilocalita;
        }
        public override string ToString()
        {
            return "Latitude: " + Lat.ToString() + " Longitude: " + Lon.ToString();
        }
        public string ToQueryString()
        {
            return "+to:" + Lat + "%2B" + Lon;
        }
        public string returnXmlString()
        {
            return ReturnXml;
        }
    }
}




