using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class InverseFastFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            List<Complex> comNumbers = new List<Complex>();

            float real = 0.0f;
            float imag = 0.0f;
            int N = InputFreqDomainSignal.Frequencies.Count;
            for (int i = 0; i < N; i++)
            {
                real = (float)(InputFreqDomainSignal.FrequenciesAmplitudes[i] * Math.Cos(InputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                imag = (float)(InputFreqDomainSignal.FrequenciesAmplitudes[i] * Math.Sin(InputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                comNumbers.Add(new Complex(real, imag));
            }


            Complex c;
            float result;
            List<float> values = new List<float>();
            for (int n = 0; n < InputFreqDomainSignal.Frequencies.Count; n++)
            {

                result = 0;
                for (int k = 0; k < InputFreqDomainSignal.Frequencies.Count; k++)
                {
                    c = new Complex(Math.Cos(2 * k * Math.PI * n / InputFreqDomainSignal.Frequencies.Count), Math.Sin(2 * k * Math.PI * n / InputFreqDomainSignal.Frequencies.Count));
                    Complex temp = Complex.Multiply(comNumbers[k], c);
                    result += (float)(temp.Real + temp.Imaginary);
                }
                result = (float)Math.Round(result);
                values.Add((float)(result / InputFreqDomainSignal.Frequencies.Count));
                Console.WriteLine(values[n]);
            }
            OutputTimeDomainSignal = new Signal(new List<float>(), false);
            OutputTimeDomainSignal.Samples = values;
        }
    }
}
