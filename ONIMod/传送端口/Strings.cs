using OUI = STRINGS.UI;

namespace RsTransferPort;

public static class MYSTRINGS {
    public static class BUILDING {
        public static class STATUSITEMS {
            public class RSRADIANTPARTICLESTRANSFERSENDERINFO {
                public static LocString NAME    = "传送出口堵塞，关闭捕获辐射粒子";
                public static LocString TOOLTIP = "传送出口堵塞，关闭捕获辐射粒子";
            }

            public class RSTRANSFERPORTCHANNELCONNECTION {
                public static LocString NAME    = "未连接频道";
                public static LocString TOOLTIP = "通过频道名称建立或加入频道";
            }

            public class RSCHANNELPLANETARYISOLATIONMODEL {
                public static LocString NAME    = "行星隔离模式";
                public static LocString TOOLTIP = "该模式无法进行跨行星或火箭传送，可在频道设置切换为宇宙互通模式";
            }

            public class RSCHANNELGLOBALCONNECTIVITYMODEL {
                public static LocString NAME    = "宇宙互通模式";
                public static LocString TOOLTIP = "该模式可进行跨行星或火箭传送，可在频道设置切换为行星隔离模式";
            }
        }
    }

    public static class BUILDINGS {
        public static class PREFABS {
            public static class RSLIQUIDTRANSFERCONDUITSENDER {
                public static LocString NAME = OUI.FormatAsLink("液体管道传送入口", LiquidTransferConduitSenderConfig.ID);
                public static LocString DESC = "你还在为管道太长而烦恼吗，来试试这个吧，解决管道太长，影响规划，性能等问题";

                public static LocString EFFECT = string.Concat("通过频道名称建立频道，将液体传送到同一频道的",
                                                               OUI.FormatAsLink("液体管道传送出口",
                                                                                LiquidTransferConduitReceiverConfig
                                                                                    .ID));

                public static LocString DEFAULTNAME = "液体管道传送入口";
            }

            public static class RSLIQUIDTRANSFERCONDUITRECEIVER {
                public static LocString NAME = OUI.FormatAsLink("液体管道传送出口", LiquidTransferConduitReceiverConfig.ID);
                public static LocString DESC = "你还在为管道太长而烦恼吗，来试试这个吧，解决管道太长，影响规划，性能等问题";

                public static LocString EFFECT = string.Concat("通过频道名称建立频道，接收同一频道",
                                                               OUI.FormatAsLink("液体管道传送入口",
                                                                                LiquidTransferConduitSenderConfig.ID));

                public static LocString DEFAULTNAME = "液体管道传送出口";
            }

            public static class RSGASTRANSFERCONDUITSENDER {
                public static LocString NAME = OUI.FormatAsLink("气体管道传送入口", GasTransferConduitSenderConfig.ID);
                public static LocString DESC = "你还在为管道太长而烦恼吗，来试试这个吧，解决管道太长，影响规划，性能等问题";

                public static LocString EFFECT = string.Concat("通过频道名称建立频道，将气体传送到同一频道的",
                                                               OUI.FormatAsLink("气体管道传送出口",
                                                                                GasTransferConduitReceiverConfig.ID));

                public static LocString DEFAULTNAME = "气体管道传送入口";
            }

            public static class RSGASTRANSFERCONDUITRECEIVER {
                public static LocString NAME = OUI.FormatAsLink("气体管道传送出口", GasTransferConduitReceiverConfig.ID);
                public static LocString DESC = "你还在为管道太长而烦恼吗，来试试这个吧，解决管道太长，影响规划，性能等问题";

                public static LocString EFFECT = string.Concat("通过频道名称建立频道，将气体传送到同一频道的",
                                                               OUI.FormatAsLink("气体管道传送入口",
                                                                                GasTransferConduitSenderConfig.ID));

                public static LocString DEFAULTNAME = "气体管道传送出口";
            }

            public static class RSSOLIDTRANSFERCONDUITSENDER {
                public static LocString NAME = OUI.FormatAsLink("固体轨道传送入口", SolidTransferConduitSenderConfig.ID);
                public static LocString DESC = "你还在为管道太长而烦恼吗，来试试这个吧，解决管道太长，影响规划，性能等问题";

