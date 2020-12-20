using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace ConsolePhotoApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();

            var user = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/users");

            var userBody = await user.Content.ReadAsStringAsync();

            var userObject = JsonConvert.DeserializeObject<List<User>>(userBody);

            var album = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/albums");

            var albumBody = await album.Content.ReadAsStringAsync();

            var albumObject = JsonConvert.DeserializeObject<List<Album>>(albumBody);

            var responce = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/photos");

            responce.EnsureSuccessStatusCode();

            var responceBody = await responce.Content.ReadAsStringAsync();

            var responceObject = JsonConvert.DeserializeObject<List<Photo>>(responceBody);
            foreach (var photoId in userObject.Where(userName => userName.Name == "Mrs. Dennis Schulist").SelectMany(userName => albumObject.Where(albumId => albumId.UserId == userName.Id).SelectMany(albumId => responceObject.Where(photoId => photoId.AlbumId == albumId.Id))))
            {
                System.Console.WriteLine($"Albumo id: {photoId.AlbumId}, Id:{photoId.Id}; url: {photoId.Url}");
            }
        }
    }
}