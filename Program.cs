using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Bitte wählen Sie Ihre Region:");
        Console.WriteLine("1. Europe");
        Console.WriteLine("2. America");
        Console.WriteLine("3. Africa");

        int regionChoice;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out regionChoice) && regionChoice >= 1 && regionChoice <= 3)
            {
                break;
            }
            Console.WriteLine("Ungültige Eingabe. Bitte geben Sie eine Zahl zwischen 1 und 3 ein.");
        }

        string region = "";
        switch (regionChoice)
        {
            case 1:
                region = "Europe";
                break;
            case 2:
                region = "America";
                break;
            case 3:
                region = "Africa";
                break;
            default:
                break;
        }

        Console.Write($"Bitte geben Sie Ihre Stadt in {region} ein (auf Englisch) :  ");
        string state = Console.ReadLine();

        string timezone = $"{region}/{state}";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://worldtimeapi.org/api/timezone/{timezone}");
                response.EnsureSuccessStatusCode(); 

                string responseBody = await response.Content.ReadAsStringAsync();
                WorldTimeApiResponse result = JsonConvert.DeserializeObject<WorldTimeApiResponse>(responseBody);

                
                DateTime dateTime = DateTime.Parse(result.datetime);

                
                string output = $"Es ist {dateTime.ToString("HH:mm")} Uhr am {dateTime.ToString("dd.MM.yyyy")}, welcher ein {GetGermanDayOfWeek(dateTime.DayOfWeek)} ist.";
                Console.WriteLine(output);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Fehler beim Abrufen der Zeit: {e.Message}");
            }
        }
    }

    static string GetGermanDayOfWeek(DayOfWeek dayOfWeek)
    {
        switch (dayOfWeek)
        {
            case DayOfWeek.Monday:
                return "Montag";
            case DayOfWeek.Tuesday:
                return "Dienstag";
            case DayOfWeek.Wednesday:
                return "Mittwoch";
            case DayOfWeek.Thursday:
                return "Donnerstag";
            case DayOfWeek.Friday:
                return "Freitag";
            case DayOfWeek.Saturday:
                return "Samstag";
            case DayOfWeek.Sunday:
                return "Sonntag";
            default:
                return "";
        }
    }
}

public class WorldTimeApiResponse
{
    public string datetime { get; set; }
}
