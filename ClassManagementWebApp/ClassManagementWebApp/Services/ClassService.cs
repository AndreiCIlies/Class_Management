using ClassManagementWebApp.DTO;

namespace ClassManagementWebApp.Services;

public class ClassService(IHttpClientFactory httpClientFactory) : IClassService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ClassManagementWebApp.ServerAPI");

    public async Task<Class> CreateClassAsync(Class @class)
    {
        var response = await _httpClient.PostAsJsonAsync("classes", @class);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Class>();
    }

    public async Task<List<Class>> GetAllClassesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Class>>("classes");
    }

    public async Task<List<Class>> GetClassesByTeacherIdAsync(string teacherId)
    {
        return await _httpClient.GetFromJsonAsync<List<Class>>($"classes/teacher/{teacherId}");
    }

    public async Task<Class?> GetClassByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Class>($"classes/{id}");
    }

    public async Task UpdateClassAsync(Class @class)
    {
        await _httpClient.PutAsJsonAsync($"classes/{@class.Id}", @class);
    }

    public async Task DeleteClassAsync(int id)
    {
        await _httpClient.DeleteAsync($"classes/{id}");
    }
}
