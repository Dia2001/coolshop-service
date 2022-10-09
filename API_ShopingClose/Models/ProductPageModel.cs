namespace API_ShopingClose.Model;

/// <summary>
/// Dữ liệu trả về khi lọc và phân trang
/// </summary>
/// <typeparam name="T">Kiểu dữ liệu của đối tượng trong mảng trả về</typeparam>
public class ProductPageModel<T>
{
    /// <summary>
    /// Vị trí trang dữ liệu láy
    /// </summary>
    public long currentPage { get; set; }

    /// <summary>
    /// Kích thước 1 trang
    /// </summary>
    public long pageSize { get; set; }

    /// <summary>
    /// Tổng số trang dữ liệu
    /// </summary>
    public long totalPage { get; set; }

    /// <summary>
    /// Tổng số bản ghi thỏa mãn điều kiện
    /// </summary>
    public long totalRecord { get; set; }

    /// <summary>
    /// Mảng đối tượng thỏa mãn điều kiện lọc và phân trang 
    /// </summary>
    public List<T> Data { get; set; } = new List<T>();
}
