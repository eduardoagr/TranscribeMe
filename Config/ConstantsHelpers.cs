namespace Config {
    public static class ConstantsHelpers {

        //syncfusion
        public const string SYNCFUSION_KEY = "NzMxMzU0QDMyMzAyZTMzMmUzME5ScGlpRzZBZVl5SUpiVllvQjBNbXU1WTIwK0pPMFl4NVF6OGdIV09YQ2s9";

        //Azure
        public const string AZURE_KEY = "c5ba659727324477b4b34e14bec0cee6";
        public const string AZURE_REGION = "westeurope";

        //Azure blob storae
        public const string AZUTE_STORAGE_ACCOUNT_NAME = "transcribemedocs";
        public const string AZUTE_STORAGE_ACCOUNT_KEY = "ejnleMkkJISfu0AwUxiJxmgyGmj3v91IH+fgbW/ODnkq1M+lotP/yoPh9ULCHLHdf17d9NdcXtxP+AStLv6/PA==";
        public const string AZURE_UPLOAD_DOUMMENRTS = "https://transcribemedocs.blob.core.windows.net/original";
        public const string AZURE_DOWNLOAD_DOCUMENTS = "https://transcribemedocs.blob.core.windows.net/translated";
        public const string AZURE_STORAGE_CONNECTIONSTRING = "DefaultEndpointsProtocol=https;AccountName=transcribemedocs;AccountKey=ejnleMkkJISfu0AwUxiJxmgyGmj3v91IH+fgbW/ODnkq1M+lotP/yoPh9ULCHLHdf17d9NdcXtxP+AStLv6/PA==;EndpointSuffix=core.windows.net";

        //File Dialog
        public const string AUDIOFILES = "Audio Files|*.mp3;";
        public const string VIDEOFILES = "Video Files|*.mp4;";
        public const string DOCUMENTSFIILES = "Document Files|*.pdf;*.doc;*.docx";
        public const string IMAGEFILES = "Image Files|*.jpeg;*.png;";

        //Name for the type of files  and folders
        public const string IMAGES = "Images";
        public const string AUDIO = "Audio";
        public const string VIDEO = "Video";
        public const string DOCUMENTS = "Documents";
        public const string TRANDCRIBED = "Transcribed";
        public const string TRANSLATIONS = "Documents translations";
        public const string TRANSCRIPTIONS = "Transcriptions";

        //Azure Translate
        public const string ENDPOINT = "https://docstranslate.cognitiveservices.azure.com/";
        public const string KEY = "eaa01f1f83324b52b82681ce40a2695e";

        //Azure blob container Names
        public static string AZURE_CONTAINER_ORIGINAL_DOCUMENT = "original";
        public static string AZURE_CONTAINER_TRANSLATED_DOCUMENT = "translated";
    }
}