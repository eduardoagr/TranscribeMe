using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace TranscribeMe.Services {
    [AddINotifyPropertyChangedInterface]
    public class AzureOcrService {
        public static async Task<string> GiveTextAsync(string ImagePath) {
            ComputerVisionClient client = new
                (new ApiKeyServiceClientCredentials(
                    ConstantsHelpers.AZURE_COMPUTER_VISION_KEY)) { Endpoint = ConstantsHelpers.AZURE_COMPUTER_VISION_URL };

            using Stream imageStream = File.OpenRead(ImagePath);
            OcrResult ocrResult = await client.RecognizePrintedTextInStreamAsync(
                detectOrientation: true, imageStream);

            string extractedText = string.Empty;

            if (ocrResult.Regions.Count > 0) {
                foreach (OcrLine line in ocrResult.Regions[0].Lines) {
                    foreach (OcrWord word in line.Words) {
                        extractedText += word.Text + " ";
                    }
                    extractedText += "\n";
                }
            }
            return extractedText;
        }
    }
}