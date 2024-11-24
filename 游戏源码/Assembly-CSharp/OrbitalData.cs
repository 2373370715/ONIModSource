using System;

// Token: 0x02000B8A RID: 2954
public class OrbitalData : Resource
{
	// Token: 0x0600386F RID: 14447 RVA: 0x0021B030 File Offset: 0x00219230
	public OrbitalData(string id, ResourceSet parent, string animFile = "earth_kanim", string initialAnim = "", OrbitalData.OrbitalType orbitalType = OrbitalData.OrbitalType.poi, float periodInCycles = 1f, float xGridPercent = 0.5f, float yGridPercent = 0.5f, float minAngle = -350f, float maxAngle = 350f, float radiusScale = 1.05f, bool rotatesBehind = true, float behindZ = 0.05f, float distance = 25f, float renderZ = 1f) : base(id, parent, null)
	{
		this.animFile = animFile;
		this.initialAnim = initialAnim;
		this.orbitalType = orbitalType;
		this.periodInCycles = periodInCycles;
		this.xGridPercent = xGridPercent;
		this.yGridPercent = yGridPercent;
		this.minAngle = minAngle;
		this.maxAngle = maxAngle;
		this.radiusScale = radiusScale;
		this.rotatesBehind = rotatesBehind;
		this.behindZ = behindZ;
		this.distance = distance;
		this.renderZ = renderZ;
	}

	// Token: 0x04002676 RID: 9846
	public string animFile;

	// Token: 0x04002677 RID: 9847
	public string initialAnim;

	// Token: 0x04002678 RID: 9848
	public float periodInCycles;

	// Token: 0x04002679 RID: 9849
	public float xGridPercent;

	// Token: 0x0400267A RID: 9850
	public float yGridPercent;

	// Token: 0x0400267B RID: 9851
	public float minAngle;

	// Token: 0x0400267C RID: 9852
	public float maxAngle;

	// Token: 0x0400267D RID: 9853
	public float radiusScale;

	// Token: 0x0400267E RID: 9854
	public bool rotatesBehind;

	// Token: 0x0400267F RID: 9855
	public float behindZ;

	// Token: 0x04002680 RID: 9856
	public float distance;

	// Token: 0x04002681 RID: 9857
	public float renderZ;

	// Token: 0x04002682 RID: 9858
	public OrbitalData.OrbitalType orbitalType;

	// Token: 0x04002683 RID: 9859
	public Func<float> GetRenderZ;

	// Token: 0x02000B8B RID: 2955
	public enum OrbitalType
	{
		// Token: 0x04002685 RID: 9861
		world,
		// Token: 0x04002686 RID: 9862
		poi,
		// Token: 0x04002687 RID: 9863
		inOrbit,
		// Token: 0x04002688 RID: 9864
		landed
	}
}
