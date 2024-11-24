using System;
using KSerialization;

// Token: 0x020012BA RID: 4794
public class EquippableFacade : KMonoBehaviour
{
	// Token: 0x06006284 RID: 25220 RVA: 0x000E04C0 File Offset: 0x000DE6C0
	public static void AddFacadeToEquippable(Equippable equippable, string facadeID)
	{
		EquippableFacade equippableFacade = equippable.gameObject.AddOrGet<EquippableFacade>();
		equippableFacade.FacadeID = facadeID;
		equippableFacade.BuildOverride = Db.GetEquippableFacades().Get(facadeID).BuildOverride;
		equippableFacade.ApplyAnimOverride();
	}

	// Token: 0x06006285 RID: 25221 RVA: 0x000E04EF File Offset: 0x000DE6EF
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OverrideName();
		this.ApplyAnimOverride();
	}

	// Token: 0x1700062A RID: 1578
	// (get) Token: 0x06006286 RID: 25222 RVA: 0x000E0503 File Offset: 0x000DE703
	// (set) Token: 0x06006287 RID: 25223 RVA: 0x000E050B File Offset: 0x000DE70B
	public string FacadeID
	{
		get
		{
			return this._facadeID;
		}
		private set
		{
			this._facadeID = value;
			this.OverrideName();
		}
	}

	// Token: 0x06006288 RID: 25224 RVA: 0x000E051A File Offset: 0x000DE71A
	public void ApplyAnimOverride()
	{
		if (this.FacadeID.IsNullOrWhiteSpace())
		{
			return;
		}
		base.GetComponent<KBatchedAnimController>().SwapAnims(new KAnimFile[]
		{
			Db.GetEquippableFacades().Get(this.FacadeID).AnimFile
		});
	}

	// Token: 0x06006289 RID: 25225 RVA: 0x000E0553 File Offset: 0x000DE753
	private void OverrideName()
	{
		base.GetComponent<KSelectable>().SetName(EquippableFacade.GetNameOverride(base.GetComponent<Equippable>().def.Id, this.FacadeID));
	}

	// Token: 0x0600628A RID: 25226 RVA: 0x000E057B File Offset: 0x000DE77B
	public static string GetNameOverride(string defID, string facadeID)
	{
		if (facadeID.IsNullOrWhiteSpace())
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + defID.ToUpper() + ".NAME");
		}
		return Db.GetEquippableFacades().Get(facadeID).Name;
	}

	// Token: 0x04004623 RID: 17955
	[Serialize]
	private string _facadeID;

	// Token: 0x04004624 RID: 17956
	[Serialize]
	public string BuildOverride;
}
