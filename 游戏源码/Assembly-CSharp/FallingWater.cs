using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Database;
using FMOD.Studio;
using FMODUnity;
using KSerialization;
using UnityEngine;

// Token: 0x020012E2 RID: 4834
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/FallingWater")]
public class FallingWater : KMonoBehaviour, ISim200ms
{
	// Token: 0x17000630 RID: 1584
	// (get) Token: 0x0600632D RID: 25389 RVA: 0x000E0C35 File Offset: 0x000DEE35
	// (set) Token: 0x0600632E RID: 25390 RVA: 0x000A5E40 File Offset: 0x000A4040
	public static FallingWater instance
	{
		get
		{
			return FallingWater._instance;
		}
		private set
		{
		}
	}

	// Token: 0x0600632F RID: 25391 RVA: 0x000E0C3C File Offset: 0x000DEE3C
	public static void DestroyInstance()
	{
		FallingWater._instance = null;
	}

	// Token: 0x06006330 RID: 25392 RVA: 0x000E0C44 File Offset: 0x000DEE44
	protected override void OnPrefabInit()
	{
		FallingWater._instance = this;
		base.OnPrefabInit();
		this.mistEffect.SetActive(false);
		this.mistPool = new GameObjectPool(new Func<GameObject>(this.InstantiateMist), 16);
	}

	// Token: 0x06006331 RID: 25393 RVA: 0x002B9084 File Offset: 0x002B7284
	protected override void OnSpawn()
	{
		this.mesh = new Mesh();
		this.mesh.MarkDynamic();
		this.mesh.name = "FallingWater";
		this.lastSpawnTime = new float[Grid.WidthInCells * Grid.HeightInCells];
		for (int i = 0; i < this.lastSpawnTime.Length; i++)
		{
			this.lastSpawnTime[i] = 0f;
		}
		this.propertyBlock = new MaterialPropertyBlock();
		this.propertyBlock.SetTexture("_MainTex", this.texture);
		this.uvFrameSize = new Vector2(1f / (float)this.numFrames, 1f);
	}

	// Token: 0x06006332 RID: 25394 RVA: 0x000E0C77 File Offset: 0x000DEE77
	protected override void OnCleanUp()
	{
		FallingWater.instance = null;
		base.OnCleanUp();
	}

	// Token: 0x06006333 RID: 25395 RVA: 0x000E0C85 File Offset: 0x000DEE85
	private float GetTime()
	{
		return Time.time % 360f;
	}

	// Token: 0x06006334 RID: 25396 RVA: 0x002B912C File Offset: 0x002B732C
	public void AddParticle(int cell, ushort elementIdx, float base_mass, float temperature, byte disease_idx, int base_disease_count, bool skip_sound = false, bool skip_decor = false, bool debug_track = false, bool disable_randomness = false)
	{
		Vector2 root_pos = Grid.CellToPos2D(cell);
		this.AddParticle(root_pos, elementIdx, base_mass, temperature, disease_idx, base_disease_count, skip_sound, skip_decor, debug_track, disable_randomness);
	}

