using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public int InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            List<float> phase = new List<float>();
            List<float> amp = new List<float>();
            float phi = 0;

            //Given sequence of Numbers

            List<float> X = InputTimeDomainSignal.Samples;
            //  List<float> Y = InputTimeDomainSignal.FrequenciesAmplitudes;
            float N = InputTimeDomainSignal.Samples.Count;


            for (int k = 0; k < N; k++)
            {
                float re = 0;
                float im = 0;
                //The discrete Fourier transform, lists of Re & Im instead of a+ib
                for (int n = 0; n < N; n++)
                {
                    phi = (2 * (float)Math.PI * k * n) / N;
                    re += X[n] * (float)Math.Cos(phi);
                    im += -X[n] * (float)Math.Sin(phi);
                }
                var ampp = Math.Sqrt(re * re + im * im);
                var phasse = Math.Atan2(im, re);

                phase.Add((float)phasse);
                amp.Add((float)ampp);
            }
            float segma = 2 * (float)(Math.PI) * InputSamplingFrequency / N;
            List<float> freq = new List<float>();
            for (int I = 1; I < N; I++)
            {
                freq.Add(segma * I);
            }
            OutputFreqDomainSignal = new Signal(X, false);
            OutputFreqDomainSignal.FrequenciesAmplitudes = amp;
            OutputFreqDomainSignal.FrequenciesPhaseShifts = phase;
            OutputFreqDomainSignal.Frequencies = freq;
        }
    }
}
