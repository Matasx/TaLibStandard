using TechnicalAnalysis.Common;
using static TechnicalAnalysis.Common.CandleSettingType;
using static TechnicalAnalysis.Common.RetCode;

namespace TechnicalAnalysis.Candles.Candle3WhiteSoldiers;

public class Candle3WhiteSoldiers : CandleIndicator
{
    private readonly double[] _shadowVeryShortPeriodTotal = new double[3];
    private readonly double[] _nearPeriodTotal = new double[3];
    private readonly double[] _farPeriodTotal = new double[3];
    private double _bodyShortPeriodTotal;

    public Candle3WhiteSoldiers(in double[] open, in double[] high, in double[] low, in double[] close)
        : base(open, high, low, close)
    {
    }

    public Candle3WhiteSoldiersResult Compute(int startIdx, int endIdx)
    {
        // Initialize output variables 
        int outBegIdx = default;
        int outNBElement = default;
        int[] outInteger = new int[endIdx - startIdx + 1];

        // Validate the requested output range.
        if (startIdx < 0)
        {
            return new Candle3WhiteSoldiersResult(OutOfRangeStartIndex, outBegIdx, outNBElement, outInteger);
        }

        if (endIdx < 0 || endIdx < startIdx)
        {
            return new Candle3WhiteSoldiersResult(OutOfRangeEndIndex, outBegIdx, outNBElement, outInteger);
        }

        // Verify required price component.
        if (Open == null || High == null || Low == null || Close == null)
        {
            return new Candle3WhiteSoldiersResult(BadParam, outBegIdx, outNBElement, outInteger);
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
            return new Candle3WhiteSoldiersResult(Success, outBegIdx, outNBElement, outInteger);
        }

        // Do the calculation using tight loops.
        // Add-up the initial period, except for the last value.
        int shadowVeryShortTrailingIdx = startIdx - GetCandleAvgPeriod(ShadowVeryShort);
        int nearTrailingIdx = startIdx - GetCandleAvgPeriod(Near);
        int farTrailingIdx = startIdx - GetCandleAvgPeriod(Far);
        int bodyShortTrailingIdx = startIdx - GetCandleAvgPeriod(BodyShort);
            
        int i = shadowVeryShortTrailingIdx;
        while (i < startIdx)
        {
            _shadowVeryShortPeriodTotal[2] += GetCandleRange(ShadowVeryShort, i - 2);
            _shadowVeryShortPeriodTotal[1] += GetCandleRange(ShadowVeryShort, i - 1);
            _shadowVeryShortPeriodTotal[0] += GetCandleRange(ShadowVeryShort, i);
            i++;
        }

        i = nearTrailingIdx;
        while (i < startIdx)
        {
            _nearPeriodTotal[2] += GetCandleRange(Near, i - 2);
            _nearPeriodTotal[1] += GetCandleRange(Near, i - 1);
            i++;
        }

        i = farTrailingIdx;
        while (i < startIdx)
        {
            _farPeriodTotal[2] += GetCandleRange(Far, i - 2);
            _farPeriodTotal[1] += GetCandleRange(Far, i - 1);
            i++;
        }

        i = bodyShortTrailingIdx;
        while (i < startIdx)
        {
            _bodyShortPeriodTotal += GetCandleRange(BodyShort, i);
            i++;
        }

        i = startIdx;
            
        /* Proceed with the calculation for the requested range.
         * Must have:
         * - three white candlesticks with consecutively higher closes
         * - Greg Morris wants them to be long, Steve Nison doesn't; anyway they should not be short
         * - each candle opens within or near the previous white real body 
         * - each candle must have no or very short upper shadow
         * - to differentiate this pattern from advance block, each candle must not be far shorter than the prior candle
         * The meanings of "not short", "very short shadow", "far" and "near" are specified with TA_SetCandleSettings;
         * here the 3 candles must be not short, if you want them to be long use TA_SetCandleSettings on BodyShort;
         * outInteger is positive (1 to 100): advancing 3 white soldiers is always bullish;
         * the user should consider that 3 white soldiers is significant when it appears in downtrend, while this function 
         * does not consider it
         */
        int outIdx = 0;
        do
        {
            outInteger[outIdx++] = RecognizeCandlePattern(i) ? 100 : 0;

            /* add the current range and subtract the first range: this is done after the pattern recognition 
             * when avgPeriod is not 0, that means "compare with the previous candles" (it excludes the current candle)
             */
            for (int totIdx = 2; totIdx >= 0; --totIdx)
            {
                _shadowVeryShortPeriodTotal[totIdx] +=
                    GetCandleRange(ShadowVeryShort, i - totIdx) -
                    GetCandleRange(ShadowVeryShort, shadowVeryShortTrailingIdx - totIdx);
            }

            for (int totIdx = 2; totIdx >= 1; --totIdx)
            {
                _farPeriodTotal[totIdx] +=
                    GetCandleRange(Far, i - totIdx) -
                    GetCandleRange(Far, farTrailingIdx - totIdx);

                _nearPeriodTotal[totIdx] +=
                    GetCandleRange(Near, i - totIdx) -
                    GetCandleRange(Near, nearTrailingIdx - totIdx);
            }

            _bodyShortPeriodTotal +=
                GetCandleRange(BodyShort, i) -
                GetCandleRange(BodyShort, bodyShortTrailingIdx);

            i++;
            shadowVeryShortTrailingIdx++;
            nearTrailingIdx++;
            farTrailingIdx++;
            bodyShortTrailingIdx++;
        } while (i <= endIdx);

        // All done. Indicate the output limits and return.
        outNBElement = outIdx;
        outBegIdx = startIdx;
            
        return new Candle3WhiteSoldiersResult(Success, outBegIdx, outNBElement, outInteger);
    }
    
