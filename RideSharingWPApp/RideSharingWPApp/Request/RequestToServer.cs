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
        //public static string preServiceURI = "http://localhost/SeniorProject/RESTFul/v1/";
        //public static string preServiceURI = "http://192.168.10.132/RESTFul/v1/";
        public static string preServiceURI = "http://54.68.126.75/RESTFul/v1/";
        

        public static async Task<string> sendGetRequest(string methodName)
        {
            string ServiceURI = preServiceURI + methodName;
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.Method = HttpMethod.Get;
            request.RequestUri = new Uri(ServiceURI);
            request.Headers.Authorization = Windows.Web.Http.Headers.HttpCredentialsHeaderValue.Parse(Global.GlobalData.APIkey);

            HttpResponseMessage response = await httpClient.SendRequestAsync(request);
            string returnString = await response.Content.ReadAsStringAsync();
            return returnString;
        }

        public static async Task<string> sendPostRequest(string methodName, Windows.Web.Http.IHttpContent content)
        {
            string ServiceURI = preServiceURI + methodName;
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(ServiceURI);
            if (Global.GlobalData.APIkey == null)
            {
                request.Headers.Authorization = Windows.Web.Http.Headers.HttpCredentialsHeaderValue.Parse("xnull");
                
            }
            else
            {
                request.Headers.Authorization = Windows.Web.Http.Headers.HttpCredentialsHeaderValue.Parse(Global.GlobalData.APIkey);
            }
            

            request.Content = content;

            HttpResponseMessage response = await httpClient.SendRequestAsync(request);
            string returnString = await response.Content.ReadAsStringAsync();
            return returnString;
        }

        public static async Task<string> sendPutRequest(string methodName, Windows.Web.Http.IHttpContent content)
        {
            string ServiceURI = preServiceURI + methodName;
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.Method = HttpMethod.Put;
            request.RequestUri = new Uri(ServiceURI);
            request.Headers.Authorization = Windows.Web.Http.Headers.HttpCredentialsHeaderValue.Parse(Global.GlobalData.APIkey);

            request.Content = content;

            HttpResponseMessage response = await httpClient.SendRequestAsync(request);
            string returnString = await response.Content.ReadAsStringAsync();
            return returnString;
        }

        public static async Task<string> sendDeleteRequest(string methodName)
        {
            string ServiceURI = preServiceURI + methodName;
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.Method = HttpMethod.Delete;
            request.RequestUri = new Uri(ServiceURI);
            request.Headers.Authorization = Windows.Web.Http.Headers.HttpCredentialsHeaderValue.Parse(Global.GlobalData.APIkey);

            HttpResponseMessage response = await httpClient.SendRequestAsync(request);
            string returnString = await response.Content.ReadAsStringAsync();
            return returnString;
        }
    }
}
