using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static  async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;
            string chave = "31f3d475b018d0b88891415340997e9d";

            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&units=metric&appid={chave}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(url);

                if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();

                    var rascuho = JObject.Parse(json);

                    DateTime time = new();
                    DateTime sunrise = time.AddSeconds((double)rascuho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = time.AddSeconds((double)rascuho["sys"]["sunset"]).ToLocalTime();


                    t = new()
                    {
                        lat = (double)rascuho["coord"]["lat"],
                        lon = (double)rascuho["coord"]["lon"],
                        description = (string)rascuho["weather"][0]["description"],
                        main = (string)rascuho["weather"][0]["main"],
                        temp_min = (double)rascuho["main"]["temp_min"],
                        temp_max = (double)rascuho["main"]["temp_max"],
                        speed = (double)rascuho["wind"]["speed"],
                        visibility = (int)rascuho["visibility"],
                        sunrise = sunrise.ToString(),
                        sunset = sunset.ToString()
                    }; // Fecha obj do tempo
                } // Fecha if se o status do serviçor foi de sucessp
            } // Fecha laço using

            return t;
        }
    }
}
