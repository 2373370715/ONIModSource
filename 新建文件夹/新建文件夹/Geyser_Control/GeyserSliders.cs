using System;
using KSerialization;
using PeterHan.PLib.Options;

namespace Geyser_Control {
    [SerializationConfig(MemberSerialization.OptIn)]
    internal class GeyserSliders : KMonoBehaviour, IMultiSliderControl, ISidescreenButtonControl, ISim4000ms {
        [MyCmpReq]
        public readonly Geyser geyser;

        [MyCmpReq]
        public readonly KSelectable kSelectable;

        [MyCmpReq]
        public readonly Studyable studyable;

        [Serialize]
        public bool enableSidescreen;

        [Serialize]
        public GeyserConfigurator.GeyserInstanceConfiguration newconfig;

        [Serialize]
        public bool runSim;

        protected ISliderControl[] sliderControls;

        [Serialize]
        public bool slidersSet;

        /// <summary>
        /// 初始化GeyserSliders类的新实例。
        /// </summary>
        public GeyserSliders() {
            // 初始化滑块控件数组，用于控制不同的参数
            sliderControls = new ISliderControl[] {
                new MassPerCycleController(this), // 控制每个周期的质量
                new IterationLengthController(this), // 控制迭代的长度
                new IterationPercentController(this), // 控制迭代的百分比
                new YearLengthController(this), // 控制年的长度
                new YearPercentController(this) // 控制年份的百分比
            };
        }

        // 获取边屏标题的字符串键
        string IMultiSliderControl.SidescreenTitleKey => "STRINGS.SLIDERS.GEYSERSLIDERS.NAME";
        
        // 获取滑块控件数组
        ISliderControl[] IMultiSliderControl.sliderControls => sliderControls;
        
        // 判断边屏是否启用
        bool IMultiSliderControl.SidescreenEnabled() { return enableSidescreen; }
        
        // 获取边屏按钮文本，根据边屏是否启用显示不同文本
        public string SidescreenButtonText =>
            enableSidescreen ? STRINGS.BUTTONS.SLIDERBUTTON.HIDE.NAME : STRINGS.BUTTONS.SLIDERBUTTON.SHOW.NAME;
        
        // 获取边屏按钮工具提示，根据研究状态显示不同提示
        public string SidescreenButtonTooltip =>
            studyable.Studied ? STRINGS.BUTTONS.SLIDERBUTTON.HIDE.TOOLTIP : STRINGS.BUTTONS.DISABLED.TOOLTIP;
        
        // 设置按钮文本覆盖，暂未实现
        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }
        
        // 判断边屏按钮是否启用，始终返回true
        bool ISidescreenButtonControl.SidescreenEnabled() { return true; }
        
        // 判断边屏按钮是否可交互，根据研究状态决定
        public bool SidescreenButtonInteractable() { return studyable.Studied; }
        
        // 处理边屏按钮按下事件，切换边屏启用状态
        public void OnSidescreenButtonPressed() {
            enableSidescreen = !enableSidescreen;
            // 如果当前可选对象被选中，则重新执行选中操作以触发UI更新
            if (kSelectable.IsSelected) {
                SelectTool.Instance.Select(null);
                SelectTool.Instance.Select(kSelectable);
            }
        }

        /// <summary>
        /// 获取水平分组标识符。
        /// </summary>
        /// <returns>始终返回-1，表示没有特定的水平分组。</returns>
        public int HorizontalGroupID() { return -1; }
        
        /// <summary>
        /// 获取按钮在侧屏幕的排序顺序。
        /// </summary>
        /// <returns>始终返回1，表示按钮在侧屏幕上的默认排序顺序。</returns>
        public int ButtonSideScreenSortOrder() { return 1; }

