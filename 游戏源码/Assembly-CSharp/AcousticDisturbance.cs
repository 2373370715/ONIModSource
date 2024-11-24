using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C0A RID: 3082
public class AcousticDisturbance
{
	// Token: 0x06003AC8 RID: 15048 RVA: 0x00228944 File Offset: 0x00226B44
	public static void Emit(object data, int EmissionRadius)
	{
		GameObject gameObject = (GameObject)data;
		Components.Cmps<MinionIdentity> liveMinionIdentities = Components.LiveMinionIdentities;
		Vector2 vector = gameObject.transform.GetPosition();
		int num = Grid.PosToCell(vector);
		int num2 = EmissionRadius * EmissionRadius;
		AcousticDisturbance.cellsInRange = GameUtil.CollectCellsBreadthFirst(num, (int cell) => !Grid.Solid[cell], EmissionRadius);
		AcousticDisturbance.DrawVisualEffect(num, AcousticDisturbance.cellsInRange);
		for (int i = 0; i < liveMinionIdentities.Count; i++)
		{
			MinionIdentity minionIdentity = liveMinionIdentities[i];
			if (minionIdentity.gameObject != gameObject.gameObject)
			{
				Vector2 vector2 = minionIdentity.transform.GetPosition();
				if (Vector2.SqrMagnitude(vector - vector2) <= (float)num2)
				{
					int item = Grid.PosToCell(vector2);
					if (AcousticDisturbance.cellsInRange.Contains(item))
					{
						StaminaMonitor.Instance smi = minionIdentity.GetSMI<StaminaMonitor.Instance>();
						if (smi != null && smi.IsSleeping())
						{
							minionIdentity.Trigger(-527751701, data);
							minionIdentity.Trigger(1621815900, data);
						}
					}
				}
			}
		}
		AcousticDisturbance.cellsInRange.Clear();
	}

	// Token: 0x06003AC9 RID: 15049 RVA: 0x00228A5C File Offset: 0x00226C5C
	private static void DrawVisualEffect(int center_cell, HashSet<int> cells)
	{
		SoundEvent.PlayOneShot(GlobalResources.Instance().AcousticDisturbanceSound, Grid.CellToPos(center_cell), 1f);
		foreach (int num in cells)
		{
			int gridDistance = AcousticDisturbance.GetGridDistance(num, center_cell);
			GameScheduler.Instance.Schedule("radialgrid_pre", AcousticDisturbance.distanceDelay * (float)gridDistance, new Action<object>(AcousticDisturbance.SpawnEffect), num, null);
		}
	}

	// Token: 0x06003ACA RID: 15050 RVA: 0x00228AF4 File Offset: 0x00226CF4
	private static void SpawnEffect(object data)
	{
		Grid.SceneLayer layer = Grid.SceneLayer.InteriorWall;
		int cell = (int)data;
		KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("radialgrid_kanim", Grid.CellToPosCCC(cell, layer), null, false, layer, false);
		kbatchedAnimController.destroyOnAnimComplete = false;
		kbatchedAnimController.Play(AcousticDisturbance.PreAnims, KAnim.PlayMode.Loop);
		GameScheduler.Instance.Schedule("radialgrid_loop", AcousticDisturbance.duration, new Action<object>(AcousticDisturbance.DestroyEffect), kbatchedAnimController, null);
	}

	// Token: 0x06003ACB RID: 15051 RVA: 0x000C5EF2 File Offset: 0x000C40F2
	private static void DestroyEffect(object data)
	{
		KBatchedAnimController kbatchedAnimController = (KBatchedAnimController)data;
		kbatchedAnimController.destroyOnAnimComplete = true;
		kbatchedAnimController.Play(AcousticDisturbance.PostAnim, KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06003ACC RID: 15052 RVA: 0x00228B58 File Offset: 0x00226D58
	private static int GetGridDistance(int cell, int center_cell)
	{
		Vector2I u = Grid.CellToXY(cell);
		Vector2I v = Grid.CellToXY(center_cell);
		Vector2I vector2I = u - v;
		return Math.Abs(vector2I.x) + Math.Abs(vector2I.y);
	}

	// Token: 0x04002817 RID: 10263
	private static readonly HashedString[] PreAnims = new HashedString[]
	{
		"grid_pre",
		"grid_loop"
	};

	// Token: 0x04002818 RID: 10264
	private static readonly HashedString PostAnim = "grid_pst";

	// Token: 0x04002819 RID: 10265
	private static float distanceDelay = 0.25f;

	// Token: 0x0400281A RID: 10266
	private static float duration = 3f;

	// Token: 0x0400281B RID: 10267
	private static HashSet<int> cellsInRange = new HashSet<int>();
}
