using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UD2_2_Bouzas_Prado_Bran.enums;
using UD2_2_Bouzas_Prado_Bran.utils;

namespace UD2_2_Bouzas_Prado_Bran.windows
{
    public partial class Musica : Window
    {
        private readonly MainWindow _mainWindow;
        private readonly VoiceRecognizer _voz;
        private readonly Opciones<MusicaEnums> _opciones;
        private MediaPlayer _player = new MediaPlayer();
        private List<string> _canciones;
        private int _indiceActual = 0;

        public Musica(MainWindow mainWindow, VoiceRecognizer voz)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _voz = voz;

            _opciones = new Opciones<MusicaEnums>();
            _opciones.Show();

            CargarCanciones();
            WindowHelper.PosicionarAbajoDerecha(this);

            _voz.EstadoActualizado += estado => Dispatcher.Invoke(() => LblEstadoVoz.Content = estado);
        }

        private void CargarCanciones()
        {
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "songs");
            _canciones = Directory.GetFiles(ruta, "*.mp3").ToList();
            if (!_canciones.Any())
            {
                MessageBox.Show("No se encontraron canciones en la carpeta /songs.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Reproducir()
        {
            if (_canciones.Count == 0) return;
            _player.Open(new Uri(_canciones[_indiceActual]));
            _player.Play();
            CambiarImagenCancion();
        }

        private void Pausar()
        {
            _player.Pause();
            CambiarImagen("stop.png");
        }

        private void Siguiente()
        {
            if (_canciones.Count == 0) return;
            _indiceActual = (_indiceActual + 1) % _canciones.Count;
            Reproducir();
        }

        private void Anterior()
        {
            if (_canciones.Count == 0) return;
            _indiceActual = (_indiceActual - 1 + _canciones.Count) % _canciones.Count;
            Reproducir();
        }

        private void CambiarImagenCancion()
        {
            string nombreArchivo = Path.GetFileNameWithoutExtension(_canciones[_indiceActual]).ToLower();
            CambiarImagen(nombreArchivo + ".png");
        }
        private void CambiarImagen(string nombreImagen)
        {
            try
            {
                ImgCancion.Source = new BitmapImage(new Uri($"/img/songs/{nombreImagen}", UriKind.Relative));
            }
            catch
            {
                ImgCancion.Source = new BitmapImage(new Uri("/img/default.png", UriKind.Relative));
            }
        }

        private async void BtnEscuchar_Click(object sender, RoutedEventArgs e)
        {
            string texto = await _voz.EscucharAsync();

            var acciones = new Dictionary<MusicaEnums, Action>
            {
                { MusicaEnums.Iniciar, () => Reproducir() },
                { MusicaEnums.Parar, () => Pausar() },
                { MusicaEnums.Posterior, () => Siguiente() },
                { MusicaEnums.Anterior, () => Anterior() },
                { MusicaEnums.Volver, () => Close() }
            };

            EnumHelper.EjecutarOpcion(texto, acciones);
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e) => Close();

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            _player.Stop();
            _opciones.Close();
            _mainWindow.Show();
        }
    }
}