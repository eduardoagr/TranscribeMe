namespace TranscribeMe.Helpers {
    public class AudioHelper {
        public static string mp3ToWav(string FilePath, string filename) {

            var mp3 = new Mp3FileReader(FilePath);
            var ws = WaveFormatConversionStream.CreatePcmStream(mp3);
            WaveFileWriter.CreateWaveFile(filename, ws);

            return Path.GetFullPath(filename);
        }

        public static string mp4ToWav(string videoPath, string wavPath) {

            var wavFile = Path.ChangeExtension(videoPath, ".wav");
            using (var reader = new MediaFoundationReader(videoPath))
            using (var resampler = new MediaFoundationResampler(
                reader, new WaveFormat(48000, 16, 2)))
            using (var writer = new WaveFileWriter(wavFile, resampler.WaveFormat)) {
                var buffer = new byte[resampler.WaveFormat.SampleRate * resampler.WaveFormat.Channels];
                int read;
                while ((read = resampler.Read(buffer, 0, buffer.Length)) > 0) {
                    writer.Write(buffer, 0, read);
                }
            }
            return wavFile;
        }
    }
}
