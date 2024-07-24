using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; } //check
        public int InputNumBits { get; set; } //check
        public Signal InputSignal { get; set; } //check
        public Signal OutputQuantizedSignal { get; set; } //check
        public List<int> OutputIntervalIndices { get; set; } //check
        public List<string> OutputEncodedSignal { get; set; } //check
        public List<float> OutputSamplesError { get; set; } //check

        public override void Run()
        {
            //At the beginning we initialize the lists to take a place in the heap.
            OutputIntervalIndices = new List<int>();
            OutputEncodedSignal = new List<string>();
            OutputSamplesError = new List<float>();


            //at first we need to specify whether the user has given us the input levels or bits, and if it is input bits we make it levels.
            if (InputNumBits > 0) { InputLevel = (int)Math.Pow(2, InputNumBits); }
            //Input Level = 2^inputNumBits;
            else
            {
                InputNumBits = (int)Math.Log(InputLevel, 2);
            }


            //Second: we get the min and max values to know the size of levels
            float max = InputSignal.Samples.Max() , min= InputSignal.Samples.Min();
            
            float QuantizedValue = (max - min) / InputLevel; //check


            //Fourth: we insert Quantized Values in the OutputQuantizedSignal.
            List<float> result = new List<float>();
            for(int i = 0; i < InputSignal.Samples.Count; i++)
            {
                float curSampleVal = 0; //refers to the quantized value
                int levelNumber = 0;
                for(float j = min; j < max; j+=QuantizedValue)
                {
                    if(InputSignal.Samples[i]>=j && InputSignal.Samples[i] <= j+QuantizedValue) //maxRange== minRange+quantizedValue
                    {
                        curSampleVal = (j + j + QuantizedValue) / 2;
                        if (curSampleVal > max) { curSampleVal -= QuantizedValue; levelNumber--; }
                        
                        OutputIntervalIndices.Add(levelNumber+1);
                        OutputSamplesError.Add(curSampleVal- InputSignal.Samples[i]);

                        //now we need to specify the number of bits of the encodedSignal
                        string encoded = "";
                        for (int k = 0; k < InputNumBits - Convert.ToString(levelNumber, 2).Length; k++)
                        {
                            encoded += "0";
                        }
                        //5bits 
                        //00001 1
                        //00010 -> 10
                        
                        encoded += Convert.ToString(levelNumber, 2);
                        OutputEncodedSignal.Add(encoded);
                        break;
                    }
                    levelNumber++;
                }
                result.Add(curSampleVal);
            }
            OutputQuantizedSignal = new Signal(result, false);
        }
        //4 levels
        //1st = 001
    }
}
