using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Subtractor : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputSignal { get; set; }

        /// <summary>
        /// To do: Subtract Signal2 from Signal1 
        /// i.e OutSig = Sig1 - Sig2 
        /// </summary>
        
        //sub function is made by youssef isaac, its purpose is not to repeat the code, acts as a pointer to the max signal
        //(the one which has more samples) and to the min signal
        void sub(Signal mx, Signal min, List<float> l)
        {
            float tmp;
            for(int i = 0; i<mx.Samples.Count; i++)
            {
                if (i <= min.Samples.Count)
                {
                     tmp = mx.Samples[i] - min.Samples[i];
                }
                else
                {
                     tmp = mx.Samples[i];
                }
                l.Add(tmp);
            }
        }
        public override void Run()
        {
            List<float> sol = new List<float>();
            if (InputSignal1.Samples.Count >= InputSignal2.Samples.Count)
            {
                sub(InputSignal1, InputSignal2, sol);
            }
            else
            {
                sub(InputSignal2, InputSignal1, sol);
            }
            OutputSignal = new Signal(sol, false);
        }
    }
}