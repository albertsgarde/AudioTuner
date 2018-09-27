using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff;
using Stuff.DSP;
using NAudio.Dsp;

namespace AudioTuner
{
    public class RunningFFT
    {
        public int M { get; }

        public int Size { get; }

        public IReadOnlyList<Complex> FFT { get; private set; }

        public int FFTPeriod { get; }

        private readonly CircularArray<float> signal;

        private readonly Complex[] cSignal;

        private int lastFFT;

        public RunningFFT(int m, int fftPeriod)
        {
            M = m;
            Size = (int)Math.Pow(2, m);
            FFTPeriod = fftPeriod;
            signal = new CircularArray<float>(Size);
            cSignal = new Complex[Size];
            lastFFT = 0;
        }

        public void NewSample(float sample)
        {
            ++lastFFT;
            signal.Add(sample);
            if (lastFFT > FFTPeriod)
                CalculateFFT();
        }

        public void NewSamples(float[] samples)
        {
            lastFFT += samples.Length;
            signal.Add(samples);
            if (lastFFT > FFTPeriod)
                CalculateFFT();
        }

        public IEnumerable<float> Amplitudes()
        {
            for (int k = 0; k < Size; ++k)
                yield return FFT[k].Length();
        }

        public float Fundamental()
        {
            float aMax = cSignal.Max(z => z.LengthSquared());
            return (float)cSignal.Select(z => z.LengthSquared()).IndexOf(aMax) / Size;
        }

        private void CalculateFFT()
        {
            for (int i = 0; i < Size; ++i)
                cSignal[i] = new Complex { X = signal[i], Y = 0 };
            FastFourierTransform.FFT(true, M, cSignal);
            FFT = new List<Complex>(cSignal);
            lastFFT = 0;
        }
    }
}
