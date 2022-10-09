using API_ShopingClose.Entities;

namespace API_ShopingClose.Model;
public class CategoryModel
{
    public long categoryId { get; set; }

    public string? name { get; set; }

    public string slug { get; set; }

    public string description { get; set; }

    public Boolean isShow { get; set; }

    public CategoryModel(Category category)
    {
        categoryId = category.CategoryID;
        name = category.CategoryName;
        slug = category.slug;
        description = category.Description;
        isShow = category.isShow;
    }
}
