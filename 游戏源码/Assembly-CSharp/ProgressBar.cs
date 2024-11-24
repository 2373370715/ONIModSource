using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001EB6 RID: 7862
[AddComponentMenu("KMonoBehaviour/scripts/ProgressBar")]
public class ProgressBar : KMonoBehaviour
{
	// Token: 0x17000A9B RID: 2715
	// (get) Token: 0x0600A52B RID: 42283 RVA: 0x0010B23E File Offset: 0x0010943E
	// (set) Token: 0x0600A52C RID: 42284 RVA: 0x0010B24B File Offset: 0x0010944B
	public Color barColor
	{
		get
		{
			return this.bar.color;
		}
		set
		{
			this.bar.color = value;
		}
	}

	// Token: 0x17000A9C RID: 2716
	// (get) Token: 0x0600A52D RID: 42285 RVA: 0x0010B259 File Offset: 0x00109459
	// (set) Token: 0x0600A52E RID: 42286 RVA: 0x0010B266 File Offset: 0x00109466
	public float PercentFull
	{
		get
		{
			return this.bar.fillAmount;
		}
		set
		{
			this.bar.fillAmount = value;
		}
	}

	// Token: 0x0600A52F RID: 42287 RVA: 0x0010B274 File Offset: 0x00109474
	public void SetVisibility(bool visible)
	{
		this.lastVisibilityValue = visible;
		this.RefreshVisibility();
	}

	// Token: 0x0600A530 RID: 42288 RVA: 0x003EBA54 File Offset: 0x003E9C54
	private void RefreshVisibility()
	{
		int myWorldId = base.gameObject.GetMyWorldId();
		bool flag = this.lastVisibilityValue;
		flag &= (!this.hasBeenInitialize || myWorldId == ClusterManager.Instance.activeWorldId);
		flag &= (!this.autoHide || SimDebugView.Instance == null || SimDebugView.Instance.GetMode() == OverlayModes.None.ID);
		base.gameObject.SetActive(flag);
		if (this.updatePercentFull == null || this.updatePercentFull.Target.IsNullOrDestroyed())
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600A531 RID: 42289 RVA: 0x003EBAF0 File Offset: 0x003E9CF0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.hasBeenInitialize = true;
		if (this.autoHide)
		{
			this.overlayUpdateHandle = Game.Instance.Subscribe(1798162660, new Action<object>(this.OnOverlayChanged));
			if (SimDebugView.Instance != null && SimDebugView.Instance.GetMode() != OverlayModes.None.ID)
			{
				base.gameObject.SetActive(false);
			}
		}
		Game.Instance.Subscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
		this.SetWorldActive(ClusterManager.Instance.activeWorldId);
		base.enabled = (this.updatePercentFull != null);
		this.RefreshVisibility();
	}

	// Token: 0x0600A532 RID: 42290 RVA: 0x003EBBA4 File Offset: 0x003E9DA4
	private void OnActiveWorldChanged(object data)
	{
		global::Tuple<int, int> tuple = (global::Tuple<int, int>)data;
		this.SetWorldActive(tuple.first);
	}

	// Token: 0x0600A533 RID: 42291 RVA: 0x0010B283 File Offset: 0x00109483
	private void SetWorldActive(int worldId)
	{
		this.RefreshVisibility();
	}

	// Token: 0x0600A534 RID: 42292 RVA: 0x0010B28B File Offset: 0x0010948B
	public void SetUpdateFunc(Func<float> func)
	{
		this.updatePercentFull = func;
		base.enabled = (this.updatePercentFull != null);
	}

	// Token: 0x0600A535 RID: 42293 RVA: 0x0010B2A3 File Offset: 0x001094A3
	public virtual void Update()
	{
		if (this.updatePercentFull != null && !this.updatePercentFull.Target.IsNullOrDestroyed())
		{
			this.PercentFull = this.updatePercentFull();
		}
	}

	// Token: 0x0600A536 RID: 42294 RVA: 0x0010B283 File Offset: 0x00109483
	public virtual void OnOverlayChanged(object data = null)
	{
		this.RefreshVisibility();
	}

	// Token: 0x0600A537 RID: 42295 RVA: 0x003EBBC4 File Offset: 0x003E9DC4
	public void Retarget(GameObject entity)
	{
		Vector3 vector = entity.transform.GetPosition() + Vector3.down * 0.5f;
		Building component = entity.GetComponent<Building>();
		if (component != null)
		{
			vector -= Vector3.right * 0.5f * (float)(component.Def.WidthInCells % 2);
		}
		else
		{
			vector -= Vector3.right * 0.5f;
		}
		base.transform.SetPosition(vector);
	}

	// Token: 0x0600A538 RID: 42296 RVA: 0x0010B2D0 File Offset: 0x001094D0
	protected override void OnCleanUp()
	{
		if (this.overlayUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.overlayUpdateHandle);
		}
		Game.Instance.Unsubscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
		base.OnCleanUp();
	}

	// Token: 0x0600A539 RID: 42297 RVA: 0x00104B30 File Offset: 0x00102D30
	private void OnBecameInvisible()
	{
		base.enabled = false;
	}

	// Token: 0x0600A53A RID: 42298 RVA: 0x00104B39 File Offset: 0x00102D39
	private void OnBecameVisible()
	{
		base.enabled = true;
	}

	// Token: 0x0600A53B RID: 42299 RVA: 0x003EBC50 File Offset: 0x003E9E50
	public static ProgressBar CreateProgressBar(GameObject entity, Func<float> updateFunc)
	{
		ProgressBar progressBar = Util.KInstantiateUI<ProgressBar>(ProgressBarsConfig.Instance.progressBarPrefab, null, false);
		progressBar.SetUpdateFunc(updateFunc);
		progressBar.transform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.transform);
		progressBar.name = ((entity != null) ? (entity.name + "_") : "") + " ProgressBar";
		progressBar.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("ProgressBar");
		progressBar.Update();
		progressBar.Retarget(entity);
		return progressBar;
	}

	// Token: 0x04008158 RID: 33112
	public Image bar;

	// Token: 0x04008159 RID: 33113
	private Func<float> updatePercentFull;

	// Token: 0x0400815A RID: 33114
	private int overlayUpdateHandle = -1;

	// Token: 0x0400815B RID: 33115
	public bool autoHide = true;

	// Token: 0x0400815C RID: 33116
	private bool lastVisibilityValue = true;

	// Token: 0x0400815D RID: 33117
	private bool hasBeenInitialize;
}
