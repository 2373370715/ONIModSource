using System;

namespace Database
{
	// Token: 0x02002146 RID: 8518
	public class OrbitalTypeCategories : ResourceSet<OrbitalData>
	{
		// Token: 0x0600B592 RID: 46482 RVA: 0x00451D98 File Offset: 0x0044FF98
		public OrbitalTypeCategories(ResourceSet parent) : base("OrbitalTypeCategories", parent)
		{
			this.backgroundEarth = new OrbitalData("backgroundEarth", this, "earth_kanim", "", OrbitalData.OrbitalType.world, 1f, 0.5f, 0.95f, 10f, 10f, 1.05f, true, 0.05f, 25f, 1f);
			this.backgroundEarth.GetRenderZ = (() => Grid.GetLayerZ(Grid.SceneLayer.Background) + 0.9f);
			this.frozenOre = new OrbitalData("frozenOre", this, "starmap_frozen_ore_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1f, true, 0.05f, 25f, 1f);
			this.heliumCloud = new OrbitalData("heliumCloud", this, "starmap_helium_cloud_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.iceCloud = new OrbitalData("iceCloud", this, "starmap_ice_cloud_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.iceRock = new OrbitalData("iceRock", this, "starmap_ice_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.purpleGas = new OrbitalData("purpleGas", this, "starmap_purple_gas_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.radioactiveGas = new OrbitalData("radioactiveGas", this, "starmap_radioactive_gas_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.rocky = new OrbitalData("rocky", this, "starmap_rocky_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.gravitas = new OrbitalData("gravitas", this, "starmap_space_junk_kanim", "", OrbitalData.OrbitalType.poi, 1f, 0.5f, 0.5f, -350f, 350f, 1.05f, true, 0.05f, 25f, 1f);
			this.orbit = new OrbitalData("orbit", this, "starmap_orbit_kanim", "", OrbitalData.OrbitalType.inOrbit, 1f, 0.25f, 0.5f, -350f, 350f, 1.05f, false, 0.05f, 4f, 1f);
			this.landed = new OrbitalData("landed", this, "starmap_landed_surface_kanim", "", OrbitalData.OrbitalType.landed, 0f, 0.5f, 0.35f, -350f, 350f, 1.05f, false, 0.05f, 4f, 1f);
		}

		// Token: 0x0400935C RID: 37724
		public OrbitalData backgroundEarth;

		// Token: 0x0400935D RID: 37725
		public OrbitalData frozenOre;

		// Token: 0x0400935E RID: 37726
		public OrbitalData heliumCloud;

		// Token: 0x0400935F RID: 37727
		public OrbitalData iceCloud;

		// Token: 0x04009360 RID: 37728
		public OrbitalData iceRock;

		// Token: 0x04009361 RID: 37729
		public OrbitalData purpleGas;

		// Token: 0x04009362 RID: 37730
		public OrbitalData radioactiveGas;

		// Token: 0x04009363 RID: 37731
		public OrbitalData rocky;

		// Token: 0x04009364 RID: 37732
		public OrbitalData gravitas;

		// Token: 0x04009365 RID: 37733
		public OrbitalData orbit;

		// Token: 0x04009366 RID: 37734
		public OrbitalData landed;
	}
}
