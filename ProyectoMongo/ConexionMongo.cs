using MongoDB.Driver;

namespace ProyectoMongo
{
    public class ConexionMongo<T>
    {
        private readonly IMongoCollection<T> _collection;

        public ConexionMongo(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<T>(collectionName);
        }

        public void InsertarDocumentos(List<T> datos)
        {
            _collection.InsertMany(datos);
        }

        public List<T> ObtenerDocumentos()
        {
            return _collection.Find(dato => true).ToList();
        }
    }
}

