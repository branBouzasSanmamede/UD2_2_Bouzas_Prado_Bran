using System.Windows;

namespace UD2_2_Bouzas_Prado_Bran.utils
{
    public static class WindowHelper
    {
        public static void PosicionarAbajoDerecha(Window window, double margen = 20)
        {
            window.ContentRendered += (_, __) =>
            {
                var screen = SystemParameters.WorkArea;
                window.Left = screen.Right - window.ActualWidth - margen;
                window.Top = screen.Bottom - window.ActualHeight - margen;
            };
        }
        public static void PosicionarArribaIzquierda(Window window, double margen = 20)
        {
            window.ContentRendered += (_, __) =>
            {
                window.Left = margen;
                window.Top = margen;
            };
        }
    }
}