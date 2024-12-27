using Demo.Core.Application.Abstraction;
using Demo.Core.Domain.Common;
using Demo.Core.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Persistence.Data.Interceptors
{
    internal class CustomSaveChangesInterceptor:SaveChangesInterceptor
    {
        private readonly ILoggedInUserService _loggedInUserService;

        public CustomSaveChangesInterceptor(ILoggedInUserService loggedInUserService)
        {
           _loggedInUserService = loggedInUserService;
        }


        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEntities(DbContext? dbContext)
        {
            if (dbContext is null)
                return;

            var entries = dbContext.ChangeTracker.Entries<IBaseAuditableEntity>()
                .Where(entity => entity.State is EntityState.Added or EntityState.Modified);

            foreach (var entry in entries)
            {
                /// if (entry.Entity is Order or OrderItem)  // if i need to set creates Orders or OrderItems with specific userId like admin
                ///     _loggedInUserService.UserId = "";

                //if (string.IsNullOrEmpty(_loggedInUserService.UserId))
                //      _loggedInUserService.UserId = "";

                if (entry.State is EntityState.Added)
                {
                    entry.Entity.CreatedBy = _loggedInUserService.UserId!;
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                }
                entry.Entity.LastModifiedBy = _loggedInUserService.UserId!;
                entry.Entity.LastModifiedOn = DateTime.UtcNow;
            }
        }
    }
}