        /// <summary>
        /// 模拟4000毫秒的事件处理函数。
        /// </summary>
        /// <param name="dt">自上次模拟以来的时间差。</param>
        public void Sim4000ms(float dt) {
            // 检查是否应该运行模拟
            if (!runSim) return;
        
            // 重置模拟运行标志，防止重复运行
            runSim = false;
        
            // 停止当前的同步管理器，准备使用新的时间值重新同步
            geyser.smi.StopSM("Resync SM with new time values");
        
            // 重新启动同步管理器
            geyser.smi.StartSM();
        
            // 如果当前项目被选中，则重新执行选中操作以确保状态一致
            if (kSelectable.IsSelected) {
                SelectTool.Instance.Select(null);
                SelectTool.Instance.Select(kSelectable);
            }
        }

        /// <summary>
        /// 当对象生成时调用的方法。
        /// </summary>
        protected override void OnSpawn() {
            // 调用基类的OnSpawn方法以执行初始化操作
            base.OnSpawn();
        
            // 如果当前对象已被研究且滑块设置已完成，则进行以下操作
            if (studyable.Studied && slidersSet) {
                // 停止当前的同步状态机，准备应用新的时间配置
                geyser.smi.StopSM("Resync SM with new time values");
        
                // 更新geyser的配置参数，以匹配新的时间比例设置
                geyser.configuration.scaledRate             = newconfig.scaledRate;
                geyser.configuration.scaledIterationLength  = newconfig.scaledIterationLength;
                geyser.configuration.scaledIterationPercent = newconfig.scaledIterationPercent;
                geyser.configuration.scaledYearLength       = newconfig.scaledYearLength;
                geyser.configuration.scaledYearPercent      = newconfig.scaledYearPercent;
        
                // 应用更新后的配置到geyser，以调整其排放值
                geyser.ApplyConfigurationEmissionValues(geyser.configuration);
        
                // 重新启动同步状态机，以新的配置继续运行
                geyser.smi.StartSM();
            }
        }

        /// <summary>
        /// 控制每个周期喷发质量的滑块控制器类。
        /// </summary>
        protected class MassPerCycleController : ISliderControl {
            /// <summary>
            /// 目标滑块控制器实例。
            /// </summary>
            public GeyserSliders target;
            
            /// <summary>
            /// 构造函数，初始化滑块控制器。
            /// </summary>
            /// <param name="t">目标滑块控制器实例。</param>
            public MassPerCycleController(GeyserSliders t) { target = t; }
            
 
            public string SliderTitleKey => "STRINGS.SLIDERS.MASSPERCYCLECONTROLLER.NAME";
            public string SliderUnits => STRINGS.SLIDERS.MASSPERCYCLECONTROLLER.UNITS;
            
            /// <summary>
            /// 获取滑块的小数位数。
            /// </summary>
            /// <param name="index">滑块索引。</param>
            /// <returns>小数位数。</returns>
            public int SliderDecimalPlaces(int index) { return 3; }
            
            public float GetSliderMin(int index) { return target.geyser.configuration.geyserType.minRatePerCycle / 3; }
            
            public float GetSliderMax(int index) { return 3 * target.geyser.configuration.geyserType.maxRatePerCycle; }
            
            public float GetSliderValue(int index) { return target.geyser.configuration.scaledRate; }
            
            public void SetSliderValue(float value, int index) {
                target.newconfig.scaledRate = value;
                if (target.geyser.configuration.scaledRate != value) {
                    target.geyser.configuration.scaledRate = value;
                    target.geyser.ApplyConfigurationEmissionValues(target.geyser.configuration);
                }
                target.slidersSet = true;
            }
            
            public string GetSliderTooltipKey(int index) { return "STRINGS.SLIDERS.MASSPERCYCLECONTROLLER.TOOLTIP"; }
            public string GetSliderTooltip(int index) { return STRINGS.SLIDERS.MASSPERCYCLECONTROLLER.TOOLTIP; }
        }

        /**
         * 迭代长度
         */
        protected class IterationLengthController : ISliderControl {
            protected GeyserSliders target;
            public IterationLengthController(GeyserSliders t) { target = t; }
            public string SliderTitleKey                 => "STRINGS.SLIDERS.ITERATIONLENGTHCONTROLLER.NAME";
            public string SliderUnits                    => STRINGS.SLIDERS.ITERATIONLENGTHCONTROLLER.UNITS;
            public int    SliderDecimalPlaces(int index) { return 4; }

