namespace ServiceRequests.Api.Persistence.Contexts
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using ServiceRequests.Api.Domain.Models;
    public class AppDbContext : DbContext
    {
        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ServiceRequest>().ToTable("ServiceRequests");
            builder.Entity<ServiceRequest>().HasKey(p => p.Id);
            builder.Entity<ServiceRequest>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();

            builder.Entity<ServiceRequest>().HasData(CreateMockServiceRequest(Guid.Parse("15CB7F2F-3106-4E71-9F12-557D489DA763"),"Test 1"));
            builder.Entity<ServiceRequest>().HasData(CreateMockServiceRequest(Guid.Parse("25CB7F2F-3106-1E71-9F12-557D489DA763"), "Test 2"));
            builder.Entity<ServiceRequest>().HasData(CreateMockServiceRequest(Guid.Parse("35CB7F2F-3106-1E71-9F12-547D489DA763"), "Test 3"));

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
    }
}
