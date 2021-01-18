using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace YoutubeConverter.Downloads
{
    /// <summary>
    /// Summary description for download
    /// </summary>
    public class download : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string file = context.Request.QueryString["file"];

            try
            {

                if (!string.IsNullOrEmpty(file) && File.Exists(context.Server.MapPath(file)))
                {
                    context.Response.Clear();
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.AddHeader("content-disposition", "attachment;filename=" + Path.GetFileName(file));
                    context.Response.WriteFile(context.Server.MapPath(file));
                    // This would be the ideal spot to collect some download statistics and / or tracking  
                    // also, you could implement other requests, such as delete the file after download  
                    context.Response.End();

                }
                else
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("File not be found!");


                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                //string name = Path.GetDirectoryName(file).Split('\\')[1];
                //System.IO.DirectoryInfo di = new DirectoryInfo(name);

                //foreach (FileInfo existingfile in di.GetFiles())
                //{
                //    existingfile.Delete();
                //}
                //foreach (DirectoryInfo dir in di.GetDirectories())
                //{
                //    dir.Delete(true);
                //}
            }
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}