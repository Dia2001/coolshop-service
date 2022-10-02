namespace API_ShopingClose.Common;

public class Validate
{
    private static readonly string[] fileImageType = { "image/png", "image/jepg", "image/jpg" };

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
