using TechnicalAnalysis.Common;
using static System.Math;
using static TechnicalAnalysis.Common.CandleSettingType;
using static TechnicalAnalysis.Common.RetCode;

namespace TechnicalAnalysis.Candles.Candle3LineStrike;

public class Candle3LineStrike : CandleIndicator
{
    private readonly double[] _nearPeriodTotal = new double[4];

    public Candle3LineStrike(in double[] open, in double[] high, in double[] low, in double[] close)
        : base(open, high, low, close)
    {
    }

    public Candle3LineStrikeResult Compute(int startIdx, int endIdx)
    {
        // Initialize output variables 
        int outBegIdx = default;
        int outNBElement = default;
        int[] outInteger = new int[endIdx - startIdx + 1];

        // Validate the requested output range.
        if (startIdx < 0)
        {
            return new Candle3LineStrikeResult(OutOfRangeStartIndex, outBegIdx, outNBElement, outInteger);
        }

        if (endIdx < 0 || endIdx < startIdx)
        {
            return new Candle3LineStrikeResult(OutOfRangeEndIndex, outBegIdx, outNBElement, outInteger);
        }

        // Verify required price component.
        if (Open == null || High == null || Low == null || Close == null)
        {
            return new Candle3LineStrikeResult(BadParam, outBegIdx, outNBElement, outInteger);
        }

        // Identify the minimum number of price bar needed to calculate at least one output.
        int lookbackTotal = GetLookback();

        // Move up the start index if there is not enough initial data.
        if (startIdx < lookbackTotal)
        {
            startIdx = lookbackTotal;
        }

        // Make sure there is still something to evaluate.
        if (startIdx > endIdx)
        {
            return new Candle3LineStrikeResult(Success, outBegIdx, outNBElement, outInteger);
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        int nearTrailingIdx = startIdx - GetCandleAvgPeriod(Near);
            
        int i = nearTrailingIdx;
        while (i < startIdx)
        {
            _nearPeriodTotal[3] += GetCandleRange(Near, i - 3);
            _nearPeriodTotal[2] += GetCandleRange(Near, i - 2);
            i++;
        }

        i = startIdx;

        /* Proceed with the calculation for the requested range.
         * Must have:
         * - three white soldiers (three black crows): three white (black) candlesticks with consecutively higher (lower) closes,
         * each opening within or near the previous real body
         * - fourth candle: black (white) candle that opens above (below) prior candle's close and closes below (above) 
         * the first candle's open
         * The meaning of "near" is specified with TA_SetCandleSettings;
         * outInteger is positive (1 to 100) when bullish or negative (-1 to -100) when bearish;
         * the user should consider that 3-line strike is significant when it appears in a trend in the same direction of
         * the first three candles, while this function does not consider it
         */
        int outIdx = 0;
        do
        {
            outInteger[outIdx++] = RecognizeCandlePattern(i) ? GetCandleColor(i - 1) * 100 : 0;

            /* add the current range and subtract the first range: this is done after the pattern recognition 
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            for (int totIdx = 3; totIdx >= 2; --totIdx)
            {
                _nearPeriodTotal[totIdx] +=
                    GetCandleRange(Near, i - totIdx) -
                    GetCandleRange(Near, nearTrailingIdx - totIdx);
            }

            i++;
            nearTrailingIdx++;
        } while (i <= endIdx);

        // All done. Indicate the output limits and return.
        outNBElement = outIdx;
        outBegIdx = startIdx;
            
        return new Candle3LineStrikeResult(Success, outBegIdx, outNBElement, outInteger);
    }
    
    /// <inheritdoc cref="CandleIndicator.RecognizeCandlePattern"/>
    public override bool RecognizeCandlePattern(int index)
    {
        bool is3LineStrike =
            // three with same color
            GetCandleColor(index - 3) == GetCandleColor(index - 2) &&
            GetCandleColor(index - 2) == GetCandleColor(index - 1) &&
            // 4th opposite color
            GetCandleColor(index) == -GetCandleColor(index - 1) &&
            // 2nd opens within/near 1st rb
            Open[index - 2] >= Min(Open[index - 3], Close[index - 3]) -
            GetCandleAverage(Near, _nearPeriodTotal[3], index - 3) &&
            Open[index - 2] <= Max(Open[index - 3], Close[index - 3]) +
            GetCandleAverage(Near, _nearPeriodTotal[3], index - 3) &&
            // 3rd opens within/near 2nd rb
            Open[index - 1] >= Min(Open[index - 2], Close[index - 2]) -
            GetCandleAverage(Near, _nearPeriodTotal[2], index - 2) &&
            Open[index - 1] <= Max(Open[index - 2], Close[index - 2]) +
            GetCandleAverage(Near, _nearPeriodTotal[2], index - 2) &&
            (
                ( // if three white
                    GetCandleColor(index - 1) == 1 &&
                    Close[index - 1] > Close[index - 2] &&
                    // consecutive higher closes
                    Close[index - 2] > Close[index - 3] &&
                    // 4th opens above prior close
                    Open[index] > Close[index - 1] &&
                    // 4th closes below 1st open
                    Close[index] < Open[index - 3]
                ) ||
                ( // if three black
                    GetCandleColor(index - 1) == -1 &&
                    Close[index - 1] < Close[index - 2] &&
                    // consecutive lower closes
                    Close[index - 2] < Close[index - 3] &&
                    // 4th opens below prior close
                    Open[index] < Close[index - 1] &&
                    // 4th closes above 1st open
                    Close[index] > Open[index - 3]
                )
            );
            
        return is3LineStrike;
    }
    
    /// <inheritdoc cref="CandleIndicator.GetLookback"/>
    public override int GetLookback()
    {
        return GetCandleAvgPeriod(Near) + 3;
    }
}