            public float GetSliderMin(int index) {
                return 0.5f * target.geyser.configuration.geyserType.minIterationLength;
            }

            public float GetSliderMax(int index) {
                return 2f * target.geyser.configuration.geyserType.maxIterationLength;
            }

            public float GetSliderValue(int index) { return target.geyser.configuration.scaledIterationLength; }

            public void SetSliderValue(float value, int index) {
                target.newconfig.scaledIterationLength = value;
                if (target.geyser.configuration.scaledIterationLength != value) {
                    target.geyser.configuration.scaledIterationLength = value;
                    target.runSim                                     = true;
                }

                target.slidersSet = true;
            }

            public string GetSliderTooltipKey(int index) { return "STRINGS.SLIDERS.ITERATIONLENGTHCONTROLLER.TOOLTIP"; }
            public string GetSliderTooltip(int    index) { return STRINGS.SLIDERS.ITERATIONLENGTHCONTROLLER.TOOLTIP; }
        }

        /**
         * 迭代比例
         */
        protected class IterationPercentController : ISliderControl {
            protected GeyserSliders target;
            public IterationPercentController(GeyserSliders t) { target = t; }
            public string SliderTitleKey => "STRINGS.SLIDERS.ITERATIONPERCENTCONTROLLER.NAME";
            public string SliderUnits => STRINGS.SLIDERS.ITERATIONPERCENTCONTROLLER.UNITS;
            public int SliderDecimalPlaces(int index) { return 5; }
            public float GetSliderMin(int index) { return 0.5f; }
            public float GetSliderMax(int index) { return 100f; }
            public float GetSliderValue(int index) { return 100f * target.geyser.configuration.scaledIterationPercent; }

            public void SetSliderValue(float value, int index) {
                value                                   /= 100f;
                target.newconfig.scaledIterationPercent =  value;
                if (target.geyser.configuration.scaledIterationPercent != value) {
                    target.geyser.configuration.scaledIterationPercent = value;
                    target.geyser.ApplyConfigurationEmissionValues(target.geyser.configuration);
                    target.runSim = true;
                }

                target.slidersSet = true;
            }

            public string GetSliderTooltipKey(int index) {
                return "STRINGS.SLIDERS.ITERATIONPERCENTCONTROLLER.TOOLTIP";
            }

            public string GetSliderTooltip(int index) { return STRINGS.SLIDERS.ITERATIONPERCENTCONTROLLER.TOOLTIP; }
        }

        /**
         * 喷发周期
         */
        protected class YearLengthController : ISliderControl {
            protected GeyserSliders target;
            public YearLengthController(GeyserSliders t) { target = t; }
            public string SliderTitleKey                 => "STRINGS.SLIDERS.YEARLENGTHCONTROLLER.NAME";
            public string SliderUnits                    => STRINGS.SLIDERS.YEARLENGTHCONTROLLER.UNITS;
            public int    SliderDecimalPlaces(int index) { return 2; }

            public float GetSliderMin(int index) {
                return Math.Min(6000f, target.geyser.configuration.geyserType.minYearLength);
            }

            public float GetSliderMax(int index) {
                return Math.Min(300000f, 3f * target.geyser.configuration.geyserType.maxYearLength);
            }

            public float GetSliderValue(int index) { return target.geyser.configuration.scaledYearLength; }

            public void SetSliderValue(float value, int index) {
                target.newconfig.scaledYearLength = value;
                if (target.geyser.configuration.scaledYearLength != value) {
                    target.geyser.configuration.scaledYearLength = value;
                    target.runSim                                = true;
                }

                target.slidersSet = true;
            }

            public string GetSliderTooltipKey(int index) { return "STRINGS.SLIDERS.YEARLENGTHCONTROLLER.TOOLTIP"; }
            public string GetSliderTooltip(int    index) { return STRINGS.SLIDERS.YEARLENGTHCONTROLLER.TOOLTIP; }
        }

