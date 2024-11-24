using System;
using System.Collections.Generic;

// Token: 0x02000B86 RID: 2950
public class AccessorySlot : Resource
{
	// Token: 0x17000272 RID: 626
	// (get) Token: 0x06003851 RID: 14417 RVA: 0x000C47C7 File Offset: 0x000C29C7
	// (set) Token: 0x06003852 RID: 14418 RVA: 0x000C47CF File Offset: 0x000C29CF
	public KAnimHashedString targetSymbolId { get; private set; }

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x06003853 RID: 14419 RVA: 0x000C47D8 File Offset: 0x000C29D8
	// (set) Token: 0x06003854 RID: 14420 RVA: 0x000C47E0 File Offset: 0x000C29E0
	public List<Accessory> accessories { get; private set; }

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x06003855 RID: 14421 RVA: 0x000C47E9 File Offset: 0x000C29E9
	public KAnimFile AnimFile
	{
		get
		{
			return this.file;
		}
	}

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x06003856 RID: 14422 RVA: 0x000C47F1 File Offset: 0x000C29F1
	// (set) Token: 0x06003857 RID: 14423 RVA: 0x000C47F9 File Offset: 0x000C29F9
	public KAnimFile defaultAnimFile { get; private set; }

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x06003858 RID: 14424 RVA: 0x000C4802 File Offset: 0x000C2A02
	// (set) Token: 0x06003859 RID: 14425 RVA: 0x000C480A File Offset: 0x000C2A0A
	public int overrideLayer { get; private set; }

	// Token: 0x0600385A RID: 14426 RVA: 0x0021ACD8 File Offset: 0x00218ED8
	public AccessorySlot(string id, ResourceSet parent, KAnimFile swap_build, int overrideLayer = 0) : base(id, parent, null)
	{
		if (swap_build == null)
		{
			Debug.LogErrorFormat("AccessorySlot {0} missing swap_build", new object[]
			{
				id
			});
		}
		this.targetSymbolId = new KAnimHashedString("snapTo_" + id.ToLower());
		this.accessories = new List<Accessory>();
		this.file = swap_build;
		this.overrideLayer = overrideLayer;
		this.defaultAnimFile = swap_build;
	}

	// Token: 0x0600385B RID: 14427 RVA: 0x0021AD48 File Offset: 0x00218F48
	public AccessorySlot(string id, ResourceSet parent, KAnimHashedString target_symbol_id, KAnimFile swap_build, KAnimFile defaultAnimFile = null, int overrideLayer = 0) : base(id, parent, null)
	{
		if (swap_build == null)
		{
			Debug.LogErrorFormat("AccessorySlot {0} missing swap_build", new object[]
			{
				id
			});
		}
		this.targetSymbolId = target_symbol_id;
		this.accessories = new List<Accessory>();
		this.file = swap_build;
		this.defaultAnimFile = ((defaultAnimFile != null) ? defaultAnimFile : swap_build);
		this.overrideLayer = overrideLayer;
	}

	// Token: 0x0600385C RID: 14428 RVA: 0x0021ADB4 File Offset: 0x00218FB4
	public void AddAccessories(KAnimFile default_build, ResourceSet parent)
	{
		KAnim.Build build = default_build.GetData().build;
		default_build.GetData().build.GetSymbol(this.targetSymbolId);
		string value = this.Id.ToLower();
		for (int i = 0; i < build.symbols.Length; i++)
		{
			string text = HashCache.Get().Get(build.symbols[i].hash);
			if (text.StartsWith(value))
			{
				Accessory accessory = new Accessory(text, parent, this, this.file.batchTag, build.symbols[i], default_build, null);
				this.accessories.Add(accessory);
				HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);
			}
		}
	}

	// Token: 0x0600385D RID: 14429 RVA: 0x000C4813 File Offset: 0x000C2A13
	public Accessory Lookup(string id)
	{
		return this.Lookup(new HashedString(id));
	}

	// Token: 0x0600385E RID: 14430 RVA: 0x0021AE70 File Offset: 0x00219070
	public Accessory Lookup(HashedString full_id)
	{
		if (!full_id.IsValid)
		{
			return null;
		}
		return this.accessories.Find((Accessory a) => a.IdHash == full_id);
	}

	// Token: 0x04002663 RID: 9827
	private KAnimFile file;
}
