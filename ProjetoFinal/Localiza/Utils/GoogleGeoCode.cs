using System;
using System.Net;
using System.Web;

namespace Utils
{
    public class GoogleGeoCode
    {
        private const string googleUri = @"http://maps.google.com/maps/geo?q=";
        private const string googleKey = "xyz";
        private const string outputType = "csv"; // Available options: csv, xml, kml, json

        private static Uri GetGeocodeUri(string address)
        {
            address = HttpUtility.UrlEncode(address);
            return new Uri(String.Format("{0}{1}&output={2}&key={3}", googleUri, address, outputType, googleKey));
        }

        /// <summary>
        /// Gets a Coordinate from a address.
        /// </summary>
        /// <param name="address">An address.
        /// <remarks>
        /// <example>1600 Amphitheatre Parkway Mountain View, CA 94043</example>
        /// </remarks>
        /// </param>
        /// <returns>A spatial coordinate that contains the latitude and longitude of the address.</returns>
        public static Coordenadas GetCoordenadas(string endereco)
        {
            WebClient client = new WebClient();
            Uri uri = GetGeocodeUri(endereco);


            /* The first number is the status code, 
            * the second is the accuracy, 
            * the third is the latitude, 
            * the fourth one is the longitude.
            */
            string[] geocodeInfo = client.DownloadString(uri).Split(',');

            return new Coordenadas(Convert.ToDecimal(geocodeInfo[2].Replace(".", ",")), Convert.ToDecimal(geocodeInfo[3].Replace(".", ",")));
        }

    }
}
