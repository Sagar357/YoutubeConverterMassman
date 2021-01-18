using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YoutubeConverter.Models;
using VideoLibrary;
using System.Threading;
using System.IO;
using YoutubeConverter.Services;

namespace YoutubeConverter.Controllers
{
    public class HomeController : Controller
    {
        [HandleError]
        public ActionResult Index()
        {
            VideoLink link = new VideoLink("");
            return View(link);
        }

        public ActionResult Index1()
        {
            VideoLink link = new VideoLink("");
            return View(link);
        }

        public string getYouTubeThumbnail(string YoutubeUrl)
        {
            string youTubeThumb = string.Empty;
            if (YoutubeUrl == "")
                return "";

            if (YoutubeUrl.IndexOf("=") > 0)
            {
                youTubeThumb = YoutubeUrl.Split('=')[1];
            }
            else if (YoutubeUrl.IndexOf("/v/") > 0)
            {
                string strVideoCode = YoutubeUrl.Substring(YoutubeUrl.IndexOf("/v/") + 3);
                int ind = strVideoCode.IndexOf("?");
                youTubeThumb = strVideoCode.Substring(0, ind == -1 ? strVideoCode.Length : ind);
            }
            else if (YoutubeUrl.IndexOf('/') < 6)
            {
                youTubeThumb = YoutubeUrl.Split('/')[3];
            }
            else if (YoutubeUrl.IndexOf('/') > 6)
            {
                youTubeThumb = YoutubeUrl.Split('/')[1];
            }

            return "http://img.youtube.com/vi/" + youTubeThumb + "/mqdefault.jpg";
        }

        [HttpPost]
        public JsonResult GetResolutions(string videoLink)
        {
            YoutubeServices services = new YoutubeServices();
         //   services.ProcessLink(videoLink);

            VideoLink link = new VideoLink(videoLink);
            var youTube = YouTube.Default;
            var video = youTube.GetAllVideos(videoLink);
            var v = video.FirstOrDefault();
            link.title = v.Title;
            link.duration = "00:00";
            link.thumb = getYouTubeThumbnail(videoLink);
            foreach (var vid in video)
            {
                if(vid.AudioBitrate>-1)
                link.resolutionList.Add(vid.Resolution);
                //if (vid.AudioBitrate == -1)
                //    link.resolutionList.isExist = false;
                //else
                //    link.isExist = true;
            }
            JsonResult result = new JsonResult();
            result = Json(link);

            return (result);
        }
        public string ExecuteThread(string videoLink ,int resolution)
        {
            var youTube = YouTube.Default; // starting point for YouTube actions
            //var video = youTube.GetVideo(videoLink); // gets a Video object with info about the video
             var vi = youTube.GetAllVideos(videoLink);
            var video = vi.FirstOrDefault(v => v.Resolution == resolution);
            //string uri = video.Uri;
            string _path = Path.Combine(Server.MapPath(@"~/Downloads/" ) ,video.FullName);
                //@"C:\Users\Comp\Desktop\mycodes\" + video.FullName;
            byte[] response = video.GetBytes();
           
            System.IO.File.WriteAllBytes(_path, response);
            return (@"~/Downloads/"+ video.FullName);
        }
        [HttpPost]
        public JsonResult Download(string videoLink ,int resolution)
        {
           
            string _path= ExecuteThread(videoLink , resolution);
            JsonResult result = new JsonResult();
            result = Json(new VideoLink(_path));
            return (result);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}