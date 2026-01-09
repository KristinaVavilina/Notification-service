using MessangerNotificationApi.Interfaces;

namespace MessangerNotificationApi.Services;

public class MessangerService : IMessangerService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public MessangerService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task SendMessageAsync(string chatId, string message)
    {
        var token = _configuration.GetValue<string>("Telegram:Token");

        var url = $"https://api.telegram.org/bot{token}/sendMessage";

        var payload = new
        {
            chat_id = chatId,
            text = message,
            parse_mode = "HTML"
        };

        var response = await _httpClient.PostAsJsonAsync(url, payload);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"Ошибка отправки в Telegram: {response.StatusCode}. Детали: {errorBody}");
        }
    }
}
