using System;
using System.Collections.Generic;
using UnityEngine;

public static class DiscreteShadowCaster {
    public enum Direction {
        North,
        East,
        South,
        West
    }

    public enum Octant {
        N_NW,
        N_NE,
        E_NE,
        E_SE,
        S_SE,
        S_SW,
        W_SW,
        W_NW
    }

    public static Direction OctantToDirection(Octant octant) {
        switch (octant) {
            case Octant.N_NW:
            case Octant.N_NE:
                return Direction.North;
            case Octant.E_NE:
            case Octant.E_SE:
                return Direction.East;
            case Octant.S_SE:
            case Octant.S_SW:
                return Direction.South;
            case Octant.W_SW:
            case Octant.W_NW:
                return Direction.West;
            default:
                return Direction.South;
        }
    }

    public static Vector2I DirectionToVector(Direction dir) {
        switch (dir) {
            case Direction.North:
                return new Vector2I(0, 1);
            case Direction.East:
                return new Vector2I(1, 0);
            case Direction.South:
                return new Vector2I(0, -1);
            case Direction.West:
                return new Vector2I(-1, 0);
            default:
                return default(Vector2I);
        }
    }

    public static Vector2I TravelDirectionToOrtogonalDiractionVector(Direction dir) {
        switch (dir) {
            case Direction.North:
            case Direction.South:
                return new Vector2I(1, 0);
            case Direction.East:
            case Direction.West:
                return new Vector2I(0, 1);
            default:
                return default(Vector2I);
        }
    }

    public static void GetVisibleCells(int        cell,
                                       List<int>  visiblePoints,
                                       int        range,
                                       LightShape shape,
                                       bool       canSeeThroughTransparent = true) {
        GetVisibleCells(cell, visiblePoints, range, 0, Direction.South, shape, canSeeThroughTransparent);
    }

    public static void GetVisibleCells(int        cell,
                                       List<int>  visiblePoints,
                                       int        range,
                                       int        width,
                                       Direction  direction,
                                       LightShape shape,
                                       bool       canSeeThroughTransparent = true) {
        visiblePoints.Add(cell);
        var cellPos = Grid.CellToXY(cell);
        if (shape == LightShape.Circle) {
            ScanOctant(cellPos,
                       range,
                       1,
                       Octant.N_NW,
                       1.0,
                       0.0,
                       visiblePoints,
                       canSeeThroughTransparent);

            ScanOctant(cellPos,
                       range,
                       1,
                       Octant.N_NE,
                       1.0,
                       0.0,
                       visiblePoints,
                       canSeeThroughTransparent);

            ScanOctant(cellPos,
                       range,
                       1,
                       Octant.E_NE,
                       1.0,
                       0.0,
                       visiblePoints,
                       canSeeThroughTransparent);

            ScanOctant(cellPos,
                       range,
                       1,
                       Octant.E_SE,
                       1.0,
                       0.0,
                       visiblePoints,
                       canSeeThroughTransparent);

            ScanOctant(cellPos,
                       range,
                       1,
                       Octant.S_SE,
                       1.0,
                       0.0,
                       visiblePoints,
                       canSeeThroughTransparent);

            ScanOctant(cellPos,
                       range,
                       1,
                       Octant.S_SW,
                       1.0,
                       0.0,
                       visiblePoints,
                       canSeeThroughTransparent);

            ScanOctant(cellPos,
                       range,
                       1,
                       Octant.W_SW,
                       1.0,
                       0.0,
                       visiblePoints,
                       canSeeThroughTransparent);

            ScanOctant(cellPos,
                       range,
                       1,
                       Octant.W_NW,
                       1.0,
                       0.0,
                       visiblePoints,
                       canSeeThroughTransparent);

            return;
        }

        if (shape == LightShape.Cone) {
            ScanOctant(cellPos,
                       range,
                       1,
                       Octant.S_SE,
                       1.0,
                       0.0,
                       visiblePoints,
                       canSeeThroughTransparent);

            ScanOctant(cellPos,
                       range,
                       1,
                       Octant.S_SW,
                       1.0,
                       0.0,
                       visiblePoints,
                       canSeeThroughTransparent);

            return;
        }

        if (shape == LightShape.Quad)
            ScanQuad(cellPos, direction, width, range, visiblePoints, canSeeThroughTransparent);
    }

