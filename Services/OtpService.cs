using System.Net.Http;
using System.Threading.Tasks;

public class OtpService
{
    private readonly HttpClient _httpClient;

    public OtpService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SendOtp(string type, string destination, string otpCode)
    {
        string endpoint = type == "email"
            ? $"http://localhost:3000/api/otp-email?api-key=xxxxxxx"
            : $"https://ipbase/opt-whatsapp?api-key=xxxxxxx";

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("destination", destination),
            new KeyValuePair<string, string>("code", otpCode)
        });

        var response = await _httpClient.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();
    }
}
