using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Reservation.Areas.Static.Controllers
{
    [Area("static")]
    [Route("/static/{path}")]
    public class StaticController : Controller
    {
        IFileService _fileService;
        public StaticController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpGet("/static/{path}")]
        public async Task<IActionResult> Get(string path)
        {   
            var file = await _fileService.GetFile(path);

            return File(file, "image/png", path);
        }
    }
}
