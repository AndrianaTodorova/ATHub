using System;
using System.Collections.Generic;
using System.Text;

namespace ATHub.Services.DataServices
{
    public interface ICategoryService
    {
        IEnumerable<string> GetCategories();
    }
}
