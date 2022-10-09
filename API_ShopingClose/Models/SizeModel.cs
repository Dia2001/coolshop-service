using API_ShopingClose.Entities;

namespace API_ShopingClose.Model;

public class SizeModel
{
    public string sizeId { get; set; }

    public string? name { get; set; }

    public string description { get; set; }

    public SizeModel(Size size)
    {
        sizeId = size.SizeID;
        name = size.SizeName;
        description = size.Description;
    }
}
