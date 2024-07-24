using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {
            List<float> foldedSignal = new List<float>();
            for(int i = InputSignal.Samples.Count - 1; i >=0; i--)
            {
                foldedSignal.Add(InputSignal.Samples[i]);
            }
            OutputFoldedSignal = new Signal(foldedSignal, InputSignal.SamplesIndices, false);
        }
    }
}
