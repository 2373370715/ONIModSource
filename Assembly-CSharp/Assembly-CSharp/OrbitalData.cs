using System;

public class OrbitalData : Resource {
    public enum OrbitalType {
        world,
        poi,
        inOrbit,
        landed
    }

    public string      animFile;
    public float       behindZ;
    public float       distance;
    public Func<float> GetRenderZ;
    public string      initialAnim;
    public float       maxAngle;
    public float       minAngle;
    public OrbitalType orbitalType;
    public float       periodInCycles;
    public float       radiusScale;
    public float       renderZ;
    public bool        rotatesBehind;
    public float       xGridPercent;
    public float       yGridPercent;

    public OrbitalData(string      id,
                       ResourceSet parent,
                       string      animFile       = "earth_kanim",
                       string      initialAnim    = "",
                       OrbitalType orbitalType    = OrbitalType.poi,
                       float       periodInCycles = 1f,
                       float       xGridPercent   = 0.5f,
                       float       yGridPercent   = 0.5f,
                       float       minAngle       = -350f,
                       float       maxAngle       = 350f,
                       float       radiusScale    = 1.05f,
                       bool        rotatesBehind  = true,
                       float       behindZ        = 0.05f,
                       float       distance       = 25f,
                       float       renderZ        = 1f) : base(id, parent) {
        this.animFile       = animFile;
        this.initialAnim    = initialAnim;
        this.orbitalType    = orbitalType;
        this.periodInCycles = periodInCycles;
        this.xGridPercent   = xGridPercent;
        this.yGridPercent   = yGridPercent;
        this.minAngle       = minAngle;
        this.maxAngle       = maxAngle;
        this.radiusScale    = radiusScale;
        this.rotatesBehind  = rotatesBehind;
        this.behindZ        = behindZ;
        this.distance       = distance;
        this.renderZ        = renderZ;
    }
}