using System.Text.Json.Serialization;

namespace InventoryManagementApplication.Models
{
    public class Developer
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("githubUrl")]
        public string GithubUrl { get; set; }

        [JsonPropertyName("linkedInUrl")]
        public string LinkedInUrl { get; set; }
    }
}
