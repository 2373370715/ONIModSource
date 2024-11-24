using System;
using STRINGS;
using UnityEngine;

// Token: 0x020013EE RID: 5102
public class HighEnergyParticleConfig : IEntityConfig
{
	// Token: 0x060068B9 RID: 26809 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060068BA RID: 26810 RVA: 0x002D834C File Offset: 0x002D654C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateBasicEntity("HighEnergyParticle", ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, ITEMS.RADIATION.HIGHENERGYPARITCLE.DESC, 1f, false, Assets.GetAnim("spark_radial_high_energy_particles_kanim"), "travel_pre", Grid.SceneLayer.FXFront2, SimHashes.Creature, null, 293f);
		EntityTemplates.AddCollision(gameObject, EntityTemplates.CollisionShape.CIRCLE, 0.2f, 0.2f);
		gameObject.AddOrGet<LoopingSounds>();
		RadiationEmitter radiationEmitter = gameObject.AddOrGet<RadiationEmitter>();
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.radiusProportionalToRads = false;
		radiationEmitter.emitRadiusX = 3;
		radiationEmitter.emitRadiusY = 3;
		radiationEmitter.emitRads = 0.4f * ((float)radiationEmitter.emitRadiusX / 6f);
		gameObject.AddComponent<HighEnergyParticle>().speed = 8f;
		return gameObject;
	}

	// Token: 0x060068BB RID: 26811 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060068BC RID: 26812 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04004EFF RID: 20223
	public const int PARTICLE_SPEED = 8;

	// Token: 0x04004F00 RID: 20224
	public const float PARTICLE_COLLISION_SIZE = 0.2f;

	// Token: 0x04004F01 RID: 20225
	public const float PER_CELL_FALLOFF = 0.1f;

	// Token: 0x04004F02 RID: 20226
	public const float FALLOUT_RATIO = 0.5f;

	// Token: 0x04004F03 RID: 20227
	public const int MAX_PAYLOAD = 500;

	// Token: 0x04004F04 RID: 20228
	public const int EXPLOSION_FALLOUT_TEMPERATURE = 5000;

	// Token: 0x04004F05 RID: 20229
	public const float EXPLOSION_FALLOUT_MASS_PER_PARTICLE = 0.001f;

	// Token: 0x04004F06 RID: 20230
	public const float EXPLOSION_EMIT_DURRATION = 1f;

	// Token: 0x04004F07 RID: 20231
	public const short EXPLOSION_EMIT_RADIUS = 6;

	// Token: 0x04004F08 RID: 20232
	public const string ID = "HighEnergyParticle";
}
