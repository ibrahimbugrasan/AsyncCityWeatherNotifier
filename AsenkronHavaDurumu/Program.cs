namespace AsenkronHavaDurumu
{
    using System.Text.Json;
    using System.Text.Json.Nodes;

    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("********************************************");
            Console.WriteLine("Asenkron Hava Durumu Uygulamasına Hoşgeldiniz!");
            Console.WriteLine("********************************************");
            while(true)
            {
                Console.WriteLine("Lütfen hava durumunu görmek istediğiniz şehiri giriniz (çıkış yazmak için e/E yazınız):");
                string sehir = Console.ReadLine();
                if (sehir.ToLower() == "e")
                {
                    break;
                }
                
                await HavaDurumuGoster(IlkBuyukHarf(sehir));
            }
            
        }
       

        public static string IlkBuyukHarf(string sehir)
        {
            char ilkHarf = sehir[0];
            string buyuk = ilkHarf.ToString().ToUpper();
            string kalan = sehir.Substring(1);
            sehir = buyuk + kalan;

            return sehir;
        }

        public static async Task HavaDurumuGoster(string sehir)
        {
            
            string apikey = "Write your OpenWeatherMap api key";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={sehir}&appid={apikey}";
            //string url = $"https://api.openweathermap.org/data/2.5/weather?lat=44.34&lon=10.99&appid={apikey}";

            using(HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage responsee = await client.GetAsync(url);
                    responsee.EnsureSuccessStatusCode();
                    string responseBody = await responsee.Content.ReadAsStringAsync();

                    JsonNode jsonNode = JsonNode.Parse(responseBody);
                    JsonNode weatherNode = jsonNode["weather"][0];
                    JsonNode havadurumuNode = weatherNode["main"];
                    JsonNode sıcaklıkNode = jsonNode["main"]["temp"];
                    double sıcaklıkKelvin = sıcaklıkNode.GetValue<double>();  
                    double sıcaklıkCelsius = sıcaklıkKelvin - 273.15;
                    


                    
                    Console.WriteLine($"\nWeather Report for {sehir}");
                    Console.WriteLine($"Condition: {havadurumuNode} ({weatherNode["description"]})");
                    Console.WriteLine($"Temperature: {sıcaklıkCelsius:F1}°C");
                    Console.WriteLine($"Humidity: {jsonNode["main"]["humidity"]}%");
                    Console.WriteLine($"Wind: {jsonNode["wind"]["speed"]} km/h\n");


                }
                catch(HttpRequestException e)
                {
                    Console.WriteLine("Hata!!! " + e);
                }
            }

        }

    }
}
