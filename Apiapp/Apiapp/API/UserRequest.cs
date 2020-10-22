using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apiapp.API
{
    public class UserRequest
    {

        private HttpClient _restclient;
        private System.Net.Http.HttpClient restClient;

        public UserRequest(HttpClient restclient)
        {
            if (restclient != null) _restclient = restclient;
            else throw new NullReferenceException("El cliente http no puede ser null");

        }

        public UserRequest(System.Net.Http.HttpClient restClient)
        {
            this.restClient = restClient;
        }

        public async Task<List<Usuario>> All()
        {

            var response = await _restclient.ExecuteAsync<List<Usuario>>(Method.GET,$"{App.BaseUrl}/User/all");

            if (response.Result != null) return response.Result;
            return new List<Usuario>();
        }
        public async Task<Usuario> Get(int id)
        {

            var response = await _restclient.ExecuteAsync<Usuario>(Method.GET, $"{App.BaseUrl}/User/get/{id}");

            if (response.Result != null) return response.Result;
            return null;
        }

        public async Task<bool> Add(Usuario usuario)
        {
            var response = await _restclient.ExecuteAsync<Usuario>(Method.POST,$"{App.BaseUrl}/User/all", new Dictionary<string, object>
            {
                {"nombre",usuario.nombre },
                {"email",usuario.email },
                {"cedula",usuario.cedula }
            });

            return response.Result != null;
        }

        public async Task<bool> Delete(int id)
        {
            var response = await _restclient.ExecuteAsync<StatusResponse>(Method.GET, $"{App.BaseUrl}/User/delete/{id}");

            return response.Result != null && response.Result.code ==200;
        }

        public async Task<bool> Update(Usuario user, int id)
        {
            var response = await _restclient.ExecuteAsync<Usuario>(Method.POST, $"{App.BaseUrl}/User/update/{id}", new Dictionary<string, object> 
            {
                {"nombre",user.nombre },
                {"email",user.email },
                {"cedula",user.cedula }
            });

            return response.Result != null ;
        }
    }
}
