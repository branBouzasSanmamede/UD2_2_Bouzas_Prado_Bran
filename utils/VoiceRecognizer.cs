using System.Speech.Recognition;
using System.Windows;

namespace UD2_2_Bouzas_Prado_Bran.utils
{
    public class VoiceRecognizer
    {
        private SpeechRecognitionEngine _recognizer;

        public VoiceRecognizer()
        {
            _recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("es-ES")); 
            _recognizer.SetInputToDefaultAudioDevice();
            _recognizer.LoadGrammar(new DictationGrammar());
        }

        public Task<string> EscucharAsync(int timeoutSeconds = 5)
        {
            Console.WriteLine("[DEBUG] EscucharAsync iniciado");
            var tcs = new TaskCompletionSource<string>();

            _recognizer.LoadGrammar(new DictationGrammar());

            EventHandler<SpeechRecognizedEventArgs> recognizedHandler = null;
            recognizedHandler = (s, e) =>
            {
                Console.WriteLine("[DEBUG] SpeechRecognized: " + e.Result.Text);
                tcs.TrySetResult(e.Result.Text);
                _recognizer.SpeechRecognized -= recognizedHandler;
            };

            _recognizer.SpeechRecognized += recognizedHandler;

            _recognizer.RecognizeCompleted += (s, e) =>
            {
                Console.WriteLine("[DEBUG] RecognizeCompleted");
                if (!tcs.Task.IsCompleted) tcs.TrySetResult(string.Empty);
            };
            try
            {
                Console.WriteLine("[DEBUG] Llamando a RecognizeAsync");
                _recognizer.RecognizeAsync(RecognizeMode.Single);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] Excepción en RecognizeAsync: " + ex.Message);
                tcs.TrySetResult(string.Empty);
            }

            Task.Run(async () =>
            {
                await Task.Delay(timeoutSeconds * 1000);
                if (!tcs.Task.IsCompleted)
                {
                    Console.WriteLine("[DEBUG] Timeout alcanzado, cancelando reconocimiento");
                    _recognizer.RecognizeAsyncCancel();
                    tcs.TrySetResult(string.Empty);
                }
            });

            return tcs.Task;
        }

        public void Dispose()
        {
            _recognizer.Dispose();
        }
    }
}
