using TechnicalAnalysis.Common;
using static TechnicalAnalysis.Common.CandleSettingType;
using static TechnicalAnalysis.Common.RetCode;

namespace TechnicalAnalysis.Candles.CandleHighWave;

public class CandleHighWave : CandleIndicator
{
    private double _bodyPeriodTotal;
    private double _shadowPeriodTotal;

    public CandleHighWave(in double[] open, in double[] high, in double[] low, in double[] close)
        : base(open, high, low, close)
    {
    }

    public CandleHighWaveResult Compute(int startIdx, int endIdx)
    {
        // Initialize output variables 
        int outBegIdx = default;
        int outNBElement = default;
        int[] outInteger = new int[endIdx - startIdx + 1];
            
        // Validate the requested output range.
        if (startIdx < 0)
        {
            return new CandleHighWaveResult(OutOfRangeStartIndex, outBegIdx, outNBElement, outInteger);
        }

        if (endIdx < 0 || endIdx < startIdx)
        {
            return new CandleHighWaveResult(OutOfRangeEndIndex, outBegIdx, outNBElement, outInteger);
        }

        // Verify required price component.
        if (Open == null || High == null || Low == null || Close == null)
        {
            return new CandleHighWaveResult(BadParam, outBegIdx, outNBElement, outInteger);
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
            return new CandleHighWaveResult(Success, outBegIdx, outNBElement, outInteger);
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        int bodyTrailingIdx = startIdx - GetCandleAvgPeriod(BodyShort);
        int shadowTrailingIdx = startIdx - GetCandleAvgPeriod(ShadowVeryLong);
            
        int i = bodyTrailingIdx;
        while (i < startIdx)
        {
            _bodyPeriodTotal += GetCandleRange(BodyShort, i);
            i++;
        }

        i = shadowTrailingIdx;
        while (i < startIdx)
        {
            _shadowPeriodTotal += GetCandleRange(ShadowVeryLong, i);
            i++;
        }

        /* Proceed with the calculation for the requested range.
         * Must have:
         * - short real body
         * - very long upper and lower shadow
         * The meaning of "short" and "very long" is specified with TA_SetCandleSettings
         * outInteger is positive (1 to 100) when white or negative (-1 to -100) when black;
         * it does not mean bullish or bearish
         */
        int outIdx = 0;
        do
        {
            outInteger[outIdx++] = RecognizeCandlePattern(i) ? GetCandleColor(i) * 100 : 0;

            /* add the current range and subtract the first range: this is done after the pattern recognition 
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            _bodyPeriodTotal +=
                GetCandleRange(BodyShort, i) -
                GetCandleRange(BodyShort, bodyTrailingIdx);

            _shadowPeriodTotal +=
                GetCandleRange(ShadowVeryLong, i) -
                GetCandleRange(ShadowVeryLong, shadowTrailingIdx);

            i++;
            bodyTrailingIdx++;
            shadowTrailingIdx++;
        } while (i <= endIdx);

        // All done. Indicate the output limits and return.
        outNBElement = outIdx;
        outBegIdx = startIdx;
            
        return new CandleHighWaveResult(Success, outBegIdx, outNBElement, outInteger);
    }
    
    /// <inheritdoc cref="CandleIndicator.RecognizeCandlePattern"/>
    public override bool RecognizeCandlePattern(int index)
    {
        bool isHighWave =
            GetRealBody(index) < GetCandleAverage(BodyShort, _bodyPeriodTotal, index) &&
            GetUpperShadow(index) > GetCandleAverage(ShadowVeryLong, _shadowPeriodTotal, index) &&
            GetLowerShadow(index) > GetCandleAverage(ShadowVeryLong, _shadowPeriodTotal, index);
            
        return isHighWave;
    }
    
    /// <inheritdoc cref="CandleIndicator.GetLookback"/>
    public override int GetLookback()
    {
        return GetCandleMaxAvgPeriod(BodyShort, ShadowVeryLong);
    }
}
