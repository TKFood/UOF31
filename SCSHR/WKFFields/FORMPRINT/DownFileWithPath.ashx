<%@ WebHandler Language="C#" Class="DownFileWithPath" %>

using System;
using System.IO;
using System.Web;

public class DownFileWithPath : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            string filepath = context.Request["filepath"];
            JGlobalLibs.DebugLog.Log(string.Format(@"DownFileWithPath filepath = {0}", filepath));
            filepath = HttpUtility.UrlDecode(filepath);
            JGlobalLibs.DebugLog.Log(string.Format(@"DownFileWithPath decode filepath = {0}", filepath));

            if (!string.IsNullOrEmpty(filepath) && File.Exists(filepath))
            {
                string filename = Path.GetFileName(filepath);
                if (context.Request.Browser.Browser == "InternetExplorer")
                {
                    filename = HttpUtility.UrlEncode(filename);
                    //filepath = context.Server.UrlPathEncode(filepath);
                }
                context.Response.Clear();
                context.Response.ContentType = "application/octet-stream";
                context.Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                context.Response.WriteFile(filepath);
                context.Response.Flush();

            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("File not be found!");


            }
            //FileInfo fi = new FileInfo(filepath);

            //long filesize = fi.Length;

            //using (Stream s = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize))
            //{
            //    byte[] buffer = new byte[bufferSize];
            //    int count = 0;
            //    int offset = 0;

            //    if (context.Request.Browser.Browser == "InternetExplorer" || context.Request.Browser.Browser == "IE" || context.Request.Browser.Browser == "Mozilla")
            //    {
            //        filepath = context.Server.UrlPathEncode(filepath);
            //    }

            //    context.Response.AddHeader("content-disposition", "attachment;filename=\"" + Path.GetFileName(filepath) + "\"");
            //    context.Response.ContentType = contentType;
            //    context.Response.AddHeader("Content-Length", filesize.ToString());

            //    while ((count = s.Read(buffer, offset, buffer.Length)) > 0)
            //    {
            //        context.Response.OutputStream.Write(buffer, offset, count);
            //        context.Response.Flush();
            //    }


            //}
        }
        catch (Exception ex)
        {
            JGlobalLibs.DebugLog.Log(string.Format(@"DownFileWithPath Error: {0}", ex.Message));
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