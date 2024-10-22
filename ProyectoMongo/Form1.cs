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
            var conexionMongoDato = new ConexionMongo<Dato>("mongodb://localhost:27017", "MongoToSql", "datos");

            var datosCsv = InsercionDatos.LeerCSV("MOCK_DATA.csv");

            conexionMongoDato.InsertarDocumentos(datosCsv);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var conexionMongoDatoJson = new ConexionMongo<DatoJson>("mongodb://localhost:27017", "MongoToSql", "datos");

            var datosJson = InsercionDatos.LeerJSON("MOCK_DATA.json");
            conexionMongoDatoJson.InsertarDocumentos(datosJson);

            MessageBox.Show("Datos JSON cargados e insertados en MongoDB");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var conexionMongoDato = new ConexionMongo<Dato>("mongodb://localhost:27017", "MongoToSql", "datos");

            var datosTab = InsercionDatos.LeerTab("MOCK_DATA.txt");
            conexionMongoDato.InsertarDocumentos(datosTab);

            MessageBox.Show("Datos Tab.txt cargados e insertados en MongoDB");

        }
    }
}