    public static void ScanQuad(Vector2I  cellPos,
                                Direction direction,
                                int       width,
                                int       range,
                                List<int> visiblePoints,
                                bool      canSeeThroughTransparent) {
        if (width <= 0 || range <= 0) return;

        var array    = new Vector2I[width];
        var s        = width % 2 == 0 ? width / 2 - 1 : Mathf.FloorToInt((width - 1) * 0.5f);
        var v        = DirectionToVector(direction);
        var v2       = TravelDirectionToOrtogonalDiractionVector(direction);
        var u        = cellPos - v2 * s;
        var vector2I = new Vector2I(-1, -1);
        for (var i = 0; i < width; i++) {
            var vector2I2 = u + v2 * i;
            var flag      = DoesOcclude(vector2I2.x, vector2I2.y, canSeeThroughTransparent);
            array[i] = flag ? vector2I : vector2I2;
        }

        foreach (var u2 in array)
            if (!(u2 == vector2I)) {
                var flag2 = false;
                var num   = 0;
                while (!flag2 && num < range) {
                    var vector2I3 = u2 + v * num;
                    flag2 = flag2 || DoesOcclude(vector2I3.x, vector2I3.y, canSeeThroughTransparent);
                    if (!flag2) {
                        var item = Grid.XYToCell(vector2I3.x, vector2I3.y);
                        if (!visiblePoints.Contains(item)) visiblePoints.Add(item);
                    }

                    num++;
                }
            }
    }

    private static bool DoesOcclude(int x, int y, bool canSeeThroughTransparent = false) {
        var num = Grid.XYToCell(x, y);
        return Grid.IsValidCell(num) && (!canSeeThroughTransparent || !Grid.Transparent[num]) && Grid.Solid[num];
    }

