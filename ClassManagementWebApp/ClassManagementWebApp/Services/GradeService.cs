using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ClassManagementWebApp.DTO;

namespace ClassManagementWebApp.Services;

public class GradeService(IHttpClientFactory httpClientFactory) : IGradeService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ClassManagementWebApp.ServerAPI");

    public async Task<Grade> CreateGradeAsync(Grade grade)
    {
        var response = await _httpClient.PostAsJsonAsync("grades", grade);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API Error: {error}");
            throw new HttpRequestException($"HTTP {response.StatusCode}: {error}");
        }

        return await response.Content.ReadFromJsonAsync<Grade>();
    }

    public async Task<List<Grade>> GetAllGradesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Grade>>("grades");
    }

    public async Task<Grade?> GetGradeByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Grade>($"grades/{id}");
    }

    public async Task UpdateGradeAsync(Grade grade)
    {
        var response = await _httpClient.PutAsJsonAsync($"grades/{grade.Id}", grade);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteGradeAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"grades/{id}");
        response.EnsureSuccessStatusCode();
    }
}
