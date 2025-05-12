using ReForm.Core.DTOs;
using ReForm.Core.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;   
using System.Text.Json;      
using Microsoft.Extensions.Configuration;


namespace ReForm.Infrastructure.Services;

public class SalesforceService : ISalesforceService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public SalesforceService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<(string AccessToken, string InstanceUrl)> GetAccessTokenAsync()
    {
        var parameters = new Dictionary<string, string>
    {
        {"grant_type", "password"},
        {"client_id", _config["Salesforce:ClientId"]},
        {"client_secret", _config["Salesforce:ClientSecret"]},
        {"username", _config["Salesforce:Username"]},
        {"password", _config["Salesforce:Password"] + _config["Salesforce:Token"]}
    };

        var response = await _httpClient.PostAsync(
            "https://login.salesforce.com/services/oauth2/token",
            new FormUrlEncodedContent(parameters));

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<SalesforceAuthResponse>();

        return (result.AccessToken, result.InstanceUrl);
    }

    public async Task CreateAccountAndContactAsync(SalesforceDto dto)
    {
        var (token, instanceUrl) = await GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var accountUrl = $"{instanceUrl}/services/data/v59.0/sobjects/Account";
        var contactUrl = $"{instanceUrl}/services/data/v59.0/sobjects/Contact";

        var accountPayload = new
        {
            Name = dto.LegalName,
            Phone = dto.Phone,
            Description = dto.Description
        };

        var accountResp = await _httpClient.PostAsJsonAsync("https://yourInstance.salesforce.com/services/data/v59.0/sobjects/Account", accountPayload);
        accountResp.EnsureSuccessStatusCode();

        var accountId = (await accountResp.Content.ReadFromJsonAsync<JsonElement>())
            .GetProperty("id").GetString();

        var contactPayload = new
        {
            LastName = dto.LegalName,
            Email = dto.Email,
            AccountId = accountId
        };

        var contactResp = await _httpClient.PostAsJsonAsync("https://yourInstance.salesforce.com/services/data/v59.0/sobjects/Contact", contactPayload);
        contactResp.EnsureSuccessStatusCode();
    }
}