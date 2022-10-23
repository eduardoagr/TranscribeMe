namespace TranscribeMe.Helpers {
    public class AudioHelper {
        public string Converter(string FilePath, string filename) {

            var mp3 = new Mp3FileReader(FilePath);
            var ws = WaveFormatConversionStream.CreatePcmStream(mp3);
            WaveFileWriter.CreateWaveFile(filename, ws);

            return Path.GetFullPath(filename);
        }
    }
}
