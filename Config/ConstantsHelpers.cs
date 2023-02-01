namespace Config {
    public static class ConstantsHelpers {

        //Syncfusion
        public const string SYNCFUSION_KEY = "NzMxMzU0QDMyMzAyZTMzMmUzME5ScGlpRzZBZVl5SUpiVllvQjBNbXU1WTIwK0pPMFl4NVF6OGdIV09YQ2s9";

        //Azure speech recognition
        public const string AZURE_SPEECH_KEY = "3ea6fbe9f32f4c2093423008f28075c3";
        public const string AZURE_SPEECH_REGION = "westeurope";

        //Bing spell checker
        public const string BING_SPELL_KEY = "f9bd3363f523405bbddd8bab4830180b";
        public const string BING_SPELL_URL = "https://api.bing.microsoft.com/v7.0/spellcheck/";

        //Azure blob storage
        public const string AZUTE_STORAGE_ACCOUNT_NAME = "transcribemedocs";
        public const string AZUTE_STORAGE_ACCOUNT_KEY = "ejnleMkkJISfu0AwUxiJxmgyGmj3v91IH+fgbW/ODnkq1M+lotP/yoPh9ULCHLHdf17d9NdcXtxP+AStLv6/PA==";
        public const string AZURE_UPLOAD_DOUMMENRTS = "https://transcribemedocs.blob.core.windows.net/original";
        public const string AZURE_DOWNLOAD_DOCUMENTS = "https://transcribemedocs.blob.core.windows.net/translated";
        public const string AZURE_DOWNLOAD_SHARE_DOCUMENTS = "cloudfilesshare";
        public const string AZURE_STORAGE_CONNECTIONSTRING = "DefaultEndpointsProtocol=https;AccountName=transcribemedocs;AccountKey=ejnleMkkJISfu0AwUxiJxmgyGmj3v91IH+fgbW/ODnkq1M+lotP/yoPh9ULCHLHdf17d9NdcXtxP+AStLv6/PA==;EndpointSuffix=core.windows.net";

        //Azure computer vision 
        public const string AZURE_COMPUTER_VISION_KEY = "7f7196a8f4d446089650733ab4b17d3b";
        public const string AZURE_COMPUTER_VISION_REGION = "westeurope";
        public const string AZURE_COMPUTER_VISION_URL = $"https://imgextract.cognitiveservices.azure.com/";

        //File Dialog
        public const string AUDIOFILES = "Audio Files (mp3)|*.mp3;";
        public const string VIDEOFILES = "Video Files (mp4)|*.mp4;";
        public const string DOCUMENTSFIILES = "Document Files (pdf, doc, docx)|*.pdf;*.doc;*.docx";
        public const string IMAGEFILES = "Image Files (jpeg, png)|*.jpeg;*.png;";

        //Name for the type of files folders
        public const string IMAGES = "Images";
        public const string AUDIOS = "Audios";
        public const string VIDEO = "Video";
        public const string DOCUMENTS = "Documents";

        //Name of folders
        public const string TRANDCRIBED = "Transcribed";
        public const string TRANSLATIONS = "Documents translations";
        public const string TRANSCRIPTIONS = "Transcriptions";
        public const string IMAGETEXT = "Image Text";

        //Azure Translate
        public const string ENDPOINT = "https://docstranslate.cognitiveservices.azure.com/";
        public const string KEY = "eaa01f1f83324b52b82681ce40a2695e";

        //Azure blob container Names
        public static string AZURE_CONTAINER_ORIGINAL_DOCUMENT = "original";
        public static string AZURE_CONTAINER_TRANSLATED_DOCUMENT = "translated";

        //File extensions
        public static string WAV = ".wav";
        public static string MP3 = ".mp3";
        public static string MP4 = ".mp4";
        public static string DOCX = ".docx";

    }
}