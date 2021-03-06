using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using AnthStat.Statistics;

namespace AnthStat.Statistics.Tests
{
    public class StatHelper_Tests
    {
        private static double TOLERANCE = 0.00001;
        private static double PERCENTILE_TOLERANCE = 0.01;

        [Fact]
        public void IsWholeNumber_Success()
        {
            double [] wholes = { 0.0, 9.0, 5.0, -5.0, 100.0, 932.0, 454.0, 7983.0 };
            double [] notWholes = { 9.7, 7.65, 4.3, -0.1, 0.00000000001, 0.03, 834.56, 99.99 };

            foreach (var whole in wholes)
            {
                Assert.True(StatisticsHelper.IsWholeNumber(whole));
            }

            foreach (var notWhole in notWholes)
            {
                Assert.False(StatisticsHelper.IsWholeNumber(notWhole));
            }
        }

        [Theory]
        [InlineData(49, 1, 49.1477, 0.0379, -0.07929359105980168560136240669803)]
        [InlineData(110, 1, 109.9352, 0.04358, 0.01352542775910235322705902688034)]
        [InlineData(49, 1, 49.8842, 0.03795, -0.46706327321797969208676755180545)]
        [InlineData(110, 1, 110.4969, 0.04226, -0.10641170700920362584779158876099)]
        [InlineData(34, 1, 33.8787, 0.03496, 0.10241478078755314955626554581229)]
        [InlineData(50, 1, 49.9652, 0.02848, 0.02445522308245640089097517490151)]
        [InlineData(13.47, -0.1733, 13.0245, 0.08262, 0.40589404468054411701689483303183)]
        [InlineData(16.52, -0.405, 16.5506, 0.08652, -0.0213971396073446674965169466961)]
        public void GetZ_Success(double rawValue, double L, double M, double S, double z)
        {
            Assert.True(Math.Abs(z - StatisticsHelper.CalculateZScore(rawValue, L, M, S)) < TOLERANCE);
        }

        [Theory]
        [InlineData(-3.0, 00.13)]
        [InlineData(-2.5, 00.62)]
        [InlineData(-2.0, 02.27)]
        [InlineData(-1.5, 06.68)]
        [InlineData(-1.0, 15.87)]
        [InlineData(-0.5, 30.85)]
        [InlineData(0.0, 50.00)]
        [InlineData(0.5, 69.15)]
        [InlineData(1.0, 84.13)]
        [InlineData(1.5, 93.32)]
        [InlineData(2.0, 97.72)]
        [InlineData(2.5, 99.38)]
        [InlineData(3.0, 99.87)]
        [InlineData(0.33, 62.93)]
        [InlineData(0.22, 58.71)]
        [InlineData(0.01, 50.40)]
        [InlineData(0.54, 70.54)]
        [InlineData(0.41, 65.91)]
        [InlineData(-4.5, 0.0003)]
        [InlineData(-4.0, 0.0032)]
        [InlineData(-3.4017596805071390108663953082457, 0.0335)]
        [InlineData(-0.18503233427487158999085862877748, 42.6602)]
        [InlineData(0.62961642936973316222835417730078, 73.5527)]
        [InlineData(2.1134935536236982591548727107661, 98.2721)]
        [InlineData(2.5351948017104595832200737120844, 99.4381)]
        [InlineData(3.0325847159162528421113690449965, 99.8788)]
        [InlineData(4.0, 99.9968)]
        [InlineData(4.5, 99.9997)]
        public void GetPercentile_Success(double z, double p)
        {
            double result = StatisticsHelper.CalculatePercentile(z);
            double diff = Math.Abs(p - result);
            Assert.True(diff < PERCENTILE_TOLERANCE);
        }

        [Theory]
        [InlineData(16.52, -0.405, 16.6, 0.0)]
        public void GetZ_ZeroS_Fail(double rawValue, double L, double M, double S)
        {
            Assert.Throws<ArgumentException>(delegate
            {
                double z = StatisticsHelper.CalculateZScore(rawValue, L, M, S);
            });
        }
    }
}
