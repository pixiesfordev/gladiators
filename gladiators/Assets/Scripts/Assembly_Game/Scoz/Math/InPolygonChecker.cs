using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 使用 Ray Casting 概念判斷一個 2D 點是否位於多邊形內
/// </summary>
public class InPolygonChecker {
    /// <summary>
    /// 判斷某個點(Vector2)是否在 polygon 之內
    /// </summary>
    public static bool IsPointInPolygonRayCasting(Vector2 target, List<Vector2> polygon) {
        int intersectionCount = 0;

        for (int i = 0; i < polygon.Count; i++) {
            Vector2 p1 = polygon[i];
            Vector2 p2 = polygon[(i + 1) % polygon.Count];

            // 目標點在Y軸上的多邊形的兩頂點p1p2間才考慮後續計算
            bool intersect = ((p1.y > target.y) != (p2.y > target.y));
            if (intersect) {

                // 求多邊形的某個邊的直線方程式                
                // x(t)=x1+t(x2-x1)
                // y(t)=y1+t(y2-y1)                

                // 計算與水平線y=target.y的交點
                // 當此邊與水平線y=target.y相交時，多邊形方程式為y1+t(y2-y1)=target.y
                // 所以t=(target.y-y1)/(y2-y1)
                // 把t帶入多邊形方程式x(t)=x1+t(x2-x1)

                float xCross = p1.x + (target.y - p1.y) * (p2.x - p1.x) / (p2.y - p1.y);

                // 若交點在 target 右側(xCross>=target.x)，就算有香蕉
                if (xCross >= target.x) {
                    intersectionCount++;
                }
            }
        }
        return intersectionCount % 2 != 0;
    }
}
