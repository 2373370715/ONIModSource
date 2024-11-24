using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B09 RID: 2825
[AddComponentMenu("KMonoBehaviour/scripts/SnapOn")]
public class SnapOn : KMonoBehaviour
{
	// Token: 0x060034FB RID: 13563 RVA: 0x000C28E9 File Offset: 0x000C0AE9
	protected override void OnPrefabInit()
	{
		this.kanimController = base.GetComponent<KAnimControllerBase>();
	}

	// Token: 0x060034FC RID: 13564 RVA: 0x0020C878 File Offset: 0x0020AA78
	protected override void OnSpawn()
	{
		foreach (SnapOn.SnapPoint snapPoint in this.snapPoints)
		{
			if (snapPoint.automatic)
			{
				this.DoAttachSnapOn(snapPoint);
			}
		}
	}

	// Token: 0x060034FD RID: 13565 RVA: 0x0020C8D4 File Offset: 0x0020AAD4
	public void AttachSnapOnByName(string name)
	{
		foreach (SnapOn.SnapPoint snapPoint in this.snapPoints)
		{
			if (snapPoint.pointName == name)
			{
				HashedString context = base.GetComponent<AnimEventHandler>().GetContext();
				if (!context.IsValid || !snapPoint.context.IsValid || context == snapPoint.context)
				{
					this.DoAttachSnapOn(snapPoint);
				}
			}
		}
	}

	// Token: 0x060034FE RID: 13566 RVA: 0x0020C968 File Offset: 0x0020AB68
	public void DetachSnapOnByName(string name)
	{
		foreach (SnapOn.SnapPoint snapPoint in this.snapPoints)
		{
			if (snapPoint.pointName == name)
			{
				HashedString context = base.GetComponent<AnimEventHandler>().GetContext();
				if (!context.IsValid || !snapPoint.context.IsValid || context == snapPoint.context)
				{
					base.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(snapPoint.overrideSymbol, 5);
					this.kanimController.SetSymbolVisiblity(snapPoint.overrideSymbol, false);
					break;
				}
			}
		}
	}

	// Token: 0x060034FF RID: 13567 RVA: 0x0020CA20 File Offset: 0x0020AC20
	private void DoAttachSnapOn(SnapOn.SnapPoint point)
	{
		SnapOn.OverrideEntry overrideEntry = null;
		KAnimFile buildFile = point.buildFile;
		string symbol_name = "";
		if (this.overrideMap.TryGetValue(point.pointName, out overrideEntry))
		{
			buildFile = overrideEntry.buildFile;
			symbol_name = overrideEntry.symbolName;
		}
		KAnim.Build.Symbol symbol = SnapOn.GetSymbol(buildFile, symbol_name);
		base.GetComponent<SymbolOverrideController>().AddSymbolOverride(point.overrideSymbol, symbol, 5);
		this.kanimController.SetSymbolVisiblity(point.overrideSymbol, true);
	}

	// Token: 0x06003500 RID: 13568 RVA: 0x0020CA94 File Offset: 0x0020AC94
	private static KAnim.Build.Symbol GetSymbol(KAnimFile anim_file, string symbol_name)
	{
		KAnim.Build.Symbol result = anim_file.GetData().build.symbols[0];
		KAnimHashedString y = new KAnimHashedString(symbol_name);
		foreach (KAnim.Build.Symbol symbol in anim_file.GetData().build.symbols)
		{
			if (symbol.hash == y)
			{
				result = symbol;
				break;
			}
		}
		return result;
	}

	// Token: 0x06003501 RID: 13569 RVA: 0x000C28F7 File Offset: 0x000C0AF7
	public void AddOverride(string point_name, KAnimFile build_override, string symbol_name)
	{
		this.overrideMap[point_name] = new SnapOn.OverrideEntry
		{
			buildFile = build_override,
			symbolName = symbol_name
		};
	}

	// Token: 0x06003502 RID: 13570 RVA: 0x000C2918 File Offset: 0x000C0B18
	public void RemoveOverride(string point_name)
	{
		this.overrideMap.Remove(point_name);
	}

	// Token: 0x040023FF RID: 9215
	private KAnimControllerBase kanimController;

	// Token: 0x04002400 RID: 9216
	public List<SnapOn.SnapPoint> snapPoints = new List<SnapOn.SnapPoint>();

	// Token: 0x04002401 RID: 9217
	private Dictionary<string, SnapOn.OverrideEntry> overrideMap = new Dictionary<string, SnapOn.OverrideEntry>();

	// Token: 0x02000B0A RID: 2826
	[Serializable]
	public class SnapPoint
	{
		// Token: 0x04002402 RID: 9218
		public string pointName;

		// Token: 0x04002403 RID: 9219
		public bool automatic = true;

		// Token: 0x04002404 RID: 9220
		public HashedString context;

		// Token: 0x04002405 RID: 9221
		public KAnimFile buildFile;

		// Token: 0x04002406 RID: 9222
		public HashedString overrideSymbol;
	}

	// Token: 0x02000B0B RID: 2827
	public class OverrideEntry
	{
		// Token: 0x04002407 RID: 9223
		public KAnimFile buildFile;

		// Token: 0x04002408 RID: 9224
		public string symbolName;
	}
}
