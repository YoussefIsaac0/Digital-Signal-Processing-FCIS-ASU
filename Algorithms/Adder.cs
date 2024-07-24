using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }


        public override void Run()
        {
            //First we get the biggest sample size, to make the difference between the other fewer signal-sized samples = 0,
            //for the sake of not throwing outOfRange exception.
            int mxSize = -1;
            for(int i = 0; i < InputSignals.Count; i++)
            {
                if (InputSignals[i].Samples.Count >= mxSize)
                {
                    mxSize = InputSignals[i].Samples.Count;
                }
            }
            //then we iterate over all signals and add its samples altogether;
            List<float> sol=new List<float>();
            for(int i = 0; i < mxSize; i++)
            {
                float tmp = 0;
                for(int j = 0; j < InputSignals.Count; j++)
                {
                    if (InputSignals[j].Samples.Count >= i)
                    {
                        tmp += InputSignals[j].Samples[i];
                    }
                }
                sol.Add(tmp);
            }
           
            OutputSignal = new Signal(sol, false);

        }
    }
}