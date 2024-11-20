using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoG18
{
    public partial class AppG18 : Form
    {
        public AppG18()
        {
            InitializeComponent();
            
        }
        private void AppG18_Load(object sender, EventArgs e)
        {
            foreach (var pais in paisesYCiudades.Keys)
            {
                cmbPaises.Items.Add(pais);
            }
        }

        // Diccionario local de países y ciudades
        private Dictionary<string, List<string>> paisesYCiudades = new Dictionary<string, List<string>>()
    {
        { "Argentina", new List<string> { "Buenos Aires", "Córdoba", "Mendoza", "Rosario", "La Plata", "Tucumán", "Mar del Plata", "Salta", "Santa Fe", "Neuquén" }},
        { "Brasil", new List<string> { "São Paulo", "Río de Janeiro", "Salvador", "Brasília", "Fortaleza", "Belo Horizonte", "Curitiba", "Manaos", "Recife", "Porto Alegre" }},
        { "México", new List<string> { "Ciudad de México", "Guadalajara", "Monterrey", "Cancún", "Puebla", "Tijuana", "León", "Zapopan", "Chihuahua", "Toluca" }},
        { "España", new List<string> { "Madrid", "Barcelona", "Valencia", "Sevilla", "Zaragoza", "Málaga", "Murcia", "Palma de Mallorca", "Las Palmas de Gran Canaria", "Bilbao" }},
        { "Francia", new List<string> { "París", "Marsella", "Lyon", "Toulouse", "Niza", "Nantes", "Estrasburgo", "Burdeos", "Lille", "Montpellier" }},
        { "Italia", new List<string> { "Roma", "Milán", "Nápoles", "Turín", "Palermo", "Génova", "Boloña", "Florencia", "Venecia", "Bolonia" }},
        { "Japón", new List<string> { "Tokio", "Osaka", "Kioto", "Hokkaido", "Nagoya", "Yokohama", "Kobe", "Sapporo", "Fukuoka", "Hiroshima" }},
        { "China", new List<string> { "Pekín", "Shanghái", "Guangzhou", "Shenzhen", "Chengdu", "Hong Kong", "Xi'an", "Hangzhou", "Nanjing", "Wuhan" }},
        { "India", new List<string> { "Nueva Delhi", "Mumbai", "Bangalore", "Hyderabad", "Ahmedabad", "Chennai", "Calcuta", "Pune", "Jaipur", "Lucknow" }},
        { "Corea del Sur", new List<string> { "Seúl", "Busán", "Incheon", "Daegu", "Daejeon", "Gwangju", "Ulsan", "Suwon", "Goyang", "Seongnam" }}
    };


        // Al seleccionar un país, cargar las ciudades correspondientes
        private void comboPaises_SelectedIndexChanged(object sender, EventArgs e)
        {
            string paisSeleccionado = cmbPaises.SelectedItem.ToString();
            CargarCiudades(paisSeleccionado);
        }

        // Cargar las ciudades en el ComboBox
        private void CargarCiudades(string pais)
        {
            cmbCuidades.Items.Clear();
            if (paisesYCiudades.ContainsKey(pais))
            {
                foreach (var ciudad in paisesYCiudades[pais])
                {
                    cmbCuidades.Items.Add(ciudad);
                }
            }
        }

        // Método que se ejecutará cuando se haga clic en el botón de "Consultar Clima"
        private async void btnConsultar_Click(object sender, EventArgs e)
        {
            string ciudad = cmbCuidades.SelectedItem?.ToString();
            string pais = cmbPaises.SelectedItem?.ToString();

            if (ciudad == null || pais == null)
            {
                MessageBox.Show("Por favor, selecciona un país y una ciudad.");
                return;
            }

            await ObtenerClima(ciudad, pais);
        }

        // Obtener el clima usando la API
        private async Task ObtenerClima(string ciudad, string pais)
        {
            string urlClima = $"https://wttr.in/{ciudad}?format=j1";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage respuesta = await client.GetAsync(urlClima);
                if (respuesta.IsSuccessStatusCode)
                {
                    string jsonRespuesta = await respuesta.Content.ReadAsStringAsync();
                    try
                    {
                        // Deserializar la respuesta JSON
                        var datosClima = JsonConvert.DeserializeObject<dynamic>(jsonRespuesta);

                        // Verificar si los datos existen antes de intentar acceder a ellos
                        if (datosClima != null && datosClima.current_condition != null)
                        {
                            var currentCondition = datosClima.current_condition[0];

                            // Mostrar los datos en los controles correspondientes
                            lblCiudad.Text = ciudad;
                            lblTemperatura.Text = $"Temperatura: {currentCondition.temp_C}°C";
                            lblHumedad.Text = $"Humedad: {currentCondition.humidity}%";
                            lblSensacionTermica.Text = $"Sensación térmica: {currentCondition.FeelsLikeC}°C";
                            lblViento.Text = $"Viento: {currentCondition.windspeedKmph} km/h";
                        }
                        else
                        {
                            MessageBox.Show("Datos del clima no disponibles.");
                        }
                    }
                    catch (JsonException ex)
                    {
                        MessageBox.Show($"Error al procesar los datos del clima: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("No se pudo obtener la información del clima.");
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void limpiar()
        {
            lblCiudad.Text = "-";
            lblHumedad .Text = "-";
            lblSensacionTermica .Text = "-";
            lblTemperatura .Text = "-";
            lblViento .Text = "-";
        }


    }
}

