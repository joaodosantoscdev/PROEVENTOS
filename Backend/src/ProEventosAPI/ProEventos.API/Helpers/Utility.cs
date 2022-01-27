using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.API.Helpers
{
    public class GeneralUtility : IUtility
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public GeneralUtility(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public async Task<string> SaveImg(IFormFile imageFile, string destiny)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName)
                .Take(10)
                .ToArray())
                .Replace(' ', '-');

            imageName = $"{imageName}{DateTime.UtcNow:yymmssfff}{Path.GetExtension(imageFile.FileName)}";

            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources/{destiny}", imageName);

            using (var FileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(FileStream);
            }

            return imageName;
        }

        public void DeleteImg(string imageName, string destiny)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources/{destiny}", imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

        }
    }
}
