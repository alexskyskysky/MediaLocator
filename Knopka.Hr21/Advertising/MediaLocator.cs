using System;
using System.Collections.Generic;
using System.IO;

namespace Knopka.Hr21.Advertising
{
    public class MediaLocator
    {
        // В конструктор передаются данные о рекламоносителях и локациях.
        // ===== пример данных =====
        // Яндекс.Директ:/ru
        // Бегущая строка в троллейбусах Екатеринбурга:/ru/svrd/ekb
        // Быстрый курьер:/ru/svrd/ekb
        // Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
        // Газета уральских москвичей:/ru/msk,/ru/permobl/,/ru/chelobl
        // ===== конец примера данных =====
        // inputStream будет уничтожен после вызова конструктора.
        private readonly Dictionary<string, HashSet<string>> locationToMediaMap = new Dictionary<string, HashSet<string>>();
        public MediaLocator(Stream inputStream)
        {
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

        // В метод передаётся локация.
        // Надо вернуть все рекламоносители, которые действуют в этой локации.
        // Например, GetMediasForLocation("/ru/svrd/pervik") должен вернуть две строки:
        // "Яндекс.Директ", "Ревдинский рабочий"
        // Порядок строк не имеет значения.
        public IEnumerable<string> GetMediasForLocation(string location)
        {
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