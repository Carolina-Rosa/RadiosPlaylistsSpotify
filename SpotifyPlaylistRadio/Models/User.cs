namespace SpotifyPlaylistRadio.Models
{
    public class User
    {
        string country;
        public string display_name;
        string email;
        string href;
        public string id;
        string product;
        string type;
        string uri;

        public User(string country, string display_name, string email, string href, string id, string product, string type, string uri)
        {
            this.country = country;
            this.display_name = display_name;
            this.email = email;
            this.href = href;
            this.id = id;
            this.product = product;
            this.type = type;
            this.uri = uri;
        }
    }
}

