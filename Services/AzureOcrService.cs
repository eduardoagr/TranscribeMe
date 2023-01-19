using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using TranscribeMe.Pages;

namespace TranscribeMe.Services {
    [AddINotifyPropertyChangedInterface]
    internal class AzureOcrService {
        public static async Task<string> GiveTextAsync(string ImagePath) {

            ComputerVisionClient client = new
                (new ApiKeyServiceClientCredentials(ConstantsHelpers.AZURE_COMPUTER_VISION_KEY))
            { Endpoint = ConstantsHelpers.AZURE_COMPUTER_VISION_URL };

            using Stream imageStream = File.OpenRead(ImagePath);
            OcrResult ocrResult = await client.RecognizePrintedTextInStreamAsync(true, imageStream);

            string extractedText = string.Empty;
            foreach (OcrLine line in ocrResult.Regions[0].Lines) {
                foreach (OcrWord word in line.Words) {
                    extractedText += word.Text + " ";
                }
                extractedText += "\n";
            }
            return extractedText;
        }
    }
}
