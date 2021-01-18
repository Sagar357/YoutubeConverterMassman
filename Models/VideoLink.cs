using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoutubeConverter.Models
{
    public class VideoLink
    {
        public VideoLink(string path)
        {
            this.videoLink = path;
            this.resolutionList = new List<int>();
        }
        public string videoLink { get; set; }
        public string thumb { get; set; }
        public string title { get; set; }
        public string duration { get; set; }
        public List<int> resolutionList { get; set; }
       
    }
    //public class videoDetail
    //{
    //    public int resolution { get; set; }
    //    public bool isExist { get; set; }
    //}
}