        /**
         * 一周期多少时间在喷发
         */
        protected class YearPercentController : ISliderControl {
            protected GeyserSliders target;
            public YearPercentController(GeyserSliders t) { target = t; }
            public string SliderTitleKey => "STRINGS.SLIDERS.YEARPERCENTCONTROLLER.NAME";
            public string SliderUnits => STRINGS.SLIDERS.YEARPERCENTCONTROLLER.UNITS;
            public int    SliderDecimalPlaces(int index) { return 5; }
            public float  GetSliderMin(int index) { return 1f; }
            public float  GetSliderMax(int index) { return 100f; }
            public float  GetSliderValue(int index) { return 100f * target.geyser.configuration.scaledYearPercent; }

            public void SetSliderValue(float value, int index) {
                value                              /= 100f;
                target.newconfig.scaledYearPercent =  value;
                if (target.geyser.configuration.scaledYearPercent != value) {
                    target.geyser.configuration.scaledYearPercent = value;
                    target.runSim                                 = true;
                }

                target.slidersSet = true;
            }

            public string GetSliderTooltipKey(int index) { return "STRINGS.SLIDERS.YEARPERCENTCONTROLLER.TOOLTIP"; }
            public string GetSliderTooltip(int    index) { return STRINGS.SLIDERS.YEARPERCENTCONTROLLER.TOOLTIP; }
        }

        /**
         * 重置
         */
        internal class ResetButton : KMonoBehaviour, ISidescreenButtonControl, ISim4000ms {
            [MyCmpReq]
            private readonly GeyserSliders geyserSliders;

            [MyCmpReq]
            private readonly KSelectable kSelectable;

            [MyCmpReq]
            private readonly Studyable studyable;

            [Serialize]
            private int clicks;

            [Serialize]
            private int runSimIterations;

            public string SidescreenButtonText =>
                clicks > 0 ? STRINGS.BUTTONS.RESETBUTTON.CONFIRM.NAME : STRINGS.BUTTONS.RESETBUTTON.NAME;

            public string SidescreenButtonTooltip =>
                studyable.Studied ? STRINGS.BUTTONS.RESETBUTTON.TOOLTIP : STRINGS.BUTTONS.DISABLED.TOOLTIP;

            public void                   SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }
            bool ISidescreenButtonControl.SidescreenEnabled() { return true; }
            public bool                   SidescreenButtonInteractable() { return studyable.Studied; }

            public void OnSidescreenButtonPressed() {
                clicks++;
                if (clicks > 1) {
                    var geyser    = geyserSliders.geyser;
                    var newconfig = geyserSliders.newconfig;
                    geyser.configuration.Init(true);
                    geyser.ApplyConfigurationEmissionValues(geyser.configuration);
                    newconfig.scaledRate             = geyser.configuration.scaledRate;
                    newconfig.scaledIterationLength  = geyser.configuration.scaledIterationLength;
                    newconfig.scaledIterationPercent = geyser.configuration.scaledIterationPercent;
                    newconfig.scaledYearLength       = geyser.configuration.scaledYearLength;
                    newconfig.scaledYearPercent      = geyser.configuration.scaledYearPercent;
                    geyserSliders.runSim             = true;
                    clicks                           = 0;
                }

                if (kSelectable.IsSelected) {
                    SelectTool.Instance.Select(null);
                    SelectTool.Instance.Select(kSelectable);
                }
            }

            public int HorizontalGroupID()         { return -1; }
            public int ButtonSideScreenSortOrder() { return 2; }

            public void Sim4000ms(float dt) {
                if (clicks == 0) return;

                runSimIterations++;
                if (runSimIterations > 3) {
                    clicks           = 0;
                    runSimIterations = 0;
                }

                if (kSelectable.IsSelected) {
                    SelectTool.Instance.Select(null);
                    SelectTool.Instance.Select(kSelectable);
                }
            }
        }
    }
}