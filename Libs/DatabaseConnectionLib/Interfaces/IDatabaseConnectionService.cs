using DatabaseConnectionLib.Models.GetContent;
using DatabaseConnectionLib.Models.GetCurrentStatus;
using DatabaseConnectionLib.Models.GetStatusHistory;
using DatabaseConnectionLib.Models.StoreMessage;
using DatabaseConnectionLib.Models.UpdateStatus;

namespace DatabaseConnectionLib.Interfaces;

public interface IDatabaseConnectionService
{
    public Task<GetContentResponse> GetContentAsync(GetContentRequest request);

    public Task<GetCurrentStatusResponse> GetCurrentStatusAsync(GetCurrentStatusRequest request);

    public Task<GetStatusHistoryResponse> GetStatusHistoryAsync(GetStatusHistoryRequest request);

    public Task<StoreMessageResponse> StoreMessageAsync(StoreMessageRequest request);

    public Task<UpdateStatusResponse> UpdateStatusAsync(UpdateStatusRequest request);
}
