using System.Text.Json;

namespace CaseStudy
{
    internal class Video
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Views { get; set; }
        public string? Uploader { get; set; }
        public string? Link { get; set; }

        public static void JSONFile(Dictionary<int, List<string>> jsonItems, string filePath)
        {
            List<Video> videoInfo = new List<Video>();

            foreach (var jsonItem in jsonItems)
            {
                int jsonItemKey = jsonItem.Key;
                List<string> jsonItemValue = jsonItem.Value;

                Video data = new Video
                {
                    Id = jsonItemKey,
                    Title = jsonItemValue[0],
                    Views = jsonItemValue[1],
                    Uploader = jsonItemValue[2],
                    Link = jsonItemValue[3],
                };

                videoInfo.Add(data);
            }


            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(videoInfo, options);
            File.WriteAllText(filePath, json);
        }
    }
}
