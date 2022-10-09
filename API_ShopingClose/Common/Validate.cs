namespace API_ShopingClose.Common;

public class Validate
{
    private static readonly string[] fileImageType = {
      "image/png", "image/jpeg", "image/jpg", "imamge/gif"};

    public static bool ValidateImageFileNameUpload(string contentType)
    {
        foreach (string type in fileImageType)
        {
            if (type.Equals(contentType))
            {
                return true;
            }
        }
        return false;
    }
}
