using System.Text;
using System.Text.Json;
using PropertyExperts.API.Models.Entities;

namespace PropertyExperts.API.Utils
{
    public static class FileHelper
    {
        private static readonly string RulesFilePath = "Rules/decision-rules.json";

        public static List<DecisionRule> LoadDecisionRules()
        {
            if (!File.Exists(RulesFilePath))
            {
                return new List<DecisionRule>();
            }

            string jsonContent = File.ReadAllText(RulesFilePath);
            return JsonSerializer.Deserialize<List<DecisionRule>>(jsonContent) ?? new List<DecisionRule>();
        }

        public static string ConvertToBase64(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }

		public static async Task<byte[]> ReadFileAsync(IFormFile file)
		{
			using var memoryStream = new MemoryStream();
			await file.CopyToAsync(memoryStream);
			return memoryStream.ToArray();
		}
	}
}
