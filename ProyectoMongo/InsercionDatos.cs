using System.Globalization;
using CsvHelper;
using MongoDB.Bson.Serialization;
using System.IO;
using System.Text.Json;

namespace ProyectoMongo
{
    public static class InsercionDatos
    {
        public static List<Dato> LeerCSV(string rutaArchivo)
        {
            using (var reader = new StreamReader("archivos/" + rutaArchivo))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<Dato>().ToList();
            }
        }

        public static List<DatoJson> LeerJSON(string rutaArchivo)
        {
            var json = File.ReadAllText("archivos/" + rutaArchivo);
            return JsonSerializer.Deserialize<List<DatoJson>>(json);
        }

        public static List<Dato> LeerTab(string rutaArchivo)
        {
            var datos = new List<Dato>();
            using (var reader = new StreamReader("archivos/" + rutaArchivo))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split('\t');
                    var dato = new Dato
                    {
                        FirstName = values[0],
                        LastName = values[1],
                        Email = values[2],
                        Phone = values[3],
                        Gender = values[4],
                        MovieGenres = values[5],
                        MovieTitle = values[6],
                        Date = DateOnly.Parse(values[7]),
                        Time = TimeOnly.Parse(values[8]),
                        Price = decimal.Parse(values[9]),
                        Seat = int.Parse(values[10]),
                        CinemaRoom = int.Parse(values[11])
                    };
                    datos.Add(dato);
                }
            }
            return datos;
        }
    }
}
