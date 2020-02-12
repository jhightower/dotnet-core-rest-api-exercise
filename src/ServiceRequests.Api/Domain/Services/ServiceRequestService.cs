namespace ServiceRequests.Api.Domain.Services
{
    using Microsoft.EntityFrameworkCore;
    using ServiceRequests.Api.Domain.Models;
    using ServiceRequests.Api.Persistence.Contexts;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public class ServiceRequestService
    {
        private readonly AppDbContext context;
        public ServiceRequestService(AppDbContext context) => this.context = context;

        public async Task<List<ServiceRequest>> ReadAllAsync() => await this.context.ServiceRequests.ToListAsync();
        public async Task<ServiceRequest> ReadByIdAsync(Guid id) => await this.context.ServiceRequests.FirstOrDefaultAsync(x => x.Id == id);
        public async Task<ServiceResult> CreateAsync(ServiceRequest sr)
        {
            var serviceRequest = await this.context.ServiceRequests.FirstOrDefaultAsync(x => x.Id == sr.Id);
            if (serviceRequest != null)
            {
                return new ServiceResult() { Message = "Service Request Already Exists", Success = false };
            }
            await this.context.ServiceRequests.AddAsync(sr);
            await this.context.SaveChangesAsync();
            return new ServiceResult() { Success = true };
        }
        public async Task<ServiceResult> UpdateAsync(ServiceRequest sr)
        {
            var serviceRequest = await this.context.ServiceRequests.FirstOrDefaultAsync(x => x.Id == sr.Id);
            if (serviceRequest == null)
            {
                return new ServiceResult() { Message = "Not Found", Success = false };
            }
            this.context.ServiceRequests.Update(sr);
            await this.context.SaveChangesAsync();
            return new ServiceResult() { Success = true };
        }

        public async Task<ServiceResult> Delete(Guid id)
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
