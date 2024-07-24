using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            List<float> sol = new List<float>();
            //First we get min and max value of the samples in the signal.
            float mx = -2000000000, min = 2000000000;
            for(int i = 0; i < InputSignal.Samples.Count; i++)
            {
                if (InputSignal.Samples[i] <= min) { min = InputSignal.Samples[i]; }
                if (InputSignal.Samples[i] >= mx) { mx = InputSignal.Samples[i]; }
            }
            //then we normalize.
            for(int i = 0;i < InputSignal.Samples.Count; i++)
            {
                float tmp = ((InputMaxRange - InputMinRange) * ((InputSignal.Samples[i] - min) / (mx - min))) +InputMinRange ;
                sol.Add(tmp);
            }
            OutputNormalizedSignal = new Signal(sol, false);
        }
    }
}
