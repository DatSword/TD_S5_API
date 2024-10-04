using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD1BlazorApp.Models;

namespace TD1BlazorApp.Services
{
    public interface IService<T>
    {
        Task<List<T>> GetItemsAsync();
    }
}