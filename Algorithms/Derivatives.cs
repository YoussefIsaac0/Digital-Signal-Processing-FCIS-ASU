using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {

            List<float> fd = new List<float>();
            List<float> sd = new List<float>();
            sd.Add(-2*InputSignal.Samples[0] + InputSignal.Samples[1]);
            for(int i = 1; i < InputSignal.Samples.Count; i++)
            {
                fd.Add(InputSignal.Samples[i] - InputSignal.Samples[i - 1]);
                if (i < InputSignal.Samples.Count - 1) sd.Add(-2 * InputSignal.Samples[i] + InputSignal.Samples[i + 1] + InputSignal.Samples[i - 1]);
                //Console.WriteLine(-2 * InputSignal.Samples[i] + InputSignal.Samples[i + 1] + InputSignal.Samples[i - 1]);

            }
            FirstDerivative = new Signal(fd, false);
            SecondDerivative = new Signal(sd, false);
        }
    }
}
