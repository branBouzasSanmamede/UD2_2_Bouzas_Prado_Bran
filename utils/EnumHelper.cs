namespace UD2_2_Bouzas_Prado_Bran.utils
{
    public static class EnumHelper
    {
        public static bool EsOpcionValida<TEnum>(string valor, out TEnum opcion) where TEnum : struct
        {
            return Enum.TryParse(valor, true, out opcion) && Enum.IsDefined(typeof(TEnum), opcion);
        }

        public static void EjecutarOpcion<TEnum>(string valor, Dictionary<TEnum, Action> acciones) where TEnum : struct
        {
            if (EsOpcionValida(valor, out TEnum opcion))
            {
                if (acciones != null && acciones.ContainsKey(opcion))
                {
                    acciones[opcion]?.Invoke();
                }
                else
                {
                    Console.WriteLine($"No hay acción definida para la opción: {opcion}");
                }
            }
            else
            {
                System.Windows.MessageBox.Show($"Opción no válida: {valor}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
        }
    }
}
