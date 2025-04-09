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
    public async Task<List<Grade>> AddMultipleGradesAsync(AddGradesToStudentRequest request)
    {
        string requestUrl = "Grades/multiple-grades"; 
        string fullRequestUrl = $"{_httpClient.BaseAddress}{requestUrl}"; 
        Console.WriteLine($"GradeService (Client): Attempting POST to URL: {fullRequestUrl}"); 

        HttpResponseMessage response = null;
        try
        {
            response = await _httpClient.PostAsJsonAsync(requestUrl, request); 
            response.EnsureSuccessStatusCode(); 
            var result = await response.Content.ReadFromJsonAsync<List<Grade>>();
            Console.WriteLine($"GradeService (Client): Successfully received {result?.Count ?? 0} grades."); 
            return result ?? new List<Grade>();
        }
        catch (HttpRequestException httpEx)
        {
            string errorContent = "N/A";
            if (response != null) 
            {
                try { errorContent = await response.Content.ReadAsStringAsync(); } catch {  }
            }
       
            string actualRequestedUrl = response?.RequestMessage?.RequestUri?.ToString() ?? fullRequestUrl;
            Console.WriteLine($"GradeService (Client) ERROR: HttpRequestException for URL '{actualRequestedUrl}'. Status Code: {httpEx.StatusCode}. Response Content: {errorContent}. Exception: {httpEx.ToString()}"); // Log EROARE DETALIAT
            throw; // Re-aruncă excepția
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GradeService (Client) ERROR: General exception during AddMultipleGradesAsync. Exception: {ex.ToString()}"); 
            throw; 
        }
    }
}