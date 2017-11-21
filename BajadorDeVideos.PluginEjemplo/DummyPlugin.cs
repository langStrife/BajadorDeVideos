﻿using System;
using System.Net;
using HtmlAgilityPack;
using System.Collections.Generic;
using BajadorDeVideos.Common;
using Newtonsoft.Json;

namespace BajadorDeVideos.PluginEjemplo
{
    //#quicktabs-tabpage-cc_video_quicktab-5 > div > div.view-content

    public class DummyPlugin : IPlugin
    {

        public string GetSourceCode(string url)
        {
            WebClient client = new WebClient();
            return client.DownloadString(url);
        }


        List<Video> IPlugin.getVideoList(string sourceCode)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(sourceCode);
            var videoNodes = htmlDoc.DocumentNode.SelectNodes("//span/a");
            List<Video> videoList = new List<Video>();

            foreach (var node in videoNodes)
            {
                if (node.Attributes["href"].Value.Contains("/video"))
                {
                    string nombre = node.InnerText; // devuelve el nombre del video
                    string videoUrl = "https://www.commoncraft.com" + node.Attributes["href"].Value;
                    videoList.Add(new Video(nombre, videoUrl));
                }
            }
            return videoList;
        }

        public Video ListVideos(List<Video> videoList)
        {
            using (IEnumerator<Video> enumVideo = videoList.GetEnumerator())
            {
                int cont = 0;
                while (enumVideo.MoveNext())
                {
                    Console.WriteLine();
                    Video vid = enumVideo.Current;
                    Console.WriteLine("Nombre: " + vid.Titulo);
                    Console.WriteLine("Link: " + vid.Link);
                    Console.WriteLine("Numero de Video: " + cont);
                    cont++;
                    Console.WriteLine();
                }

                Console.WriteLine("Ingrese el numero del video que desea descargar");
                int selectedVideo;

                while (!int.TryParse(Console.ReadLine(), out selectedVideo) || selectedVideo < 0 || selectedVideo >= cont)
                {
                    Console.WriteLine();
                    Console.WriteLine("ERROR: El valor debe ser mayor a 0 y menor a " + cont);
                    Console.WriteLine("Intentelo Nuevamente");
                }
                return videoList[selectedVideo];
            }
        }

        public byte[] Bajar(Video video)
        {
            WebClient client = new WebClient();
            HtmlDocument htmlDoc = new HtmlDocument();
            var sourceCode = GetSourceCode(video.Link);
            htmlDoc.LoadHtml(sourceCode);


            var nodeFrame = htmlDoc.DocumentNode.SelectSingleNode("//video");
            var src = nodeFrame.Attributes["src"].Value;
            int start = src.IndexOf("https://embed");
            int end = src.IndexOf('"', start);
            string videoUrl = src.Substring(start, end - start);

            var archivo = new byte[0];
            return archivo = client.DownloadData(videoUrl);
            
        }
    }
}