                public static LocString EFFECT = string.Concat("通过频道名称建立频道，接收同一频道",
                                                               OUI.FormatAsLink("固体轨道传送出口",
                                                                                SolidTransferConduitReceiverConfig.ID));

                public static LocString DEFAULTNAME = "固体轨道传送入口";
            }

            public static class RSSOLIDTRANSFERCONDUITRECEIVER {
                public static LocString NAME = OUI.FormatAsLink("固体轨道传送出口", SolidTransferConduitReceiverConfig.ID);
                public static LocString DESC = "你还在为管道太长而烦恼吗，来试试这个吧，解决管道太长，影响规划，性能等问题";

                public static LocString EFFECT = string.Concat("通过频道名称建立频道，接收同一频道",
                                                               OUI.FormatAsLink("气体管道传送入口",
                                                                                SolidTransferConduitSenderConfig.ID));

                public static LocString DEFAULTNAME = "固体轨道传送出口";
            }

            public static class RSWIRELESSPOWERPORT {
                public static LocString NAME        = OUI.FormatAsLink("无线电力端口", WirelessPowerPortConfig.ID);
                public static LocString DESC        = "你还在为管道太长而烦恼吗，来试试这个吧，解决管道太长，影响规划，性能等问题";
                public static LocString EFFECT      = "通过频道名称建立频道，同一频道可视为同一条线路";
                public static LocString DEFAULTNAME = "无线电力端口";
            }

            public static class RSWIRELESSLOGICSENDER {
                public static LocString NAME = OUI.FormatAsLink("无线信号播报端口", WirelessLogicSenderConfig.ID);
                public static LocString DESC = "你还在为管道太长而烦恼吗，来试试这个吧，解决管道太长，影响规划，性能等问题";

                public static LocString EFFECT = string.Concat("通过频道名称建立频道，将信号播报给同一频道的",
                                                               OUI.FormatAsLink("无线信号接收端口",
                                                                                WirelessLogicSenderConfig.ID));

                public static LocString DEFAULTNAME = "无线信号播报端口";
            }

            public static class RSWIRELESSLOGICRECEIVER {
                public static LocString NAME = OUI.FormatAsLink("无线信号接收端口", WirelessLogicReceiverConfig.ID);
                public static LocString DESC = "你还在为管道太长而烦恼吗，来试试这个吧，解决管道太长，影响规划，性能等问题";

                public static LocString EFFECT = string.Concat("通过频道名称建立频道，将信号播报给同一频道的",
                                                               OUI.FormatAsLink("无线信号接收端口",
                                                                                WirelessLogicSenderConfig.ID));

                public static LocString DEFAULTNAME = "无线信号接收端口";
            }

            public static class RSRADIANTPARTICLESTRANSFERSENDER {
                public static LocString NAME = OUI.FormatAsLink("辐射粒子传送入口", RadiantParticlesTransferSenderConfig.ID);
                public static LocString DESC = "你还在为管道太长而烦恼吗，来试试这个吧，解决管道太长，影响规划，性能等问题";

                public static LocString EFFECT = string.Concat("通过频道名称建立频道,将辐射粒子传送到同一频道的",
                                                               OUI.FormatAsLink("辐射粒子传送出口",
                                                                                RadiantParticlesTransferReceiverConfig
                                                                                    .ID));

                public static LocString DEFAULTNAME         = "辐射粒子传送入口";
                public static LocString LOGIC_PORT          = "需要辐射粒子";
                public static LocString LOGIC_PORT_ACTIVE   = "<b><style=\"logic_on\">绿色信号</style></b>: 接收到出口发送的绿色信号";
                public static LocString LOGIC_PORT_INACTIVE = "<b><style=\"logic_off\">红色信号</style></b>: 接收到出口发送的红色信号";
            }

            public static class RSRADIANTPARTICLESTRANSFERRECEIVER {
                public static LocString NAME = OUI.FormatAsLink("辐射粒子传送出口", RadiantParticlesTransferReceiverConfig.ID);
                public static LocString DESC = "你还在为管道太长而烦恼吗，来试试这个吧，解决管道太长，影响规划，性能等问题";

