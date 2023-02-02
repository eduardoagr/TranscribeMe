namespace TranscribeMe.Services {
    public class GeoServices {
        public HttpClient client {
            get; set;
        }

        public GeoServices() {
            client = new HttpClient();
        }

        public async Task<CurrentCity?> GetLocation(double lat, double lon) {
            var url = $"https://geocode.maps.co/reverse?lat={lat}&lon={lon}";

            var res = await client.GetAsync(url);

            if (res.IsSuccessStatusCode) {
                var geo = await res.Content.ReadFromJsonAsync<CurrentCity>();

                return geo;

            }

            return null;
        }
    }
}

