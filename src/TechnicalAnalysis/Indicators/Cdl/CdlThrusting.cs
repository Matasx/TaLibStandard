﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CdlThrusting.cs" company="GLPM">
//   Copyright (c) GLPM. All rights reserved.
// </copyright>
// <summary>
//   Defines CdlThrusting.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using TechnicalAnalysis.Candle;

namespace TechnicalAnalysis
{
    public static partial class TAMath
    {
        public static CdlThrusting CdlThrusting(
            int startIdx,
            int endIdx,
            double[] open,
            double[] high,
            double[] low,
            double[] close)
        {
            int outBegIdx = default;
            int outNBElement = default;
            int[] outInteger = new int[endIdx - startIdx + 1];

            CandleThrusting candle = new (open, high, low, close);
            RetCode retCode = candle.CdlThrusting(startIdx, endIdx, ref outBegIdx, ref outNBElement, ref outInteger);
            
            return new CdlThrusting(retCode, outBegIdx, outNBElement, outInteger);
        }

        public static CdlThrusting CdlThrusting(
            int startIdx,
            int endIdx,
            float[] open,
            float[] high,
            float[] low,
            float[] close)
            => CdlThrusting(startIdx, endIdx, open.ToDouble(), high.ToDouble(), low.ToDouble(), close.ToDouble());
    }

    public class CdlThrusting : IndicatorBase
    {
        public CdlThrusting(RetCode retCode, int begIdx, int nbElement, int[] integer)
            : base(retCode, begIdx, nbElement)
        {
            this.Integer = integer;
        }

        public int[] Integer { get; }
    }
}
