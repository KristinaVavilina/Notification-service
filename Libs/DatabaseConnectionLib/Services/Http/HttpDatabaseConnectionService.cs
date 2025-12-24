using DatabaseConnectionLib.Interfaces;
using DatabaseConnectionLib.Models.GetContent;
using DatabaseConnectionLib.Models.GetCurrentStatus;
using DatabaseConnectionLib.Models.GetStatusHistory;
using DatabaseConnectionLib.Models.StoreMessage;
using DatabaseConnectionLib.Models.UpdateStatus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnectionLib.Services.Http;

internal class HttpDatabaseConnectionService : IDatabaseConnectionService
{
    private readonly IHttpClientFactory _httpClientFactory;

    private Uri? GetContentUri { get; set; }
    private Uri? GetCurrentStatusUri { get; set; }
    private Uri? GetStatusHistoryUri { get; set; }
    private Uri? StoreMessageUri { get; set; }
    private Uri? UpdateStatusUri { get; set; }

    public HttpDatabaseConnectionService(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        _httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        GetStatusHistoryUri = configuration.GetValue<Uri>(nameof(GetStatusHistoryUri));
        UpdateStatusUri = configuration.GetValue<Uri>(nameof(UpdateStatusUri));
        StoreMessageUri = configuration.GetValue<Uri>(nameof(StoreMessageUri));
        GetContentUri = configuration.GetValue<Uri>(nameof(GetContentUri));
    }

    public async Task<TResponse> PostHttpAsync<TRequest, TResponse>(Uri uri, TRequest request)
    {
        using var client = _httpClientFactory.CreateClient();

        var response = await client.PostAsJsonAsync(uri, request);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<TResponse>())!;
    }

    public async Task<TResponse> GetByIdHttpAsync<TResponse>(Uri uri, Guid id)
    {
        using var client = _httpClientFactory.CreateClient();

        var newUri = new Uri($"{GetCurrentStatusUri}?id={id}");
        return (await client.GetFromJsonAsync<TResponse>(newUri))!;
    }

    public async Task<StoreMessageResponse> StoreMessageAsync(StoreMessageRequest request)
    {
        return await PostHttpAsync<StoreMessageRequest, StoreMessageResponse>(StoreMessageUri, request);
    }

    public async Task<UpdateStatusResponse> UpdateStatusAsync(UpdateStatusRequest request)
    {
        return await PostHttpAsync<UpdateStatusRequest, UpdateStatusResponse>(UpdateStatusUri, request);
    }

    public async Task<GetContentResponse> GetContentAsync(GetContentRequest request)
    {
        return await GetByIdHttpAsync<GetContentResponse>(GetContentUri, request.Id);
    }

    public async Task<GetCurrentStatusResponse> GetCurrentStatusAsync(GetCurrentStatusRequest request)
    {
        return await GetByIdHttpAsync<GetCurrentStatusResponse>(GetCurrentStatusUri, request.Id);
    }

    public async Task<GetStatusHistoryResponse> GetStatusHistoryAsync(GetStatusHistoryRequest request)
    {
        return await GetByIdHttpAsync<GetStatusHistoryResponse>(GetStatusHistoryUri, request.Id);
    }
}
