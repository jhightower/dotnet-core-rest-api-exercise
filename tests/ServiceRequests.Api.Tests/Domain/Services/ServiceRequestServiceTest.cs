namespace ServiceRequests.Api.Tests.Domain.Services
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using ServiceRequests.Api.Domain.Models;
    using ServiceRequests.Api.Domain.Services;
    using ServiceRequests.Api.Persistence.Contexts;
    using System.Threading.Tasks;
    using System.Linq;
    using System.Collections.Generic;

    [TestFixture]
    public class ServiceRequestServiceTest
	{
        [Test]
        public async Task CreateAsync_Success()
        {
            //ARRANGE
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateAsync_Success")
                .Options;
            var expectedServiceRequest = new ServiceRequest()
            {
                BuildingCode = Guid.NewGuid().ToString(),
                CreatedBy = "Larry",
                CreatedDate = DateTime.Today.AddDays(1),
                CurrentStatus = CurrentStatus.Created,
                Description = "Roger and Me",
                Id = Guid.NewGuid(),
                LastModifiedBy = "Fred",
                LastUpdatedBy = DateTime.Now
            };
            ServiceResult serviceResult = null;
            //ACT
            using (var context = new AppDbContext(options))
            {
                var service = new ServiceRequestService(context);
                serviceResult = await service.CreateAsync(expectedServiceRequest);
            }
            //ASSERT
            Assert.IsTrue(serviceResult.Success);
            using (var context = new AppDbContext(options))
            {
                Assert.AreEqual(1, context.ServiceRequests.Count());
                var actualServiceRequest = context.ServiceRequests.Single();
                Assert.AreEqual(expectedServiceRequest.Id, actualServiceRequest.Id);
                Assert.AreEqual(expectedServiceRequest.BuildingCode, actualServiceRequest.BuildingCode);
                Assert.AreEqual(expectedServiceRequest.CreatedBy, actualServiceRequest.CreatedBy);
                Assert.AreEqual(expectedServiceRequest.CreatedDate, actualServiceRequest.CreatedDate);
                Assert.AreEqual(expectedServiceRequest.CurrentStatus, actualServiceRequest.CurrentStatus);
                Assert.AreEqual(expectedServiceRequest.Description, actualServiceRequest.Description);
            }
        }

        [Test]
        public async Task UpdateAsync_Success()
        {
            //ARRANGE
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UpdateAsync_Success")                
                .Options;

            ServiceRequest expectedServiceRequest = null;
            ServiceResult serviceResult = null;
            //ACT
            using (var context = new AppDbContext(options))
            {
                context.Database.EnsureCreated();
                expectedServiceRequest = context.ServiceRequests.Single(x => x.Id == MockServiceRequestIds.ServiceRequestId1);
                expectedServiceRequest.Description = "BOB FARLEY";
                expectedServiceRequest.CurrentStatus = CurrentStatus.InProgress;
                var service = new ServiceRequestService(context);
                serviceResult = await service.UpdateAsync(expectedServiceRequest);
            }
            //ASSERT
            Assert.IsTrue(serviceResult.Success);
            using (var context = new AppDbContext(options))
            {
                var actualServiceRequest = context.ServiceRequests.Single(x => x.Id == MockServiceRequestIds.ServiceRequestId1);
                Assert.AreEqual(expectedServiceRequest.Id, actualServiceRequest.Id);
                Assert.AreEqual(expectedServiceRequest.BuildingCode, actualServiceRequest.BuildingCode);
                Assert.AreEqual(expectedServiceRequest.CreatedBy, actualServiceRequest.CreatedBy);
                Assert.AreEqual(expectedServiceRequest.CreatedDate, actualServiceRequest.CreatedDate);
                Assert.AreEqual(expectedServiceRequest.CurrentStatus, actualServiceRequest.CurrentStatus);
                Assert.AreEqual(expectedServiceRequest.Description, actualServiceRequest.Description);
            }
        }

        [Test]
        public async Task ReadAllAsync_Success()
        {
            //ARRANGE
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ReadAllAsync_Success")
                .Options;

            var serviceRequests = new List<ServiceRequest>();
            //ACT
            using (var context = new AppDbContext(options))
            {
                context.Database.EnsureCreated();
                var service = new ServiceRequestService(context);
                serviceRequests = await service.ReadAllAsync();
            }
            //ASSERT
            using (var context = new AppDbContext(options))
            {
                Assert.AreEqual(3, context.ServiceRequests.Count());
                var ids = serviceRequests.Select(x => x.Id).ToArray();
                Assert.IsTrue(ids.Contains(MockServiceRequestIds.ServiceRequestId1));
                Assert.IsTrue(ids.Contains(MockServiceRequestIds.ServiceRequestId2));
                Assert.IsTrue(ids.Contains(MockServiceRequestIds.ServiceRequestId3));
            }
        }

        [Test]
        public async Task ReadAsync_Success()
        {
            //ARRANGE
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ReadAllAsync_Success")
                .Options;

            ServiceRequest actualServiceRequest = null;
            //ACT
            using (var context = new AppDbContext(options))
            {
                context.Database.EnsureCreated();
                var service = new ServiceRequestService(context);
                actualServiceRequest = await service.ReadByIdAsync(MockServiceRequestIds.ServiceRequestId1);
            }
            //ASSERT
            Assert.AreEqual(MockServiceRequestIds.ServiceRequestId1, actualServiceRequest.Id);
        }

    }
}
