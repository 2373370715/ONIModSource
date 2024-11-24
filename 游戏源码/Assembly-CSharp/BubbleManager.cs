using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C63 RID: 3171
[AddComponentMenu("KMonoBehaviour/scripts/BubbleManager")]
public class BubbleManager : KMonoBehaviour, ISim33ms, IRenderEveryTick
{
	// Token: 0x06003CC5 RID: 15557 RVA: 0x000C7544 File Offset: 0x000C5744
	public static void DestroyInstance()
	{
		BubbleManager.instance = null;
	}

	// Token: 0x06003CC6 RID: 15558 RVA: 0x000C754C File Offset: 0x000C574C
	protected override void OnPrefabInit()
	{
		BubbleManager.instance = this;
	}

	// Token: 0x06003CC7 RID: 15559 RVA: 0x0022ED3C File Offset: 0x0022CF3C
	public void SpawnBubble(Vector2 position, Vector2 velocity, SimHashes element, float mass, float temperature)
	{
		BubbleManager.Bubble item = new BubbleManager.Bubble
		{
			position = position,
			velocity = velocity,
			element = element,
			temperature = temperature,
			mass = mass
		};
		this.bubbles.Add(item);
	}

	// Token: 0x06003CC8 RID: 15560 RVA: 0x0022ED8C File Offset: 0x0022CF8C
	public void Sim33ms(float dt)
	{
		ListPool<BubbleManager.Bubble, BubbleManager>.PooledList pooledList = ListPool<BubbleManager.Bubble, BubbleManager>.Allocate();
		ListPool<BubbleManager.Bubble, BubbleManager>.PooledList pooledList2 = ListPool<BubbleManager.Bubble, BubbleManager>.Allocate();
		foreach (BubbleManager.Bubble bubble in this.bubbles)
		{
			bubble.position += bubble.velocity * dt;
			bubble.elapsedTime += dt;
			int num = Grid.PosToCell(bubble.position);
			if (!Grid.IsVisiblyInLiquid(bubble.position) || Grid.Element[num].id == bubble.element)
			{
				pooledList2.Add(bubble);
			}
			else
			{
				pooledList.Add(bubble);
			}
		}
		foreach (BubbleManager.Bubble bubble2 in pooledList2)
		{
			SimMessages.AddRemoveSubstance(Grid.PosToCell(bubble2.position), bubble2.element, CellEventLogger.Instance.FallingWaterAddToSim, bubble2.mass, bubble2.temperature, byte.MaxValue, 0, true, -1);
		}
		this.bubbles.Clear();
		this.bubbles.AddRange(pooledList);
		pooledList2.Recycle();
		pooledList.Recycle();
	}

	// Token: 0x06003CC9 RID: 15561 RVA: 0x0022EEE4 File Offset: 0x0022D0E4
	public void RenderEveryTick(float dt)
	{
		ListPool<SpriteSheetAnimator.AnimInfo, BubbleManager>.PooledList pooledList = ListPool<SpriteSheetAnimator.AnimInfo, BubbleManager>.Allocate();
		SpriteSheetAnimator spriteSheetAnimator = SpriteSheetAnimManager.instance.GetSpriteSheetAnimator("liquid_splash1");
		foreach (BubbleManager.Bubble bubble in this.bubbles)
		{
			SpriteSheetAnimator.AnimInfo item = new SpriteSheetAnimator.AnimInfo
			{
				frame = spriteSheetAnimator.GetFrameFromElapsedTimeLooping(bubble.elapsedTime),
				elapsedTime = bubble.elapsedTime,
				pos = new Vector3(bubble.position.x, bubble.position.y, 0f),
				rotation = Quaternion.identity,
				size = Vector2.one,
				colour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
			};
			pooledList.Add(item);
		}
		pooledList.Recycle();
	}

	// Token: 0x0400296C RID: 10604
	public static BubbleManager instance;

	// Token: 0x0400296D RID: 10605
	private List<BubbleManager.Bubble> bubbles = new List<BubbleManager.Bubble>();

	// Token: 0x02000C64 RID: 3172
	private struct Bubble
	{
		// Token: 0x0400296E RID: 10606
		public Vector2 position;

		// Token: 0x0400296F RID: 10607
		public Vector2 velocity;

		// Token: 0x04002970 RID: 10608
		public float elapsedTime;

		// Token: 0x04002971 RID: 10609
		public int frame;

		// Token: 0x04002972 RID: 10610
		public SimHashes element;

		// Token: 0x04002973 RID: 10611
		public float temperature;

		// Token: 0x04002974 RID: 10612
		public float mass;
	}
}
