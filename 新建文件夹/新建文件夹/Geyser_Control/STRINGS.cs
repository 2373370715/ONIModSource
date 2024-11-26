namespace Geyser_Control {
    // Token: 0x02000012 RID: 18
    internal class STRINGS {
        // Token: 0x02000013 RID: 19
        public class SLIDERS {
            // Token: 0x02000014 RID: 20
            public class GEYSERSLIDERS {
                // Token: 0x04000021 RID: 33
                public static LocString NAME = "";
            }

            // Token: 0x02000015 RID: 21
            public class MASSPERCYCLECONTROLLER {
                // Token: 0x04000022 RID: 34
                public static LocString NAME = "Mass Per Cycle";

                // Token: 0x04000023 RID: 35
                public static LocString TOOLTIP = "Theoretical mass if always active";

                // Token: 0x04000024 RID: 36
                public static LocString UNITS = "kg";
            }

            // Token: 0x02000016 RID: 22
            public class ITERATIONLENGTHCONTROLLER {
                // Token: 0x04000025 RID: 37
                public static LocString NAME = "Iteration Length";

                // Token: 0x04000026 RID: 38
                public static LocString TOOLTIP = "Total Eruption Period including Idle";

                // Token: 0x04000027 RID: 39
                public static LocString UNITS = "s";
            }

            // Token: 0x02000017 RID: 23
            public class ITERATIONPERCENTCONTROLLER {
                // Token: 0x04000028 RID: 40
                public static LocString NAME = "Iteration Percent";

                // Token: 0x04000029 RID: 41
                public static LocString TOOLTIP = "Percentage of Total Eruption Period to erupt";

                // Token: 0x0400002A RID: 42
                public static LocString UNITS = "%";
            }

            // Token: 0x02000018 RID: 24
            public class YEARLENGTHCONTROLLER {
                // Token: 0x0400002B RID: 43
                public static LocString NAME = "Year Length";

                // Token: 0x0400002C RID: 44
                public static LocString TOOLTIP = "Total Life Cycle including Dormancy";

                // Token: 0x0400002D RID: 45
                public static LocString UNITS = "s";
            }

            // Token: 0x02000019 RID: 25
            public class YEARPERCENTCONTROLLER {
                // Token: 0x0400002E RID: 46
                public static LocString NAME = "Year Percent";

                // Token: 0x0400002F RID: 47
                public static LocString TOOLTIP = "Percentage of Total Life Cycle to be Active";

                // Token: 0x04000030 RID: 48
                public static LocString UNITS = "%";
            }
        }

        // Token: 0x0200001A RID: 26
        public class BUTTONS {
            // Token: 0x0200001B RID: 27
            public class SLIDERBUTTON {
                // Token: 0x0200001C RID: 28
                public class SHOW {
                    // Token: 0x04000031 RID: 49
                    public static LocString NAME = "Show Slider Controls";
                }

                // Token: 0x0200001D RID: 29
                public class HIDE {
                    // Token: 0x04000032 RID: 50
                    public static LocString NAME = "Hide Slider Controls";

                    // Token: 0x04000033 RID: 51
                    public static LocString TOOLTIP = "";
                }
            }

            // Token: 0x0200001E RID: 30
            public class RESETBUTTON {
                // Token: 0x04000034 RID: 52
                public static LocString NAME = "Reset to Original Values";

                // Token: 0x04000035 RID: 53
                public static LocString TOOLTIP = "";

                // Token: 0x0200001F RID: 31
                public class CONFIRM {
                    // Token: 0x04000036 RID: 54
                    public static LocString NAME = "CONFIRM";
                }
            }

            // Token: 0x02000020 RID: 32
            public class DORMANCYBUTTON {
                // Token: 0x02000021 RID: 33
                public class ENTER {
                    // Token: 0x04000037 RID: 55
                    public static LocString NAME = "Enter Dormancy";

                    // Token: 0x04000038 RID: 56
                    public static LocString TOOLTIP = "";
                }

                // Token: 0x02000022 RID: 34
                public class EXIT {
                    // Token: 0x04000039 RID: 57
                    public static LocString NAME = "Exit Dormancy";

                    // Token: 0x0400003A RID: 58
                    public static LocString TOOLTIP = "";
                }
            }

            // Token: 0x02000023 RID: 35
            public class ERUPTIONBUTTON {
                // Token: 0x0400003B RID: 59
                public static LocString NAME = "Advance To Next Eruption";

                // Token: 0x0400003C RID: 60
                public static LocString TOOLTIP = "";
            }

            // Token: 0x02000024 RID: 36
            public class DISABLED {
                // Token: 0x0400003D RID: 61
                public static LocString TOOLTIP = "Analyze To Enable";
            }
        }

        // Token: 0x02000025 RID: 37
        public class CONFIG {
            // Token: 0x02000026 RID: 38
            public class BREAKVANILLALIMITS {
                // Token: 0x0400003E RID: 62
                public static LocString TITLE = "Break Vanilla Limits";

                // Token: 0x0400003F RID: 63
                public static LocString TOOLTIP = "Allows values outside normal range";
            }

            // Token: 0x02000027 RID: 39
            public class BREAKFACTOR {
                // Token: 0x04000040 RID: 64
                public static LocString TITLE = "Break Limit Factor";

                // Token: 0x04000041 RID: 65
                public static LocString TOOLTIP = "Factor by which to expand Mass Per Cycle slider limits";
            }

            // Token: 0x02000028 RID: 40
            public class RESETBUTTON {
                // Token: 0x04000042 RID: 66
                public static LocString TITLE = "Enable Reset Button";

                // Token: 0x04000043 RID: 67
                public static LocString TOOLTIP = "Allows resetting a geyser to it's originally spawned values";
            }

            // Token: 0x02000029 RID: 41
            public class DORMANCYBUTTON {
                // Token: 0x04000044 RID: 68
                public static LocString TITLE = "Enable Dormancy Button";

                // Token: 0x04000045 RID: 69
                public static LocString TOOLTIP = "Enters/Exits Dormancy";
            }

            // Token: 0x0200002A RID: 42
            public class ERUPTIONBUTTON {
                // Token: 0x04000046 RID: 70
                public static LocString TITLE = "Enable Eruption Button";

                // Token: 0x04000047 RID: 71
                public static LocString TOOLTIP = "Triggers the next scheduled Eruption";
            }

            // Token: 0x0200002B RID: 43
            public class ALLOWINSTANTANALYSIS {
                // Token: 0x04000048 RID: 72
                public static LocString TITLE = "Enable Instant Analysis";

                // Token: 0x04000049 RID: 73
                public static LocString TOOLTIP
                    = "Analyzing a geyser will complete instantly with no need for a scientist.\nDatabanks still drop";
            }
        }
    }
}