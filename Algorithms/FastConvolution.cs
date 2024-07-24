using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            
            int size = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;
            for (int i = 0; i < size; i++)
            {
                if (InputSignal1.Samples.Count - 1 < i)
                {
                    InputSignal1.Samples.Add(0);
                   
                }

                if (InputSignal2.Samples.Count - 1 < i)
                {
                    InputSignal2.Samples.Add(0);
                   
                }
            }
            DiscreteFourierTransform firstSignal = new DiscreteFourierTransform();
            firstSignal.InputTimeDomainSignal = InputSignal1;
            DiscreteFourierTransform secondSignal = new DiscreteFourierTransform();
            secondSignal.InputTimeDomainSignal = InputSignal2;
            firstSignal.Run();
            secondSignal.Run();
            Signal signal1 = firstSignal.OutputFreqDomainSignal;
            Signal signal2 = secondSignal.OutputFreqDomainSignal;
            Signal output = new Signal(false,new List<float>(), new List<float>(), new List<float>());
            for(int i=0;i< signal1.Samples.Count; i++)
            {
                Complex c1 = new Complex(signal1.FrequenciesAmplitudes[i] * (float)Math.Cos(signal1.FrequenciesPhaseShifts[i]),
                    signal1.FrequenciesAmplitudes[i] * (float)Math.Sin(signal1.FrequenciesPhaseShifts[i]));
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
            OutputConvolvedSignal = res.OutputTimeDomainSignal;


        }
    }
}
