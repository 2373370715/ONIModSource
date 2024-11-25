using System;

public class 建筑 {
    public class 基地 {
        public class 存储箱 {
            public static float 容量 = 20000f * 10000;
        }

        public static class 机械气闸 {
            public static float 不通电速度 = 0.65f;
            public static float 通电速度  = 5f * 20;
            public static bool  隔水隔气  = true;
        }
    }

    public class 电力 {
        public static class 巨型电池 {
            public static float 产热   = 1f;
            public static float 容量   = 40000f     * 100;
            public static float 能量损失 = 3.3333333f * 0;
        }

        public static class 电池 {
            public static float 产热   = 0.5f;
            public static float 容量   = 10000f     * 100;
            public static float 能量损失 = 1.6666666f * 0;
        }

        public static class 智能电池 {
            public static float 产热   = 0.5f;
            public static float 容量   = 20000f     * 100;
            public static float 能量损失 = 6.6666667f * 0;
        }

        public static class 蒸汽机 {
            public static float 输入蒸汽温度 = Tools.getTemperature(100);
            public static float 输出水温度  = Tools.getTemperature(20);
        }
    }

    public class 食物 {
        public static class 电动烤炉 {
            public static float 产热    = 0f;
            public static bool  需要复制人 = false;
        }

        public static class 孵化器 {
            public static bool  需要抱抱 = false;
            public static float 加速速率 = 400f * 100;
            public static float 产热   = 0f;
            public static float 电力   = 240f * 0;
        }

        public static class 冰箱 {
            public static float 容量 = 100f * 100000f;
        }
    }

    public class 站台 {
        public static class 超级计算机 {
            public static bool 需要水 = false;
        }
    }

    public class 精炼 {
        public static class 聚合物压塑器 {
            public static bool 输出水蒸气  = false;
            public static bool 输出二氧化碳 = false;
        }
    }

    public class 水管 {
        public static class 排液口 {
            public static float 压力 = 100000000f;
        }

        public static class 液泵 {
            public static byte 半径 = 3;
        }
    }

    public class 通风 {
        public static class 排气口 {
            public static float 压力 = 100000000f;
        }

        public static class 气泵 {
            public static byte 半径 = 5;
        }
    }

    public class 氧气 {
        public class 碳素脱离器 {
            public static byte 半径 = 10;
        }
    }
}

public static class 系统 {
    public static bool 过热 = false;

    public static class 电线 {
        public static float Max1000  = 1000f  * 1000;
        public static float Max2000  = 2000f  * 1000;
        public static float Max20000 = 20000f * 1000;
        public static float Max50000 = 50000f * 1000;
    }

    public static class 复制人 {
        public static float 挖掘掉落倍率 = 2f;
        public static float 速度     = 100f;
    }

    public static class 游戏速度 {
        public static int low    = 1;
        public static int midium = 3;
        public static int high   = 5;
    }
}

public static class 动物 {
    public static bool 拥挤   = false;
    public static bool 驯化加速 = true;
}