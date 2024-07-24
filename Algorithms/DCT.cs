using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> sol = new List<float>();
            int N = InputSignal.Samples.Count;
            for (int i = 0; i <N ; i++)
            {
                float temp = 0;
                for (int j = 0; j < N; j++)
                {
                    temp += InputSignal.Samples[j] * (float)Math.Cos((float)(Math.PI / (4 * N)) * (2 * (j) - 1) * (2 * (i) - 1));
                }
                temp *= (float)Math.Sqrt(2 / (float)N);
                sol.Add(temp);
            }
            OutputSignal = new Signal(sol, false);
        }
    }
}
