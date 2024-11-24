using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001993 RID: 6547
[AddComponentMenu("KMonoBehaviour/scripts/SpaceArtifact")]
public class SpaceArtifact : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x0600887A RID: 34938 RVA: 0x00353ED4 File Offset: 0x003520D4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.loadCharmed && DlcManager.IsExpansion1Active())
		{
			base.gameObject.AddTag(GameTags.CharmedArtifact);
			this.SetEntombedDecor();
		}
		else
		{
			this.loadCharmed = false;
			this.SetAnalyzedDecor();
		}
		this.UpdateStatusItem();
		Components.SpaceArtifacts.Add(this);
		this.UpdateAnim();
	}

	// Token: 0x0600887B RID: 34939 RVA: 0x000F9486 File Offset: 0x000F7686
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.SpaceArtifacts.Remove(this);
	}

	// Token: 0x0600887C RID: 34940 RVA: 0x000F9499 File Offset: 0x000F7699
	public void RemoveCharm()
	{
		base.gameObject.RemoveTag(GameTags.CharmedArtifact);
		this.UpdateStatusItem();
		this.loadCharmed = false;
		this.UpdateAnim();
		this.SetAnalyzedDecor();
	}

	// Token: 0x0600887D RID: 34941 RVA: 0x000F94C4 File Offset: 0x000F76C4
	private void SetEntombedDecor()
	{
		base.GetComponent<DecorProvider>().SetValues(DECOR.BONUS.TIER0);
	}

	// Token: 0x0600887E RID: 34942 RVA: 0x000F94D6 File Offset: 0x000F76D6
	private void SetAnalyzedDecor()
	{
		base.GetComponent<DecorProvider>().SetValues(this.artifactTier.decorValues);
	}

	// Token: 0x0600887F RID: 34943 RVA: 0x00353F34 File Offset: 0x00352134
	public void UpdateStatusItem()
	{
		if (base.gameObject.HasTag(GameTags.CharmedArtifact))
		{
			base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.ArtifactEntombed, null);
			return;
		}
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.ArtifactEntombed, false);
	}

	// Token: 0x06008880 RID: 34944 RVA: 0x000F94EE File Offset: 0x000F76EE
	public void SetArtifactTier(ArtifactTier tier)
	{
		this.artifactTier = tier;
	}

	// Token: 0x06008881 RID: 34945 RVA: 0x000F94F7 File Offset: 0x000F76F7
	public ArtifactTier GetArtifactTier()
	{
		return this.artifactTier;
	}

	// Token: 0x06008882 RID: 34946 RVA: 0x000F94FF File Offset: 0x000F76FF
	public void SetUIAnim(string anim)
	{
		this.ui_anim = anim;
	}

	// Token: 0x06008883 RID: 34947 RVA: 0x000F9508 File Offset: 0x000F7708
	public string GetUIAnim()
	{
		return this.ui_anim;
	}

	// Token: 0x06008884 RID: 34948 RVA: 0x00353F98 File Offset: 0x00352198
	public List<Descriptor> GetEffectDescriptions()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (base.gameObject.HasTag(GameTags.CharmedArtifact))
		{
			Descriptor item = new Descriptor(STRINGS.BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.PAYLOAD_DROP_RATE.Replace("{chance}", GameUtil.GetFormattedPercent(this.artifactTier.payloadDropChance * 100f, GameUtil.TimeSlice.None)), STRINGS.BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.PAYLOAD_DROP_RATE_TOOLTIP.Replace("{chance}", GameUtil.GetFormattedPercent(this.artifactTier.payloadDropChance * 100f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		Descriptor item2 = new Descriptor(string.Format("This is an artifact from space", Array.Empty<object>()), string.Format("This is the tooltip string", Array.Empty<object>()), Descriptor.DescriptorType.Information, false);
		list.Add(item2);
		return list;
	}

	// Token: 0x06008885 RID: 34949 RVA: 0x000F9510 File Offset: 0x000F7710
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return this.GetEffectDescriptions();
	}

	// Token: 0x06008886 RID: 34950 RVA: 0x00354048 File Offset: 0x00352248
	private void UpdateAnim()
	{
		string s;
		if (base.gameObject.HasTag(GameTags.CharmedArtifact))
		{
			s = "entombed_" + this.uniqueAnimNameFragment.Replace("idle_", "");
		}
		else
		{
			s = this.uniqueAnimNameFragment;
		}
		base.GetComponent<KBatchedAnimController>().Play(s, KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06008887 RID: 34951 RVA: 0x003540AC File Offset: 0x003522AC
	[OnDeserialized]
	public void OnDeserialize()
	{
		Pickupable component = base.GetComponent<Pickupable>();
		if (component != null)
		{
			component.deleteOffGrid = false;
		}
	}

	// Token: 0x040066AD RID: 26285
	public const string ID = "SpaceArtifact";

	// Token: 0x040066AE RID: 26286
	private const string charmedPrefix = "entombed_";

	// Token: 0x040066AF RID: 26287
	private const string idlePrefix = "idle_";

	// Token: 0x040066B0 RID: 26288
	[SerializeField]
	private string ui_anim;

	// Token: 0x040066B1 RID: 26289
	[Serialize]
	private bool loadCharmed = true;

	// Token: 0x040066B2 RID: 26290
	public ArtifactTier artifactTier;

	// Token: 0x040066B3 RID: 26291
	public ArtifactType artifactType;

	// Token: 0x040066B4 RID: 26292
	public string uniqueAnimNameFragment;
}
