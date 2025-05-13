using ReForm.Core.DTOs;

namespace ReForm.Core.Interfaces;

public interface ISalesforceService
{
    Task<(string AccessToken, string InstanceUrl)> GetAccessTokenAsync(string authorizationCode);

    Task CreateAccountAndContactAsync(SalesforceDto dto, string authorizationCode);

    string GetSalesforceAuthorizationUrl();
}