    private static void ScanOctant(Vector2I  cellPos,
                                   int       range,
                                   int       depth,
                                   Octant    octant,
                                   double    startSlope,
                                   double    endSlope,
                                   List<int> visiblePoints,
                                   bool      canSeeThroughTransparent = true) {
        var num  = range * range;
        var num2 = 0;
        var num3 = 0;
        switch (octant) {
            case Octant.N_NW:
                num3 = cellPos.y + depth;
                if (num3 >= Grid.HeightInCells) return;

                num2 = cellPos.x - Convert.ToInt32(startSlope * Convert.ToDouble(depth));
                if (num2 < 0) num2 = 0;
                while (GetSlope(num2, num3, cellPos.x, cellPos.y, false) <= endSlope) {
                    if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num) {
                        if (DoesOcclude(num2, num3, canSeeThroughTransparent)) {
                            if (num2 - 1 >= 0                                              &&
                                !DoesOcclude(num2 - 1, num3,     canSeeThroughTransparent) &&
                                !DoesOcclude(num2 - 1, num3 - 1, canSeeThroughTransparent))
                                ScanOctant(cellPos,
                                           range,
                                           depth + 1,
                                           octant,
                                           startSlope,
                                           GetSlope(num2 - 0.5, num3 - 0.5, cellPos.x, cellPos.y, false),
                                           visiblePoints,
                                           canSeeThroughTransparent);
                        } else {
                            if (num2 - 1 >= 0 && DoesOcclude(num2 - 1, num3, canSeeThroughTransparent))
                                startSlope = -GetSlope(num2 - 0.5, num3 + 0.5, cellPos.x, cellPos.y, false);

                            if (!DoesOcclude(num2, num3 - 1, canSeeThroughTransparent) &&
                                !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
                                visiblePoints.Add(Grid.XYToCell(num2, num3));
                        }
                    }

                    num2++;
                }

                num2--;
                break;
            case Octant.N_NE:
                num3 = cellPos.y + depth;
                if (num3 >= Grid.HeightInCells) return;

                num2 = cellPos.x + Convert.ToInt32(startSlope * Convert.ToDouble(depth));
                if (num2 >= Grid.WidthInCells) num2 = Grid.WidthInCells - 1;
                while (GetSlope(num2, num3, cellPos.x, cellPos.y, false) >= endSlope) {
                    if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num) {
                        if (DoesOcclude(num2, num3, canSeeThroughTransparent)) {
                            if (num2 + 1 < Grid.HeightInCells                              &&
                                !DoesOcclude(num2 + 1, num3,     canSeeThroughTransparent) &&
                                !DoesOcclude(num2 + 1, num3 - 1, canSeeThroughTransparent)) {
                                var slope = GetSlope(num2 + 0.5, num3 - 0.5, cellPos.x, cellPos.y, false);
                                ScanOctant(cellPos,
                                           range,
                                           depth + 1,
                                           octant,
                                           startSlope,
                                           slope,
                                           visiblePoints,
                                           canSeeThroughTransparent);
                            }
                        } else {
                            if (num2 + 1 < Grid.HeightInCells && DoesOcclude(num2 + 1, num3, canSeeThroughTransparent))
                                startSlope = GetSlope(num2 + 0.5, num3 + 0.5, cellPos.x, cellPos.y, false);

                            if (!DoesOcclude(num2, num3 - 1, canSeeThroughTransparent) &&
                                !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
                                visiblePoints.Add(Grid.XYToCell(num2, num3));
                        }
                    }

                    num2--;
                }

                num2++;
                break;
            case Octant.E_NE:
                num2 = cellPos.x + depth;
                if (num2 >= Grid.WidthInCells) return;

                num3 = cellPos.y + Convert.ToInt32(startSlope * Convert.ToDouble(depth));
                if (num3 >= Grid.HeightInCells) num3 = Grid.HeightInCells - 1;
                while (GetSlope(num2, num3, cellPos.x, cellPos.y, true) >= endSlope) {
                    if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num) {
                        if (DoesOcclude(num2, num3, canSeeThroughTransparent)) {
                            if (num3 + 1 < Grid.HeightInCells                          &&
                                !DoesOcclude(num2, num3 + 1, canSeeThroughTransparent) &&
                                !DoesOcclude(num2       - 1, num3 + 1, canSeeThroughTransparent))
                                ScanOctant(cellPos,
                                           range,
                                           depth + 1,
                                           octant,
                                           startSlope,
                                           GetSlope(num2 - 0.5, num3 + 0.5, cellPos.x, cellPos.y, true),
                                           visiblePoints,
                                           canSeeThroughTransparent);
                        } else {
                            if (num3 + 1 < Grid.HeightInCells && DoesOcclude(num2, num3 + 1, canSeeThroughTransparent))
                                startSlope = GetSlope(num2 + 0.5, num3 + 0.5, cellPos.x, cellPos.y, true);

                            if (!DoesOcclude(num2 - 1, num3, canSeeThroughTransparent) &&
                                !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
                                visiblePoints.Add(Grid.XYToCell(num2, num3));
                        }
                    }

                    num3--;
                }

                num3++;
                break;
            case Octant.E_SE:
                num2 = cellPos.x + depth;
                if (num2 >= Grid.WidthInCells) return;

                num3 = cellPos.y - Convert.ToInt32(startSlope * Convert.ToDouble(depth));
                if (num3 < 0) num3 = 0;
                while (GetSlope(num2, num3, cellPos.x, cellPos.y, true) <= endSlope) {
                    if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num) {
                        if (DoesOcclude(num2, num3, canSeeThroughTransparent)) {
                            if (num3 - 1 >= 0                                          &&
                                !DoesOcclude(num2, num3 - 1, canSeeThroughTransparent) &&
                                !DoesOcclude(num2       - 1, num3 - 1, canSeeThroughTransparent))
                                ScanOctant(cellPos,
                                           range,
                                           depth + 1,
                                           octant,
                                           startSlope,
                                           GetSlope(num2 - 0.5, num3 - 0.5, cellPos.x, cellPos.y, true),
                                           visiblePoints,
                                           canSeeThroughTransparent);
                        } else {
                            if (num3 - 1 >= 0 && DoesOcclude(num2, num3 - 1, canSeeThroughTransparent))
                                startSlope = -GetSlope(num2 + 0.5, num3 - 0.5, cellPos.x, cellPos.y, true);

                            if (!DoesOcclude(num2 - 1, num3, canSeeThroughTransparent) &&
                                !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
                                visiblePoints.Add(Grid.XYToCell(num2, num3));
                        }
                    }

                    num3++;
                }

                num3--;
                break;
            case Octant.S_SE:
                num3 = cellPos.y - depth;
                if (num3 < 0) return;

                num2 = cellPos.x + Convert.ToInt32(startSlope * Convert.ToDouble(depth));
                if (num2 >= Grid.WidthInCells) num2 = Grid.WidthInCells - 1;
                while (GetSlope(num2, num3, cellPos.x, cellPos.y, false) <= endSlope) {
                    if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num) {
                        if (DoesOcclude(num2, num3, canSeeThroughTransparent)) {
                            if (num2 + 1 < Grid.WidthInCells                               &&
                                !DoesOcclude(num2 + 1, num3,     canSeeThroughTransparent) &&
                                !DoesOcclude(num2 + 1, num3 + 1, canSeeThroughTransparent)) {
                                var slope2 = GetSlope(num2 + 0.5, num3 + 0.5, cellPos.x, cellPos.y, false);
                                ScanOctant(cellPos,
                                           range,
                                           depth + 1,
                                           octant,
                                           startSlope,
                                           slope2,
                                           visiblePoints,
                                           canSeeThroughTransparent);
                            }
                        } else {
                            if (num2 + 1 < Grid.WidthInCells && DoesOcclude(num2 + 1, num3, canSeeThroughTransparent))
                                startSlope = -GetSlope(num2 + 0.5, num3 - 0.5, cellPos.x, cellPos.y, false);

                            if (!DoesOcclude(num2, num3 + 1, canSeeThroughTransparent) &&
                                !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
                                visiblePoints.Add(Grid.XYToCell(num2, num3));
                        }
                    }

                    num2--;
                }

                num2++;
                break;
            case Octant.S_SW:
                num3 = cellPos.y - depth;
                if (num3 < 0) return;

                num2 = cellPos.x - Convert.ToInt32(startSlope * Convert.ToDouble(depth));
                if (num2 < 0) num2 = 0;
                while (GetSlope(num2, num3, cellPos.x, cellPos.y, false) >= endSlope) {
                    if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num) {
                        if (DoesOcclude(num2, num3, canSeeThroughTransparent)) {
                            if (num2 - 1 >= 0                                              &&
                                !DoesOcclude(num2 - 1, num3,     canSeeThroughTransparent) &&
                                !DoesOcclude(num2 - 1, num3 + 1, canSeeThroughTransparent)) {
                                var slope3 = GetSlope(num2 - 0.5, num3 + 0.5, cellPos.x, cellPos.y, false);
                                ScanOctant(cellPos,
                                           range,
                                           depth + 1,
                                           octant,
                                           startSlope,
                                           slope3,
                                           visiblePoints,
                                           canSeeThroughTransparent);
                            }
                        } else {
                            if (num2 - 1 >= 0 && DoesOcclude(num2 - 1, num3, canSeeThroughTransparent))
                                startSlope = GetSlope(num2 - 0.5, num3 - 0.5, cellPos.x, cellPos.y, false);

                            if (!DoesOcclude(num2, num3 + 1, canSeeThroughTransparent) &&
                                !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
                                visiblePoints.Add(Grid.XYToCell(num2, num3));
                        }
                    }

                    num2++;
                }

                num2--;
                break;
            case Octant.W_SW:
                num2 = cellPos.x - depth;
                if (num2 < 0) return;

                num3 = cellPos.y - Convert.ToInt32(startSlope * Convert.ToDouble(depth));
                if (num3 < 0) num3 = 0;
                while (GetSlope(num2, num3, cellPos.x, cellPos.y, true) >= endSlope) {
                    if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num) {
                        if (DoesOcclude(num2, num3, canSeeThroughTransparent)) {
                            if (num3 - 1 >= 0                                          &&
                                !DoesOcclude(num2, num3 - 1, canSeeThroughTransparent) &&
                                !DoesOcclude(num2       + 1, num3 - 1, canSeeThroughTransparent))
                                ScanOctant(cellPos,
                                           range,
                                           depth + 1,
                                           octant,
                                           startSlope,
                                           GetSlope(num2 + 0.5, num3 - 0.5, cellPos.x, cellPos.y, true),
                                           visiblePoints,
                                           canSeeThroughTransparent);
                        } else {
                            if (num3 - 1 >= 0 && DoesOcclude(num2, num3 - 1, canSeeThroughTransparent))
                                startSlope = GetSlope(num2 - 0.5, num3 - 0.5, cellPos.x, cellPos.y, true);

                            if (!DoesOcclude(num2 + 1, num3, canSeeThroughTransparent) &&
                                !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
                                visiblePoints.Add(Grid.XYToCell(num2, num3));
                        }
                    }

                    num3++;
                }

                num3--;
                break;
            case Octant.W_NW:
                num2 = cellPos.x - depth;
                if (num2 < 0) return;

                num3 = cellPos.y + Convert.ToInt32(startSlope * Convert.ToDouble(depth));
                if (num3 >= Grid.HeightInCells) num3 = Grid.HeightInCells - 1;
                while (GetSlope(num2, num3, cellPos.x, cellPos.y, true) <= endSlope) {
                    if (GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num) {
                        if (DoesOcclude(num2, num3, canSeeThroughTransparent)) {
                            if (num3 + 1 < Grid.HeightInCells                          &&
                                !DoesOcclude(num2, num3 + 1, canSeeThroughTransparent) &&
                                !DoesOcclude(num2       + 1, num3 + 1, canSeeThroughTransparent))
                                ScanOctant(cellPos,
                                           range,
                                           depth + 1,
                                           octant,
                                           startSlope,
                                           GetSlope(num2 + 0.5, num3 + 0.5, cellPos.x, cellPos.y, true),
                                           visiblePoints,
                                           canSeeThroughTransparent);
                        } else {
                            if (num3 + 1 < Grid.HeightInCells && DoesOcclude(num2, num3 + 1, canSeeThroughTransparent))
                                startSlope = -GetSlope(num2 - 0.5, num3 + 0.5, cellPos.x, cellPos.y, true);

                            if (!DoesOcclude(num2 + 1, num3, canSeeThroughTransparent) &&
                                !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
                                visiblePoints.Add(Grid.XYToCell(num2, num3));
                        }
                    }

                    num3--;
                }

                num3++;
                break;
        }

        if (num2 < 0)
            num2                                 = 0;
        else if (num2 >= Grid.WidthInCells) num2 = Grid.WidthInCells - 1;

        if (num3 < 0)
            num3                                  = 0;
        else if (num3 >= Grid.HeightInCells) num3 = Grid.HeightInCells - 1;

        if ((depth < range) & !DoesOcclude(num2, num3, canSeeThroughTransparent))
            ScanOctant(cellPos,
                       range,
                       depth + 1,
                       octant,
                       startSlope,
                       endSlope,
                       visiblePoints,
                       canSeeThroughTransparent);
    }

    private static double GetSlope(double pX1, double pY1, double pX2, double pY2, bool pInvert) {
        if (pInvert) return (pY1 - pY2) / (pX1 - pX2);

        return (pX1 - pX2) / (pY1 - pY2);
    }

    private static int GetVisDistance(int pX1, int pY1, int pX2, int pY2) {
        return (pX1 - pX2) * (pX1 - pX2) + (pY1 - pY2) * (pY1 - pY2);
    }
}