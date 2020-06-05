
// The Five Questions:
//1. What is the input that needs to be sent?:	Band name {artist} and Song name {title}
//2. What is the format of the input?:		    The body will be in JSON format
//3. What is the name/location/identity of the code to be run?:     The URL and specific verb to use
//4. What is the output that will be returned?: The response body and the response code
//5. What is the format of the output?:         The body will be in JSON and the response code will be a table

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ConsoleTables;

namespace APIclientApp_SongIndex
{
    class Song
    {
        // Using C#-style property names
        // The next property I declare no matter what I call it
        // The program should get it from the 'id' JSON key

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("artist")]
        public string BandName { get; set; }

        [JsonPropertyName("title")]
        public string SongName { get; set; }

        [JsonPropertyName("lyrics")]
        public string SongLyrics { get; set; }
    }

    class Program
    {
        private static async Task GetOneSongAsync(string bandname, string songname)
        {
            try  // try-catch statement
            {
                // This 'client' variable will be used to interact with the API
                var client = new HttpClient();

                // A variable to GET a stream from an API
                var url = $"https://api.lyrics.ovh/v1/{bandname}/{songname}";
                var responseAsStream = await client.GetStreamAsync(url);

                // This is a List of items
                var item = await JsonSerializer.DeserializeAsync<Song>(responseAsStream);

                // Create a new ConsoleTable
                var table = new ConsoleTable("Id", "Band Name", "Song Name", "Lyrics");

                // Create the table. NOTE: Due to the length of song lyrics which can take up many characters,
                // I limited the number of characters shown in the row to 11 characters<Project Sdk="Microsoft.NET.Sdk">
                table.AddRow(item.Id, bandname, songname, item.SongLyrics.Substring(0, 10));
                table.Write(Format.Alternative);
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("Item does not exist.");
            }
        }
        static async Task Main(string[] args)
        {
            // Create while loop to loop through the menu
            var keepGoing = true;
            while (keepGoing)
            {
                Console.Write("Get (S)ong lyrics or (Q)uit: ");
                var choice = Console.ReadLine().ToUpper();

                switch (choice)
                {

                    case "S":
                        Console.Write("Enter the name of the band artist that recorded the song: ");
                        var bandname = Console.ReadLine();

                        Console.WriteLine($"Band Name: {bandname}");

                        Console.Write("Enter the name of a song by that artist: ");
                        var songname = Console.ReadLine();

                        Console.WriteLine($"Song Name: {songname}");

                        //await GetOneSongAsync(BandName, SongName);
                        await GetOneSongAsync(bandname, songname);

                        Console.WriteLine("Press ENTER to continue");
                        Console.ReadLine();
                        break;

                    case "Q":
                        keepGoing = false;
                        break;
                }
            }
        }
    }
}