                public static LocString EFFECT = string.Concat("通过频道名称建立频道，接收同一频道",
                                                               OUI.FormatAsLink("辐射粒子传送入口",
                                                                                RadiantParticlesTransferSenderConfig
                                                                                    .ID),
                                                               ".");

                public static LocString DEFAULTNAME       = "辐射粒子传送出口";
                public static LocString LOGIC_PORT        = "需要辐射粒子";
                public static LocString LOGIC_PORT_ACTIVE = "<b><style=\"logic_on\">绿色信号</style></b>: 启用传送，并发送绿色信号到入口";

                public static LocString LOGIC_PORT_INACTIVE
                    = "<b><style=\"logic_off\">红色信号</style></b>: 禁用传送，并发送红色信号到入口";
            }

            public static class RSTRANSFERPORTCENTER {
                public static LocString NAME   = OUI.FormatAsLink("传送端口终端", TransferPortCenterConfig.ID);
                public static LocString DESC   = "已发现的世界是会进行一格一物模拟，将开发过完成的世界设置为未发现有助于优化性能";
                public static LocString EFFECT = "设置是否已发现某些世界";
            }
        }
    }

    public static class UI {
        public static class NEWBUILDCATEGORIES {
            public static class RS_TRANSFER_PORT {
                public static LocString NAME           = "传送端口";
                public static LocString BUILDMENUTITLE = "传送端口";
            }
        }

        public static class TOOLTIPS {
            public static LocString PORTCHANNELMODE_OVERLAY_STRING = "显示传送端口的频道建立情况";
        }

        public static class TOOLS {
            public static class FILTERLAYERS {
                public static LocString RS_ALL_PORT            = "所有端口";
                public static LocString RS_GAS_PORT            = "气体端口";
                public static LocString RS_LIQUID_PORT         = "液体端口";
                public static LocString RS_SOLID_PORT          = "固体端口";
                public static LocString RS_POWER_PORT          = "电力端口";
                public static LocString RS_LOGIC_PORT          = "信号端口";
                public static LocString RS_HEP_PORT            = "辐射粒子端口";
                public static LocString RS_CENTER_LINE         = "中心连接线";
                public static LocString RS_NEAR_LINE           = "就近连接线";
                public static LocString RS_Hide_LINE           = "隐藏连接线";
                public static LocString RS_DISABLE_LINE_ANIM   = "禁用连线动画";
                public static LocString RS_SHOW_PRIORITY       = "显示优先级信息";
                public static LocString RS_ONLY_NULL_CHANNEL   = "仅显示(空频道)";
                public static LocString RS_ONLY_GLOBAL_CHANNEL = "仅显示全球互通模式频道";
            }
        }

        public static class USERMENU {
            public static class SHOWOVERLAYSELF_BUTTON {
                public static LocString NAME = "显示该频道概览";

                //显示该频道概览
                public static LocString TOOLTIP = "仅显示该频道的概览";
            }
        }

        public static class OVERLAYS {
            public static class PORTCHANNELMODE {
                public static LocString NAME   = "传送频道概览";
                public static LocString BUTTON = "传送频道概览";
            }
        }

        public static class SIDESCREEN {
            public class RS_PORT_CHANNEL {
                public static LocString TITLE                  = "传送端口频道设置";
                public static LocString CHANNEL_NAME           = "频道名称";
                public static LocString CHANNEL_LIST           = "频道列表";
                public static LocString CHANNEL_NULL           = "(空频道)";
                public static LocString DETAIL_LEVEL_TOOLTIP   = "切换列表信息展示等级";
                public static LocString WARIN_BATCH_MODE       = "批量修改模式";
                public static LocString BATCH_NAME_TOOLTIP     = "进入批量修改模式,可以修改同频道下的所有端口的频道名称或频道模式";
                public static LocString CANDIDATE_NAME_TOOLTIP = "打开或关闭候选词库";
                public static LocString GLOBAL_TOOLTIP         = "开启或关闭全球互通模式,可批量操作";

                public static LocString PRIORITY_TOOLTIP
                    = "传送优先级，可在传送频道概览查看，可批量操作\\n<color=#888888>灰色</color>: 无\\n<color=#4ABEC9>蓝色</color>: 该优先级有被使用\\n<color=#EFB258>黄色</color>: 当前端口的优先级";

