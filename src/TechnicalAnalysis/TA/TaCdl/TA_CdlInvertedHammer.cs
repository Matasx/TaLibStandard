namespace TechnicalAnalysis
{
    using System;

    internal partial class TACore
    {
        public static RetCode CdlInvertedHammer(
            int startIdx,
            int endIdx,
            double[] inOpen,
            double[] inHigh,
            double[] inLow,
            double[] inClose,
            ref int outBegIdx,
            ref int outNBElement,
            int[] outInteger)
        {
            double num5;
            double num10;
            double num15;
            double num20;
            double num25;
            double num30;
            double num49;
            double num55;
            if (startIdx < 0)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeEndIndex;
            }

            if (inOpen == null || inHigh == null || inLow == null || inClose == null)
            {
                return RetCode.BadParam;
            }

            if (outInteger == null)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = CdlInvertedHammerLookback();
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            double bodyPeriodTotal = 0.0;
            int bodyTrailingIdx = startIdx - Globals.candleSettings[2].avgPeriod;
            double shadowLongPeriodTotal = 0.0;
            int shadowLongTrailingIdx = startIdx - Globals.candleSettings[4].avgPeriod;
            double shadowVeryShortPeriodTotal = 0.0;
            int shadowVeryShortTrailingIdx = startIdx - Globals.candleSettings[7].avgPeriod;
            int i = bodyTrailingIdx;
            while (true)
            {
                double num70;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num70 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num69;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num69 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num66;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            double num67;
                            double num68;
                            if (inClose[i] >= inOpen[i])
                            {
                                num68 = inClose[i];
                            }
                            else
                            {
                                num68 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num67 = inOpen[i];
                            }
                            else
                            {
                                num67 = inClose[i];
                            }

                            num66 = inHigh[i] - num68 + (num67 - inLow[i]);
                        }
                        else
                        {
                            num66 = 0.0;
                        }

                        num69 = num66;
                    }

                    num70 = num69;
                }

                bodyPeriodTotal += num70;
                i++;
            }

            i = shadowLongTrailingIdx;
            while (true)
            {
                double num65;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                {
                    num65 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num64;
                    if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                    {
                        num64 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num61;
                        if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                        {
                            double num62;
                            double num63;
                            if (inClose[i] >= inOpen[i])
                            {
                                num63 = inClose[i];
                            }
                            else
                            {
                                num63 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num62 = inOpen[i];
                            }
                            else
                            {
                                num62 = inClose[i];
                            }

                            num61 = inHigh[i] - num63 + (num62 - inLow[i]);
                        }
                        else
                        {
                            num61 = 0.0;
                        }

                        num64 = num61;
                    }

                    num65 = num64;
                }

                shadowLongPeriodTotal += num65;
                i++;
            }

            i = shadowVeryShortTrailingIdx;
            while (true)
            {
                double num60;
                if (i >= startIdx)
                {
                    break;
                }

                if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                {
                    num60 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num59;
                    if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                    {
                        num59 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num56;
                        if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                        {
                            double num57;
                            double num58;
                            if (inClose[i] >= inOpen[i])
                            {
                                num58 = inClose[i];
                            }
                            else
                            {
                                num58 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num57 = inOpen[i];
                            }
                            else
                            {
                                num57 = inClose[i];
                            }

                            num56 = inHigh[i] - num58 + (num57 - inLow[i]);
                        }
                        else
                        {
                            num56 = 0.0;
                        }

                        num59 = num56;
                    }

                    num60 = num59;
                }

                shadowVeryShortPeriodTotal += num60;
                i++;
            }

            int outIdx = 0;
            Label_0313:
            if (Globals.candleSettings[2].avgPeriod != 0.0)
            {
                num55 = bodyPeriodTotal / Globals.candleSettings[2].avgPeriod;
            }
            else
            {
                double num54;
                if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
                {
                    num54 = Math.Abs(inClose[i] - inOpen[i]);
                }
                else
                {
                    double num53;
                    if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                    {
                        num53 = inHigh[i] - inLow[i];
                    }
                    else
                    {
                        double num50;
                        if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                        {
                            double num51;
                            double num52;
                            if (inClose[i] >= inOpen[i])
                            {
                                num52 = inClose[i];
                            }
                            else
                            {
                                num52 = inOpen[i];
                            }

                            if (inClose[i] >= inOpen[i])
                            {
                                num51 = inOpen[i];
                            }
                            else
                            {
                                num51 = inClose[i];
                            }

                            num50 = inHigh[i] - num52 + (num51 - inLow[i]);
                        }
                        else
                        {
                            num50 = 0.0;
                        }

                        num53 = num50;
                    }

                    num54 = num53;
                }

                num55 = num54;
            }

            if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
            {
                num49 = 2.0;
            }
            else
            {
                num49 = 1.0;
            }

            if (Math.Abs(inClose[i] - inOpen[i]) < Globals.candleSettings[2].factor * num55 / num49)
            {
                double num41;
                double num47;
                double num48;
                if (inClose[i] >= inOpen[i])
                {
                    num48 = inClose[i];
                }
                else
                {
                    num48 = inOpen[i];
                }

                if (Globals.candleSettings[4].avgPeriod != 0.0)
                {
                    num47 = shadowLongPeriodTotal / Globals.candleSettings[4].avgPeriod;
                }
                else
                {
                    double num46;
                    if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
                    {
                        num46 = Math.Abs(inClose[i] - inOpen[i]);
                    }
                    else
                    {
                        double num45;
                        if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                        {
                            num45 = inHigh[i] - inLow[i];
                        }
                        else
                        {
                            double num42;
                            if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                            {
                                double num43;
                                double num44;
                                if (inClose[i] >= inOpen[i])
                                {
                                    num44 = inClose[i];
                                }
                                else
                                {
                                    num44 = inOpen[i];
                                }

                                if (inClose[i] >= inOpen[i])
                                {
                                    num43 = inOpen[i];
                                }
                                else
                                {
                                    num43 = inClose[i];
                                }

                                num42 = inHigh[i] - num44 + (num43 - inLow[i]);
                            }
                            else
                            {
                                num42 = 0.0;
                            }

                            num45 = num42;
                        }

                        num46 = num45;
                    }

                    num47 = num46;
                }

                if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                {
                    num41 = 2.0;
                }
                else
                {
                    num41 = 1.0;
                }

                if (inHigh[i] - num48 > Globals.candleSettings[4].factor * num47 / num41)
                {
                    double num33;
                    double num39;
                    double num40;
                    if (inClose[i] >= inOpen[i])
                    {
                        num40 = inOpen[i];
                    }
                    else
                    {
                        num40 = inClose[i];
                    }

                    if (Globals.candleSettings[7].avgPeriod != 0.0)
                    {
                        num39 = shadowVeryShortPeriodTotal / Globals.candleSettings[7].avgPeriod;
                    }
                    else
                    {
                        double num38;
                        if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
                        {
                            num38 = Math.Abs(inClose[i] - inOpen[i]);
                        }
                        else
                        {
                            double num37;
                            if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                            {
                                num37 = inHigh[i] - inLow[i];
                            }
                            else
                            {
                                double num34;
                                if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                                {
                                    double num35;
                                    double num36;
                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num36 = inClose[i];
                                    }
                                    else
                                    {
                                        num36 = inOpen[i];
                                    }

                                    if (inClose[i] >= inOpen[i])
                                    {
                                        num35 = inOpen[i];
                                    }
                                    else
                                    {
                                        num35 = inClose[i];
                                    }

                                    num34 = inHigh[i] - num36 + (num35 - inLow[i]);
                                }
                                else
                                {
                                    num34 = 0.0;
                                }

                                num37 = num34;
                            }

                            num38 = num37;
                        }

                        num39 = num38;
                    }

                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        num33 = 2.0;
                    }
                    else
                    {
                        num33 = 1.0;
                    }

                    if (num40 - inLow[i] < Globals.candleSettings[7].factor * num39 / num33)
                    {
                        double num31;
                        double num32;
                        if (inOpen[i] > inClose[i])
                        {
                            num32 = inOpen[i];
                        }
                        else
                        {
                            num32 = inClose[i];
                        }

                        if (inOpen[i - 1] < inClose[i - 1])
                        {
                            num31 = inOpen[i - 1];
                        }
                        else
                        {
                            num31 = inClose[i - 1];
                        }

                        if (num32 < num31)
                        {
                            outInteger[outIdx] = 100;
                            outIdx++;
                            goto Label_073E;
                        }
                    }
                }
            }

            outInteger[outIdx] = 0;
            outIdx++;
            Label_073E:
            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num30 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num29;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num29 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num26;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        double num27;
                        double num28;
                        if (inClose[i] >= inOpen[i])
                        {
                            num28 = inClose[i];
                        }
                        else
                        {
                            num28 = inOpen[i];
                        }

                        if (inClose[i] >= inOpen[i])
                        {
                            num27 = inOpen[i];
                        }
                        else
                        {
                            num27 = inClose[i];
                        }

                        num26 = inHigh[i] - num28 + (num27 - inLow[i]);
                    }
                    else
                    {
                        num26 = 0.0;
                    }

                    num29 = num26;
                }

                num30 = num29;
            }

            if (Globals.candleSettings[2].rangeType == RangeType.RealBody)
            {
                num25 = Math.Abs(inClose[bodyTrailingIdx] - inOpen[bodyTrailingIdx]);
            }
            else
            {
                double num24;
                if (Globals.candleSettings[2].rangeType == RangeType.HighLow)
                {
                    num24 = inHigh[bodyTrailingIdx] - inLow[bodyTrailingIdx];
                }
                else
                {
                    double num21;
                    if (Globals.candleSettings[2].rangeType == RangeType.Shadows)
                    {
                        double num22;
                        double num23;
                        if (inClose[bodyTrailingIdx] >= inOpen[bodyTrailingIdx])
                        {
                            num23 = inClose[bodyTrailingIdx];
                        }
                        else
                        {
                            num23 = inOpen[bodyTrailingIdx];
                        }

                        if (inClose[bodyTrailingIdx] >= inOpen[bodyTrailingIdx])
                        {
                            num22 = inOpen[bodyTrailingIdx];
                        }
                        else
                        {
                            num22 = inClose[bodyTrailingIdx];
                        }

                        num21 = inHigh[bodyTrailingIdx] - num23 + (num22 - inLow[bodyTrailingIdx]);
                    }
                    else
                    {
                        num21 = 0.0;
                    }

                    num24 = num21;
                }

                num25 = num24;
            }

            bodyPeriodTotal += num30 - num25;
            if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
            {
                num20 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num19;
                if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                {
                    num19 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num16;
                    if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                    {
                        double num17;
                        double num18;
                        if (inClose[i] >= inOpen[i])
                        {
                            num18 = inClose[i];
                        }
                        else
                        {
                            num18 = inOpen[i];
                        }

                        if (inClose[i] >= inOpen[i])
                        {
                            num17 = inOpen[i];
                        }
                        else
                        {
                            num17 = inClose[i];
                        }

                        num16 = inHigh[i] - num18 + (num17 - inLow[i]);
                    }
                    else
                    {
                        num16 = 0.0;
                    }

                    num19 = num16;
                }

                num20 = num19;
            }

            if (Globals.candleSettings[4].rangeType == RangeType.RealBody)
            {
                num15 = Math.Abs(inClose[shadowLongTrailingIdx] - inOpen[shadowLongTrailingIdx]);
            }
            else
            {
                double num14;
                if (Globals.candleSettings[4].rangeType == RangeType.HighLow)
                {
                    num14 = inHigh[shadowLongTrailingIdx] - inLow[shadowLongTrailingIdx];
                }
                else
                {
                    double num11;
                    if (Globals.candleSettings[4].rangeType == RangeType.Shadows)
                    {
                        double num12;
                        double num13;
                        if (inClose[shadowLongTrailingIdx] >= inOpen[shadowLongTrailingIdx])
                        {
                            num13 = inClose[shadowLongTrailingIdx];
                        }
                        else
                        {
                            num13 = inOpen[shadowLongTrailingIdx];
                        }

                        if (inClose[shadowLongTrailingIdx] >= inOpen[shadowLongTrailingIdx])
                        {
                            num12 = inOpen[shadowLongTrailingIdx];
                        }
                        else
                        {
                            num12 = inClose[shadowLongTrailingIdx];
                        }

                        num11 = inHigh[shadowLongTrailingIdx] - num13 + (num12 - inLow[shadowLongTrailingIdx]);
                    }
                    else
                    {
                        num11 = 0.0;
                    }

                    num14 = num11;
                }

                num15 = num14;
            }

            shadowLongPeriodTotal += num20 - num15;
            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
            {
                num10 = Math.Abs(inClose[i] - inOpen[i]);
            }
            else
            {
                double num9;
                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                {
                    num9 = inHigh[i] - inLow[i];
                }
                else
                {
                    double num6;
                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        double num7;
                        double num8;
                        if (inClose[i] >= inOpen[i])
                        {
                            num8 = inClose[i];
                        }
                        else
                        {
                            num8 = inOpen[i];
                        }

                        if (inClose[i] >= inOpen[i])
                        {
                            num7 = inOpen[i];
                        }
                        else
                        {
                            num7 = inClose[i];
                        }

                        num6 = inHigh[i] - num8 + (num7 - inLow[i]);
                    }
                    else
                    {
                        num6 = 0.0;
                    }

                    num9 = num6;
                }

                num10 = num9;
            }

            if (Globals.candleSettings[7].rangeType == RangeType.RealBody)
            {
                num5 = Math.Abs(inClose[shadowVeryShortTrailingIdx] - inOpen[shadowVeryShortTrailingIdx]);
            }
            else
            {
                double num4;
                if (Globals.candleSettings[7].rangeType == RangeType.HighLow)
                {
                    num4 = inHigh[shadowVeryShortTrailingIdx] - inLow[shadowVeryShortTrailingIdx];
                }
                else
                {
                    double num;
                    if (Globals.candleSettings[7].rangeType == RangeType.Shadows)
                    {
                        double num2;
                        double num3;
                        if (inClose[shadowVeryShortTrailingIdx] >= inOpen[shadowVeryShortTrailingIdx])
                        {
                            num3 = inClose[shadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num3 = inOpen[shadowVeryShortTrailingIdx];
                        }

                        if (inClose[shadowVeryShortTrailingIdx] >= inOpen[shadowVeryShortTrailingIdx])
                        {
                            num2 = inOpen[shadowVeryShortTrailingIdx];
                        }
                        else
                        {
                            num2 = inClose[shadowVeryShortTrailingIdx];
                        }

                        num = inHigh[shadowVeryShortTrailingIdx] - num3 + (num2 - inLow[shadowVeryShortTrailingIdx]);
                    }
                    else
                    {
                        num = 0.0;
                    }

                    num4 = num;
                }

                num5 = num4;
            }

            shadowVeryShortPeriodTotal += num10 - num5;
            i++;
            bodyTrailingIdx++;
            shadowLongTrailingIdx++;
            shadowVeryShortTrailingIdx++;
            if (i <= endIdx)
            {
                goto Label_0313;
            }

            outNBElement = outIdx;
            outBegIdx = startIdx;
            return RetCode.Success;
        }

        public static int CdlInvertedHammerLookback()
        {
            int avgPeriod;
            if ((Globals.candleSettings[2].avgPeriod <= Globals.candleSettings[4].avgPeriod
                     ? Globals.candleSettings[4].avgPeriod
                     : Globals.candleSettings[2].avgPeriod) > Globals.candleSettings[7].avgPeriod)
            {
                avgPeriod = Globals.candleSettings[2].avgPeriod <= Globals.candleSettings[4].avgPeriod
                                ? Globals.candleSettings[4].avgPeriod
                                : Globals.candleSettings[2].avgPeriod;
            }
            else
            {
                avgPeriod = Globals.candleSettings[7].avgPeriod;
            }

            return avgPeriod + 1;
        }
    }
}
