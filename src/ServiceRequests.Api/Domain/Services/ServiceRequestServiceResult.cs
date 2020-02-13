namespace ServiceRequests.Api.Domain.Services
{
    using ServiceRequests.Api.Domain.Models;
    public class ServiceRequestServiceResult : ServiceResult
    {
        public ServiceRequest ServiceRequest { get; set; }
    }
}
