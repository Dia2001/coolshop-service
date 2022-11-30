namespace API_ShopingClose.Common;

public static class Utils
{

    public static string getSlugFromName(string name)
    {
        return name.NonUnicode().ToLower().Replace(" ", "-");
    }

    public static string NonUnicode(this string text)
    {
        string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
    "đ",
    "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
    "í","ì","ỉ","ĩ","ị",
    "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
    "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
    "ý","ỳ","ỷ","ỹ","ỵ",};
        string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
    "d",
    "e","e","e","e","e","e","e","e","e","e","e",
    "i","i","i","i","i",
    "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
    "u","u","u","u","u","u","u","u","u","u","u",
    "y","y","y","y","y",};
        for (int i = 0; i < arr1.Length; i++)
        {
            text = text.Replace(arr1[i], arr2[i]);
            text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
        }
        return text;
    }

    public static async Task<string> UploadFile(IFormFile file)
    {
        try
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd-HHmmss") + file.FileName;
            string path = Constants.ROOT_PATH_IMAGE_PRODUCT + fileName;

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return fileName;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return "";
        }
    }

    public static int getMaxDateOfMonth(int month, int year)
    {
        int maxDateMonth = 0;
        if (month >= 1 && month <= 12)
        {
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12: maxDateMonth = 31; break;
                case 4:
                case 6:
                case 9:
                case 11: maxDateMonth = 30; break;

                case 2:
                    if (year % 400 == 0 || (year % 4 == 0 && year % 100 != 0))    // leap year
                        maxDateMonth = 29;
                    else
                        maxDateMonth = 28;
                    break;
            }
        }
        // You can use another, quicker way
        /*  
        *  DateTime.DaysInMonth(1980, 08);
        */
        return maxDateMonth;
    }

    // function get start day and end day
    public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
    {
        for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            yield return day;
    }

    // function get clause filter by price 
    public static IDictionary<string, string> getDecimalFilter(decimal? price)
    {
        string priceClauseOne = "";
        string priceClauseTwo = "";

        if (price == 100000)
        {
            priceClauseOne = $"product.Price < {price}";
        }
        else
        {
            if (price == 200000)
            {
                priceClauseOne = $"product.Price >= {100000}";
                priceClauseTwo = $"product.Price <= {price}";
            }
            else
            {
                if (price == 350000)
                {
                    priceClauseOne = $"product.Price >= {200000}";
                    priceClauseTwo = $"product.Price <= {price}";
                }
                else
                {
                    if (price == 500000)
                    {
                        priceClauseOne = $"product.Price >= {350000}";
                        priceClauseTwo = $"product.Price <= {price}";
                    }
                    else
                    {
                        if (price == 700000)
                        {
                            priceClauseOne = $"product.Price >= {500000}";
                            priceClauseTwo = $"product.Price <= {price}";
                        }
                        else
                        {
                            priceClauseOne = $"product.Price >= {700000}";
                        }
                    }
                }
            }
        }

        IDictionary<string, string> priceClause = new Dictionary<string, string>();

        priceClause.Add("priceClauseOne", priceClauseOne);
        priceClause.Add("priceClauseTwo", priceClauseTwo);

        return priceClause;
    }

    // function get filter clause in sort
    public static string getSortClause(string sort)
    {
        string sortClause = "";

        if (sort.Equals("az"))
        {
            sortClause = "product.ProductName ASC";
        }
        else
        {
            if (sort.Equals("za"))
            {
                sortClause = "product.ProductName DES";
            }
            else
            {
                if (sort.Equals("cheap"))
                {
                    sortClause = "product.Price ASC";
                }
                else
                {
                    if (sort.Equals("decrease"))
                    {
                        sortClause = "product.Price DES";
                    }
                    else
                    {
                        if (sort.Equals("newer"))
                        {
                            sortClause = "product.CreatedDate DESC";
                        }
                        else
                        {
                            sortClause = "product.Rate DESC";
                        }
                    }
                }
            }
        }

        return sortClause;
    }
}
