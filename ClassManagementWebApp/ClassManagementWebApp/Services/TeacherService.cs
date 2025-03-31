using ClassManagementWebApp.DTO;
using ClassManagementWebApp.Interfaces;
using Newtonsoft.Json;

namespace ClassManagementWebApp.Services;

public class TeacherService(IHttpClientFactory httpClientFactory) : ITeacherService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ClassManagementWebApp.ServerAPI");

    public async Task<Teacher?> GetTeacherByIdAsync(string id)
    {
        var response = await _httpClient.GetAsync($"teachers/{id}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Teacher>(content);
        }
        return null;
    }

    public async Task UpdateTeacherAsync(Teacher teacher)
    {
        var response = await _httpClient.PutAsJsonAsync($"teachers/{teacher.Id}", teacher);
    }
}