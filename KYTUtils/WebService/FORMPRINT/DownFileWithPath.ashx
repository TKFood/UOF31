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
            //JGlobalLibs.DebugLog.Log(string.Format(@"DownFileWithPath filepath = {0}", filepath));
            filepath = HttpUtility.UrlDecode(filepath);
            //JGlobalLibs.DebugLog.Log(string.Format(@"DownFileWithPath decode filepath = {0}", filepath));

            if (!string.IsNullOrEmpty(filepath) && File.Exists(filepath))
            {
                string filename = Path.GetFileName(filepath);
                if (context.Request.Browser.Browser == "InternetExplorer")
                {
                    filename = HttpUtility.UrlEncode(filename);
                    //filepath = context.Server.UrlPathEncode(filepath);
                }
                //JGlobalLibs.DebugLog.Log(string.Format(@"DownFileWithPat.Browser = {0}", context.Request.Browser.Browser));


                using (FileStream fs = new FileStream(filepath, FileMode.Open))
                {
                    context.Response.Clear();
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.AddHeader("content-disposition", "attachment;filename=" + filename);

                    long filesize = 0;
                    filesize = fs.Length;
                    byte[] buffer = new byte[(int)filesize];
                    fs.Read(buffer, 0, (int)filesize);
                    context.Response.BinaryWrite(buffer);
                    context.Response.Flush();
                }

            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("File not be found!");


            }
        }
        catch (Exception ex)
        {
            //JGlobalLibs.DebugLog.Log(string.Format(@"DownFileWithPath Error: {0}", ex.Message));
            throw new MissingMethodException(string.Format(@"DownFileWithPath Error: {0}", ex.Message));
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