using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace PhoneApp2
{
    class RequestToServer
    {
        public static string bingAPIkey = "Ajze-B_0BaOUYxiJ0Hizj6wnyAnyRDPI5jfvDa1J7zkrCQZz2GNZkIigjLhi__nM";
        public static string preServiceURI = "http://dev.virtualearth.net/REST/v1/Locations/";


        public static async Task<string> sendGetRequest(string methodName)
        {
            //
            string ServiceURI = preServiceURI + methodName +"/?o=json&key=Ajze-B_0BaOUYxiJ0Hizj6wnyAnyRDPI5jfvDa1J7zkrCQZz2GNZkIigjLhi__nM";
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

        /*public static async Task<string> sendPostRequest(string methodName, Windows.Web.Http.IHttpContent content)
        {
            string ServiceURI = "http://192.168.10.74/RESTFul/v1/" + methodName;
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(ServiceURI);
            request.Headers.Authorization = Windows.Web.Http.Headers.HttpCredentialsHeaderValue.Parse("ce657571fcbe01921ce838df4cccddf4");

            request.Content = content;

            HttpResponseMessage response = await httpClient.SendRequestAsync(request);
            string returnString = await response.Content.ReadAsStringAsync();
            return returnString;
        }*/
    }
}
