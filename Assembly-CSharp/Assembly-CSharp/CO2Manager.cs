﻿using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/CO2Manager")]
public class CO2Manager : KMonoBehaviour, ISim33ms
{
		public static void DestroyInstance()
	{
		CO2Manager.instance = null;
	}

		protected override void OnPrefabInit()
	{
		CO2Manager.instance = this;
		this.prefab.gameObject.SetActive(false);
		this.breathPrefab.SetActive(false);
		this.co2Pool = new GameObjectPool(new Func<GameObject>(this.InstantiateCO2), 16);
		this.breathPool = new GameObjectPool(new Func<GameObject>(this.InstantiateBreath), 16);
	}

		private GameObject InstantiateCO2()
	{
		GameObject gameObject = GameUtil.KInstantiate(this.prefab, Grid.SceneLayer.Front, null, 0);
		gameObject.SetActive(false);
		return gameObject;
	}

		private GameObject InstantiateBreath()
	{
		GameObject gameObject = GameUtil.KInstantiate(this.breathPrefab, Grid.SceneLayer.Front, null, 0);
		gameObject.SetActive(false);
		return gameObject;
	}

		public void Sim33ms(float dt)
	{
		Vector2I vector2I = default(Vector2I);
		Vector2I vector2I2 = default(Vector2I);
		Vector3 b = this.acceleration * dt;
		int num = this.co2Items.Count;
		for (int i = 0; i < num; i++)
		{
			CO2 co = this.co2Items[i];
			co.velocity += b;
			co.lifetimeRemaining -= dt;
			Grid.PosToXY(co.transform.GetPosition(), out vector2I);
			co.transform.SetPosition(co.transform.GetPosition() + co.velocity * dt);
			Grid.PosToXY(co.transform.GetPosition(), out vector2I2);
			int num2 = Grid.XYToCell(vector2I.x, vector2I.y);
			for (int j = vector2I.y; j >= vector2I2.y; j--)
			{
				int num3 = Grid.XYToCell(vector2I.x, j);
				bool flag = !Grid.IsValidCell(num3) || co.lifetimeRemaining <= 0f;
				if (!flag)
				{
					Element element = Grid.Element[num3];
					flag = (element.IsLiquid || element.IsSolid || (Grid.Properties[num3] & 1) > 0);
				}
				if (flag)
				{
					int gameCell = num3;
					bool flag2 = false;
					if (num2 != num3)
					{
						gameCell = num2;
						flag2 = true;
					}
					else
					{
						bool flag3 = false;
						int num4 = -1;
						int num5 = -1;
						foreach (CellOffset offset in OxygenBreather.DEFAULT_BREATHABLE_OFFSETS)
						{
							int num6 = Grid.OffsetCell(num3, offset);
							if (Grid.IsValidCell(num6))
							{
								Element element2 = Grid.Element[num6];
								if (element2.id == SimHashes.CarbonDioxide || element2.HasTag(GameTags.Breathable))
								{
									num4 = num6;
									flag3 = true;
									flag2 = true;
									break;
								}
								if (element2.IsGas)
								{
									num5 = num6;
									flag2 = true;
								}
							}
						}
						if (flag2)
						{
							if (flag3)
							{
								gameCell = num4;
							}
							else
							{
								gameCell = num5;
							}
						}
					}
					if (flag2)
					{
						co.TriggerDestroy();
						SimMessages.ModifyMass(gameCell, co.mass, byte.MaxValue, 0, CellEventLogger.Instance.CO2ManagerFixedUpdate, co.temperature, SimHashes.CarbonDioxide);
						num--;
						this.co2Items[i] = this.co2Items[num];
						this.co2Items.RemoveAt(num);
						break;
					}
				}
				num2 = num3;
			}
		}
	}

		public void SpawnCO2(Vector3 position, float mass, float temperature, bool flip)
	{
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
		GameObject gameObject = this.co2Pool.GetInstance();
		gameObject.transform.SetPosition(position);
		gameObject.SetActive(true);
		CO2 component = gameObject.GetComponent<CO2>();
		component.mass = mass;
		component.temperature = temperature;
		component.velocity = Vector3.zero;
		component.lifetimeRemaining = 3f;
		KBatchedAnimController component2 = component.GetComponent<KBatchedAnimController>();
		component2.TintColour = this.tintColour;
		component2.onDestroySelf = new Action<GameObject>(this.OnDestroyCO2);
		component2.FlipX = flip;
		component.StartLoop();
		this.co2Items.Add(component);
	}

		public void SpawnBreath(Vector3 position, float mass, float temperature, bool flip)
	{
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
		this.SpawnCO2(position, mass, temperature, flip);
		GameObject gameObject = this.breathPool.GetInstance();
		gameObject.transform.SetPosition(position);
		gameObject.SetActive(true);
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		component.TintColour = this.tintColour;
		component.onDestroySelf = new Action<GameObject>(this.OnDestroyBreath);
		component.FlipX = flip;
		component.Play("breath", KAnim.PlayMode.Once, 1f, 0f);
	}

		private void OnDestroyCO2(GameObject co2_go)
	{
		co2_go.SetActive(false);
		this.co2Pool.ReleaseInstance(co2_go);
	}

		private void OnDestroyBreath(GameObject breath_go)
	{
		breath_go.SetActive(false);
		this.breathPool.ReleaseInstance(breath_go);
	}

		private const float CO2Lifetime = 3f;

		[SerializeField]
	private Vector3 acceleration;

		[SerializeField]
	private CO2 prefab;

		[SerializeField]
	private GameObject breathPrefab;

		[SerializeField]
	private Color tintColour;

		private List<CO2> co2Items = new List<CO2>();

		private GameObjectPool breathPool;

		private GameObjectPool co2Pool;

		public static CO2Manager instance;
}
