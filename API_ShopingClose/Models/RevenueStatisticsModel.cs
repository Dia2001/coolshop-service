namespace API_ShopingClose.Models
{
    public class RevenueStatisticsModel
    {
        public List<decimal> turnover { get; set; }

        public List<int> order { get; set; }

        public List<String> label { get; set; }

        public int num { get; set; }
    }
}
