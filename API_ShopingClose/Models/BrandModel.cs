using API_ShopingClose.Entities;

namespace API_ShopingClose.Model;

public class BrandModel
{
    public long brandId { get; set; }

    public string? name { get; set; }

    public string description { get; set; }

    public BrandModel(Brand brand)
    {
        brandId = brand.BrandID;
        name = brand.BrandName;
        description = brand.Description;
    }
}
