namespace ServiceRequests.Api.Tests.Controllers
{
    using System;
    using NUnit.Framework;
    using ServiceRequests.Api.Domain.Models;
    using ServiceRequests.Api.Domain.Services;
    using System.Threading.Tasks;
    using System.Linq;
    using System.Collections.Generic;
    using ServiceRequests.Api.Controllers;
    using Moq;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc;

    public class ServiceRequestsControllerTests
    {

        [Test]
        public async Task GetServiceRequests_Success()
        {
            // Arrange
            var mockServiceRequestService = new Moq.Mock<IServiceRequestService>(MockBehavior.Strict);
            var expectectedServiceRequests = new List<ServiceRequest>
            {
                CreateMockServiceRequest(Guid.NewGuid(),nameof(GetServiceRequests_Success) + "1"),
                CreateMockServiceRequest(Guid.NewGuid(), nameof(GetServiceRequests_Success) + "2"),
                CreateMockServiceRequest(Guid.NewGuid(), nameof(GetServiceRequests_Success) + "3"),
                CreateMockServiceRequest(Guid.NewGuid(), nameof(GetServiceRequests_Success) + "4")
            };
            mockServiceRequestService.Setup(x => x.ReadAllAsync()).ReturnsAsync(expectectedServiceRequests);
            var serviceRequestsController = new ServiceRequestsController(mockServiceRequestService.Object);
            // Act
            var actualServiceRequests = await serviceRequestsController.GetServiceRequests();
            // Assert
            Assert.AreEqual(4, actualServiceRequests.Value.Count());
            foreach (var expectectedServiceRequest in expectectedServiceRequests)
            {
                Assert.IsTrue(actualServiceRequests.Value.Any(x => x.Id.Equals(expectectedServiceRequest.Id)));
            }
            mockServiceRequestService.VerifyAll();
        }

        [Test]
        public async Task GetServiceRequest_Success()
        {
            // Arrange
            var mockServiceRequestService = new Moq.Mock<IServiceRequestService>(MockBehavior.Strict);
            var expectectedServiceRequest = CreateMockServiceRequest(Guid.NewGuid(), nameof(GetServiceRequest_Success));
            mockServiceRequestService.Setup(x => x.ReadByIdAsync(It.IsAny<Guid>())).ReturnsAsync(expectectedServiceRequest);
            var serviceRequestsController = new ServiceRequestsController(mockServiceRequestService.Object);
            // Act
            var actualServiceRequestResult = await serviceRequestsController.GetServiceRequest(expectectedServiceRequest.Id);
            // Assert
            Assert.IsTrue(this.IsSameServiceRequest(expectectedServiceRequest, actualServiceRequestResult.Value));
            mockServiceRequestService.VerifyAll();
        }

        [Test]
        public async Task PostServiceRequest_Success()
        {
            // Arrange
            var mockServiceRequestService = new Moq.Mock<IServiceRequestService>(MockBehavior.Strict);
            var expectectedServiceRequest = CreateMockServiceRequest(Guid.NewGuid(), "Random Desc");
            var serviceRequestServiceResult = new ServiceRequestServiceResult
            {
                ServiceRequest = expectectedServiceRequest,
                Success = true,
            };
            mockServiceRequestService
                .Setup(x => x.CreateAsync(It.IsAny<ServiceRequest>()))
                .ReturnsAsync(serviceRequestServiceResult);
            var serviceRequestsController = new ServiceRequestsController(mockServiceRequestService.Object);
            // Act
            var actualServiceRequestResult = await serviceRequestsController.PostServiceRequest(expectectedServiceRequest);
            // Assert

            var createdAtActionResult = (actualServiceRequestResult.Result as CreatedAtActionResult);
            Assert.IsTrue(this.IsSameServiceRequest(expectectedServiceRequest, (ServiceRequest)createdAtActionResult.Value));
            mockServiceRequestService.VerifyAll();
        }

        [Test]
        public async Task PutServiceRequest_Success()
        {
            // Arrange
            var mockServiceRequestService = new Moq.Mock<IServiceRequestService>(MockBehavior.Strict);
            var expectectedServiceRequest = CreateMockServiceRequest(Guid.NewGuid(), "Random Desc");
            var serviceRequestServiceResult = new ServiceRequestServiceResult
            {
                ServiceRequest = expectectedServiceRequest,
                Success = true,
            };
            mockServiceRequestService
                .Setup(x => x.UpdateAsync(It.IsAny<ServiceRequest>()))
                .ReturnsAsync(serviceRequestServiceResult);
            var serviceRequestsController = new ServiceRequestsController(mockServiceRequestService.Object);
            // Act
            var actualServiceRequestResult = await serviceRequestsController.PutServiceRequest(expectectedServiceRequest.Id, expectectedServiceRequest);
            // Assert
            Assert.IsTrue(this.IsSameServiceRequest(expectectedServiceRequest, actualServiceRequestResult.Value));
            mockServiceRequestService.VerifyAll();
        }

        [Test]
        public async Task DeleteServiceRequest_Success()
        {
            // Arrange
            var mockServiceRequestService = new Moq.Mock<IServiceRequestService>(MockBehavior.Strict);
            var expectectedServiceRequest = CreateMockServiceRequest(Guid.NewGuid(), "Random Desc");
            var serviceResult = new ServiceResult
            {
                Success = true,
            };
            var deleteGuid = Guid.NewGuid();
            mockServiceRequestService
                .Setup(x => x.DeleteAsync(deleteGuid))
                .ReturnsAsync(serviceResult);
            var serviceRequestsController = new ServiceRequestsController(mockServiceRequestService.Object);
            // Act
            var actualServiceRequestResult = await serviceRequestsController.DeleteServiceRequest(deleteGuid);
            // Assert
            Assert.IsNotNull(actualServiceRequestResult);
            Assert.AreEqual(201, ((IStatusCodeActionResult)actualServiceRequestResult).StatusCode);
            mockServiceRequestService.VerifyAll();
        }

        public static ServiceRequest CreateMockServiceRequest(System.Guid id, string description)
        {
            var sr = new ServiceRequest()
            {
                BuildingCode = Guid.NewGuid().ToString(),
                CreatedBy = "Larry",
                CreatedDate = DateTime.Today.AddDays(1),
                CurrentStatus = CurrentStatus.Created,
                Description = description,
                Id = id,
                LastModifiedBy = "Fred",
                LastUpdatedBy = DateTime.Now
            };
            return sr;
        }

        public bool IsSameServiceRequest(ServiceRequest sr1, ServiceRequest sr2)
        {
#pragma warning disable IDE0022 // Use expression body for methods
            return sr1.Id.Equals(sr2.Id) &&
                sr1.BuildingCode.Equals(sr2.BuildingCode) &&
                sr1.CreatedBy.Equals(sr2.CreatedBy) &&
                sr1.CreatedDate.Equals(sr2.CreatedDate) &&
                sr1.CurrentStatus.Equals(sr2.CurrentStatus) &&
                sr1.Description.Equals(sr2.Description) &&
                sr1.LastModifiedBy.Equals(sr2.LastModifiedBy) &&
                sr1.LastUpdatedBy.Equals(sr2.LastUpdatedBy);
#pragma warning restore IDE0022 // Use expression body for methods

        }

    }
}
