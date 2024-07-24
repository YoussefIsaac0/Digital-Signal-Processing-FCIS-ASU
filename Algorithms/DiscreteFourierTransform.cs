using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
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
            float segma = (int)(2 * Math.PI) / (InputTimeDomainSignal.Samples.Count * (1 / InputSamplingFrequency));
            List<float> freq = new List<float>();
            for (int i = 0; i < N; i++)
            {
                freq.Add((float)Math.Round((segma * (i+1)),1));
            }
            OutputFreqDomainSignal = new Signal(X, false);
            OutputFreqDomainSignal.FrequenciesAmplitudes = amp;
            OutputFreqDomainSignal.FrequenciesPhaseShifts = phase;
            OutputFreqDomainSignal.Frequencies = freq;

        }
    }
}
