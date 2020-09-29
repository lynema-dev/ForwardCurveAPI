using System;
using System.Dynamic;

namespace c_sharp_dll
{
    public class DynamicCalc : DynamicObject
    {
        ForwardCurve calc;
        public DynamicCalc()
        {
             calc = new ForwardCurve();
        }

        public string DynamicCalcEcho()
        {
            return "echo";
        }

        public double LinearInterpolation(double x, double x0, double x1, 
            double y0, double y1)
        {
            return calc.LinearInterpolation(x, x0, x1, y0, y1);
        }

        public double[,] ForwardCurveCalc(double[] tenors, double[] rates,
            double valuationdate, int frequencymonths)
        {
            return calc.ForwardCurveCalc(tenors, rates, valuationdate, frequencymonths);
        }

    }
}
