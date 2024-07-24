using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }
        float normalize(List<float> fSig, List<float> sSig)
        {
            double signal1Sum = 0, signal2Sum = 0;
            for (int i = 0; i < fSig.Count; i++)
            {
                signal1Sum += fSig[i] * fSig[i];
                signal2Sum += sSig[i] * sSig[i];
            }
            return (float)Math.Sqrt(signal1Sum * signal2Sum) / fSig.Count;
        }
        void fastCorrelate(Signal sig1, Signal sig2)
        {
            if (sig1 != sig2 && InputSignal2 != null && sig1.Samples.Count!=sig2.Samples.Count)
            {
                int totalSamplesCount = sig1.Samples.Count + sig2.Samples.Count - 1;
                for (int i = sig1.Samples.Count; i < totalSamplesCount; i++)
                    sig1.Samples.Add(0);
                for (int i = sig2.Samples.Count; i < totalSamplesCount; i++)
                    sig2.Samples.Add(0);
            }
            double normSum = normalize(sig1.Samples, sig2.Samples);
            DiscreteFourierTransform firstSignal = new DiscreteFourierTransform();
            firstSignal.InputTimeDomainSignal = sig1;
            DiscreteFourierTransform secondSignal = new DiscreteFourierTransform();
            secondSignal.InputTimeDomainSignal = sig2;
            firstSignal.Run();
            secondSignal.Run();
            Signal signal1 = firstSignal.OutputFreqDomainSignal;
            Signal signal2 = secondSignal.OutputFreqDomainSignal;
            Signal output = new Signal(false, new List<float>(), new List<float>(), new List<float>());

            for (int i = 0; i < signal1.Samples.Count; i++)
            {
                Complex c1 = new Complex(signal1.FrequenciesAmplitudes[i] * (float)Math.Cos(signal1.FrequenciesPhaseShifts[i]),
                    signal1.FrequenciesAmplitudes[i] * (float)Math.Sin(signal1.FrequenciesPhaseShifts[i]));
                c1 = Complex.Conjugate(c1);
                Complex c2 = new Complex(signal2.FrequenciesAmplitudes[i] * (float)Math.Cos(signal2.FrequenciesPhaseShifts[i]),
                    signal2.FrequenciesAmplitudes[i] * (float)Math.Sin(signal2.FrequenciesPhaseShifts[i]));
                Complex d = Complex.Multiply(c1, c2);
                output.FrequenciesAmplitudes.Add((float)d.Magnitude);
                output.FrequenciesPhaseShifts.Add((float)Math.Atan2(d.Imaginary, d.Real));
                output.Frequencies.Add((float)(d.Real + d.Imaginary));
            }
            InverseDiscreteFourierTransform res = new InverseDiscreteFourierTransform();
            res.InputFreqDomainSignal = output;
            res.Run();

            for (int i = 0; i < res.OutputTimeDomainSignal.Samples.Count; i++)
            {
                res.OutputTimeDomainSignal.Samples[i] /= res.OutputTimeDomainSignal.Samples.Count;
            }
            OutputNonNormalizedCorrelation = res.OutputTimeDomainSignal.Samples;
            for (int i = 0; i < OutputNonNormalizedCorrelation.Count; i++)
                OutputNormalizedCorrelation.Add((float)(OutputNonNormalizedCorrelation[i] / normSum));
        }
        public override void Run()
        {
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();
            if (InputSignal2 == null) fastCorrelate(InputSignal1, InputSignal1);
            else fastCorrelate(InputSignal1, InputSignal2);

        }
    }
}