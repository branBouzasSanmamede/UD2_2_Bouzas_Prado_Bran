using System.Windows;
using UD2_2_Bouzas_Prado_Bran.utils;
using UD2_2_Bouzas_Prado_Bran.windows;

namespace UD2_2_Bouzas_Prado_Bran
{
    public partial class MainWindow : Window
    {
        private readonly VoiceRecognizer _voz;
        public MainWindow()
        {
            InitializeComponent();
            _voz = new VoiceRecognizer();
        }
        private void BtnNavegador_Click(object sender, RoutedEventArgs e)
        {
            new Navegador(this, _voz).Show();
            this.Hide();
        }
        private void BtnArchivo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Botón 'Abrir archivo' pulsado");
        }
        private void BtnMusica_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Botón 'Abrir música' pulsado");
        }
        private void BtnAyuda_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Botón 'AYUDA!' pulsado");
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            _voz?.Dispose();
            Application.Current.Shutdown();
        }
    }
}