using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.API.Helpers
{
    public interface IUtility
    {
        Task<string> SaveImg(IFormFile formFile, string destiny);
        void DeleteImg(string imageName, string destiny);
    }
}
