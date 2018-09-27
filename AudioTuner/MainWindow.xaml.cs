using NAudio.Wave;
using Stuff.DSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NAudio.Dsp;
using NAudio.Wave;
using Stuff;
using System.Diagnostics;

namespace AudioTuner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RunningFrequencyAnalyzer rfa;

        private RunningDFT rdft;

        private RunningFFT rfft;

        private WaveIn waveIn;

        private BufferedWaveProvider bwp;

        public MainWindow()
        {
            InitializeComponent();

            waveIn = new WaveIn();

            waveIn.DataAvailable += HandleWaveInData;

            var m = 12;


            rfa = new RunningFrequencyAnalyzer(1000, RunningFrequencyAnalyzer.LogFreqs(Math.Pow(2, 1d / 1200), 440f / 44100, -2400, 2400));
            rdft = new RunningDFT(1000);
            rfft = new RunningFFT(m, 100);

            var signal = Signal.SineWave(440, 2);

            rfft.NewSamples(signal.ToArray());

            ResultFrequency.Text = "" + rfft.Fundamental() * 44100;
        }

        private void HandleWaveInData(object sender, WaveInEventArgs e)
        {
            bwp.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        private void TestFile()
        {
            using (var audioFile = new AudioFileReader("C:/Users/alber/Documents/Synth/wav/sine_440.wav"))
            {
                var stream = audioFile.Take(new TimeSpan(0, 0, 3));

                var bufferLength = 2000;
                float[] buffer = new float[bufferLength];
                Console.WriteLine(buffer[100]);
                stream.Read(buffer, 0, bufferLength);
                rfa.NewSamples(buffer);

                ResultFrequency.Text = "" + rfa.Fundamental() * 44100;
            }
        }

        private void CalculateFundamental(object sender, RoutedEventArgs e)
        {
            float freq;
            if (float.TryParse(InputFrequency.Text, out freq))
            {
                var signal = Signal.SineWave(freq, 2);
                rfft.NewSamples(signal.ToArray());

                ResultFrequency.Text = "" + rfft.Fundamental() * 44100;
            }
        }
    }
}
