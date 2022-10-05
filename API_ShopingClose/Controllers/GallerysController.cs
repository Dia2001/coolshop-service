using API_ShopingClose.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_ShopingClose.Controllers
{
    [Route("api/v1/galleries")]
    [ApiController]
    public class GallerysController : ControllerBase
    {
        GalleryDeptService _galleryservice;

        public GallerysController(GalleryDeptService galleryservice)
        {
            _galleryservice = galleryservice;
        }

        [HttpGet]
        public IActionResult GetAllGalleries()
        {
            try
            {

                var galleries = _galleryservice.GetAllGalleries();

                if (galleries != null)
                {
                    return StatusCode(StatusCodes.Status200OK, galleries);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }
    }
}
