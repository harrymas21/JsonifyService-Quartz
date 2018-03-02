using JsonifySVC.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace JsonifySVC.Job
{
    public class JsonJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            //simulating querying and getting data
            List<SampleData> data = new List<SampleData>();
            data.Add(new SampleData() { Id = 1, SSN = 64654, Message = "A jgghfgh jhghg" });
            data.Add(new SampleData() { Id = 2, SSN = 255546, Message = "A j uuufu" });
            data.Add(new SampleData() { Id = 3, SSN = 26564654, Message = "A jgyg uyyugu" });
            data.Add(new SampleData() { Id = 4, SSN = 26549872, Message = "A jhgu yguyg y" });
            data.Add(new SampleData() { Id = 5, SSN = 54679132, Message = "A jhgyu uyguyg" });

            //fetched required data and convert to json
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(data);

            //write data to file
            string fileName = DateTime.Now.ToString("yyyyMMdd") + "SSN_messages";
            string filePath = @"D:\JSON_FILES\" + fileName + ".json";

            //For methods that are inherently synchronous, you need to wrap them in your own Task so you can await it.
            await Task.Run(()=>
            {
                File.WriteAllText(filePath, json);
                ////start FTP - SENDING
                //FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://www.contoso.com/test.htm");
                //request.Method = WebRequestMethods.Ftp.UploadFile;

                //// This example assumes the FTP site uses anonymous logon.  
                //request.Credentials = new NetworkCredential("anonymous", "janeDoe@contoso.com");

                //// Copy the contents of the file to the request stream.  
                //StreamReader sourceStream = new StreamReader(filePath);
                //byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                //sourceStream.Close();
                //request.ContentLength = fileContents.Length;

                //Stream requestStream = request.GetRequestStream();
                //requestStream.Write(fileContents, 0, fileContents.Length);
                //requestStream.Close();

                //FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                //Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

                //response.Close();
            }
            );
        }

    }
}
