using System.Text.Json;

namespace CaseStudy
{
    internal class Job
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Keywords { get; set; }
        public string? Company { get; set; }
        public string? Location { get; set; }
        public string? Link { get; set; }

        public static void JSONFile(Dictionary<int, List<string>> jsonItems, string filePath)
        {
            List<Job> jobInfo = new List<Job>();

            foreach (var jsonItem in jsonItems)
            {
                int jsonItemKey = jsonItem.Key;
                List<string> jsonItemValue = jsonItem.Value;

                Job data = new Job
                {
                    Id = jsonItemKey,
                    Title = jsonItemValue[0],
                    Keywords = jsonItemValue[1],
                    Company = jsonItemValue[2],
                    Location = jsonItemValue[3],
                    Link = jsonItemValue[4]
                };

                jobInfo.Add(data);
            }


            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(jobInfo, options);
            File.WriteAllText(filePath, json);
        }
    }
}
