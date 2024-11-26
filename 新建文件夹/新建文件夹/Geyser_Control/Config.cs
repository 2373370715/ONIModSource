using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace Geyser_Control {
    [ConfigFile("config.json", true, true), RestartRequired]
    internal class Config : SingletonOptions<Config> {
        public Config() {
            breakVanilla = false;
            breakFactor  = 3;
            重置           = true;
            休眠           = false;
            喷发           = false;
            即时分析         = false;
        }

        [Option("STRINGS.CONFIG.BREAKVANILLALIMITS.TITLE", "STRINGS.CONFIG.BREAKVANILLALIMITS.TOOLTIP", ""),
         JsonProperty]
        public bool breakVanilla { get; set; }

        [Option("STRINGS.CONFIG.BREAKFACTOR.TITLE", "STRINGS.CONFIG.BREAKFACTOR.TOOLTIP", ""), JsonProperty]
        public int breakFactor { get; set; }

        [Option("STRINGS.CONFIG.RESETBUTTON.TITLE", "STRINGS.CONFIG.RESETBUTTON.TOOLTIP", ""), JsonProperty]
        public bool 重置 { get; set; }

        [Option("STRINGS.CONFIG.DORMANCYBUTTON.TITLE", "STRINGS.CONFIG.DORMANCYBUTTON.TOOLTIP", ""), JsonProperty]
        public bool 休眠 { get; set; }

        [Option("STRINGS.CONFIG.ERUPTIONBUTTON.TITLE", "STRINGS.CONFIG.ERUPTIONBUTTON.TOOLTIP", ""), JsonProperty]
        public bool 喷发 { get; set; }

        [Option("STRINGS.CONFIG.ALLOWINSTANTANALYSIS.TITLE", "STRINGS.CONFIG.ALLOWINSTANTANALYSIS.TOOLTIP", ""),
         JsonProperty]
        public bool 即时分析 { get; set; }
    }
}