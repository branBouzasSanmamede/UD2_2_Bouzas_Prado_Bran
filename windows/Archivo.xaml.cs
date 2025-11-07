using Microsoft.Win32;
using System.IO;
using System.Windows;
using UD2_2_Bouzas_Prado_Bran.enums;
using UD2_2_Bouzas_Prado_Bran.utils;

namespace UD2_2_Bouzas_Prado_Bran.windows
{
    public partial class Archivo : Window
    {
        private readonly MainWindow _mainWindow;
        private readonly VoiceRecognizer _voz;
        private Opciones<ArchivoEnums> _opciones;

        public Archivo(MainWindow mainWindow, VoiceRecognizer voz)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _voz = voz;

            _voz.EstadoActualizado += (estado) =>
            {
                Dispatcher.Invoke(() => LblEstadoVoz.Content = estado);
            };

            _opciones = new Opciones<ArchivoEnums>();
            _opciones.Show();

            WindowHelper.PosicionarAbajoDerecha(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var screen = SystemParameters.WorkArea;
            Left = screen.Right - Width - 20;
            Top = screen.Bottom - Height - 20;
        }

        private void AbrirArchivo()
        {
            OpenFileDialog dlg = new OpenFileDialog { Filter = "Todos los archivos|*.*" };
            if (dlg.ShowDialog() == true)
            {
                string nombre = Path.GetFileName(dlg.FileName);
                LstArchivos.Items.Add(nombre);
                MessageBox.Show($"Archivo abierto: {nombre}", "Abrir archivo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void GuardarArchivo()
        {
            SaveFileDialog dlg = new SaveFileDialog { Filter = "Texto|*.txt" };
            if (dlg.ShowDialog() == true)
            {
                File.WriteAllText(dlg.FileName, "Archivo generado por comando de voz.");
                MessageBox.Show($"Archivo guardado: {dlg.FileName}", "Guardar archivo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EliminarLista()
        {
            LstArchivos.Items.Clear();
            MessageBox.Show("Lista de archivos vaciada.", "Eliminar lista", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnRefrescar_Click(object sender, RoutedEventArgs e) => EliminarLista();

        private async void BtnEscuchar_Click(object sender, RoutedEventArgs e)
        {
            string texto = await _voz.EscucharAsync();

            if (!string.IsNullOrWhiteSpace(texto))
            {
                var acciones = new Dictionary<ArchivoEnums, Action>
                {
                    { ArchivoEnums.AbrirArchivo, () => AbrirArchivo() },
                    { ArchivoEnums.GuardarArchivo, () => GuardarArchivo() },
                    { ArchivoEnums.EliminarLista, () => EliminarLista() },
                    { ArchivoEnums.Volver, () => this.Close() }
                };

                EnumHelper.EjecutarOpcion<ArchivoEnums>(texto, acciones);
            }
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            _opciones.Close();
            _mainWindow?.Show();
        }
    }
}
