namespace ServiceRequests.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using ServiceRequests.Api.Domain.Models;
    using ServiceRequests.Api.Domain.Services;

    [Route("api/servicerequest")]
    [ApiController]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly IServiceRequestService serviceRequestService;
        public ServiceRequestsController(IServiceRequestService serviceRequestService) => this.serviceRequestService = serviceRequestService;

        // GET: api/ServiceRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetServiceRequests()
        {
            var list = await this.serviceRequestService.ReadAllAsync();
            if (list.Any())
            {
                return list;
            }
            else
            {
                return new ObjectResult(list) { StatusCode = StatusCodes.Status204NoContent };
            }
        }

        // GET: api/ServiceRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceRequest>> GetServiceRequest(Guid id)
        {
            var serviceRequest = await this.serviceRequestService.ReadByIdAsync(id);
            if (serviceRequest == null)
            {
                return this.NotFound();
            }

            return serviceRequest;
        }

        // POST: api/ServiceRequests
        [HttpPost]
        public async Task<ActionResult<ServiceRequest>> PostServiceRequest(ServiceRequest serviceRequest)
        {
            var result = await this.serviceRequestService.CreateAsync(serviceRequest);

            if (result.Success)
            {
                return this.CreatedAtAction(nameof(GetServiceRequest), new { id = serviceRequest.Id }, result.ServiceRequest);
            }
            else
            {
                return this.BadRequest(result.Message);
            }
        }

        // PUT: api/ServiceRequests/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceRequest>> PutServiceRequest(Guid id, ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.Id)
            {
                return this.BadRequest();
            }

            var result = await this.serviceRequestService.UpdateAsync(serviceRequest);

            if (!result.Success)
            {
                if (result.Message == "Not Found")
                {
                    return this.NotFound();
                }
                else
                {
                    return this.BadRequest();
                }
            }

            return result.ServiceRequest;
        }

        // DELETE: api/ServiceRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceRequest(Guid id)
        {
            var result = await this.serviceRequestService.DeleteAsync(id);

            if (!result.Success)
            {
                if (result.Message == "Not Found")
                {
                    return this.NotFound();
                }
                else
                {
                    return this.BadRequest();
                }
            }

            // The requiremnts state a 201 should return
            // noth sure if the is right
            return new StatusCodeResult(StatusCodes.Status201Created);
        }

    }
}
