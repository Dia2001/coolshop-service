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

        public async Task<Galleries> GetGalleriesByID(long gallryId)
        {
            try
            {
                string sql = "SELECT * FROM gallery where GalleryID = @GalleryID";
                var parameters = new DynamicParameters();
                parameters.Add("@GalleryID", gallryId);
                return await _conn.QueryFirstAsync<Galleries>(sql, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> addListGalleries(List<Galleries> galleries)
        {
            try
            {
                string sql = "INSERT INTO gallery (ProductID, Thumbnail) values(@ProductID, @Thumbnail)";
                await _conn.ExecuteAsync(sql, galleries);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> removeGallery(long gallryId)
        {
            try
            {
                string sql = "DELETE FROM gallery where GalleryID = @GalleryId";
                var parameters = new DynamicParameters();
                parameters.Add("@GalleryID", gallryId);
                await _conn.ExecuteAsync(sql, parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
