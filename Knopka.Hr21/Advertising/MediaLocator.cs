using System;
using System.Collections.Generic;
using System.IO;

namespace Knopka.Hr21.Advertising {

    public class MediaLocator {

        private readonly Dictionary<string, HashSet<string>> locationToMediaMap = new Dictionary<string, HashSet<string>>();
        public MediaLocator(Stream inputStream) {
            using (StreamReader reader = new StreamReader(inputStream)) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line)) {
                        string[] parts = line.Split(':');
                        if (parts.Length == 2) {
                            string media = parts[0].Trim();
                            string[] locations = parts[1].Split(',');
                            foreach (string location in locations)
                                AddMediaToLocation(location.Trim(), media);
                        }
                    }
                }
            }
        }
        private void AddMediaToLocation(string location, string media) {
            if (!locationToMediaMap.ContainsKey(location)) {
                locationToMediaMap[location] = new HashSet<string>();
            }
            locationToMediaMap[location].Add(media);
        }
        public IEnumerable<string> GetMediasForLocation(string location) {
            List<string> result = new List<string>();
            string[] locationParts = location.Split('/');
            for (int i = locationParts.Length; i > 0; i--) {
                string currentLocation = string.Join("/", locationParts, 0, i);
                if (locationToMediaMap.ContainsKey(currentLocation)) {
                    result.AddRange(locationToMediaMap[currentLocation]);
                }
            }
            return result;
        }
    }
}