

using MongoDB.Bson;
using MongoDB.Driver;
using System.Data.SqlClient;
using System.Globalization;

namespace ProyectoMongo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                List<Dato> datos = LeerCSV();
                Insertar(datos);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public List<Dato> LeerCSV()
        {
            try
            {

                string archivo = "Archivos\\MOCK_DATA.csv";
                string delimitador = ",";
                List<Dato> datos = LeerTexto(archivo, delimitador);


                return datos;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                List<Dato> datos = LeerTAB();
                Insertar(datos);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public List<Dato> LeerTAB()
        {
            try
            {

                string archivo = "Archivos\\MOCK_DATA_TAB.txt";
                string delimitador = "\t";
                List<Dato> datos = LeerTexto(archivo, delimitador);


                return datos;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                List<Dato> datos = LeerJson();
                Insertar(datos);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private List<Dato> LeerJson()
        {
            try
            {
                string data = File.ReadAllText("Archivos\\MOCK_DATA.json");
                var options = new System.Text.Json.JsonSerializerOptions();
                options.PropertyNameCaseInsensitive = true;

                var datosJson = System.Text.Json.JsonSerializer.Deserialize<List<DatoJson>>(data, options);

                List<Dato> datos = new List<Dato>();

                foreach (var datoJson in datosJson)
                {
                    Dato dato = new Dato();
                    dato.FirstName = datoJson.FirstName;
                    dato.LastName = datoJson.LastName;
                    dato.Email = datoJson.Email;
                    dato.Phone = datoJson.Phone;
                    dato.Gender = datoJson.Gender;
                    dato.MovieGenres = datoJson.MovieGenres;
                    dato.MovieTitle = datoJson.MovieTitle;
                    dato.Date = DateOnly.Parse(datoJson.Date);
                    dato.Time = TimeOnly.Parse(datoJson.Time);
                    string precioLimpio = datoJson.Price.Replace("$", "").Replace(",", "");
                    dato.Price = decimal.Parse(precioLimpio);
                    dato.Seat = datoJson.Seat;
                    dato.CinemaRoom = datoJson.CinemaRoom;
                    datos.Add(dato);
                }
                return datos;



            }
            catch (Exception)
            {

                throw;
            }
        }

        private void Insertar(List<Dato> datos)
        {
            try
            {
                Dato dato = new Dato();
                dato.Agregar(datos);
                MessageBox.Show("Datos agregados correctamente :D");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<Dato> LeerTexto(string archivo, string delimitador)
        {
            try
            {
                string[] data = File.ReadAllLines(archivo);

                bool isHeader = true;
                List<Dato> datos = new List<Dato>();

                foreach (string line in data)
                {
                    if (isHeader)
                    {
                        isHeader = false;
                        continue;
                    }
                    var renglon = line.Split(delimitador);

                    DateOnly.TryParse(renglon[8], out DateOnly date);
                    TimeOnly.TryParse(renglon[9], out TimeOnly time);
                    decimal.TryParse(renglon[10], out decimal price);
                    int.TryParse(renglon[11], out int seat);
                    int.TryParse(renglon[12], out int cinemaRoom);

                    Dato dato = new Dato
                    {
                        FirstName = renglon[1],
                        LastName = renglon[2],
                        Email = renglon[3],
                        Phone = renglon[4],
                        Gender = renglon[5],
                        MovieGenres = renglon[6],
                        MovieTitle = renglon[7],
                        Date = date,
                        Time = time,
                        Price = price,
                        Seat = seat,
                        CinemaRoom = cinemaRoom
                    };

                    datos.Add(dato);
                }
                return datos;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                var datos = ObtenerDatosDeMongoDB();

                InsertarDatosEnSqlServer(datos);

                MessageBox.Show("Datos transferidos exitosamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private List<Dato> ObtenerDatosDeMongoDB()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("MongoToSql");
            var collection = database.GetCollection<Dato>("datos");
            return collection.Find(new BsonDocument()).ToList();
        }

        private void InsertarDatosEnSqlServer(List<Dato> datos)
        {
            var connectionString = "Server=localhost\\SQLEXPRESS;Database=MongoToSql;Trusted_Connection=True;";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var dato in datos)
                {
                    int personaId;

                    string personaQuery = @"
            IF NOT EXISTS (SELECT 1 FROM Personas WHERE Email = @Email)
            BEGIN
                INSERT INTO Personas (FirstName, LastName, Email, Phone, Gender)
                VALUES (@FirstName, @LastName, @Email, @Phone, @Gender);
                SELECT CAST(scope_identity() AS int);
            END
            ELSE
            BEGIN
                SELECT PersonaId FROM Personas WHERE Email = @Email;  -- Cambia 'Id' por 'PersonaId'
            END";

                    using (var personaCommand = new SqlCommand(personaQuery, connection))
                    {
                        personaCommand.Parameters.AddWithValue("@FirstName", dato.FirstName);
                        personaCommand.Parameters.AddWithValue("@LastName", dato.LastName);
                        personaCommand.Parameters.AddWithValue("@Email", dato.Email);
                        personaCommand.Parameters.AddWithValue("@Phone", dato.Phone);

                        var genderValue = dato.Gender.Length > 10 ? dato.Gender.Substring(0, 10) : dato.Gender;
                        personaCommand.Parameters.AddWithValue("@Gender", genderValue);

                        personaId = (int)personaCommand.ExecuteScalar();
                    }

                    int peliculaId;

                    var peliculaQuery = @"
            IF NOT EXISTS (SELECT 1 FROM Peliculas WHERE MovieTitle = @MovieTitle)
            BEGIN
                INSERT INTO Peliculas (MovieTitle, MovieGenres)
                VALUES (@MovieTitle, @MovieGenres);
                SELECT CAST(scope_identity() AS int);
            END
            ELSE
            BEGIN
                SELECT PeliculaId FROM Peliculas WHERE MovieTitle = @MovieTitle;  -- Cambia 'Id' por 'PeliculaId'
            END";

                    using (var peliculaCommand = new SqlCommand(peliculaQuery, connection))
                    {
                        peliculaCommand.Parameters.AddWithValue("@MovieTitle", dato.MovieTitle);
                        peliculaCommand.Parameters.AddWithValue("@MovieGenres", dato.MovieGenres);

                        peliculaId = (int)peliculaCommand.ExecuteScalar();
                    }

                    var funcionQuery = @"
            INSERT INTO Funciones (Fecha, Hora, Precio, Asiento, SalaDeCine, PersonaId, PeliculaId)
            VALUES (@Fecha, @Hora, @Precio, @Asiento, @SalaDeCine, @PersonaId, @PeliculaId)";

                    using (var funcionCommand = new SqlCommand(funcionQuery, connection))
                    {
                        var formattedDate = dato.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                        var formattedTime = dato.Time.ToString("HH:mm:ss", CultureInfo.InvariantCulture);

                        funcionCommand.Parameters.AddWithValue("@Fecha", formattedDate);
                        funcionCommand.Parameters.AddWithValue("@Hora", formattedTime);
                        funcionCommand.Parameters.AddWithValue("@Precio", dato.Price);
                        funcionCommand.Parameters.AddWithValue("@Asiento", dato.Seat);
                        funcionCommand.Parameters.AddWithValue("@SalaDeCine", dato.CinemaRoom);
                        funcionCommand.Parameters.AddWithValue("@PersonaId", personaId);
                        funcionCommand.Parameters.AddWithValue("@PeliculaId", peliculaId);

                        funcionCommand.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
