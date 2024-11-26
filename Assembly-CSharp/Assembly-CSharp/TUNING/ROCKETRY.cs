using UnityEngine;

namespace TUNING {
    public class ROCKETRY {
        // 自毁功能的退款比例，设定为50%
        public const float SELF_DESTRUCT_REFUND_FACTOR = 0.5f;

        // 货物舱的容量，单位可能是百分比或者特定的数量
        public const float ENTITIES_CARGO_BAY_CLUSTER_CAPACITY = 100f;

        // 任务持续时间的缩放因子，可能用于计算或者调整任务的持续时间
        public static float MISSION_DURATION_SCALE = 1800f;

        // 质量惩罚的指数，用于计算质量对某些操作或性能的影响
        public static float MASS_PENALTY_EXPONENT = 3.2f;

        // 质量惩罚的除数，与质量惩罚指数一起使用以计算质量相关的惩罚
        public static float MASS_PENALTY_DIVISOR = 300f;

        // 货物容量的缩放因子，可能用于计算货物的装载量或相关指标
        public static float CARGO_CAPACITY_SCALE = 10f;

        // 液体货物舱的容量，单位可能是升或其他合适的单位
        public static float LIQUID_CARGO_BAY_CLUSTER_CAPACITY = 2700f;

        // 固体货物舱的容量，单位与液体货物舱类似
        public static float SOLID_CARGO_BAY_CLUSTER_CAPACITY = 2700f;

        // 气体货物舱的容量，单位可能是立方米或其他合适的单位
        public static float GAS_CARGO_BAY_CLUSTER_CAPACITY = 1100f;

        // 火箭内部尺寸，使用二维向量表示，单位可能是像素或者游戏中的任意单位
        public static Vector2I ROCKET_INTERIOR_SIZE = new Vector2I(32, 32);

        public static float MassFromPenaltyPercentage(float penaltyPercentage = 0.5f) {
            return -(1f / Mathf.Pow(penaltyPercentage - 1f, 5f));
        }

        public static float CalculateMassWithPenalty(float realMass) {
            var b = Mathf.Pow(realMass / MASS_PENALTY_DIVISOR, MASS_PENALTY_EXPONENT);
            return Mathf.Max(realMass, b);
        }

        public class DESTINATION_RESEARCH {
            public static int EVERGREEN = 10;
            public static int BASIC     = 50;
            public static int HIGH      = 150;
        }

        public class DESTINATION_ANALYSIS {
            public static int   DISCOVERED                   = 50;
            public static int   COMPLETE                     = 100;
            public static float DEFAULT_CYCLES_PER_DISCOVERY = 0.5f;
        }

        public class DESTINATION_THRUST_COSTS {
            public static int LOW       = 3;
            public static int MID       = 5;
            public static int HIGH      = 7;
            public static int VERY_HIGH = 9;
        }

        public class CLUSTER_FOW {
            public static float POINTS_TO_REVEAL          = 100f;
            public static float DEFAULT_CYCLES_PER_REVEAL = 0.5f;
        }

        public class ENGINE_EFFICIENCY {
            public static float WEAK    = 20f;
            public static float MEDIUM  = 40f;
            public static float STRONG  = 60f;
            public static float BOOSTER = 30f;
        }

        public class ROCKET_HEIGHT {
            public static int VERY_SHORT              = 10;
            public static int SHORT                   = 16;
            public static int MEDIUM                  = 20;
            public static int TALL                    = 25;
            public static int VERY_TALL               = 35;
            public static int MAX_MODULE_STACK_HEIGHT = VERY_TALL - 5;
        }

        public class OXIDIZER_EFFICIENCY {
            public static float VERY_LOW = 0.334f;
            public static float LOW      = 1f;
            public static float HIGH     = 1.33f;
        }

        public class DLC1_OXIDIZER_EFFICIENCY {
            public static float VERY_LOW = 1f;
            public static float LOW      = 2f;
            public static float HIGH     = 4f;
        }

        public class CARGO_CONTAINER_MASS {
            public static float STATIC_MASS  = 1000f;
            public static float PAYLOAD_MASS = 1000f;
        }

        public class BURDEN {
            public static int INSIGNIFICANT = 1;
            public static int MINOR         = 2;
            public static int MINOR_PLUS    = 3;
            public static int MODERATE      = 4;
            public static int MODERATE_PLUS = 5;
            public static int MAJOR         = 6;
            public static int MAJOR_PLUS    = 7;
            public static int MEGA          = 9;
            public static int MONUMENTAL    = 15;
        }

        public class ENGINE_POWER {
            // 定义不同阶段和强度的常量值
            public static int EARLY_WEAK       = 16; // 初期弱
            public static int EARLY_STRONG     = 23; // 初期强
            public static int MID_VERY_STRONG  = 48; // 中期很强
            public static int MID_STRONG       = 31; // 中期强
            public static int MID_WEAK         = 27; // 中期弱
            public static int LATE_STRONG      = 34; // 后期强
            public static int LATE_VERY_STRONG = 55; // 后期很强
        }

        public class FUEL_COST_PER_DISTANCE {
            public static float VERY_LOW     = 0.033333335f;
            public static float LOW          = 0.0375f;
            public static float MEDIUM       = 0.075f;
            public static float HIGH         = 0.09375f;
            public static float VERY_HIGH    = 0.15f;
            public static float GAS_VERY_LOW = 0.025f;
            public static float GAS_LOW      = 0.027777778f;
            public static float GAS_HIGH     = 0.041666668f;
            public static float PARTICLES    = 0.33333334f;
        }
    }
}