                public static LocString PRIORITY_LINE_INFO = "优先级: {0}   入口: {1}   出口: {2}";
            }

            public class RS_CANDIDATE_NAME {
                public static LocString TITLE               = "候选名字";
                public static LocString SUPPLY_STATE_0      = "切换供应或回收，当前:<style=\"KKeyword\">无</style>";
                public static LocString SUPPLY_STATE_1      = "切换供应或回收，当前:<style=\"KKeyword\">供应</style>";
                public static LocString SUPPLY_STATE_2      = "切换供应或回收，当前:<style=\"KKeyword\">回收</style>";
                public static LocString TEMPERATURE_STATE_0 = "切换高温或低温，当前:<style=\"KKeyword\">无</style>";
                public static LocString TEMPERATURE_STATE_1 = "切换高温或低温，当前:<style=\"KKeyword\">低温</style>";
                public static LocString TEMPERATURE_STATE_2 = "切换高温或低温，当前:<style=\"KKeyword\">高温</style>";

                public static class S_NAMES {
                    public static LocString SUPPLY           = "供应";
                    public static LocString RECYCLE          = "回收";
                    public static LocString HIGH_TEMPERATURE = "高温";
                    public static LocString LOW_TEMPERATURE  = "低温";
                }

                public static class LABELS {
                    public static LocString GAS_0     = "基地氧气";
                    public static LocString GAS_1     = "工业氧气";
                    public static LocString GAS_2     = "二氧化碳";
                    public static LocString GAS_3     = "氯气";
                    public static LocString GAS_4     = "氢气";
                    public static LocString GAS_5     = "天然气";
                    public static LocString GAS_6     = "蒸汽";
                    public static LocString GAS_7     = "杂气";
                    public static LocString LIQUID_0  = "清水";
                    public static LocString LIQUID_1  = "污染水";
                    public static LocString LIQUID_2  = "浓盐水";
                    public static LocString LIQUID_3  = "盐水";
                    public static LocString LIQUID_4  = "石油";
                    public static LocString LIQUID_5  = "原油";
                    public static LocString LIQUID_6  = "乙醇";
                    public static LocString LIQUID_7  = "冷却剂";
                    public static LocString LIQUID_8  = "杂水";
                    public static LocString LIQUID_9  = "液氢";
                    public static LocString LIQUID_10 = "液氧";
                    public static LocString LIQUID_11 = "粘性凝胶";
                    public static LocString LIQUID_12 = "石脑油";
                    public static LocString SOLID_0   = "基地存储";
                    public static LocString SOLID_1   = "食物冷藏";
                    public static LocString SOLID_2   = "杀毒";
                    public static LocString SOLID_3   = "泥土";
                    public static LocString SOLID_4   = "硫";
                    public static LocString SOLID_5   = "磷";
                    public static LocString SOLID_6   = "菌泥";
                    public static LocString SOLID_7   = "漂白石";
                    public static LocString SOLID_8   = "有机物";
                    public static LocString SOLID_9   = "过滤介质";
                    public static LocString POWER_0   = "基地高复合导线";
                    public static LocString POWER_1   = "高复合导线";
                    public static LocString POWER_2   = "基地导线";
                    public static LocString POWER_3   = "导线";
                    public static LocString POWER_4   = "基地电线";
                    public static LocString POWER_5   = "电线";
                    public static LocString HEP_0     = "粒子1";
                    public static LocString HEP_1     = "粒子25";
                    public static LocString HEP_2     = "粒子2550";
                    public static LocString HEP_3     = "粒子100";
                    public static LocString HEP_4     = "粒子500";
                    public static LocString HEP_5     = "粒子回收";
                    public static LocString LOGIC_0   = "陨石雨";
                    public static LocString LOGIC_1   = "火箭1";
                    public static LocString LOGIC_2   = "火箭2";
                    public static LocString LOGIC_3   = "火箭3";
                    public static LocString LOGIC_4   = "火箭4";
                }
            }

            public static class WORLDDISCOVEREDSIDESCREEN {
                public static LocString TITLE = "世界发现设置";
                public static LocString HEADE = "是否已发现世界，未发现的世界不会一格一物模拟";
            }
        }
    }
}