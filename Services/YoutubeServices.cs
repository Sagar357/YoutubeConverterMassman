using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace YoutubeConverter.Services
{
    public class YoutubeServices
    {
        public void ProcessLink(string link)
        {
            try
            {
                Uri videoUri = new Uri(link);
                string videoID = HttpUtility.ParseQueryString(videoUri.Query).Get("v");
                string videoInfoUrl = "https://www.youtube.com/get_video_info?video_id="+
                    videoID;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(videoInfoUrl);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream,
                    Encoding.GetEncoding("utf-8"));

                string videoInfo = HttpUtility.HtmlDecode(reader.ReadToEnd());

                NameValueCollection videoParams = HttpUtility.ParseQueryString(videoInfo);

                if (videoParams["reason"] != null)
                {
                    string reason = videoParams["reason"];
                    return;
                }

                Dictionary<string, string> keyPair = new Dictionary<string, string>();

                foreach (string element in videoParams.AllKeys)
                {
                    keyPair.Add(element ,videoParams[element]);
                }


                string[] videoURLs = videoParams["url_encoded_fmt_stream_map"].Split(',');
                foreach (string vURL in videoURLs)
                {
                    string sURL = HttpUtility.HtmlEncode(vURL);

                    NameValueCollection urlParams = HttpUtility.ParseQueryString(sURL);
                    string videoFormat = HttpUtility.HtmlDecode(urlParams["type"]);

                    sURL = HttpUtility.HtmlDecode(urlParams["url"]);
                    sURL += "&signatre=" + HttpUtility.HtmlDecode(urlParams["sig"]);
                    sURL += "&type=" + videoFormat;
                    sURL += "&title=" + HttpUtility.HtmlDecode(videoParams["title"]);

                    videoFormat = urlParams["quality"] + " - " + videoFormat.Split(';')
                        [0].Split('/')[1];

                    Dictionary<string ,string>  ddlVideoFormats = new Dictionary<string, string>();
                    ddlVideoFormats.Add(videoFormat, sURL);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}