namespace ServiceRequests.Api.Domain.Services
{
    using Microsoft.EntityFrameworkCore;
    using ServiceRequests.Api.Domain.Models;
    using ServiceRequests.Api.Persistence.Contexts;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public class ServiceRequestService : IServiceRequestService
    {
        private readonly AppDbContext context;
        public ServiceRequestService(AppDbContext context) => this.context = context;

        public async Task<List<ServiceRequest>> ReadAllAsync() => await this.context.ServiceRequests.ToListAsync();
        public async Task<ServiceRequest> ReadByIdAsync(Guid id) => await this.context.ServiceRequests.FirstOrDefaultAsync(x => x.Id == id);
        public async Task<ServiceRequestServiceResult> CreateAsync(ServiceRequest sr)
        {
            var serviceRequest = await this.context.ServiceRequests.FirstOrDefaultAsync(x => x.Id == sr.Id);
            if (serviceRequest != null)
            {
                return new ServiceRequestServiceResult() { Message = "Service Request Already Exists", Success = false };
            }
            this.context.ServiceRequests.Add(sr);
            await this.context.SaveChangesAsync();

            return new ServiceRequestServiceResult() { Success = true, ServiceRequest = sr };
        }
        public async Task<ServiceRequestServiceResult> UpdateAsync(ServiceRequest sr)
        {
            var serviceRequest = await this.context.ServiceRequests.FirstOrDefaultAsync(x => x.Id == sr.Id);
            if (serviceRequest == null)
            {
                return new ServiceRequestServiceResult() { Message = "Not Found", Success = false };
            }

            var existing = await this.context.ServiceRequests.FindAsync(sr.Id);
            if (existing != null)
            {
                this.context.Entry(existing).CurrentValues.SetValues(sr);
            }

            await this.context.SaveChangesAsync();

            return new ServiceRequestServiceResult() { Success = true, ServiceRequest = sr };
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            var serviceRequest = await this.context.ServiceRequests.FirstOrDefaultAsync(x => x.Id == id);
            if (serviceRequest == null)
            {
                return new ServiceResult() { Message = "Not Found", Success = false };
            }
            this.context.ServiceRequests.Remove(serviceRequest);
            await this.context.SaveChangesAsync();
            return new ServiceResult() { Success = true };
        }
    }
}
