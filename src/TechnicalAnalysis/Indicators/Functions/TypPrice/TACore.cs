using TechnicalAnalysis.Common;

namespace TechnicalAnalysis;

internal static partial class TACore
{
    public static RetCode TypPrice(
        int startIdx,
        int endIdx,
        in double[] inHigh,
        in double[] inLow,
        in double[] inClose,
        ref int outBegIdx,
        ref int outNBElement,
        ref double[] outReal)
    {
        if (startIdx < 0)
        {
            return RetCode.OutOfRangeStartIndex;
        }

        if (endIdx < 0 || endIdx < startIdx)
        {
            return RetCode.OutOfRangeEndIndex;
        }

        if (inHigh == null || inLow == null || inClose == null)
        {
            return RetCode.BadParam;
        }

        if (outReal == null)
        {
            return RetCode.BadParam;
        }

        int outIdx = 0;
        for (int i = startIdx; i <= endIdx; i++)
        {
            outReal[outIdx] = (inHigh[i] + inLow[i] + inClose[i]) / 3.0;
            outIdx++;
        }

        outNBElement = outIdx;
        outBegIdx = startIdx;
        return RetCode.Success;
    }

    public static int TypPriceLookback()
    {
        return 0;
    }
}