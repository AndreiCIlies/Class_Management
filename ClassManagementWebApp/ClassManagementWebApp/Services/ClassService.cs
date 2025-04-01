using ClassManagementWebApp.DTO;

namespace ClassManagementWebApp.Services;

public class ClassService : IClassService
{
    private readonly HttpClient _httpClient;

    public ClassService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Class> CreateClassAsync(Class @class)
    {
        var response = await _httpClient.PostAsJsonAsync("api/classes", @class);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Class>();
    }

    public async Task<List<Class>> GetAllClassesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Class>>("api/classes") ?? new();
    }

    public async Task<Class?> GetClassByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Class>($"api/classes/{id}");
    }

    public async Task<List<Class>> GetTeacherClassesAsync(string teacherId)
    {
        return await _httpClient.GetFromJsonAsync<List<Class>>($"api/classes/teacher/{teacherId}") ?? new();
    }

    public async Task UpdateClassAsync(Class @class)
    {
        await _httpClient.PutAsJsonAsync($"api/classes/{@class.Id}", @class);
    }

    public async Task DeleteClassAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/classes/{id}");
    }
}
