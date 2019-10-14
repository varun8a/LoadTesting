using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ParallelTestScript
{
   public  class MessageClient
    {
        private HttpClient client;

        public MessageClient()
        {
            client = new HttpClient();
        }

        private static async Task<bool> SendToEconnect(string message, TextWriter log)
        {
           
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://mobileapp-meap-qa.ase-meap-onprem-prodqa.p.azurewebsites.net/tables/MessageQueue");

                    //var authData = string.Format("{0}:{1}", MeapUserName, MeapUserPassword);
                    //var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                    //client.DefaultRequestHeaders.Accept.Clear();

                    //message = message.Replace("Header2", "Header");
                    HttpContent stringContent = new StringContent(message, UnicodeEncoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue

                    HttpResponseMessage response = await client.PostAsync("", stringContent);

                    log.WriteLine("GOT reponse from econnect");
                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                        log.WriteLine(data);
                    }
                    else
                    {
                        log.WriteLine(response.ToString());
                    }

                }
            }
            catch (Exception)
            {
                // Need to throw other wise message would be removed from service bus.
                throw;
            }
            return true;
        }

        public async Task<IEnumerable<bool>> GetUsersInParallel(IEnumerable<string> msgs)
        {
            TextWriter tx =new StringWriter();
            var tasks = msgs.Select(msg => SendToEconnect(msg,tx));
            var users = await Task.WhenAll(tasks);

            return users;
        }
    }
}
