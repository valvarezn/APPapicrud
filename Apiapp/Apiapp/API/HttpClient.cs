using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Apiapp.API
{
    public class HttpClient 
    {
        private Dictionary<string, string> _headers;

        public HttpClient(Dictionary<string,string> headers)
        {
            if (headers != null) _headers = headers;
            else _headers = new Dictionary<string, string>();
        }

        public string this[string key]
        {
            get
            {
                return _headers[key];
            }

            set
            {
                _headers[key] = value;
            }
        }

        public async Task<HttpResponse<T>> ExecuteAsync<T>(Method method, string baseUrl,Dictionary<string,object> formdata = null)
        {
            var client = new RestSharp.RestClient(baseUrl);
            var request = new RestRequest(method);

            foreach (var header in _headers)
            {
                request.AddHeader(header.Key, header.Value);
            }
            if (formdata != null) {
                foreach (var item in formdata)
                {
                    request.AddParameter(item.Key, item.Value);
                }
            }


            IRestResponse response = null;

            Exception ex = null;
            try
            {
                response = await client.ExecuteTaskAsync(request);
            }
            catch (Exception _ex)
            {
                ex = _ex;
            }
            
            var httpresponse = new HttpResponse<T>
            {
                Response = response
            };

            if( response == null)
            {
                httpresponse.Status = new StatusResponse
                {
                    code = -1,
                    message = $"No se pudo obtener una respuesta del servidor,stacktrace :{ex.StackTrace}"
                };
                return httpresponse;
            }

            var jsonresult = response.Content;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                
                var telement = JsonConvert.DeserializeObject<T>(jsonresult);
                httpresponse.Result = telement;
            }
            else
            {
                var status = JsonConvert.DeserializeObject<StatusResponse>(jsonresult);
                httpresponse.Status = status;
            }

            return httpresponse;
        }

    }

}
