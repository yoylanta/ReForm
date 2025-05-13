using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ReForm.Core.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace ReForm.Infrastructure.Services;

public class SalesforceService(
    HttpClient httpClient,
    IOptions<SalesforceOptions> opts,
    IHttpContextAccessor httpContextAccessor) : ISalesforceService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IOptions<SalesforceOptions> _opts = opts;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private const string RedirectPath = "/UserProfile/Profile";


    public string GetSalesforceAuthorizationUrl()
    {
        return "https://login.salesforce.com/services/oauth2/authorize?" +
               $"response_type=code&client_id={_opts.Value.ClientId}&redirect_uri={GetRedirectUri()}";
    }

    public async Task<(string AccessToken, string InstanceUrl)> GetAccessTokenAsync(string authorizationCode)
    {
        var redirectUri = GetRedirectUri();

        var parameters = new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code", // Use authorization code flow
            ["client_id"] = _opts.Value.ClientId,
            ["client_secret"] = _opts.Value.ClientSecret,
            ["code"] = authorizationCode, // Code from the redirect URI
            ["redirect_uri"] = redirectUri.ToString() // Ensure this matches what you configured in Salesforce app
        };

        var response = await _httpClient.PostAsync(
            "https://login.salesforce.com/services/oauth2/token",
            new FormUrlEncodedContent(parameters));

        response.EnsureSuccessStatusCode(); 

        var result = await response.Content.ReadFromJsonAsync<SalesforceAuthResponse>();

        return (result.AccessToken, result.InstanceUrl);
    }

    public async Task CreateAccountAndContactAsync(SalesforceDto dto, string authorizationCode)
    {
        var (token, instanceUrl) = await GetAccessTokenAsync(authorizationCode);

        if (string.IsNullOrEmpty(token))
        {
            throw new InvalidOperationException("Failed to obtain a valid access token.");
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var accountUrl = $"{instanceUrl}/services/data/v59.0/sobjects/Account";
        var contactUrl = $"{instanceUrl}/services/data/v59.0/sobjects/Contact";

        var accountPayload = new
        {
            Name = dto.LegalName,
            Phone = dto.Phone,
            Description = dto.Description
        };

        var apiBase = $"{instanceUrl}/services/data/v59.0/sobjects";

        var accountResp = await _httpClient.PostAsJsonAsync(
            $"{apiBase}/Account",
            accountPayload);
        accountResp.EnsureSuccessStatusCode();

        var accountId = (await accountResp.Content
            .ReadFromJsonAsync<JsonElement>())
            .GetProperty("id").GetString();

        var contactResp = await _httpClient.PostAsJsonAsync(
            $"{apiBase}/Contact",
            new
            {
                LastName = dto.LegalName,
                Email = dto.Email,
                AccountId = accountId
            });

        var content = await contactResp.Content.ReadAsStringAsync();

        contactResp.EnsureSuccessStatusCode();
    }

    private Uri GetRedirectUri()
    {
        // Retrieve the host and the scheme (http/https)
        var scheme = _httpContextAccessor.HttpContext.Request.Scheme;
        var host = _httpContextAccessor.HttpContext.Request.Host.ToString(); // Full host with port (e.g., localhost:44380)

        // Check for invalid host
        if (string.IsNullOrEmpty(host))
        {
            throw new InvalidOperationException("Server base address is not available.");
        }

        // Construct the full base URL by combining scheme and host
        var baseUrl = $"{scheme}://{_opts.Value.RedirectUri ?? host}";

        // Construct the full Redirect URI by combining the base URL with the relative path
        return new Uri(new Uri(baseUrl), RedirectPath);
    }
}


public class SalesforceAuthResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("instance_url")]
    public string InstanceUrl { get; set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonPropertyName("signature")]
    public string Signature { get; set; }

    [JsonPropertyName("scope")]
    public string Scope { get; set; }

    [JsonPropertyName("id_token")]
    public string IdToken { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    [JsonPropertyName("issued_at")]
    public string IssuedAt { get; set; }
}
