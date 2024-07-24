using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {

            List<float> convolution = new List<float>();

            int fx = InputSignal1.Samples.Count;
            int fh = InputSignal2.Samples.Count;

            int min = InputSignal1.SamplesIndices.Min() + InputSignal2.SamplesIndices.Min();
            int max = InputSignal1.SamplesIndices.Max() + InputSignal2.SamplesIndices.Max();



            int counter = fx + fh - 1;

            for (int i = 0; i < counter; i++)
            {
                float sum = 0;

                for (int k = 0; k < fx; k++)
                {
                    // in range of f(x)
                    if ((i - k) >= 0 && (i - k) < fh)
                        sum += InputSignal1.Samples[k] * InputSignal2.Samples[i - k];
                }
                convolution.Add(sum);

            }
            List<int> indecies = new List<int>();

            for (int y = min; y <= max; y++)
            {
                indecies.Add(y);
            }
            if (convolution[counter - 1] == 0)
            {
                convolution.RemoveAt(counter - 1);
                indecies.RemoveAt(counter - 1);
            }
            OutputConvolvedSignal = new Signal(convolution, indecies, false);

        }
    }
}