	// Token: 0x06006335 RID: 25397 RVA: 0x002B915C File Offset: 0x002B735C
	public void AddParticle(Vector2 root_pos, ushort elementIdx, float base_mass, float temperature, byte disease_idx, int base_disease_count, bool skip_sound = false, bool skip_decor = false, bool debug_track = false, bool disable_randomness = false)
	{
		int num = Grid.PosToCell(root_pos);
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		if (temperature <= 0f || base_mass <= 0f)
		{
			global::Debug.LogError(string.Format("Unexpected water mass/temperature values added to the falling water manager T({0}) M({1})", temperature, base_mass));
		}
		float time = this.GetTime();
		if (!skip_sound)
		{
			FallingWater.SoundInfo soundInfo;
			if (!this.topSounds.TryGetValue(num, out soundInfo))
			{
				soundInfo = default(FallingWater.SoundInfo);
				soundInfo.handle = LoopingSoundManager.StartSound(this.liquid_top_loop, root_pos, true, true);
			}
			soundInfo.startTime = time;
			LoopingSoundManager.Get().UpdateSecondParameter(soundInfo.handle, FallingWater.HASH_LIQUIDVOLUME, SoundUtil.GetLiquidVolume(base_mass));
			this.topSounds[num] = soundInfo;
		}
		int num2 = base_disease_count;
		while (base_mass > 0f)
		{
			float num3 = UnityEngine.Random.value * 2f * this.particleMassVariation - this.particleMassVariation;
			float num4 = Mathf.Max(0f, Mathf.Min(base_mass, this.particleMassToSplit + num3));
			float num5 = num4 / base_mass;
			base_mass -= num4;
			int num6 = (int)(num5 * (float)base_disease_count);
			num6 = Mathf.Min(num2, num6);
			num2 = Mathf.Max(0, num2 - num6);
			int frame = UnityEngine.Random.Range(0, this.numFrames);
			Vector2 b = disable_randomness ? Vector2.zero : new Vector2(this.jitterStep * Mathf.Sin(this.offset), this.jitterStep * Mathf.Sin(this.offset + 17f));
			Vector2 b2 = disable_randomness ? Vector2.zero : new Vector2(UnityEngine.Random.Range(-this.multipleOffsetRange.x, this.multipleOffsetRange.x), UnityEngine.Random.Range(-this.multipleOffsetRange.y, this.multipleOffsetRange.y));
			Element element = ElementLoader.elements[(int)elementIdx];
			Vector2 vector = root_pos;
			bool flag = !skip_decor && this.SpawnLiquidTopDecor(time, Grid.CellLeft(num), false, element);
			bool flag2 = !skip_decor && this.SpawnLiquidTopDecor(time, Grid.CellRight(num), true, element);
			Vector2 vector2 = Vector2.ClampMagnitude(this.initialOffset + b + b2, 1f);
			if (flag || flag2)
			{
				if (flag && flag2)
				{
					vector += vector2;
					vector.x += 0.5f;
				}
				else if (flag)
				{
					vector += vector2;
				}
				else
				{
					vector.x += 1f - vector2.x;
					vector.y += vector2.y;
				}
			}
			else
			{
				vector += vector2;
				vector.x += 0.5f;
			}
			int num7 = Grid.PosToCell(vector);
			if ((Grid.Element[num7].state & Element.State.Solid) == Element.State.Solid || (Grid.Properties[num7] & 2) != 0)
			{
				vector.y = Mathf.Floor(vector.y + 1f);
			}
			this.physics.Add(new FallingWater.ParticlePhysics(vector, Vector2.zero, frame, elementIdx, (int)Grid.WorldIdx[num]));
			this.particleProperties.Add(new FallingWater.ParticleProperties(elementIdx, num4, temperature, disease_idx, num6, debug_track));
		}
	}

