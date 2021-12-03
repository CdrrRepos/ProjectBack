using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectBack.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace ProjectBack.Proxy
{
    public class AppProxy
    {
     
        public HttpClient clienteHttp = new HttpClient();

        public AppProxy()
        {
            clienteHttp.BaseAddress = new Uri("http://190.145.81.67:5200/api/");

        }

        public LoginResponse proxyLogin()
        {
            LoginResponse data = new LoginResponse();

            LoginRequest res = new LoginRequest();
            res.userName = "user";
            res.password = "1234";

            try
            {
                var request = clienteHttp.PostAsync("Login", res, new JsonMediaTypeFormatter()).Result;

                if (request.IsSuccessStatusCode)
                {
                    var response = request.Content.ReadAsStringAsync().Result;
                    var formatResponse = JsonConvert.DeserializeObject<LoginResponse>(response);
                    data = formatResponse;
                }

                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }

        public List<VehiculoResponse> ConsultaVehiculos(string path,string fecha)
        {
            List<VehiculoResponse> data = new List<VehiculoResponse>();

            try
            {
                LoginResponse loRe = new LoginResponse();
                loRe = proxyLogin();

                clienteHttp.DefaultRequestHeaders.Add("Authorization", "Bearer " + loRe.token);
                var request = clienteHttp.GetAsync(path + fecha).Result;

                if (request.IsSuccessStatusCode)
                {
                    var val = request.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<List<VehiculoResponse>>(val);
                }

                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }

    }
}
