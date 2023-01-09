using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IFileService
    {
        public Task<string> Save(string path, Stream f);

        public Task<string> Save(Stream f);

        public Task<Stream> GetFile(string path);

    }
}
