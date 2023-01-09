using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class FileService : IFileService
    {
        private static string SubFolder = "/Images/";
        static FileService()
        {
            Directory.CreateDirectory(Path.GetFullPath(Directory.GetCurrentDirectory() + SubFolder));


        }

        public Task<Stream> GetFile(string path)
        {
            var stream = File.OpenText(Path.GetFullPath(Directory.GetCurrentDirectory() + SubFolder + path));
            return Task.FromResult(stream.BaseStream);
        }

        public async Task<string> Save(string path, Stream str)
        {
            using (var stream = System.IO.File.Create(Path.GetFullPath(Directory.GetCurrentDirectory() + SubFolder+path)))
            {
               await str.CopyToAsync(stream);
            
            }
            return path;
        }

        public Task<string> Save(Stream f)
        {
            var path = Guid.NewGuid().ToString();
            return Save(path, f);
        }
    }
}
