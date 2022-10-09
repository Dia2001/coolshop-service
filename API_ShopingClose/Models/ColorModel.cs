using API_ShopingClose.Entities;

namespace API_ShopingClose.Model;

public class ColorModel
{
    public string colorId { get; set; }

    public string? name { get; set; }

    public string description { get; set; }

    public ColorModel(Color color)
    {
        colorId = color.ColorID;
        name = color.ColorName;
        description = color.Description;
    }
}
