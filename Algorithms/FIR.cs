using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.IO;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }  //Sampling frequency
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; } //StopBand that we will determine the window type accordingly
        public float InputTransitionBand { get; set; } //Transition width
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }
        //global
        float omega = 0;
        float fcDash = 0;
        float fc1 = 0;
        float fc2 = 0;
        float omega1 = 0;
        float omega2 = 0;

        //Filter Functions
        List<float> LowPass(float N)
        {
            List<float> filterRes = new List<float>();
            double temp;
            for (int i = (int)(-N / 2); i < N / 2; i++)
            {
                if (i == 0)
                {
                    temp= 2 * fcDash;
                }
                else
                {
                    temp= (2 * fcDash) / (i * omega) * (Math.Sin(i * omega));
                }
                filterRes.Add((float)temp);
            }
            return filterRes;
        }

        List<float> HighPass(float N)
        {
            List<float> filterRes = new List<float>();
            float temp;
            for (int i = (int)(-N / 2); i < N / 2; i++)
            {
                if (i == 0)
                {
                    temp= 1 - 2 * fcDash;
                }
                else
                {
                    temp= -1 * ((2 * fcDash) / (i *omega) * ((float)Math.Sin(i *omega)));
                }
                filterRes.Add((float)temp);
            }
            return filterRes;
        }

        List<float> BandPass(float N)
        {
            List<float> filterRes = new List<float>();
            double temp;
            for (int i = (int)(-N / 2); i < N / 2; i++)
            {
                if (i == 0)
                {
                    temp= 2 * (fc2 - fc1);
                }
                else
                {
                   temp=(((2 * fc2) / (i * omega2) * ((float)(Math.Sin(i * omega2)))) -
                        ((2 * fc1) / (i * omega1) *((float)(Math.Sin(i * omega1)))));
                }
                filterRes.Add((float)temp);
            }
            return filterRes;
        }

        List<float> BandStop(float N)
        {
            List<float> filterRes = new List<float>();
            double temp;
            for (int i = (int)(-N / 2); i < N / 2; i++)
            {
                if (i == 0)
                {
                    temp= (1 - (2 * (fc2 - fc1)));
                }
                else
                {
                    temp = (((2 * fc1) / (i * omega1) * ((float)(Math.Sin(i * omega1)))) -
                        ((2 * fc2) / (i * omega2) * ((float)(Math.Sin(i * omega2)))));
                }
                filterRes.Add((float)temp);
            }
            return filterRes;
        }
        List<float> DetermineAndExecuteFilter(float filter)
        {
            List<float> res = new List<float>();
            switch (InputFilterType)
            {
                case DSPAlgorithms.DataStructures.FILTER_TYPES.LOW:
                    //To avoid smearing effect (fc that is in the middle of the transition width)
                    fcDash = (float)(InputCutOffFrequency + (InputTransitionBand / 2)) / InputFS; //FC`
                    omega = (float)(2 * fcDash * Math.PI);
                    res = LowPass(filter);
                    break;
                case DSPAlgorithms.DataStructures.FILTER_TYPES.HIGH:
                    fcDash = ((float)InputCutOffFrequency - (InputTransitionBand / 2)) / InputFS;
                    omega = (float)(2 * fcDash * Math.PI);
                    res = HighPass(filter);
                    break;
                case DSPAlgorithms.DataStructures.FILTER_TYPES.BAND_PASS:
                    fc1 = ((float)InputF1 - (InputTransitionBand / 2)) / InputFS;
                    fc2 = ((float)InputF2 + (InputTransitionBand / 2)) / InputFS;
                    omega1 = (2 * (float)Math.PI * fc1);
                    omega2 = (2 * (float)Math.PI * fc2);
                    res = BandPass(filter);
                    break;
                case DSPAlgorithms.DataStructures.FILTER_TYPES.BAND_STOP:
                    fc1 = ((float)InputF1 + (InputTransitionBand / 2)) / InputFS;
                    fc2 = ((float)InputF2 - (InputTransitionBand / 2)) / InputFS;
                    omega1 = (2 * (float)Math.PI * fc1);
                    omega2 = (2 * (float)Math.PI * fc2);
                    res = BandStop(filter);
                    break;

            }
            return res;
        }

        //Making functions for every window functions, to facilitate the implementation
        List<List<float>> RectangularWindow(float N)
        {
            if ((int)N % 2 == 0)
            {
                N++;
            }
            else if (N - (int)N != 0)
            {
                N += 2;
            }
            List<List<float>> outputLists = new List<List<float>>();
            List<float> indices = new List<float>();
            List<float> HN = DetermineAndExecuteFilter(N);
            List<float> windowRes = new List<float>();
            for (int i = (int)(-N / 2); i < N / 2; i++)
            {
                windowRes.Add(1);
            }
            for (int i = (int)(-N / 2); i < N / 2; i++)
            {
                indices.Add(i);
            }
            outputLists.Add(windowRes);
            outputLists.Add(HN);
            outputLists.Add(indices);
            return outputLists;
        }

        List<List<float>> HanningWindow(float N)
        {
            if ((int)N % 2 == 0)
            {
                N++;
            }
            else if (N - (int)N != 0)
            {
                N += 2;
            }
            List<List<float>> outputLists = new List<List<float>>();
            List<float> indices = new List<float>();
            List<float> HN = DetermineAndExecuteFilter(N);
            List<float> windowRes = new List<float>();
            for (int i = (int)(-N / 2); i < N / 2; i++)
            {
                windowRes.Add(0.5f + 0.5f * (float)Math.Cos((2 * Math.PI * i) / (int)N));
            }
            for (int i = (int)(-N / 2); i < N / 2; i++)
            {
                indices.Add(i);
            }
            outputLists.Add(windowRes);
            outputLists.Add(HN);
            outputLists.Add(indices);
            return outputLists;
        }

        List<List<float>> HammingWindow(float N)
        {
            if ((int)N % 2 == 0)
            {
                N++;
            }
            else if (N - (int)N != 0)
            {
                N += 2;
            }
            List<List<float>> outputLists = new List<List<float>>();
            List<float> indices = new List<float>();
            List<float> HN = DetermineAndExecuteFilter(N);
            List<float> windowRes = new List<float>();
            for (int i = (int)(-N / 2); i < N / 2; i++)
            {
                double temp = 0.54f + 0.46f * Math.Cos((2 * Math.PI * i) / (int)N);
                windowRes.Add((float)temp);
            }
            for (int i = (int)(-N / 2); i < N / 2; i++)
            {
                indices.Add(i);
            }
            outputLists.Add(windowRes);
            outputLists.Add(HN);
            outputLists.Add(indices);
            return outputLists;
        }


        List<List<float>> BlackMan(float N)
        {
            if ((int)N % 2 == 0)
            {
                N++;
            }
            else if (N - (int)N != 0)
            {
                N += 2;
            }
            List<List<float>> outputLists = new List<List<float>>();
            List<float> indices = new List<float>();
            List<float> HN = DetermineAndExecuteFilter(N);
            List<float> windowRes = new List<float>();
            double temp;
            for (int i = (int)(-N / 2); i < N / 2; i++)
            {
                temp= 0.42f + (0.5f * (float)Math.Cos((2 * (float)Math.PI * i) / (int)(N - 1)))
                    + (0.08f * (float)Math.Cos((4 * (float)Math.PI * i) / (int)(N - 1)));
                windowRes.Add((float)temp);
            }

            for (int i = (int)(-N / 2); i < N / 2; i++)
            {
                indices.Add(i);
            }
            outputLists.Add(windowRes);
            outputLists.Add(HN);
            outputLists.Add(indices);
            return outputLists;
        }


        public override void Run()
        {
            float df = InputTransitionBand / InputFS; //Normalization of delta f
            float N;

            //check the attenuation to determine which window should we use
            List<List<float>> functionOutput = new List<List<float>>();
            if (InputStopBandAttenuation <= 21) { N = 0.9f / df; functionOutput = RectangularWindow(N); }
            else if (InputStopBandAttenuation > 21 && InputStopBandAttenuation <= 44) { N = 3.1f / df; functionOutput = HanningWindow(N); }
            else if (InputStopBandAttenuation > 44 && InputStopBandAttenuation <= 53) { N = 3.3f / df; functionOutput = HammingWindow(N); }
            else  {N = 5.5f / df; functionOutput = BlackMan(N); }

            List<float> WN = functionOutput[0];
            List<float> HN = functionOutput[1];
            List<int> indices = new List<int>();
            List<float> finalSample = new List<float>();
            for(int i = 0; i < functionOutput[2].Count; i++)
            {
                indices.Add((int)functionOutput[2][i]);
                finalSample.Add(WN[i] * HN[i]);
            }

            //Convolving the input signal with the calculated coofecients
            OutputHn = new Signal(finalSample,indices,false);
            DirectConvolution fc = new DirectConvolution();
            fc.InputSignal1 = InputTimeDomainSignal;
            fc.InputSignal2 = OutputHn;
            fc.Run();            
            OutputYn = fc.OutputConvolvedSignal;


            //Writing results in a file
            StreamWriter ff = new StreamWriter("OutputYn.txt", true);
            for (int i = 0; i < OutputYn.Samples.Count; i++)
            {
                ff.Write(OutputYn.Samples[i] + "\n");
            }
            ff.Close();




        }
    }
}
