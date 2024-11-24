using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000BA6 RID: 2982
public abstract class DevToolEntityTarget
{
	// Token: 0x06003926 RID: 14630
	public abstract string GetTag();

	// Token: 0x06003927 RID: 14631
	[return: TupleElementNames(new string[]
	{
		"cornerA",
		"cornerB"
	})]
	public abstract Option<ValueTuple<Vector2, Vector2>> GetScreenRect();

	// Token: 0x06003928 RID: 14632 RVA: 0x000C4FAF File Offset: 0x000C31AF
	public string GetDebugName()
	{
		return "[" + this.GetTag() + "] " + this.ToString();
	}

	// Token: 0x02000BA7 RID: 2983
	public class ForUIGameObject : DevToolEntityTarget
	{
		// Token: 0x0600392A RID: 14634 RVA: 0x000C4FCC File Offset: 0x000C31CC
		public ForUIGameObject(GameObject gameObject)
		{
			this.gameObject = gameObject;
		}

		// Token: 0x0600392B RID: 14635 RVA: 0x0021DF10 File Offset: 0x0021C110
		[return: TupleElementNames(new string[]
		{
			"cornerA",
			"cornerB"
		})]
		public override Option<ValueTuple<Vector2, Vector2>> GetScreenRect()
		{
			if (this.gameObject.IsNullOrDestroyed())
			{
				return Option.None;
			}
			RectTransform component = this.gameObject.GetComponent<RectTransform>();
			if (component.IsNullOrDestroyed())
			{
				return Option.None;
			}
			Canvas componentInParent = this.gameObject.GetComponentInParent<Canvas>();
			if (component.IsNullOrDestroyed())
			{
				return Option.None;
			}
			if (!componentInParent.worldCamera.IsNullOrDestroyed())
			{
				DevToolEntityTarget.ForUIGameObject.<>c__DisplayClass2_0 CS$<>8__locals1;
				CS$<>8__locals1.camera = componentInParent.worldCamera;
				Vector3[] array = new Vector3[4];
				component.GetWorldCorners(array);
				return new ValueTuple<Vector2, Vector2>(DevToolEntityTarget.ForUIGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(array[0]), ref CS$<>8__locals1), DevToolEntityTarget.ForUIGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(array[2]), ref CS$<>8__locals1));
			}
			if (componentInParent.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				Vector3[] array2 = new Vector3[4];
				component.GetWorldCorners(array2);
				return new ValueTuple<Vector2, Vector2>(DevToolEntityTarget.ForUIGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_1(array2[0]), DevToolEntityTarget.ForUIGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_1(array2[2]));
			}
			return Option.None;
		}

		// Token: 0x0600392C RID: 14636 RVA: 0x000C4FDB File Offset: 0x000C31DB
		public override string GetTag()
		{
			return "UI";
		}

		// Token: 0x0600392D RID: 14637 RVA: 0x000C4FE2 File Offset: 0x000C31E2
		public override string ToString()
		{
			return DevToolEntity.GetNameFor(this.gameObject);
		}

		// Token: 0x0600392E RID: 14638 RVA: 0x000C4FEF File Offset: 0x000C31EF
		[CompilerGenerated]
		internal static Vector2 <GetScreenRect>g__ScreenPointToScreenPosition|2_0(Vector2 coord, ref DevToolEntityTarget.ForUIGameObject.<>c__DisplayClass2_0 A_1)
		{
			return new Vector2(coord.x, (float)A_1.camera.pixelHeight - coord.y);
		}

		// Token: 0x0600392F RID: 14639 RVA: 0x000C500F File Offset: 0x000C320F
		[CompilerGenerated]
		internal static Vector2 <GetScreenRect>g__ScreenPointToScreenPosition|2_1(Vector2 coord)
		{
			return new Vector2(coord.x, (float)Screen.height - coord.y);
		}

		// Token: 0x040026DC RID: 9948
		public GameObject gameObject;
	}

	// Token: 0x02000BA9 RID: 2985
	public class ForWorldGameObject : DevToolEntityTarget
	{
		// Token: 0x06003930 RID: 14640 RVA: 0x000C5029 File Offset: 0x000C3229
		public ForWorldGameObject(GameObject gameObject)
		{
			this.gameObject = gameObject;
		}

		// Token: 0x06003931 RID: 14641 RVA: 0x0021E034 File Offset: 0x0021C234
		[return: TupleElementNames(new string[]
		{
			"cornerA",
			"cornerB"
		})]
		public override Option<ValueTuple<Vector2, Vector2>> GetScreenRect()
		{
			if (this.gameObject.IsNullOrDestroyed())
			{
				return Option.None;
			}
			DevToolEntityTarget.ForWorldGameObject.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.camera = Camera.main;
			if (CS$<>8__locals1.camera.IsNullOrDestroyed())
			{
				return Option.None;
			}
			KCollider2D component = this.gameObject.GetComponent<KCollider2D>();
			if (component.IsNullOrDestroyed())
			{
				return Option.None;
			}
			return new ValueTuple<Vector2, Vector2>(DevToolEntityTarget.ForWorldGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(component.bounds.min), ref CS$<>8__locals1), DevToolEntityTarget.ForWorldGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(component.bounds.max), ref CS$<>8__locals1));
		}

		// Token: 0x06003932 RID: 14642 RVA: 0x000C5038 File Offset: 0x000C3238
		public override string GetTag()
		{
			return "World";
		}

		// Token: 0x06003933 RID: 14643 RVA: 0x000C503F File Offset: 0x000C323F
		public override string ToString()
		{
			return DevToolEntity.GetNameFor(this.gameObject);
		}

		// Token: 0x06003934 RID: 14644 RVA: 0x000C504C File Offset: 0x000C324C
		[CompilerGenerated]
		internal static Vector2 <GetScreenRect>g__ScreenPointToScreenPosition|2_0(Vector2 coord, ref DevToolEntityTarget.ForWorldGameObject.<>c__DisplayClass2_0 A_1)
		{
			return new Vector2(coord.x, (float)A_1.camera.pixelHeight - coord.y);
		}

		// Token: 0x040026DE RID: 9950
		public GameObject gameObject;
	}

	// Token: 0x02000BAB RID: 2987
	public class ForSimCell : DevToolEntityTarget
	{
		// Token: 0x06003935 RID: 14645 RVA: 0x000C506C File Offset: 0x000C326C
		public ForSimCell(int cellIndex)
		{
			this.cellIndex = cellIndex;
		}

		// Token: 0x06003936 RID: 14646 RVA: 0x0021E0F0 File Offset: 0x0021C2F0
		[return: TupleElementNames(new string[]
		{
			"cornerA",
			"cornerB"
		})]
		public override Option<ValueTuple<Vector2, Vector2>> GetScreenRect()
		{
			DevToolEntityTarget.ForSimCell.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.camera = Camera.main;
			if (CS$<>8__locals1.camera.IsNullOrDestroyed())
			{
				return Option.None;
			}
			Vector2 a = Grid.CellToPosCCC(this.cellIndex, Grid.SceneLayer.Background);
			Vector2 b = Grid.HalfCellSizeInMeters * Vector2.one;
			Vector2 v = a - b;
			Vector2 v2 = a + b;
			return new ValueTuple<Vector2, Vector2>(DevToolEntityTarget.ForSimCell.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(v), ref CS$<>8__locals1), DevToolEntityTarget.ForSimCell.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(v2), ref CS$<>8__locals1));
		}

		// Token: 0x06003937 RID: 14647 RVA: 0x000C507B File Offset: 0x000C327B
		public override string GetTag()
		{
			return "Sim Cell";
		}

		// Token: 0x06003938 RID: 14648 RVA: 0x000C5082 File Offset: 0x000C3282
		public override string ToString()
		{
			return this.cellIndex.ToString();
		}

		// Token: 0x06003939 RID: 14649 RVA: 0x000C508F File Offset: 0x000C328F
		[CompilerGenerated]
		internal static Vector2 <GetScreenRect>g__ScreenPointToScreenPosition|2_0(Vector2 coord, ref DevToolEntityTarget.ForSimCell.<>c__DisplayClass2_0 A_1)
		{
			return new Vector2(coord.x, (float)A_1.camera.pixelHeight - coord.y);
		}

		// Token: 0x040026E0 RID: 9952
		public int cellIndex;
	}
}
