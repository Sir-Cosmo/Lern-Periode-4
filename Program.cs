using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        await MakeRequest();
        Console.ReadLine();
    }

    static async Task MakeRequest()
    {
        char runCode;

        do
        {
            string city = GetCity();

            string apiUrl = $"http://worldtimeapi.org/api/timezone/Europe/{city}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        WorldTimeApiResponse apiResponse = JsonSerializer.Deserialize<WorldTimeApiResponse>(responseBody, options);

                        if (apiResponse != null)
                        {
                            
                            string time = apiResponse.Datetime.Substring(11, 8);

                            Console.WriteLine($"Current Time in {apiResponse.Timezone}: {time}");
                        }
                        else
                        {
                            Console.WriteLine("Unable to parse API response.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Request failed. Status Code: {response.StatusCode}");
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Error making request: {e.Message}");
                }
                catch (JsonException e)
                {
                    Console.WriteLine($"Error parsing JSON response: {e.Message}");
                }
            }

            Console.WriteLine("Do you want to make another request? [y/n]");
            runCode = Convert.ToChar(Console.ReadLine());
        } while (runCode == 'y');
    }

    static string GetCity()
    {
        Console.WriteLine("Please enter the city (e.g., Berlin, London, Paris):");
        string city = Console.ReadLine().Trim().Replace(" ", "_"); 
        return city;
    }

    public class WorldTimeApiResponse
    {
        public string Datetime { get; set; }
        public string Timezone { get; set; }
    }
}
