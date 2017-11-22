using System.Collections.Generic;

namespace BajadorDeVideos.Common
{
    public interface IPlugin
    {
        string GetSourceCode(string url);
        Video ListVideos(List<Video> videoList);
        List<Video> getVideoList(string urlListado);
        byte[] Bajar(Video video);
        int DisplayMenu();
        string SelectMenu();
    }
}