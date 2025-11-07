using UD2_2_Bouzas_Prado_Bran.windows;

namespace UD2_2_Bouzas_Prado_Bran.utils
{
    public class Opciones<TEnum> : OpcionesBase where TEnum : Enum
    {
        public Opciones()
        {
            LstOpciones.ItemsSource = Enum.GetNames(typeof(TEnum));
        }
    }
}