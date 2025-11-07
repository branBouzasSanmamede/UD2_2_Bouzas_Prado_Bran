using System.Diagnostics;
using System.Speech.Recognition;
using System.Text;
using System.Windows;

namespace UD2_2_Bouzas_Prado_Bran.utils
{
    public class VoiceRecognizer
    {
        public event Action<string>? EstadoActualizado;
        private SpeechRecognitionEngine _recognizer;

        public VoiceRecognizer()
        {
            _recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("es-ES")); 
            _recognizer.SetInputToDefaultAudioDevice();
            _recognizer.LoadGrammar(new DictationGrammar());
        }

        public Task<string> EscucharAsync(int timeoutSeconds = 5)
        {
            var tcs = new TaskCompletionSource<string>();
            EstadoActualizado?.Invoke("🎤 Escuchando...");

            EventHandler<SpeechRecognizedEventArgs>? recognizedHandler = null;
            recognizedHandler = (s, e) =>
            {
                string texto = e.Result.Text.Trim().ToLower();
                EstadoActualizado?.Invoke($"✅ Reconocido: {texto}");
                tcs.TrySetResult(texto);
                _recognizer.SpeechRecognized -= recognizedHandler;
            };

            _recognizer.SpeechRecognized += recognizedHandler;

            _recognizer.RecognizeCompleted += (s, e) =>
            {
                if (!tcs.Task.IsCompleted)
                {
                    EstadoActualizado?.Invoke("⏹️ Reconocimiento completado sin resultado");
                    tcs.TrySetResult(string.Empty);
                }
            };

            _recognizer.RecognizeAsync(RecognizeMode.Single);

            Task.Run(async () =>
            {
                await Task.Delay(timeoutSeconds * 1000);
                if (!tcs.Task.IsCompleted)
                {
                    Debug.WriteLine("[DEBUG] Timeout alcanzado, cancelando reconocimiento");
                    EstadoActualizado?.Invoke("⏱️ Tiempo agotado, cancelando reconocimiento");
                    _recognizer.RecognizeAsyncCancel();
                    tcs.TrySetResult(string.Empty);
                }
            });

            return tcs.Task;
        }

        public void Dispose()
        {
            _recognizer.RecognizeAsyncStop();
            _recognizer.Dispose();
        }
    }
}
