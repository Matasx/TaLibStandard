﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CdlMatchingLow.cs" company="GLPM">
//   Copyright (c) GLPM. All rights reserved.
// </copyright>
// <summary>
//   Defines CdlMatchingLow.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using TechnicalAnalysis.Candle;

namespace TechnicalAnalysis
{
    public static partial class TAMath
    {
        public static CdlMatchingLow CdlMatchingLow(
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

            CandleMatchingLow candle = new (open, high, low, close);
            RetCode retCode = candle.CdlMatchingLow(startIdx, endIdx, ref outBegIdx, ref outNBElement, ref outInteger);
            
            return new CdlMatchingLow(retCode, outBegIdx, outNBElement, outInteger);
        }

        public static CdlMatchingLow CdlMatchingLow(
            int startIdx,
            int endIdx,
            float[] open,
            float[] high,
            float[] low,
            float[] close)
            => CdlMatchingLow(startIdx, endIdx, open.ToDouble(), high.ToDouble(), low.ToDouble(), close.ToDouble());
    }

    public class CdlMatchingLow : IndicatorBase
    {
        public CdlMatchingLow(RetCode retCode, int begIdx, int nbElement, int[] integer)
            : base(retCode, begIdx, nbElement)
        {
            this.Integer = integer;
        }

        public int[] Integer { get; }
    }
}
