using System;
using System.Net;
using System.Net.Http;
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
            Console.WriteLine("Descargando Codigo Fuente...");
            return client.DownloadString(url);
        }


        List<Video> IPlugin.getVideoList(string sourceCode)
        {
            HtmlDocument htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(sourceCode);
            var videoNodes = htmlDoc.DocumentNode.SelectNodes("//span/a");
            List<Video> videoList = new List<Video>();

            Console.WriteLine("Creando Lista de Videos");
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

            HttpClient client = new HttpClient();
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlVideo = web.Load(video.Link);
            var iframeSource = htmlVideo.DocumentNode.SelectSingleNode("//iframe").GetAttributeValue("src", string.Empty);
            if (!iframeSource.Contains("https:"))
            {
                iframeSource = "https:" + iframeSource;
            }
            htmlVideo = web.Load(iframeSource);
            string htmlScript = htmlVideo.DocumentNode.SelectSingleNode("//body").InnerHtml;
            int start = htmlScript.IndexOf("https://embed");
            int end = htmlScript.IndexOf('"', start);
            string videoUrl = htmlScript.Substring(start, end - start);
            byte[] archivo = new byte[0];
            return archivo = client.GetByteArrayAsync(videoUrl).Result;

        }

        public int DisplayMenu()
        {
            int input;

            Console.WriteLine();
            Console.WriteLine("0. Salir");
            Console.WriteLine("1. Descargar de https://www.commoncraft.com");
            Console.WriteLine("2. Descargar de GoGoAnime [SIN IMPLEMENTAR]");
            Console.WriteLine("3. Ingresar url propia (Puede no funcionar)[SIN IMPLEMENTAR]");
            return input = Convert.ToInt32(Console.ReadLine());
        }

        public string SelectMenu()
        {
            int input;
            string url;

            do
            {
                input = DisplayMenu();
            } while (input < 0 || input > 3);

            if (input == 0)
            {
                url = null;
            }

            else
            if (input == 1)
            {
                return url = "https://www.commoncraft.com/videolist";
            }

            else
            if (input == 2)
            {
                return url = "https://ww3.gogoanime.io/category/dragon-ball-kai";
            }

            else
            if (input == 3)
            {
                Console.WriteLine("Ingrese la url que desea: ");
                return url = Console.ReadLine();
            }
            return null;
        }
    }
}
