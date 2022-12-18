using System.Text;

namespace CaseStudy
{
    internal class Util
    {
        public static string EncapsulateString(string value)
        {
            return '"' + value + '"';
        }

        public static void CSVFile(List<string> csvItems, string filePath)
        {
            string csvLine = string.Empty;

            foreach (string csvItem in csvItems)
            {
                string encapsulatedItem = EncapsulateString(csvItem);
                if (csvItem != csvItems.Last())
                {
                    csvLine += encapsulatedItem + ",";
                }
                else
                {
                    csvLine += encapsulatedItem;
                } 
            }

            File.WriteAllText(filePath, csvLine);   
        }

        public static void CSVFile(Dictionary<int, List<string>> csvItems, string filePath)
        {
            StringBuilder sb = new StringBuilder(); 

            foreach(var csvItem in csvItems)
            { 
                string csvLine = EncapsulateString(Convert.ToString(csvItem.Key)) + ",";

                foreach(var csvItemInfo in csvItem.Value)
                {
                    if (csvItemInfo != csvItem.Value.Last())
                    {
                        csvLine += EncapsulateString(csvItemInfo) + ",";
                    }
                    else
                    {
                        csvLine += EncapsulateString(csvItemInfo);
                    }
                    
                }
                sb.AppendLine(csvLine);
            }

            File.WriteAllText(filePath, sb.ToString());
        }
    }
}
