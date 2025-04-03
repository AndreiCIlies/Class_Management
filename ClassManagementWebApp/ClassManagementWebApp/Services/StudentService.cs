using ClassManagementWebApp.DTO;
using ClassManagementWebApp.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;

namespace ClassManagementWebApp.Services;

public class StudentService(IHttpClientFactory httpClientFactory) : IStudentService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ClassManagementWebApp.ServerAPI");

    public async Task<Student> CreateStudentAsync(Student student)
    {
        var response = await _httpClient.PostAsJsonAsync("students", student);
        return await response.Content.ReadFromJsonAsync<Student>();
    }

    public async Task<List<Student>> GetAllStudentsAsync()
	{
		var response = await _httpClient.GetAsync("students");
		if (response.IsSuccessStatusCode)
		{
			var content = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<List<Student>>(content) ?? new List<Student>();
		}
		return new List<Student>();
	}

	public async Task<Student?> GetStudentByIdAsync(string id)
    {
        var response = await _httpClient.GetAsync($"students/{id}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Student>(content);
        }
        return null;
    }

    public async Task UpdateStudentAsync(Student student)
    {
        var response = await _httpClient.PutAsJsonAsync($"students/{student.Id}", student);
    }
}