	// Token: 0x06006336 RID: 25398 RVA: 0x002B947C File Offset: 0x002B767C
	private bool SpawnLiquidTopDecor(float time, int cell, bool flip, Element element)
	{
		if (Grid.IsValidCell(cell) && Grid.Element[cell] == element)
		{
			Vector3 vector = Grid.CellToPosCBC(cell, Grid.SceneLayer.TileMain);
			if (CameraController.Instance.IsVisiblePos(vector))
			{
				Pair<int, bool> key = new Pair<int, bool>(cell, flip);
				FallingWater.MistInfo mistInfo;
				if (!this.mistAlive.TryGetValue(key, out mistInfo))
				{
					mistInfo = default(FallingWater.MistInfo);
					mistInfo.fx = this.SpawnMist();
					mistInfo.fx.TintColour = element.substance.colour;
					Vector3 position = vector + (flip ? (-Vector3.right) : Vector3.right) * 0.5f;
					mistInfo.fx.transform.SetPosition(position);
					mistInfo.fx.FlipX = flip;
				}
				mistInfo.deathTime = Time.time + this.mistEffectMinAliveTime;
				this.mistAlive[key] = mistInfo;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006337 RID: 25399 RVA: 0x002B9568 File Offset: 0x002B7768
	public void SpawnLiquidSplash(float x, int cell, bool forceSplash = false)
	{
		float time = this.GetTime();
		float num = this.lastSpawnTime[cell];
		if (time - num >= this.minSpawnDelay || forceSplash)
		{
			this.lastSpawnTime[cell] = time;
			Vector2 a = Grid.CellToPos2D(cell);
			a.x = x - 0.5f;
			int num2 = UnityEngine.Random.Range(0, this.liquid_splash.names.Length);
			Vector2 vector = a + new Vector2(this.liquid_splash.offset.x, this.liquid_splash.offset.y);
			SpriteSheetAnimManager.instance.Play(this.liquid_splash.names[num2], new Vector3(vector.x, vector.y, this.renderOffset.z), new Vector2(this.liquid_splash.size.x, this.liquid_splash.size.y), Color.white);
		}
	}

	// Token: 0x06006338 RID: 25400 RVA: 0x002B9660 File Offset: 0x002B7860
	public void UpdateParticles(float dt)
	{
		if (dt <= 0f || this.simUpdateDelay >= 0)
		{
			return;
		}
		this.offset = (this.offset + dt) % 360f;
		int count = this.physics.Count;
		Vector2 b = Physics.gravity * dt * this.gravityScale;
		for (int i = 0; i < count; i++)
		{
			FallingWater.ParticlePhysics particlePhysics = this.physics[i];
			Vector3 vector = particlePhysics.position;
			int num;
			int num2;
			Grid.PosToXY(vector, out num, out num2);
			particlePhysics.velocity += b;
			Vector3 b2 = particlePhysics.velocity * dt;
			Vector3 v = vector + b2;
			particlePhysics.position = v;
			this.physics[i] = particlePhysics;
			int num3;
			int num4;
			Grid.PosToXY(particlePhysics.position, out num3, out num4);
			int num5 = (num2 > num4) ? num2 : num4;
			int num6 = (num2 > num4) ? num4 : num2;
			int j = num5;
			while (j >= num6)
			{
				int num7 = j * Grid.WidthInCells + num;
				int cell = (j + 1) * Grid.WidthInCells + num;
				if (Grid.IsValidCell(num7) && (int)Grid.WorldIdx[num7] != particlePhysics.worldIdx)
				{
					this.RemoveParticle(i, ref count);
					break;
				}
				if (Grid.IsValidCell(num7))
				{
					Element element = Grid.Element[num7];
					Element.State state = element.state & Element.State.Solid;
					bool flag = false;
					if (state == Element.State.Solid || (Grid.Properties[num7] & 2) != 0)
					{
						this.AddToSim(cell, i, ref count);
					}
					else
					{
						switch (state)
						{
						case Element.State.Vacuum:
							if (element.id == SimHashes.Vacuum)
							{
								flag = true;
							}
							else
							{
								this.RemoveParticle(i, ref count);
							}
							break;
						case Element.State.Gas:
							flag = true;
							break;
						case Element.State.Liquid:
						{
							FallingWater.ParticleProperties particleProperties = this.particleProperties[i];
							Element element2 = ElementLoader.elements[(int)particleProperties.elementIdx];
							if (element2.id == element.id)
							{
								if (Grid.Mass[num7] <= element.defaultValues.mass)
								{
									flag = true;
								}
								else
								{
									this.SpawnLiquidSplash(particlePhysics.position.x, cell, false);
									this.AddToSim(num7, i, ref count);
								}
							}
							else if (element2.molarMass > element.molarMass)
							{
								flag = true;
							}
							else
							{
								this.SpawnLiquidSplash(particlePhysics.position.x, cell, false);
								this.AddToSim(cell, i, ref count);
							}
							break;
						}
						}
					}
					if (!flag)
					{
						break;
					}
					j--;
				}
				else
				{
					if (Grid.IsValidCell(cell))
					{
						this.SpawnLiquidSplash(particlePhysics.position.x, cell, false);
						this.AddToSim(cell, i, ref count);
						break;
					}
					this.RemoveParticle(i, ref count);
					break;
				}
			}
		}
		float time = this.GetTime();
		this.UpdateSounds(time);
		this.UpdateMistFX(Time.time);
	}

	// Token: 0x06006339 RID: 25401 RVA: 0x002B9950 File Offset: 0x002B7B50
	private void UpdateMistFX(float t)
	{
		this.mistClearList.Clear();
		foreach (KeyValuePair<Pair<int, bool>, FallingWater.MistInfo> keyValuePair in this.mistAlive)
		{
			if (t > keyValuePair.Value.deathTime)
			{
				keyValuePair.Value.fx.Play("end", KAnim.PlayMode.Once, 1f, 0f);
				this.mistClearList.Add(keyValuePair.Key);
			}
		}
		foreach (Pair<int, bool> key in this.mistClearList)
		{
			this.mistAlive.Remove(key);
		}
		this.mistClearList.Clear();
	}

	// Token: 0x0600633A RID: 25402 RVA: 0x002B9A44 File Offset: 0x002B7C44
	private void UpdateSounds(float t)
	{
		this.clearList.Clear();
		foreach (KeyValuePair<int, FallingWater.SoundInfo> keyValuePair in this.topSounds)
		{
			FallingWater.SoundInfo value = keyValuePair.Value;
			if (t - value.startTime >= this.stopTopLoopDelay)
			{
				if (value.handle != HandleVector<int>.InvalidHandle)
				{
					LoopingSoundManager.StopSound(value.handle);
				}
				this.clearList.Add(keyValuePair.Key);
			}
		}
		foreach (int key in this.clearList)
		{
			this.topSounds.Remove(key);
		}
		this.clearList.Clear();
		foreach (KeyValuePair<int, FallingWater.SoundInfo> keyValuePair2 in this.splashSounds)
		{
			FallingWater.SoundInfo value2 = keyValuePair2.Value;
			if (t - value2.startTime >= this.stopSplashLoopDelay)
			{
				if (value2.handle != HandleVector<int>.InvalidHandle)
				{
					LoopingSoundManager.StopSound(value2.handle);
				}
				this.clearList.Add(keyValuePair2.Key);
			}
		}
		foreach (int key2 in this.clearList)
		{
			this.splashSounds.Remove(key2);
		}
		this.clearList.Clear();
	}

	// Token: 0x0600633B RID: 25403 RVA: 0x002B9C14 File Offset: 0x002B7E14
	public Dictionary<int, float> GetInfo(int cell)
	{
		Dictionary<int, float> dictionary = new Dictionary<int, float>();
		int count = this.physics.Count;
		for (int i = 0; i < count; i++)
		{
			if (Grid.PosToCell(this.physics[i].position) == cell)
			{
				FallingWater.ParticleProperties particleProperties = this.particleProperties[i];
				float num = 0f;
				dictionary.TryGetValue((int)particleProperties.elementIdx, out num);
				num += particleProperties.mass;
				dictionary[(int)particleProperties.elementIdx] = num;
			}
		}
		return dictionary;
	}

	// Token: 0x0600633C RID: 25404 RVA: 0x000E0C92 File Offset: 0x000DEE92
	private float GetParticleVolume(float mass)
	{
		return Mathf.Clamp01((mass - (this.particleMassToSplit - this.particleMassVariation)) / (2f * this.particleMassVariation));
	}

	// Token: 0x0600633D RID: 25405 RVA: 0x002B9C98 File Offset: 0x002B7E98
	private void AddToSim(int cell, int particleIdx, ref int num_particles)
	{
		bool flag = false;
		for (;;)
		{
			if ((Grid.Element[cell].state & Element.State.Solid) == Element.State.Solid || (Grid.Properties[cell] & 2) != 0)
			{
				cell += Grid.WidthInCells;
				if (!Grid.IsValidCell(cell))
				{
					break;
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				goto Block_3;
			}
		}
		return;
		Block_3:
		FallingWater.ParticleProperties particleProperties = this.particleProperties[particleIdx];
		SimMessages.AddRemoveSubstance(cell, particleProperties.elementIdx, CellEventLogger.Instance.FallingWaterAddToSim, particleProperties.mass, particleProperties.temperature, particleProperties.diseaseIdx, particleProperties.diseaseCount, true, -1);
		this.RemoveParticle(particleIdx, ref num_particles);
		float time = this.GetTime();
		float num = this.lastSpawnTime[cell];
		if (time - num >= this.minSpawnDelay)
		{
			this.lastSpawnTime[cell] = time;
			Vector3 vector = Grid.CellToPosCCC(cell, Grid.SceneLayer.TileMain);
			vector.z = 0f;
			if (CameraController.Instance.IsAudibleSound(vector))
			{
				bool flag2 = true;
				FallingWater.SoundInfo soundInfo;
				if (this.splashSounds.TryGetValue(cell, out soundInfo))
				{
					soundInfo.splashCount++;
					if (soundInfo.splashCount > this.splashCountLoopThreshold)
					{
						if (soundInfo.handle == HandleVector<int>.InvalidHandle)
						{
							soundInfo.handle = LoopingSoundManager.StartSound(this.liquid_splash_loop, vector, true, true);
						}
						LoopingSoundManager.Get().UpdateFirstParameter(soundInfo.handle, FallingWater.HASH_LIQUIDDEPTH, SoundUtil.GetLiquidDepth(cell));
						LoopingSoundManager.Get().UpdateSecondParameter(soundInfo.handle, FallingWater.HASH_LIQUIDVOLUME, this.GetParticleVolume(particleProperties.mass));
						flag2 = false;
					}
				}
				else
				{
					soundInfo = default(FallingWater.SoundInfo);
					soundInfo.handle = HandleVector<int>.InvalidHandle;
				}
				soundInfo.startTime = time;
				this.splashSounds[cell] = soundInfo;
				if (flag2)
				{
					EventInstance instance = SoundEvent.BeginOneShot(this.liquid_splash_initial, vector, 1f, false);
					instance.setParameterByName("liquidDepth", SoundUtil.GetLiquidDepth(cell), false);
					instance.setParameterByName("liquidVolume", this.GetParticleVolume(particleProperties.mass), false);
					SoundEvent.EndOneShot(instance);
				}
			}
		}
	}

	// Token: 0x0600633E RID: 25406 RVA: 0x002B9E90 File Offset: 0x002B8090
	private void RemoveParticle(int particleIdx, ref int num_particles)
	{
		num_particles--;
		this.physics[particleIdx] = this.physics[num_particles];
		this.particleProperties[particleIdx] = this.particleProperties[num_particles];
		this.physics.RemoveAt(num_particles);
		this.particleProperties.RemoveAt(num_particles);
	}

	// Token: 0x0600633F RID: 25407 RVA: 0x002B9EF0 File Offset: 0x002B80F0
	public void Render()
	{
		List<Vector3> vertices = MeshUtil.vertices;
		List<Color32> colours = MeshUtil.colours32;
		List<Vector2> uvs = MeshUtil.uvs;
		List<int> indices = MeshUtil.indices;
		uvs.Clear();
		vertices.Clear();
		indices.Clear();
		colours.Clear();
		float num = this.particleSize.x * 0.5f;
		float num2 = this.particleSize.y * 0.5f;
		Vector2 a = new Vector2(-num, -num2);
		Vector2 a2 = new Vector2(num, -num2);
		Vector2 a3 = new Vector2(num, num2);
		Vector2 a4 = new Vector2(-num, num2);
		float y = 1f;
		float y2 = 0f;
		int num3 = Mathf.Min(this.physics.Count, 16249);
		if (num3 < this.physics.Count)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Too many water particles to render. Wanted",
				this.physics.Count,
				"but truncating to limit"
			});
		}
		for (int i = 0; i < num3; i++)
		{
			Vector2 position = this.physics[i].position;
			float d = Mathf.Lerp(0.25f, 1f, Mathf.Clamp01(this.particleProperties[i].mass / this.particleMassToSplit));
			vertices.Add(position + a * d);
			vertices.Add(position + a2 * d);
			vertices.Add(position + a3 * d);
			vertices.Add(position + a4 * d);
			int frame = this.physics[i].frame;
			float x = (float)frame * this.uvFrameSize.x;
			float x2 = (float)(frame + 1) * this.uvFrameSize.x;
			uvs.Add(new Vector2(x, y2));
			uvs.Add(new Vector2(x2, y2));
			uvs.Add(new Vector2(x2, y));
			uvs.Add(new Vector2(x, y));
			Color32 colour = this.physics[i].colour;
			colours.Add(colour);
			colours.Add(colour);
			colours.Add(colour);
			colours.Add(colour);
			int num4 = i * 4;
			indices.Add(num4);
			indices.Add(num4 + 1);
			indices.Add(num4 + 2);
			indices.Add(num4);
			indices.Add(num4 + 2);
			indices.Add(num4 + 3);
		}
		this.mesh.Clear();
		this.mesh.SetVertices(vertices);
		this.mesh.SetUVs(0, uvs);
		this.mesh.SetColors(colours);
		this.mesh.SetTriangles(indices, 0);
		int layer = LayerMask.NameToLayer("Water");
		Vector4 value = PropertyTextures.CalculateClusterWorldSize();
		this.material.SetVector("_ClusterWorldSizeInfo", value);
		Graphics.DrawMesh(this.mesh, this.renderOffset, Quaternion.identity, this.material, layer, null, 0, this.propertyBlock);
	}

	// Token: 0x06006340 RID: 25408 RVA: 0x000E0CB5 File Offset: 0x000DEEB5
	private KBatchedAnimController SpawnMist()
	{
		GameObject instance = this.mistPool.GetInstance();
		instance.SetActive(true);
		KBatchedAnimController component = instance.GetComponent<KBatchedAnimController>();
		component.Play("loop", KAnim.PlayMode.Loop, 1f, 0f);
		return component;
	}

	// Token: 0x06006341 RID: 25409 RVA: 0x000E0CE9 File Offset: 0x000DEEE9
	private GameObject InstantiateMist()
	{
		GameObject gameObject = GameUtil.KInstantiate(this.mistEffect, Grid.SceneLayer.BuildingBack, null, 0);
		gameObject.SetActive(false);
		gameObject.GetComponent<KBatchedAnimController>().onDestroySelf = new Action<GameObject>(this.ReleaseMist);
		return gameObject;
	}

	// Token: 0x06006342 RID: 25410 RVA: 0x000E0D18 File Offset: 0x000DEF18
	private void ReleaseMist(GameObject go)
	{
		go.SetActive(false);
		this.mistPool.ReleaseInstance(go);
	}

	// Token: 0x06006343 RID: 25411 RVA: 0x000E0D2D File Offset: 0x000DEF2D
	public void Sim200ms(float dt)
	{
		if (this.simUpdateDelay >= 0)
		{
			this.simUpdateDelay--;
			return;
		}
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06006344 RID: 25412 RVA: 0x002BA214 File Offset: 0x002B8414
	[OnSerializing]
	private void OnSerializing()
	{
		List<Element> elements = ElementLoader.elements;
		Diseases diseases = Db.Get().Diseases;
		this.serializedParticleProperties = new List<FallingWater.SerializedParticleProperties>();
		foreach (FallingWater.ParticleProperties particleProperties in this.particleProperties)
		{
			FallingWater.SerializedParticleProperties item = default(FallingWater.SerializedParticleProperties);
			item.elementID = elements[(int)particleProperties.elementIdx].id;
			item.diseaseID = ((particleProperties.diseaseIdx != byte.MaxValue) ? diseases[(int)particleProperties.diseaseIdx].IdHash : HashedString.Invalid);
			item.mass = particleProperties.mass;
			item.temperature = particleProperties.temperature;
			item.diseaseCount = particleProperties.diseaseCount;
			this.serializedParticleProperties.Add(item);
		}
	}

	// Token: 0x06006345 RID: 25413 RVA: 0x000E0D52 File Offset: 0x000DEF52
	[OnSerialized]
	private void OnSerialized()
	{
		this.serializedParticleProperties = null;
	}

	// Token: 0x06006346 RID: 25414 RVA: 0x002BA304 File Offset: 0x002B8504
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 26))
		{
			for (int i = 0; i < this.physics.Count; i++)
			{
				int num = Grid.PosToCell(this.physics[i].position);
				if (Grid.IsValidCell(num))
				{
					FallingWater.ParticlePhysics value = this.physics[i];
					value.worldIdx = (int)Grid.WorldIdx[num];
					this.physics[i] = value;
				}
			}
		}
		if (this.serializedParticleProperties != null)
		{
			Diseases diseases = Db.Get().Diseases;
			this.particleProperties.Clear();
			using (List<FallingWater.SerializedParticleProperties>.Enumerator enumerator = this.serializedParticleProperties.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FallingWater.SerializedParticleProperties serializedParticleProperties = enumerator.Current;
					FallingWater.ParticleProperties item = default(FallingWater.ParticleProperties);
					item.elementIdx = ElementLoader.GetElementIndex(serializedParticleProperties.elementID);
					item.diseaseIdx = ((serializedParticleProperties.diseaseID != HashedString.Invalid) ? diseases.GetIndex(serializedParticleProperties.diseaseID) : byte.MaxValue);
					item.mass = serializedParticleProperties.mass;
					item.temperature = serializedParticleProperties.temperature;
					item.diseaseCount = serializedParticleProperties.diseaseCount;
					this.particleProperties.Add(item);
				}
				goto IL_15A;
			}
		}
		this.particleProperties = this.properties;
		IL_15A:
		this.properties = null;
	}

	// Token: 0x040046C2 RID: 18114
	private const float STATE_TRANSITION_TEMPERATURE_BUFER = 3f;

	// Token: 0x040046C3 RID: 18115
	private const byte FORCED_ALPHA = 191;

	// Token: 0x040046C4 RID: 18116
	private int simUpdateDelay = 2;

	// Token: 0x040046C5 RID: 18117
	[SerializeField]
	private Vector2 particleSize;

	// Token: 0x040046C6 RID: 18118
	[SerializeField]
	private Vector2 initialOffset;

	// Token: 0x040046C7 RID: 18119
	[SerializeField]
	private float jitterStep;

	// Token: 0x040046C8 RID: 18120
	[SerializeField]
	private Vector3 renderOffset;

	// Token: 0x040046C9 RID: 18121
	[SerializeField]
	private float minSpawnDelay;

	// Token: 0x040046CA RID: 18122
	[SerializeField]
	private float gravityScale = 0.05f;

	// Token: 0x040046CB RID: 18123
	[SerializeField]
	private float particleMassToSplit = 75f;

	// Token: 0x040046CC RID: 18124
	[SerializeField]
	private float particleMassVariation = 15f;

	// Token: 0x040046CD RID: 18125
	[SerializeField]
	private Vector2 multipleOffsetRange;

	// Token: 0x040046CE RID: 18126
	[SerializeField]
	private GameObject mistEffect;

	// Token: 0x040046CF RID: 18127
	[SerializeField]
	private float mistEffectMinAliveTime = 2f;

	// Token: 0x040046D0 RID: 18128
	[SerializeField]
	private Material material;

	// Token: 0x040046D1 RID: 18129
	[SerializeField]
	private Texture2D texture;

	// Token: 0x040046D2 RID: 18130
	[SerializeField]
	private int numFrames;

	// Token: 0x040046D3 RID: 18131
	[SerializeField]
	private FallingWater.DecorInfo liquid_splash;

	// Token: 0x040046D4 RID: 18132
	[SerializeField]
	private EventReference liquid_top_loop;

	// Token: 0x040046D5 RID: 18133
	[SerializeField]
	private EventReference liquid_splash_initial;

	// Token: 0x040046D6 RID: 18134
	[SerializeField]
	private EventReference liquid_splash_loop;

	// Token: 0x040046D7 RID: 18135
	[SerializeField]
	private float stopTopLoopDelay = 0.2f;

	// Token: 0x040046D8 RID: 18136
	[SerializeField]
	private float stopSplashLoopDelay = 1f;

	// Token: 0x040046D9 RID: 18137
	[SerializeField]
	private int splashCountLoopThreshold = 10;

	// Token: 0x040046DA RID: 18138
	[Serialize]
	private List<FallingWater.ParticlePhysics> physics = new List<FallingWater.ParticlePhysics>();

	// Token: 0x040046DB RID: 18139
	private List<FallingWater.ParticleProperties> particleProperties = new List<FallingWater.ParticleProperties>();

	// Token: 0x040046DC RID: 18140
	[Serialize]
	private List<FallingWater.SerializedParticleProperties> serializedParticleProperties;

	// Token: 0x040046DD RID: 18141
	[Serialize]
	private List<FallingWater.ParticleProperties> properties = new List<FallingWater.ParticleProperties>();

	// Token: 0x040046DE RID: 18142
	private Dictionary<int, FallingWater.SoundInfo> topSounds = new Dictionary<int, FallingWater.SoundInfo>();

	// Token: 0x040046DF RID: 18143
	private Dictionary<int, FallingWater.SoundInfo> splashSounds = new Dictionary<int, FallingWater.SoundInfo>();

	// Token: 0x040046E0 RID: 18144
	private GameObjectPool mistPool;

	// Token: 0x040046E1 RID: 18145
	private Mesh mesh;

	// Token: 0x040046E2 RID: 18146
	private float offset;

	// Token: 0x040046E3 RID: 18147
	private float[] lastSpawnTime;

	// Token: 0x040046E4 RID: 18148
	private Dictionary<Pair<int, bool>, FallingWater.MistInfo> mistAlive = new Dictionary<Pair<int, bool>, FallingWater.MistInfo>();

	// Token: 0x040046E5 RID: 18149
	private Vector2 uvFrameSize;

	// Token: 0x040046E6 RID: 18150
	private MaterialPropertyBlock propertyBlock;

	// Token: 0x040046E7 RID: 18151
	private static FallingWater _instance;

	// Token: 0x040046E8 RID: 18152
	private List<int> clearList = new List<int>();

	// Token: 0x040046E9 RID: 18153
	private List<Pair<int, bool>> mistClearList = new List<Pair<int, bool>>();

	// Token: 0x040046EA RID: 18154
	private static HashedString HASH_LIQUIDDEPTH = "liquidDepth";

	// Token: 0x040046EB RID: 18155
	private static HashedString HASH_LIQUIDVOLUME = "liquidVolume";

	// Token: 0x020012E3 RID: 4835
	[Serializable]
	private struct DecorInfo
	{
		// Token: 0x040046EC RID: 18156
		public string[] names;

		// Token: 0x040046ED RID: 18157
		public Vector2 offset;

		// Token: 0x040046EE RID: 18158
		public Vector2 size;
	}

	// Token: 0x020012E4 RID: 4836
	private struct SoundInfo
	{
		// Token: 0x040046EF RID: 18159
		public float startTime;

		// Token: 0x040046F0 RID: 18160
		public int splashCount;

		// Token: 0x040046F1 RID: 18161
		public HandleVector<int>.Handle handle;
	}

	// Token: 0x020012E5 RID: 4837
	private struct MistInfo
	{
		// Token: 0x040046F2 RID: 18162
		public KBatchedAnimController fx;

		// Token: 0x040046F3 RID: 18163
		public float deathTime;
	}

	// Token: 0x020012E6 RID: 4838
	private struct ParticlePhysics
	{
		// Token: 0x06006349 RID: 25417 RVA: 0x002BA540 File Offset: 0x002B8740
		public ParticlePhysics(Vector2 position, Vector2 velocity, int frame, ushort elementIdx, int worldIdx)
		{
			this.position = position;
			this.velocity = velocity;
			this.frame = frame;
			this.colour = ElementLoader.elements[(int)elementIdx].substance.colour;
			this.colour.a = 191;
			this.worldIdx = worldIdx;
		}

		// Token: 0x040046F4 RID: 18164
		public Vector2 position;

		// Token: 0x040046F5 RID: 18165
		public Vector2 velocity;

		// Token: 0x040046F6 RID: 18166
		public int frame;

		// Token: 0x040046F7 RID: 18167
		public Color32 colour;

		// Token: 0x040046F8 RID: 18168
		public int worldIdx;
	}

	// Token: 0x020012E7 RID: 4839
	private struct SerializedParticleProperties
	{
		// Token: 0x040046F9 RID: 18169
		public SimHashes elementID;

		// Token: 0x040046FA RID: 18170
		public HashedString diseaseID;

		// Token: 0x040046FB RID: 18171
		public float mass;

		// Token: 0x040046FC RID: 18172
		public float temperature;

		// Token: 0x040046FD RID: 18173
		public int diseaseCount;
	}

	// Token: 0x020012E8 RID: 4840
	private struct ParticleProperties
	{
		// Token: 0x0600634A RID: 25418 RVA: 0x000E0D7B File Offset: 0x000DEF7B
		public ParticleProperties(ushort elementIdx, float mass, float temperature, byte disease_idx, int disease_count, bool debug_track)
		{
			this.elementIdx = elementIdx;
			this.diseaseIdx = disease_idx;
			this.mass = mass;
			this.temperature = temperature;
			this.diseaseCount = disease_count;
		}

		// Token: 0x040046FE RID: 18174
		public ushort elementIdx;

		// Token: 0x040046FF RID: 18175
		public byte diseaseIdx;

		// Token: 0x04004700 RID: 18176
		public float mass;

		// Token: 0x04004701 RID: 18177
		public float temperature;

		// Token: 0x04004702 RID: 18178
		public int diseaseCount;
	}
}
