using API_ShopingClose.Entities;
using Dapper;
using MySqlConnector;

namespace API_ShopingClose.Service
{
    public class GalleryDeptService
    {
        private readonly MySqlConnection _conn;

        public GalleryDeptService(MySqlConnection conn)
        {

            _conn = conn;
        }

        public IEnumerable<Galleries> GetAllGalleries()
        {
            string getAllGallerysCommand = "SELECT * FROM gallery;";
            var result = this._conn.Query<Galleries>(getAllGallerysCommand);
            return result;
        }

        public async Task<IEnumerable<Galleries>> GetAllGalleriesByProductId(Guid productId)
        {
            string sql = "SELECT * FROM gallery where ProductID = @ProductID;";
            var parameters = new DynamicParameters();
            parameters.Add("@ProductID", productId);

            return (await _conn.QueryAsync<Galleries>(sql, parameters)).ToList();
        }
    }
}
