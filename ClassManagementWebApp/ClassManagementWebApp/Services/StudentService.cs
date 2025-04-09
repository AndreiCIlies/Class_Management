using ClassManagementWebApp.DTO;
using ClassManagementWebApp.Interfaces;
using Newtonsoft.Json;

namespace ClassManagementWebApp.Services;

public class StudentService(IHttpClientFactory httpClientFactory) : IStudentService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ClassManagementWebApp.ServerAPI");

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

    public async Task<List<Class>> GetStudentClassesAsync(string studentId)
    {
        var response = await _httpClient.GetAsync($"Classes/student/{studentId}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Class>>(content) ?? [];
        }
        return [];
    }

    public async Task<List<Grade>> GetStudentGradesAsync(string studentId)
    {
        var response = await _httpClient.GetAsync($"Grades/student/{studentId}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Grade>>(content) ?? [];
        }
        return [];
    }

    public async Task UpdateStudentAsync(Student student)
    {
        var response = await _httpClient.PutAsJsonAsync($"students/{student.Id}", student);
    }
}