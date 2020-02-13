namespace ServiceRequests.Api.Domain.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ServiceRequests.Api.Domain.Models;

    public interface IServiceRequestService
    {
        public Task<ServiceRequestServiceResult> CreateAsync(ServiceRequest sr);
        public Task<ServiceResult> DeleteAsync(Guid id);
        public Task<List<ServiceRequest>> ReadAllAsync();
        public Task<ServiceRequest> ReadByIdAsync(Guid id);
        public Task<ServiceRequestServiceResult> UpdateAsync(ServiceRequest sr);
    }
}