    /// <inheritdoc cref="CandleIndicator.RecognizeCandlePattern"/>
    public override bool RecognizeCandlePattern(int index)
    {
        bool is3WhiteSoldiers =
            // 1st white
            GetCandleColor(index - 2) == 1 &&
            // very short upper shadow
            GetUpperShadow(index - 2) < GetCandleAverage(ShadowVeryShort, _shadowVeryShortPeriodTotal[2], index - 2) &&
            // 2nd white                
            GetCandleColor(index - 1) == 1 &&
            // very short upper shadow
            GetUpperShadow(index - 1) < GetCandleAverage(ShadowVeryShort, _shadowVeryShortPeriodTotal[1], index - 1) &&
            // 3rd white   
            GetCandleColor(index) == 1 &&
            // very short upper shadow
            GetUpperShadow(index) < GetCandleAverage(ShadowVeryShort, _shadowVeryShortPeriodTotal[0], index) &&
            // consecutive higher closes           
            Close[index] > Close[index - 1] &&
            Close[index - 1] > Close[index - 2] &&
            // 2nd opens within/near 1st real body
            Open[index - 1] > Open[index - 2] &&
            Open[index - 1] <= Close[index - 2] +
            GetCandleAverage(Near, _nearPeriodTotal[2], index - 2) &&
            // 3rd opens within/near 2nd real body
            Open[index] > Open[index - 1] &&
            Open[index] <= Close[index - 1] +
            GetCandleAverage(Near, _nearPeriodTotal[1], index - 1) &&
            // 2nd not far shorter than 1st
            GetRealBody(index - 1) > GetRealBody(index - 2) -
            GetCandleAverage(Far, _farPeriodTotal[2], index - 2) &&
            // 3rd not far shorter than 2nd
            GetRealBody(index) > GetRealBody(index - 1) -
            GetCandleAverage(Far, _farPeriodTotal[1], index - 1) &&
            // not short real body
            GetRealBody(index) >
            GetCandleAverage(BodyShort, _bodyShortPeriodTotal, index);
            
        return is3WhiteSoldiers;
    }
    
    /// <inheritdoc cref="CandleIndicator.GetLookback"/>
    public override int GetLookback()
    {
        return GetCandleMaxAvgPeriod(ShadowVeryShort, BodyShort, Far, Near) + 2;
    }
}
