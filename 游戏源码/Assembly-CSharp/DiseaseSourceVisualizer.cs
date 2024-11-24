using System;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001240 RID: 4672
[AddComponentMenu("KMonoBehaviour/scripts/DiseaseSourceVisualizer")]
public class DiseaseSourceVisualizer : KMonoBehaviour
{
	// Token: 0x06005FB3 RID: 24499 RVA: 0x000DE58F File Offset: 0x000DC78F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateVisibility();
		Components.DiseaseSourceVisualizers.Add(this);
	}

	// Token: 0x06005FB4 RID: 24500 RVA: 0x002AB7F4 File Offset: 0x002A99F4
	protected override void OnCleanUp()
	{
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnViewModeChanged));
		base.OnCleanUp();
		Components.DiseaseSourceVisualizers.Remove(this);
		if (this.visualizer != null)
		{
			UnityEngine.Object.Destroy(this.visualizer);
			this.visualizer = null;
		}
	}

	// Token: 0x06005FB5 RID: 24501 RVA: 0x002AB858 File Offset: 0x002A9A58
	private void CreateVisualizer()
	{
		if (this.visualizer != null)
		{
			return;
		}
		if (GameScreenManager.Instance.worldSpaceCanvas == null)
		{
			return;
		}
		this.visualizer = Util.KInstantiate(Assets.UIPrefabs.ResourceVisualizer, GameScreenManager.Instance.worldSpaceCanvas, null);
	}

	// Token: 0x06005FB6 RID: 24502 RVA: 0x002AB8A8 File Offset: 0x002A9AA8
	public void UpdateVisibility()
	{
		this.CreateVisualizer();
		if (string.IsNullOrEmpty(this.alwaysShowDisease))
		{
			this.visible = false;
		}
		else
		{
			Disease disease = Db.Get().Diseases.Get(this.alwaysShowDisease);
			if (disease != null)
			{
				this.SetVisibleDisease(disease);
			}
		}
		if (OverlayScreen.Instance != null)
		{
			this.Show(OverlayScreen.Instance.GetMode());
		}
	}

	// Token: 0x06005FB7 RID: 24503 RVA: 0x002AB910 File Offset: 0x002A9B10
	private void SetVisibleDisease(Disease disease)
	{
		Sprite overlaySprite = Assets.instance.DiseaseVisualization.overlaySprite;
		Color32 colorByName = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
		Image component = this.visualizer.transform.GetChild(0).GetComponent<Image>();
		component.sprite = overlaySprite;
		component.color = colorByName;
		this.visible = true;
	}

	// Token: 0x06005FB8 RID: 24504 RVA: 0x000DE5A8 File Offset: 0x000DC7A8
	private void Update()
	{
		if (this.visualizer == null)
		{
			return;
		}
		this.visualizer.transform.SetPosition(base.transform.GetPosition() + this.offset);
	}

	// Token: 0x06005FB9 RID: 24505 RVA: 0x000DE5E0 File Offset: 0x000DC7E0
	private void OnViewModeChanged(HashedString mode)
	{
		this.Show(mode);
	}

	// Token: 0x06005FBA RID: 24506 RVA: 0x000DE5E9 File Offset: 0x000DC7E9
	public void Show(HashedString mode)
	{
		base.enabled = (this.visible && mode == OverlayModes.Disease.ID);
		if (this.visualizer != null)
		{
			this.visualizer.SetActive(base.enabled);
		}
	}

	// Token: 0x040043DE RID: 17374
	[SerializeField]
	private Vector3 offset;

	// Token: 0x040043DF RID: 17375
	private GameObject visualizer;

	// Token: 0x040043E0 RID: 17376
	private bool visible;

	// Token: 0x040043E1 RID: 17377
	public string alwaysShowDisease;
}
