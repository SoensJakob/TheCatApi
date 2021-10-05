using EO_JS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EO_JS.Repositories
{
    public class CatRepository
    {

        private const String _APIKEY = "61b1c494-dd04-47ae-9ee3-15d03739d14a";


        private static async Task<HttpClient> GetClientAsync()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("x-api-key", _APIKEY);

            return client;
        }

        public static async Task<List<Breed>> GetBreedsAsync()
        {
            using (HttpClient client = await GetClientAsync())
            {
                String url = "https://api.thecatapi.com/v1/breeds";
                String json = await client.GetStringAsync(url);

                if (json != null)
                {
                    List<Breed> breeds = JsonConvert.DeserializeObject<List<Breed>>(json);
                    
                    
                    return breeds;
                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task<BreedInfo> GetBreedByImageIdAsync(string id)
        {
            using (HttpClient client = await GetClientAsync())
            {
                String url = $"https://api.thecatapi.com/v1/breeds/{id}";
                String json = await client.GetStringAsync(url);

                if (json != null)
                {
                    BreedInfo breed = JsonConvert.DeserializeObject<BreedInfo>(json);
                    return breed;
                }
                else
                {
                    return null;
                }
            }
        }


        public static async Task<BreedInfo> GetBreedByIdAsync(string id)
        {
            using (HttpClient client = await GetClientAsync())
            {
                String url = $"https://api.thecatapi.com/v1/breeds/{id}";
                String json = await client.GetStringAsync(url);

                if (json != null)
                {
                    BreedInfo breed = JsonConvert.DeserializeObject<BreedInfo>(json);
                    return breed;
                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task<ImageCat> GetImageByBreedAsync(string breed)
        {
            using (HttpClient client = await GetClientAsync())
            {
                String url = $"https://api.thecatapi.com/v1/images/search?size=med&breed_ids={breed}&include_breeds=false";
                String json = await client.GetStringAsync(url);
                

                if (json != null)
                {
                    List<ImageCat> images = JsonConvert.DeserializeObject<List<ImageCat>>(json);
                    ImageCat image = images[0];
                    return image;
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<String> GetOriginsAsync(List<Breed> breeds)
        {
            
            
            List<String> origins = new List<string>();

            foreach (Breed b in breeds)
            {

                if (origins.Contains(b.Origin))
                {

                }
                else
                {
                    origins.Add(b.Origin);
                }

            }
            return origins;
        }

        public async static Task<List<Breed>> GetBreedsByOriginAsync(List<Breed> breeds, String origin)
        {

            
            List<Breed> breedsByOrigin = new List<Breed>();

            foreach (Breed b in breeds)
            {
                if (b.Origin == origin)
                {
                    b.Image = await CatRepository.GetImageByBreedAsync(b.Id);

                    breedsByOrigin.Add(b);
                }

            }
            return breedsByOrigin;
        }
        public static async Task<List<ImageCat>> GetImagesByBreedAsync(string breed)
        {
            using (HttpClient client = await GetClientAsync())
            {
                String url = $"https://api.thecatapi.com/v1/images/search?size=small&breed_ids={breed}&limit=10&include_breeds=false";
                String json = await client.GetStringAsync(url);


                if (json != null)
                {
                    List<ImageCat> images = JsonConvert.DeserializeObject<List<ImageCat>>(json);

                    return images;
                }
                else
                {
                    return null;
                }
            }
        }

        public static async Task<List<Favourite>> GetFavouritesAsync(String user)
        {
            using (HttpClient client = await GetClientAsync())
            {
                String url = $"https://api.thecatapi.com/v1/favourites?sub_id={user}";
                String json = await client.GetStringAsync(url);


                if (json != null)
                {
                    List<Favourite> favourites = JsonConvert.DeserializeObject<List<Favourite>>(json);
                    return favourites;
                }
                else
                {
                    return null;
                }
            }
        }


        public async static Task<String> AddFavouriteAsync(JObject jsonObject)
        {
            using (HttpClient client = await GetClientAsync())
            {
                String url = $"https://api.thecatapi.com/v1/favourites";
                try
                {
                    
                    String json = JsonConvert.SerializeObject(jsonObject);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);
                    var contents = await response.Content.ReadAsStringAsync();
                    Favourite favourite = JsonConvert.DeserializeObject<Favourite>(contents);

                    if (response.IsSuccessStatusCode == false)
                    {
                        string errorMsg = $"Unsuccesful POST to url: {url} --- object:{json}";
                        throw new Exception(errorMsg);
                    }
                    return favourite.FavouriteId ;
                }
                catch (Exception ex)
                {
                    string errorMsg = $"Unsuccesful POST to url: {url} --- {ex.Message}";
                    throw new Exception(errorMsg);
                }

            }

        }

        public async static Task DeleteFavouriteAsync(String id)
        {
            using (HttpClient client = await GetClientAsync())
            {
                String url = $"https://api.thecatapi.com/v1/favourites/{id}";
                try
                {
                    var response = await client.DeleteAsync(url);
                    if (response.IsSuccessStatusCode == false)
                    {
                        string errorMsg = $"Unsuccesful Delete to url: {url}";
                        throw new Exception(errorMsg);
                    }
                }
                catch (Exception ex)
                {
                    string errorMsg = $"Unsuccesful POST to url: {url} --- {ex.Message}";
                    throw new Exception(errorMsg);
                }

            }

        }

        public static async Task<UserInfo> GetUserInformation(String name)
        {
            using (HttpClient client = await GetClientAsync())
            {
                String url = $"https://eindopdrachtdp.azurewebsites.net/api/users/{name}";
                String json = await client.GetStringAsync(url);


                if (json != null)
                {

                    List<UserInfo> userInfo = JsonConvert.DeserializeObject<List<UserInfo>>(json);
                    UserInfo user = new UserInfo();
                    if (userInfo.Count == 0)
                    {
                        user = null;
                    }
                    else
                    {
                        user = userInfo[0];
                    }
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        public async static Task AddUser(UserInfo user)
        {
            using (HttpClient client = await GetClientAsync())
            {
                String url = $"https://eindopdrachtdp.azurewebsites.net/api/cat/register";
                try
                {
                    
                    String json = JsonConvert.SerializeObject(user);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);
                    var contents = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode == false)
                    {
                        string errorMsg = $"Unsuccesful POST to url: {url} --- object:{json}";
                        throw new Exception(errorMsg);
                    }
                }
                catch (Exception ex)
                {
                    string errorMsg = $"Unsuccesful POST to url: {url} --- {ex.Message}";
                    throw new Exception(errorMsg);
                }

            }

        }


    }
}
