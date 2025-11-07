using System.Windows;
using UD2_2_Bouzas_Prado_Bran.utils;

namespace UD2_2_Bouzas_Prado_Bran.windows
{
    public partial class OpcionesBase : Window
    {
        public OpcionesBase()
        {
            InitializeComponent();
            WindowHelper.PosicionarArribaIzquierda(this);
        }
    }
}