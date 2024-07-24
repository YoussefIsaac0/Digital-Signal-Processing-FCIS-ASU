using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
            float maxCorrelatedValue = -100000000;
            float solIndex = -1;

            if (InputSignal2 == null)
            {
                List<float> sol = new List<float>();
                List<double> firstSignal = new List<double>();
                List<double> secondSignal = new List<double>();
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                {
                    firstSignal.Add(InputSignal1.Samples[i]);
                    secondSignal.Add(InputSignal1.Samples[i]);
                }
                if (InputSignal1.Periodic != true)
                {
                    for (int i = 0; i < secondSignal.Count; i++)
                    {
                        double sum = 0;
                        if (i != 0) // so shift 
                        {
                            for (int j = 0; j < secondSignal.Count - 1; j++)
                            {
                                secondSignal[j] = secondSignal[j + 1];
                                sum += secondSignal[j] * firstSignal[j];
                            }
                            secondSignal[secondSignal.Count - 1] = 0;
                            sum += secondSignal[secondSignal.Count - 1] * firstSignal[firstSignal.Count - 1];
                        }
                        else
                        {
                            for (int j = 0; j < secondSignal.Count; j++)
                                sum += secondSignal[j] * secondSignal[j];
                        }
                        sol.Add((float)sum / secondSignal.Count);
                        if ((float)sum / secondSignal.Count > maxCorrelatedValue)
                        {
                            maxCorrelatedValue = (float)sum / secondSignal.Count;
                            solIndex = i;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < secondSignal.Count; i++)
                    {
                        double sum = 0;
                        if (i != 0) // so shift 
                        {
                            double first_element = secondSignal[0];
                            for (int j = 0; j < secondSignal.Count - 1; j++)
                            {
                                secondSignal[j] = secondSignal[j + 1];
                                sum += secondSignal[j] * firstSignal[j];
                            }
                            secondSignal[secondSignal.Count - 1] = first_element;
                            sum += secondSignal[secondSignal.Count - 1] * firstSignal[firstSignal.Count - 1];
                        }
                        else
                        {
                            for (int j = 0; j < secondSignal.Count; j++)
                                sum += secondSignal[j] * secondSignal[j];
                        }
                        sol.Add((float)sum / secondSignal.Count);
                        if ((float)sum / secondSignal.Count > maxCorrelatedValue)
                        {
                            maxCorrelatedValue = (float)sum / secondSignal.Count;
                            solIndex = i;
                        }
                    }

                }
            }
            else
            { //if the second sample exists then it's cross correlation
                List<float> sol = new List<float>();
                List<double> firstSignal = new List<double>();
                List<double> secondSignal = new List<double>();
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    firstSignal.Add(InputSignal1.Samples[i]);
                for (int i = 0; i < InputSignal2.Samples.Count; i++)
                    secondSignal.Add(InputSignal2.Samples[i]);
                if (firstSignal != secondSignal)
                {
                    int totalSamplesCount = firstSignal.Count + secondSignal.Count - 1;
                    for (int i = firstSignal.Count; i < totalSamplesCount; i++)
                        firstSignal.Add(0);
                    for (int i = secondSignal.Count; i < totalSamplesCount; i++)
                        secondSignal.Add(0);
                }

                if (InputSignal1.Periodic != true)
                {
                    for (int i = 0; i < secondSignal.Count; i++)
                    {
                        double sum = 0;
                        if (i != 0) // so shift 
                        {
                            double first_element = 0;
                            for (int j = 0; j < secondSignal.Count - 1; j++)
                            {
                                secondSignal[j] = secondSignal[j + 1];
                                sum += secondSignal[j] * firstSignal[j];
                            }
                            secondSignal[secondSignal.Count - 1] = first_element;
                            sum += secondSignal[secondSignal.Count - 1] * firstSignal[firstSignal.Count - 1];
                        }
                        else
                        {
                            for (int j = 0; j < secondSignal.Count; j++)
                                sum += firstSignal[j] * secondSignal[j];
                        }
                        sol.Add((float)sum / secondSignal.Count);
                        if ((float)sum / secondSignal.Count > maxCorrelatedValue)
                        {
                            maxCorrelatedValue = (float)sum / secondSignal.Count;
                            solIndex = i;
                        }
                    }
                }

                else
                {
                    for (int i = 0; i < secondSignal.Count; i++)
                    {
                        double sum = 0;
                        if (i != 0)
                        {
                            double first_element = secondSignal[0];
                            for (int j = 0; j < secondSignal.Count - 1; j++)
                            {
                                secondSignal[j] = secondSignal[j + 1];
                                sum += secondSignal[j] * firstSignal[j];
                            }
                            secondSignal[secondSignal.Count - 1] = first_element;
                            sum += secondSignal[secondSignal.Count - 1] * firstSignal[firstSignal.Count - 1];
                        }
                        else
                        {
                            for (int j = 0; j < secondSignal.Count; j++)
                                sum += firstSignal[j] * secondSignal[j];
                        }
                        sol.Add((float)sum / secondSignal.Count);
                        if ((float)sum / secondSignal.Count > maxCorrelatedValue)
                        {
                            maxCorrelatedValue = (float)sum / secondSignal.Count;
                            solIndex = i;
                        }
                    }
                }


               
            }
            OutputTimeDelay = solIndex * InputSamplingPeriod;


        }
    }
}