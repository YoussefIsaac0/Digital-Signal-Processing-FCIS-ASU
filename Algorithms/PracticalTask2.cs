﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        /*Instead of writing the same Stream code after every operation, a global function is made to avoid redundancy
         Res is the signal we will Write in the file, while the path is the file path we want to write within
        finally type attribute is used to determine whether the signal that meant to be written is in time domain,
        or in frequency domain .
        and also to check the parameters we want to write in the file.*/

        void S_Write(Signal Res, string path, string type)
        {
            using (StreamWriter writer = new StreamWriter(path)) {

                if(type == "DFT") writer.WriteLine(1); //freq domain
                else writer.WriteLine(0); // time domain
                writer.WriteLine(0);
                int c = 0;
                if (type == "DFT") c= Res.Frequencies.Count;
                else c= Res.Samples.Count;

                for (int i = 0; i < c; i++) {
                    if(type == "dc") writer.WriteLine( i + " " + Res.Samples[i]);
                    else if (type== "DFT") writer.WriteLine(Res.Frequencies[i] + " " + Res.FrequenciesAmplitudes[i] + " " + Res.FrequenciesPhaseShifts[i]);
                    else  writer.WriteLine(Res.SamplesIndices[i] + " " + Res.Samples[i]);
                }
            }

        }
        public override void Run()
        {
            Signal InputSignal = LoadSignal(SignalPath);

            //FIR
            FIR fir = new FIR();
            fir.InputStopBandAttenuation = 50;
            fir.InputTransitionBand = 500;
            fir.InputF1 = miniF;
            fir.InputF2 = maxF;
            fir.InputFS = Fs;
            fir.InputFilterType = FILTER_TYPES.BAND_PASS;
            fir.InputTimeDomainSignal = InputSignal;
            fir.Run();
            S_Write(fir.OutputYn, "FIR.txt", "FIR");

            //Sampling
            Sampling s = new Sampling();
            if (newFs < 2 * maxF) Console.WriteLine("newFs is not valid."); //to avoid aliasing
            else
            {
                s.InputSignal = fir.OutputYn;
                s.L = L;
                s.M = M;
                s.Run();
                S_Write(s.OutputSignal, "Sampling.txt", "Sampling");
            }

            //Removing DC component
            DC_Component dc = new DC_Component();
            if (newFs < 2 * maxF) dc.InputSignal = fir.OutputYn;
            else dc.InputSignal = s.OutputSignal;
            dc.Run();
            S_Write(dc.OutputSignal, "DC.txt","dc");

            //Normalization
            Normalizer n = new Normalizer();
            n.InputSignal = dc.OutputSignal;
            n.InputMinRange = -1;
            n.InputMaxRange = 1;
            n.Run();
            S_Write(n.OutputNormalizedSignal, "normalizer.txt", "n");

            //DFT
            DiscreteFourierTransform dft = new DiscreteFourierTransform();
            dft.InputTimeDomainSignal = n.OutputNormalizedSignal;
            if (newFs < 2 * maxF) dft.InputSamplingFrequency = Fs;
            else dft.InputSamplingFrequency = newFs;
            dft.Run();
            OutputFreqDomainSignal = dft.OutputFreqDomainSignal;
            S_Write(dft.OutputFreqDomainSignal, "DFT.txt", "DFT");
        }

        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
    }
}
