using System;
using System.Collections.Generic;
using System.IO;
using BajadorDeVideos.Common;
using BajadorDeVideos.PluginEjemplo;
using HtmlAgilityPack;

namespace BajadorDeVideos.Consola
{   
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Agregar soporte para:
                - COMMONCRAFT
                - baseUrl: https://www.commoncraft.com/videolist
                - como listar:
                    - buscar todos los links que estan dentro de un <span class="field-content">
                - como bajar video:
                    - obtener la pagina asociada a cada video
                    - Obtener la direccion del primer IFRAME que aparece en la pagina, donde se encuentra el reproductor
                    - De esa URL del iframe, dentro del codigo de inicializacion del player de video, bajar alguna de las URLs indicadas como "url":"https://embed....."


                - GOGOANIME
                    - baseUrl: direccion original de serie (ej, https://ww3.gogoanime.io/category/dragon-ball-kai)
                    - como listar: 
                        - Obtener del HTML que devuelve, el campo movie_id 
                        - Obtener de la URL https://ww3.gogoanime.io/load-list-episode?ep_start=0&ep_end=10000&id=MOVIE_ID el listado de episodios (reemplazando MOVIE_ID por el numero obtenido)
                    - como bajar video:
                        - obtener la pagina asociada a cada video
                        - Obtener la direccion del primer IFRAME que aparece en la pagina, donde se encuentra el reproductor
                        - Obtener la segunda url del iframe
                        - De esa URL del iframe, obtener la URL del video (tag source, dentro de video, el que diga type="video/mp4")
             */

            //args[0] = "https://www.commoncraft.com/videolist";
            //if (args.Length != 1)
            //{
            //    Console.WriteLine("Se requiere un unico parametro 'baseUrl'");
            //    return;
            //}

            string url = "https://www.commoncraft.com/videolist";


            IPlugin plugin;
            plugin = new DummyPlugin();

            //Consigo el codigo fuente de una url dada
            string sourceCode = plugin.GetSourceCode(url);

            //Consigo una lista de objetos Video del codigo fuente conseguido
            List<Video> videoList = plugin.getVideoList(sourceCode);

            //Listo los videos por consola y le pido al usuario que elija uno
            Video selectedVideo = plugin.ListVideos(videoList);

            byte[] archivo = plugin.Bajar(selectedVideo);

            File.WriteAllBytes(selectedVideo.Titulo + ".mp4", archivo);
        }


            //List<Video> listaVideos = plugin.ListarVideosDisponibles(url);

            //if (listaVideos.Count > 0)
            //{
            //    byte[] contenido = plugin.Bajar(listaVideos[0]);
            //    File.WriteAllBytes("video.txt", contenido);
            //}
    }
}
