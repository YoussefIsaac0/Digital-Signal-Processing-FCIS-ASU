using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        double normalize(List<double>fSig, List<double> sSig)
        {
            double signal1Sum = 0, signal2Sum = 0;
            for (int i = 0; i < fSig.Count; i++)
            {
                signal1Sum += fSig[i] * fSig[i];
                signal2Sum += sSig[i] * sSig[i];
            }
            return Math.Sqrt(signal1Sum * signal2Sum) / fSig.Count;
        }
        void Correlate(Signal sig1,Signal sig2)
        {
            List<float> sol = new List<float>();
            List<double> firstSignal = new List<double>();
            List<double> secondSignal = new List<double>();
            for (int i = 0; i < sig1.Samples.Count; i++)
                firstSignal.Add(sig1.Samples[i]);
            for (int i = 0; i < sig2.Samples.Count; i++)
                secondSignal.Add(sig2.Samples[i]);
            if (firstSignal != secondSignal && InputSignal2!=null)
            {
                int totalSamplesCount = firstSignal.Count + secondSignal.Count - 1;
                for (int i = firstSignal.Count; i < totalSamplesCount; i++)
                    firstSignal.Add(0);
                for (int i = secondSignal.Count; i < totalSamplesCount; i++)
                    secondSignal.Add(0);
            }

            double normSum = normalize(firstSignal, secondSignal);
            
          

            if (InputSignal1.Periodic != true) // then when we shift, the first value becomes the last value, else the last value becomes 0
            {
                for (int i = 0; i < secondSignal.Count; i++)
                {
                    double sum = 0;
                    if (i != 0) // so shift 
                    {
                        double first_element = 0;
                        for (int j = 0; j < secondSignal.Count - 1; j++)
                        {
                            secondSignal[j] = secondSignal[j + 1];
                            sum += secondSignal[j] * firstSignal[j];
                        }
                        secondSignal[secondSignal.Count - 1] = first_element;
                        sum += secondSignal[secondSignal.Count - 1] * firstSignal[firstSignal.Count - 1];
                    }
                    else
                    {
                        for (int j = 0; j < secondSignal.Count; j++)
                            sum += firstSignal[j] * secondSignal[j];
                    }
                    sol.Add((float)sum / secondSignal.Count);
                }
            }

            else
            {
                for (int i = 0; i < secondSignal.Count; i++)
                {
                    double sum = 0;
                    if (i != 0)
                    {
                        double first_element = secondSignal[0];
                        for (int j = 0; j < secondSignal.Count - 1; j++)
                        {
                            secondSignal[j] = secondSignal[j + 1];
                            sum += secondSignal[j] * firstSignal[j];
                        }
                        secondSignal[secondSignal.Count - 1] = first_element;
                        sum += secondSignal[secondSignal.Count - 1] * firstSignal[firstSignal.Count - 1];
                    }
                    else
                    {
                        for (int j = 0; j < secondSignal.Count; j++)
                            sum += firstSignal[j] * secondSignal[j];
                    }
                    sol.Add((float)sum / secondSignal.Count);
                }
            }

            OutputNonNormalizedCorrelation = sol;

            for (int i = 0; i < OutputNonNormalizedCorrelation.Count; i++)
                OutputNormalizedCorrelation.Add((float)(OutputNonNormalizedCorrelation[i] / normSum));
    }

        public override void Run()
        {
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();
            if (InputSignal2 == null) Correlate(InputSignal1, InputSignal1);
            else Correlate(InputSignal1, InputSignal2);



        }
    }
}