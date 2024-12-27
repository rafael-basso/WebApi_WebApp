using UserAPI.DB;

namespace UserFRONT.Services
{
    public class GetUsersService
    {
        private readonly ILogger<GetUsersService> _logger;
        private readonly HttpClient _httpClient;

        public GetUsersService(ILogger<GetUsersService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

                var response = await _httpClient.GetAsync("User", cts.Token); //aqui é o [Route] da CONTROLLER na API

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<User>>();
                }
                else
                {
                    return new List<User>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Users");
                throw;
            }
        }
    }
}
