using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eventgeneratingstats
{
    class engine
    {
        private Form1 form;
        Random rand = new Random();

        public engine(Form1 form)
        {
            this.form = form;
        }

        private double GetAvg(int[] numbers)
        {
            int up = 0;
            int down = 0;

            for (int i = 0; i < 5; i++)
            {
                up += (i + 1) * numbers[i];
                down += numbers[i];
            }

            return (double)up / down;
        }

        private double GetAbsAvg(List<double> values)
        {
            double ans = 0;

            for (int i = 0; i < 5; i++)
            {
                ans += (i + 1) * values[i];                
            }

            return (double)ans;
        }

        private double GetVar(int[] numbers, double avg)
        {
            double up = 0;
            int down = 0;

            for (int i = 0; i < 5; i++)
            {
                up += Math.Pow((i + 1 - avg), 2) * numbers[i];
                down += numbers[i];
            }

            return (double)up / down;
        }

        private double GetAbsVar(List<double> values, double absAvg)
        {
            double ans = 0;

            for (int i = 0; i < 5; i++)
            {
                ans += Math.Pow((i + 1 - absAvg), 2) * values[i];
            }

            return (double)ans;
        }

        private double getChi(int[] numbers, List<double> values, int number)
        {
            double ans = 0;

            for (int i = 0;i < 5 ;i++)
            {
                ans += Math.Pow(numbers[i], 2) / (number * values[i]);
            }

            return ans - (double)number;
        }

        public bool CheckChi(double chi)
        {
            double alpha = 11.07;

            if (chi < alpha)
                return true;
            else
                return false;
        }

        public (double[], double, double, double, double, double, bool) Calculate()
        {
            int[] numbers = new int[5];
            double[] stats = new double[5];

            List<double> values = form.GetValues();
            List<double> probs = form.GetValues();

            int number = (int)values[5];
            values.RemoveAt(5);

            double absAvg = GetAbsAvg(values);
            double absVar = GetAbsVar(values, absAvg);
            

            double prob1 = values[0];
            double prob2 = values[0] + values[1];
            double prob3 = values[0] + values[1] + values[2];
            double prob4 = values[0] + values[1] + values[2] + values[3];
            double prob5 = values[0] + values[1] + values[2] + values[3] + values[4];

            probs[0] = prob1;
            probs[1] = prob2;
            probs[2] = prob3;
            probs[3] = prob4;
            probs[4] = prob5;           

            for (int i = 0; i < number; i++)
            {
                double answer = rand.NextDouble();

                for (int j = 0; j < probs.Count; j++)
                {
                    if (answer - probs[j] < 0)
                    {
                        answer = j;
                        break;
                    }
                }

                numbers[(int)answer]++;
            }

            for (int i = 0; i < 5; i++)
            {
                stats[i] = (double)numbers[i] / (double)number;
            }

            double avg = Math.Round(GetAvg(numbers), 2);
            double var = Math.Round(GetVar(numbers,avg), 2);
            double chi = Math.Round(getChi(numbers, values, number),2);

            return (stats, avg, var, absAvg, absVar, chi, CheckChi(chi));
        }
    }
}
