﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CdlRiseFall3Methods.cs" company="GLPM">
//   Copyright (c) GLPM. All rights reserved.
// </copyright>
// <summary>
//   Defines CdlRiseFall3Methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using TechnicalAnalysis.Candle;

namespace TechnicalAnalysis
{
    public static partial class TAMath
    {
        public static CdlRiseFall3Methods CdlRiseFall3Methods(
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

            CandleRiseFall3Methods candle = new (open, high, low, close);
            RetCode retCode = candle.CdlRiseFall3Methods(startIdx, endIdx, ref outBegIdx, ref outNBElement, ref outInteger);
            
            return new CdlRiseFall3Methods(retCode, outBegIdx, outNBElement, outInteger);
        }

        public static CdlRiseFall3Methods CdlRiseFall3Methods(
            int startIdx,
            int endIdx,
            float[] open,
            float[] high,
            float[] low,
            float[] close)
            => CdlRiseFall3Methods(
                startIdx,
                endIdx,
                open.ToDouble(),
                high.ToDouble(),
                low.ToDouble(),
                close.ToDouble());
    }

    public class CdlRiseFall3Methods : IndicatorBase
    {
        public CdlRiseFall3Methods(RetCode retCode, int begIdx, int nbElement, int[] integer)
            : base(retCode, begIdx, nbElement)
        {
            this.Integer = integer;
        }

        public int[] Integer { get; }
    }
}
