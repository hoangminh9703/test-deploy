using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace WhisperTranscriptionApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TranscriptionController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly TranscriptionConfig _config;

    public TranscriptionController(IHttpClientFactory httpClientFactory, TranscriptionConfig config)
    {
        _httpClient = httpClientFactory.CreateClient();
        _config = config;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadAudio(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File not provided.");

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _config.OpenAIKey);

        using var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(file.OpenReadStream());
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg");

        content.Add(streamContent, "file", file.FileName);
        content.Add(new StringContent("whisper-1"), "model");

        var response = await _httpClient.PostAsync("https://api.openai.com/v1/audio/transcriptions", content);
        var result = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, new
            {
                status = response.StatusCode,
                error = result
            });
        }

        return Ok(result);
    }
}
