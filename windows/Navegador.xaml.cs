using System.Windows;
using System.Windows.Media;
using UD2_2_Bouzas_Prado_Bran.enums;
using UD2_2_Bouzas_Prado_Bran.utils;

namespace UD2_2_Bouzas_Prado_Bran.windows
{
    public partial class Navegador : Window
    {
        private readonly MainWindow _mainWindow;
        private readonly VoiceRecognizer _voz;
        public Navegador(MainWindow mainWindow, VoiceRecognizer voz)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _voz = voz;
            _voz.EstadoActualizado += (estado) =>
            {
                Dispatcher.Invoke(() => LblEstadoVoz.Content = estado);
            };
        }
        private void BtnAtras_Click(object sender, RoutedEventArgs e) { if (web.CanGoBack) web.GoBack(); }
        private void BtnAdelante_Click(object sender, RoutedEventArgs e) { if (web.CanGoForward) web.GoForward(); }
        private void BtnRecargar_Click(object sender, RoutedEventArgs e) { web.Reload(); }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string texto = TxtBusqueda.Text;
            if (string.IsNullOrWhiteSpace(texto)) return;

            string url = texto.StartsWith("http") ? texto : $"https://www.google.com/search?q={Uri.EscapeDataString(texto)}";

            web.Source = new Uri(url);
        }
        private async void BtnVoz_Click(object sender, RoutedEventArgs e)
        {
            string texto = await _voz.EscucharAsync();

            if (!string.IsNullOrWhiteSpace(texto))
            {
                TxtBusqueda.Text = texto;

                var acciones = new Dictionary<NavegadorEnums, Action>
                {
                    { NavegadorEnums.Google, () => web.Source = new Uri("https://www.google.com") },
                    { NavegadorEnums.YouTube, () => web.Source = new Uri("https://www.youtube.com") },
                    { NavegadorEnums.Wikipedia, () => web.Source = new Uri("https://www.wikipedia.org") },
                    { NavegadorEnums.Buscar, () => BtnBuscar_Click(sender, e) },
                    { NavegadorEnums.Cerrar, () => this.Close() }
                };

                EnumHelper.EjecutarOpcion<NavegadorEnums>(texto, acciones);
            }
        }
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            _mainWindow?.Show();
        }
    }
}
