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
    }
}
