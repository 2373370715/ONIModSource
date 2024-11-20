using System.Collections.Generic;

public static class OffsetTable {
    /// <summary>
    /// 对给定的二维数组进行镜像处理，并返回处理后的新数组。
    /// </summary>
    /// <param name="table">原始的二维数组，每个元素是一个CellOffset数组。</param>
    /// <returns>镜像处理后的二维数组。</returns>
    public static CellOffset[][] Mirror(CellOffset[][] table) {
        // 创建一个列表，用于存储原始数组和镜像数组
        var list = new List<CellOffset[]>();
        // 遍历输入的二维数组
        foreach (var array in table) {
            // 将当前数组添加到列表中
            list.Add(array);
            // 检查数组中第一个元素的x坐标是否非零，以决定是否需要进行镜像处理
            if (array[0].x != 0) {
                // 创建一个新的数组，用于存储镜像后的元素
                var array2 = new CellOffset[array.Length];
                // 遍历当前数组，进行镜像处理
                for (var j = 0; j < array2.Length; j++) {
                    // 将当前数组的元素复制到新数组中
                    array2[j] = array[j];
                    // 反转新数组中元素的x坐标，实现镜像效果
                    array2[j].x = -array2[j].x;
                }
                // 将镜像后的数组添加到列表中
                list.Add(array2);
            }
        }
        // 将列表转换为二维数组并返回
        return list.ToArray();
    }
}