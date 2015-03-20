using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;


namespace RideSharingWPApp.Request
{
    class RequestToServer
    {
        public string preServiceURI = "http://192.168.10.74/RESTFul/v1/";
        

        public static async Task<string> sendGetRequest(string methodName, Windows.Web.Http.IHttpContent content)
        {
            string ServiceURI = "http://192.168.10.74/RESTFul/v1/" + methodName;
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.Method = HttpMethod.Get;
            request.RequestUri = new Uri(ServiceURI);
            request.Headers.Authorization = Windows.Web.Http.Headers.HttpCredentialsHeaderValue.Parse("ce657571fcbe01921ce838df4cccddf4");
           

            
            //Windows.Web.Http.HttpStringContent content =
            //request.Content = IReadOnlyCollection.

            HttpResponseMessage response = await httpClient.SendRequestAsync(request);
            string returnString = await response.Content.ReadAsStringAsync();
            return returnString;
        }
    }
}
