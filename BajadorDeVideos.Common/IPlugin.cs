using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace BajadorDeVideos.Common
{
    public interface IPlugin
    {
        string GetSourceCode(string url);
        Video ListVideos(List<Video> videoList);
        List<Video> getVideoList(string urlListado);
        byte[] Bajar(Video video);
    }
}