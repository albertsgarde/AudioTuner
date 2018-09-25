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

namespace AudioTuner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            using (var audioFile = new AudioFileReader("C:/Users/alber/Documents/Synth/wav/sine_440.wav"))
            {
                var stream = audioFile.Take(new TimeSpan(0, 0, 3));

                var bufferLength = 100000;
                float[] buffer = new float[bufferLength];
                stream.Read(buffer, 0, bufferLength);

                var rdft = new RunningDFT(2000);
                var f_s = Math.PI * 2 / 44100;
                var f = 970;
                for (int i = 0; i < 4000; ++i)
                    rdft.NewSample((float)Math.Sin(i*f_s*f));

                Frequency.Text = "" + rdft.Fundamental() * 44100;
            }
        }
    }
}
