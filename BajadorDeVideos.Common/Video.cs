namespace BajadorDeVideos.Common
{
    public class Video
    {
        public string Titulo { get; set; }
        public string Link { get; set; }

        public Video(string titulo, string link)
        {
            Titulo = titulo;
            Link = link;
        }
    }
}