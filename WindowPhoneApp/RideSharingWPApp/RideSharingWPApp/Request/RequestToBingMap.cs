using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace RideSharingWPApp.Request
{
    class RequestToBingMap
    {
        public static string bingAPIkey = "Ajze-B_0BaOUYxiJ0Hizj6wnyAnyRDPI5jfvDa1J7zkrCQZz2GNZkIigjLhi__nM";
        public static string preServiceURI = "http://dev.virtualearth.net/REST/v1/Locations/";

        public static async Task<string> sendGeoCodingRequest(string methodName)
        {
            //
            string ServiceURI = preServiceURI + methodName + "/?o=json&key=" + bingAPIkey;
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.Method = HttpMethod.Get;
            request.RequestUri =
                new Uri(ServiceURI);
            //request.Headers.Authorization = Windows.Web.Http.Headers.HttpCredentialsHeaderValue.Parse("ce657571fcbe01921ce838df4cccddf4");

            HttpResponseMessage response = await httpClient.SendRequestAsync(request);
            string returnString = await response.Content.ReadAsStringAsync();
            return returnString;
        }
    }
}
