using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Service.Helpers
{
    public static class ServiceHelper
    {
        private const string ROUTER_URL = "http://192.168.75.93:3000";

        public static void RegisterService(ServiceMetadata metadata)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(ROUTER_URL + "/register");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(metadata);
                streamWriter.Write(json);
            }

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                if (httpResponse.StatusCode != HttpStatusCode.NoContent)
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        throw new Exception($"Failed to register service. StatusCode: [{httpResponse.StatusCode}], Response: [{result}].");
                    }
                }
            }
            catch(WebException ex)
            {
                throw new Exception($"Failed to register service. Message: [{ex.Message}]");
            }
        }

        public static Service GetService(string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                throw new ArgumentException("The service name have to be specified.");
            }

            var serviceUri = $"{ROUTER_URL}/{serviceName}";

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(serviceUri);
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                //.. will throw WebException if statusCode != 2XX
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;

                // ?: Status 503?
                if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    // -> Service not found, throw error!
                    throw new Exception($"Service with name [{serviceName}] has not been registered.");
                }
                // E -> Service is registered, but the endpoint probably is not active (status = 404, 500, etc..)
            }
            // Basic service check OK! Continue..
            return new Service(serviceUri);
        }
    }

    public class Service
    {
        private string Uri { get; set; }

        public Service(string uri)
        {
            Uri = uri;
        }

        public T Get<T>(string path)
        {
            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString($"{Uri}{path}");
                return JsonConvert.DeserializeObject<T>(json);
            }
        }
    }

    public class ServiceMetadata
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "port")]
        public int Port { get; set; }
    }
}
