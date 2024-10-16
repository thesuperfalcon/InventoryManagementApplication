using InventoryManagementApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace InventoryManagementApplication.DAL
{
    public class LogManager
    {
        private static readonly Uri BaseAddress = new Uri("https://localhost:44353/");
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CreateLogAsync(Log log)
        {
            if (log != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = BaseAddress;
                    var json = JsonSerializer.Serialize(log);

                    StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("api/Log", httpContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Error logging activity. Status Code: {response.StatusCode}");
                    }
                    else
                    {
                        Console.WriteLine("Log successfully created.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Error! Log is null.");
            }
        }

        public async Task<List<Log>> GetAllLogsAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                HttpResponseMessage response = await client.GetAsync("api/Log");

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    List<Log> logs = JsonSerializer.Deserialize<List<Log>>(responseString);
                    return logs ?? new List<Log>();
                }
                return new List<Log>();
            }
        }

        public async Task DeleteLogAsync(int? id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;

                var response = await client.DeleteAsync($"api/Log/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error deleting log. Status Code: {response.StatusCode}");
                }
            }
        }

        public async Task EditLogAsync(Log log)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = BaseAddress;
                if (log != null)
                {
                    var content = new StringContent(JsonSerializer.Serialize(log), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync($"api/Log/{log.Id}", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Error updating log. Status Code: {response.StatusCode}");
                    }
                }
            }
        }

		public async Task LogActivityAsync(object entity, EntityState state, object entityNoChanges = null)
		{
			var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

			string action = state switch
			{
				EntityState.Added => "Skapad",
				EntityState.Modified => "Uppdaterad",
				EntityState.Deleted => "Borttagen",
				_ => throw new ArgumentException($"Unexpected entity state: {state}"),
			};

			string entityName = GetEntityName(entity);
			string details = state == EntityState.Modified
				? GetModifiedDetails(entity, entityNoChanges, action, entityName)
				: $"{action}: {entityName}";

			var log = new Log
			{
				UserId = userId,
				Action = action,
				EntityType = entity.GetType().Name,
				EntityName = entityName,
				EntityDetails = details,
				TimeStamp = DateTime.UtcNow
			};

			await CreateLogAsync(log);
		}

		private string GetModifiedDetails(object entity, object entityNoChanges, string action, string entityName)
		{
			if (entityNoChanges == null)
				throw new ArgumentNullException(nameof(entityNoChanges), "Previous state of the entity cannot be null when modified.");

			string changes = CheckChanges(entity, entityNoChanges);
			return $"{action}: {entityName}. Ändringar: {changes}";
		}

		private string GetEntityName(object entity) => entity switch
		{
			Product product => product.Name,
			Storage storage => storage.Name,
			_ => "Unknown entity"
		};

		private string CheckChanges(object entity, object noChangesEntity)
		{
			if (entity == null || noChangesEntity == null)
				throw new ArgumentNullException("Entity and noChangesEntity cannot be null");

			if (entity.GetType() != noChangesEntity.GetType())
				throw new ArgumentException("Both entities must be of the same type.");

			var differences = entity.GetType()
				.GetProperties()
				.Where(property => !new HashSet<string> { "Created", "Updated", "Id" }.Contains(property.Name) &&
								  !typeof(IEnumerable<InventoryTracker>).IsAssignableFrom(property.PropertyType))
				.Select(property =>
				{
					var currentValue = property.GetValue(entity);
					var originalValue = property.GetValue(noChangesEntity);
					return !Equals(currentValue, originalValue)
						? $"{property.Name} ändrad: {originalValue} -> {currentValue}"
						: null;
				})
				.Where(change => change != null);

			return differences.Any() ? string.Join(", ", differences) : "Inga skillnader funna.";
		}
	}
}
