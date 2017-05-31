using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using System.Linq;
using System.Threading.Tasks;

namespace SC2Bot.WebSocket.Helpers
{
    public class ImgurHelper
    {
        public async Task<string> GetARandomImageAlbum(string IDAlbum)
        {
            var client = new ImgurClient(Infos.ImgurAPI);
            var endpoint = new AlbumEndpoint(client);
            var images = await endpoint.GetAlbumImagesAsync(IDAlbum);

            int id = GeneralHelper.GetRandom(images.Count());

            return images.ElementAt(id).Link;            
        }
    }
}
