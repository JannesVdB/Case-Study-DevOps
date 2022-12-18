using System.Text.Json;

namespace CaseStudy
{
    internal class Plant
    {
        public string? Name { get; set; }
        public string? BotanicalName { get; set; }
        public string? Type { get; set; }
        public string? SunExposure { get; set; }
        public string? SoilType { get; set; }
        public string? BloomTime { get; set; }
        public string? NativeArea { get; set; }

        public static void JSONFile(List<string> jsonItems, string filePath)
        {
            Plant data = new Plant
            {
                Name = jsonItems[0],
                BotanicalName = jsonItems[1],
                Type = jsonItems[2],
                SunExposure = jsonItems[3],
                SoilType = jsonItems[4],
                BloomTime = jsonItems[5],
                NativeArea = jsonItems[6]
            };

            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, json);
        }
    }
}
