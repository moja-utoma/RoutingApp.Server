using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoutingApp.API.Services.Interfaces
{
    public interface IDeletable
    {
        Task DeleteAsync(int id);
    }
}