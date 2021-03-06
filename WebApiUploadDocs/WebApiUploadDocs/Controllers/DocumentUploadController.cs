﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using WebApiUploadDocs.Models;

namespace WebApiUploadDocs.Controllers
{
    public class DocumentUploadController : ApiController
    {
        /// <summary>
        /// Upload Document.....
        /// </summary>      
        /// <returns></returns>
        [HttpPost]
        [Route("api/DocumentUpload/MediaUpload")]
        public async Task<HttpResponseMessage> MediaUpload()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            //access form data
            NameValueCollection formData = provider.FormData;
            //access files
            IList<HttpContent> files = provider.Files;

            HttpContent file1 = files[0];
            var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');

            ////-------------------------------------For testing----------------------------------
            //to append any text in filename.
            //var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"') + DateTime.Now.ToString("yyyyMMddHHmmssfff"); //ToDo: Uncomment this after UAT as per Jeeevan

            //List<string> tempFileName = thisFileName.Split('.').ToList();
            //int counter = 0;
            //foreach (var f in tempFileName)
            //{
            //    if (counter == 0)
            //        thisFileName = f;

            //    if (counter > 0)
            //    {
            //        thisFileName = thisFileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + f;
            //    }
            //    counter++;
            //}

            ////-------------------------------------For testing----------------------------------

            string filename = String.Empty;
            Stream input = await file1.ReadAsStreamAsync();
            string directoryName = String.Empty;
            string URL = String.Empty;
            string tempDocUrl = WebConfigurationManager.AppSettings["DocsUrl"];

            if (formData["ClientDocs"] == "ClientDocs")
            {
                var path = HttpRuntime.AppDomainAppPath;
                directoryName = System.IO.Path.Combine(path, "ClientDocument");
                filename = System.IO.Path.Combine(directoryName, thisFileName);

                //Deletion exists file
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                string DocsPath = tempDocUrl + "/" + "ClientDocument" + "/";
                URL = DocsPath + thisFileName;

            }


            //Directory.CreateDirectory(@directoryName);
            //using (Stream file = File.OpenWrite(filename))
            using (Stream file = File.OpenWrite(HttpContext.Current.Server.MapPath("~/ClientDocument/test.png")))
            {
                input.CopyTo(file);
                //close file
                file.Close();
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("DocsUrl", URL);
            return response;
        }

    }
}
