using ReForm.Core.DTOs;

namespace ReForm.Core.Interfaces;

public interface ISalesforceService
{
    Task<(string AccessToken, string InstanceUrl)> GetAccessTokenAsync();

    Task CreateAccountAndContactAsync(SalesforceDto dto);
}
