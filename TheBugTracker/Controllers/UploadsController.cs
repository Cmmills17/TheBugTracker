using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using TheBugTracker.Data;
using TheBugTracker.Models;

namespace TheBugTracker.Controllers
{
    [Route("uploads")]
    [ApiController]
    public class UploadsController(ApplicationDbContext context) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        [OutputCache(VaryByRouteValueNames = ["id"], Duration = 60 * 60 * 24)]


        public async Task<IActionResult> GetImage(Guid id)
        {
            FileUpload? image = await context.Uploads.FirstOrDefaultAsync(img => img.Id == id);

            if (image == null)
            {
                return NotFound();
            }

            return File(image.Data!, image.FileType!);

        }
    }
}
