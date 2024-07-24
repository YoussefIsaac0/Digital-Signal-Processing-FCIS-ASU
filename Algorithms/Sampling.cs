using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }


        Signal FIR(List<float> s, List<int> d)
        {
            FIR fir = new FIR();
            fir.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
            fir.InputFS = 8000;
            fir.InputStopBandAttenuation = 50;
            fir.InputCutOffFrequency = 1500;
            fir.InputTransitionBand = 500;
            fir.InputTimeDomainSignal = new Signal(s, d, false);
            fir.Run();
            return fir.OutputYn;
        }

        Signal DownSample(Signal InputSig, bool both) //Both bool checks whether L and M !=0, because if both exists
                                                     // we will FIR one time only, so we cancel the Fir of DownSample
        {
            Signal temp1;
            List<int> indices = new List<int>();
            List<float> modifiedSamples = new List<float>();
            if (!both) { temp1 = FIR(InputSig.Samples, InputSig.SamplesIndices); }
            else { temp1 = InputSig; }
            int j = 0;
            for (int i = 0; i < temp1.Samples.Count; i += M, j++)
            {
                modifiedSamples.Add(temp1.Samples[i]);
                indices.Add(j);
            }
            return new Signal(modifiedSamples, indices, false);
        }

        Signal UpSample()
        {
            List<int> indices = new List<int>();
            List<float> modifiedSamples = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                modifiedSamples.Add(InputSignal.Samples[i]);
                indices.Add(i + i * (L - 1)); //if M-1=5
                if (i != (InputSignal.Samples.Count - 1))
                {
                    for (int j = 1; j <= L - 1; j++)
                    {
                        modifiedSamples.Add(0);
                        indices.Add(i + i * (L - 1) + j);
                    }
                }
            }
            return FIR(modifiedSamples, indices);
        }

        public override void Run()
        {
            if (M == 0 && L != 0)
            {
                OutputSignal = UpSample();

            }
            else if (M != 0 && L == 0)
            {
                OutputSignal = DownSample(InputSignal, false);
            }
            else if (M != 0 && L != 0)
            {
                Signal temp = UpSample();
                OutputSignal = DownSample(temp, true);
            }
            else {
                Console.WriteLine("Invalid Option");
                OutputSignal = new Signal(new List<float>(), new List<int>(),false); // to write an empty file in PT2
            };



        }
    }
    // Down => FIR ->downsampling
    // UP   => up -> FIR
    // both => up -> FIR ->Down

}