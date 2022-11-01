using API_ShopingClose.Service;
using Microsoft.AspNetCore.Mvc;
using API_ShopingClose.Entities;
using API_ShopingClose.Model;
using API_ShopingClose.Common;

namespace API_ShopingClose.Controllers;

[Route("api/v1")]
[ApiController]
public class GallerysController : ControllerBase
{
    GalleryDeptService _galleryservice;

    public GallerysController(GalleryDeptService galleryservice)
    {
        _galleryservice = galleryservice;
    }

    [HttpGet]
    [Route("galleries")]
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

    [HttpGet]
    [Route("galleries/{productId}")]
    public async Task<IActionResult> getAllGallertyOneProducts([FromRoute] Guid productId)
    {
        try
        {
            List<Galleries> allGalleries = (await _galleryservice.GetAllGalleriesByProductId(productId)).ToList();

            List<GalleryModel> galleryOneProduct = new List<GalleryModel>();

            foreach (Galleries gallery in allGalleries)
            {
                galleryOneProduct.Add(new GalleryModel(gallery));
            }

            if (allGalleries != null)
            {
                return StatusCode(StatusCodes.Status200OK, galleryOneProduct);
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

    [HttpPost]
    [Route("galleries/{productId}")]
    public async Task<IActionResult> addImageInProductByProductId([FromRoute] Guid productId, [FromForm] IFormFile[] files)
    {
        try
        {
            List<Galleries> galleries = new List<Galleries>();

            foreach (IFormFile file in files)
            {
                Galleries gallery = new Galleries();
                gallery.ProductID = productId;
                gallery.Thumbnail = await Utils.UploadFile(file);

                galleries.Add(gallery);
            }

            await _galleryservice.addListGalleries(galleries);

            return StatusCode(StatusCodes.Status201Created, "Thêm ảnh thành công");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(StatusCodes.Status400BadRequest, "e001");
        }
    }

    [HttpDelete]
    [Route("galleries/{galleryId}")]
    public async Task<IActionResult> deleteGallery([FromRoute] long galleryId)
    {
        try
        {
            Galleries gallery = await _galleryservice.GetGalleriesByID(galleryId);
            if (gallery != null)
            {
                // Xoa file anh goc (Tam thoi an di do may bi loi khong cho xoa)
                // System.IO.File.Delete(Constants.ROOT_PATH_IMAGE_PRODUCT + gallery.Thumbnail);
                await _galleryservice.removeGallery(galleryId);
                return StatusCode(StatusCodes.Status200OK, "Xóa ảnh thành công!");
            }
            return StatusCode(StatusCodes.Status400BadRequest, "e001");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(StatusCodes.Status400BadRequest, "e001");
        }
    }
}
