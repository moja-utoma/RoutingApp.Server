using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace RoutingApp.API.Data.Interceptors
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        // Synchronous SaveChanges interception
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            SoftDeleteEntities(eventData);
            return result;
        }

        // Asynchronous SaveChangesAsync interception
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            SoftDeleteEntities(eventData);
            return ValueTask.FromResult(result);
        }

        // Common logic to mark entities as soft-deleted
        private void SoftDeleteEntities(DbContextEventData eventData)
        {
            if (eventData.Context == null) return;

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Deleted && entry.Entity is ISoftDeletable delete)
                {
                    entry.State = EntityState.Modified;
                    delete.IsDeleted = true;
                    delete.DeletedAt = DateTimeOffset.UtcNow;
                }
            }
        }
    }
}