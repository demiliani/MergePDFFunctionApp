using MergePDFFunctionApp.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.IO;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace MergePDFFunctionApp
{
    public class MergePDFFunction
    {
        private readonly ILogger<MergePDFFunction> _logger;

        public MergePDFFunction(ILogger<MergePDFFunction> logger)
        {
            _logger = logger;
        }

        [Function("MergePDF")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            using StreamReader reader = new(req.Body);
            string requestBody = await reader.ReadToEndAsync();

            List<PDFDocument> documents = JsonConvert.DeserializeObject<List<PDFDocument>>(requestBody);

            string result = await MergeDocuments(documents);
            return new OkObjectResult(result);
        }

        private async Task<string> MergeDocuments(List<PDFDocument> documents)
        {
            try
            {
                PdfDocument outputDocument = new PdfDocument();

                foreach (PDFDocument document in documents)
                {
                    //Load the PDF document
                    var memoryStream = new MemoryStream(Convert.FromBase64String(document.Base64Content));
                    var inputDocument = PdfReader.Open(memoryStream, PdfDocumentOpenMode.Import);
                    //Gets the pages
                    int count = inputDocument.PageCount;
                    for (int i = 0; i < count; i++)
                    {
                        //append
                        outputDocument.AddPage(inputDocument.Pages[i]);
                    }
                }

                //Return the Base64 of the merged document
                using (var stream = new MemoryStream())
                {
                    //save result of merge
                    outputDocument.Save(stream);
                    return Convert.ToBase64String(stream.ToArray());
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
