using TechnicalAnalysis.Candles.CandleHammer;
using TechnicalAnalysis.Common;

// ReSharper disable once CheckNamespace
namespace TechnicalAnalysis
{
    public static partial class TAMath
    {
        public static CandleHammerResult CdlHammer(
            int startIdx, int endIdx, double[] open, double[] high, double[] low, double[] close)
        {
            return new CandleHammer(open, high, low, close)
                .Compute(startIdx, endIdx);
        }

        public static CandleHammerResult CdlHammer(
            int startIdx, int endIdx, float[] open, float[] high, float[] low, float[] close)
        {
            return CdlHammer(startIdx, endIdx,
                open.ToDouble(), high.ToDouble(), low.ToDouble(), close.ToDouble());
        }
    }
}
