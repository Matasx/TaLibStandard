﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CdlHikkakeMod.cs" company="GLPM">
//   Copyright (c) GLPM. All rights reserved.
// </copyright>
// <summary>
//   Defines CdlHikkakeMod.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using TechnicalAnalysis.Candle;

namespace TechnicalAnalysis
{
    public static partial class TAMath
    {
        public static CdlHikkakeMod CdlHikkakeMod(
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

            CandleHikkakeMod candle = new (open, high, low, close);
            RetCode retCode = candle.CdlHikkakeMod(startIdx, endIdx, ref outBegIdx, ref outNBElement, ref outInteger);
            
            return new CdlHikkakeMod(retCode, outBegIdx, outNBElement, outInteger);
        }

        public static CdlHikkakeMod CdlHikkakeMod(
            int startIdx,
            int endIdx,
            float[] open,
            float[] high,
            float[] low,
            float[] close)
            => CdlHikkakeMod(startIdx, endIdx, open.ToDouble(), high.ToDouble(), low.ToDouble(), close.ToDouble());
    }

    public class CdlHikkakeMod : IndicatorBase
    {
        public CdlHikkakeMod(RetCode retCode, int begIdx, int nbElement, int[] integer)
            : base(retCode, begIdx, nbElement)
        {
            this.Integer = integer;
        }

        public int[] Integer { get; }
    }
}
