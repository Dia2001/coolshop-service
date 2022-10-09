using API_ShopingClose.Service;
using Microsoft.AspNetCore.Mvc;
using API_ShopingClose.Entities;
using API_ShopingClose.Model;

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

                List<GalleryModel> galleryModels = new List<GalleryModel>();

                foreach (Galleries gallery in galleries)
                {
                    galleryModels.Add(new GalleryModel(gallery));
                }

                if (galleries != null)
                {
                    return StatusCode(StatusCodes.Status200OK, galleryModels);
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
