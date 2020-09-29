using System;

namespace c_sharp_dll
{
    public class ForwardCurve
    {
        public double[,] ForwardCurveCalc(double[] tenors, double[] rates, 
            double valuationdate, int frequencymonths)
        {
            Utils utils = new Utils();
            DateTime dtValuationdate = utils.DateFromGregorian(valuationdate);

            //create a new array of time and rates 
            double[,] curve = new double[tenors.Length + 1, 2];
            curve[0, 0] = 0;
            curve[0, 1] = 0;

            for (int i = 0; i <= tenors.Length - 1; i++)
            {
                curve[i + 1, 0] = tenors[i];
                curve[i + 1, 1] = rates[i];
            }

            //create an array for the forward rates using frequencymonths
            double[,] forwardcurve = new double[(int)curve[curve.Length/2 - 1, 0], 4];
            double[,] spotAndForwardRate = new double[1,2];

            for (int i = 0; i <= forwardcurve.Length/4 - 1; i++)
            {
                if (i == 0)
                {
                    spotAndForwardRate[0, 0] = curve[0, 1];
                    spotAndForwardRate[0, 1] = curve[0, 1];
                }
                else
                {
                    spotAndForwardRate = SpotAndForwardRate(curve, i, frequencymonths);
                }
                forwardcurve[i, 0] = i;
                forwardcurve[i, 1] = utils.GregorianFromDate(dtValuationdate.AddMonths(i * 12));
                forwardcurve[i, 2] = spotAndForwardRate[0, 0];
                forwardcurve[i, 3] = spotAndForwardRate[0, 1];
            }

            return forwardcurve;
        }

        //calculated to the spot and forward rates using interpolation
        private double[,] SpotAndForwardRate(double[,] curve, double x, int frequencymonths)
        {
            double[,] returnArray = new double[1, 2];
            double spotrate, spotratematurity;
            double spottenor = x;
            double maturitytenor = x + Convert.ToDouble(frequencymonths) / 12;
            int iSpotMaturity = 0;

            for (int i = 0; i <= curve.Length/2 - 1; i++)
            {
                if (curve[i, 0] > spottenor)
                {
                    iSpotMaturity = i;
                    break;
                }
            }

            spotrate = InterpolatedRate(iSpotMaturity, spottenor);
            spotratematurity = InterpolatedRate(iSpotMaturity, maturitytenor);

            //sub function
            double InterpolatedRate(int i, double xval)
            {
                double x0, x1, y0, y1;

                if (i == 0)
                    return 0;

                x1 = curve[i, 0];
                y1 = curve[i, 1];
                x0 = curve[i - 1, 0];
                y0 = curve[i - 1, 1];
                
                return LinearInterpolation(xval, x0, x1, y0, y1);
            }

            double frequency = 12 / frequencymonths;
            double forwardrate = (Math.Pow(Math.Pow(1 + spotratematurity / frequency, maturitytenor * frequency) 
                / Math.Pow(1 + spotrate / frequency, spottenor * frequency), 1 
                / ((maturitytenor - spottenor) * frequency)) - 1) * frequency;

            returnArray[0, 0] = spotrate;
            returnArray[0, 1] = forwardrate;
            return returnArray;
        }

        public double LinearInterpolation(double x, double x0, double x1, double y0, double y1)
        {
            if ((x1 - x0) == 0)
                return (y0 + y1) / 2;
            return y0 + (x - x0) * (y1 - y0) / (x1 - x0);
        }


    }

}
