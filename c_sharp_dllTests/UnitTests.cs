using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace c_sharp_dll.Tests
{
    [TestClass()]
    public class UnitTests
    {
        readonly ForwardCurve fc = new ForwardCurve();

        [TestMethod()]
        public void LinearInterpolationUnitTest()
        {
            double result = fc.LinearInterpolation(0.5, 0, 1, 10, 20);
            Assert.AreEqual(15, result);
        }

        [TestMethod()]
        public void ForwardCurveCalcTest()
        {
            double[] tenors = new double[8];
            tenors[0] = 1;
            tenors[1] = 3;
            tenors[2] = 5;
            tenors[3] = 10;
            tenors[4] = 20;
            tenors[5] = 30;
            tenors[6] = 40;
            tenors[7] = 50;

            Double[] rates = new double[8];
            rates[0] = 0.025;
            rates[1] = 0.03; 
            rates[2] = 0.03;
            rates[3] = 0.035;
            rates[4] = 0.0355;
            rates[5] = 0.0365;
            rates[6] = 0.0365;
            rates[7] = 0.037;

            double valuationdate = 20200630;
            int frequency = 6;

            double[,] result = fc.ForwardCurveCalc(tenors, rates, valuationdate, frequency);

            Assert.AreEqual(0.03, result[3,2]);
            Assert.AreEqual(0.035, result[10,2]);

        }

        [TestMethod()]
        public void ForwardCurveCalcWithNegativeRatesTest()
        {
            double[] tenors = new double[8];
            tenors[0] = 1;
            tenors[1] = 3;
            tenors[2] = 5;
            tenors[3] = 10;
            tenors[4] = 20;
            tenors[5] = 30;
            tenors[6] = 40;
            tenors[7] = 50;

            Double[] rates = new double[8];
            rates[0] = 0.025;
            rates[1] = 0.03;
            rates[2] = 0.03;
            rates[3] = 0.0;
            rates[4] = 0.0;
            rates[5] = -0.0365;
            rates[6] = -0.0365;
            rates[7] = -0.037;

            double valuationdate = 20200630;
            int frequency = 6;

            double[,] result = fc.ForwardCurveCalc(tenors, rates, valuationdate, frequency);

            Assert.AreEqual(0.0, result[11, 3]);
        }

    }
}