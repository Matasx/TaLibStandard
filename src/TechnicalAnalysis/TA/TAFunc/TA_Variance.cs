namespace TechnicalAnalysis
{
    internal static partial class TACore
    {
        public static RetCode Variance(
            int startIdx,
            int endIdx,
            double[] inReal,
            int optInTimePeriod,
            double optInNbDev,
            ref int outBegIdx,
            ref int outNBElement,
            double[] outReal)
        {
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inReal == null)
            {
                return RetCode.BadParam;
            }

            if (optInTimePeriod is < 1 or > 100000)
            {
                return RetCode.BadParam;
            }

            if (optInNbDev is < -3E+37 or > 3E+37)
            {
                return RetCode.BadParam;
            }

            if (outReal == null)
            {
                return RetCode.BadParam;
            }

            return TA_INT_VAR(startIdx, endIdx, inReal, optInTimePeriod, ref outBegIdx, ref outNBElement, outReal);
        }

        public static int VarianceLookback(int optInTimePeriod, double optInNbDev)
        {
            if (optInTimePeriod is < 1 or > 100000)
            {
                return -1;
            }

            if (optInNbDev is < -3E+37 or > 3E+37)
            {
                return -1;
            }

            return optInTimePeriod - 1;
        }
    }
}
