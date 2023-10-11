namespace net_il_mio_fotoalbum.Models
{
    public class Photo
    {
        public Photo() { }

        public Photo(string title, string description, string url, bool visible)
        {
            Title = title;
            Description = description;
            Url = url;
            Visible = visible;
        }

        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public bool Visible { get; set; }

        public List<Category>? Categories { get; set; }
    }
}
