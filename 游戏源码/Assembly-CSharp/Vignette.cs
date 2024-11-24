using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200204C RID: 8268
public class Vignette : KMonoBehaviour
{
	// Token: 0x0600B00C RID: 45068 RVA: 0x001126CF File Offset: 0x001108CF
	public static void DestroyInstance()
	{
		Vignette.Instance = null;
	}

	// Token: 0x0600B00D RID: 45069 RVA: 0x00422EB4 File Offset: 0x004210B4
	protected override void OnSpawn()
	{
		this.looping_sounds = base.GetComponent<LoopingSounds>();
		base.OnSpawn();
		Vignette.Instance = this;
		this.defaultColor = this.image.color;
		Game.Instance.Subscribe(1983128072, new Action<object>(this.Refresh));
		Game.Instance.Subscribe(1585324898, new Action<object>(this.Refresh));
		Game.Instance.Subscribe(-1393151672, new Action<object>(this.Refresh));
		Game.Instance.Subscribe(-741654735, new Action<object>(this.Refresh));
		Game.Instance.Subscribe(-2062778933, new Action<object>(this.Refresh));
	}

	// Token: 0x0600B00E RID: 45070 RVA: 0x001126D7 File Offset: 0x001108D7
	public void SetColor(Color color)
	{
		this.image.color = color;
	}

	// Token: 0x0600B00F RID: 45071 RVA: 0x00422F78 File Offset: 0x00421178
	public void Refresh(object data)
	{
		AlertStateManager.Instance alertManager = ClusterManager.Instance.activeWorld.AlertManager;
		if (alertManager == null)
		{
			return;
		}
		if (alertManager.IsYellowAlert())
		{
			this.SetColor(this.yellowAlertColor);
			if (!this.showingYellowAlert)
			{
				this.looping_sounds.StartSound(GlobalAssets.GetSound("YellowAlert_LP", false), true, false, true);
				this.showingYellowAlert = true;
			}
		}
		else
		{
			this.showingYellowAlert = false;
			this.looping_sounds.StopSound(GlobalAssets.GetSound("YellowAlert_LP", false));
		}
		if (alertManager.IsRedAlert())
		{
			this.SetColor(this.redAlertColor);
			if (!this.showingRedAlert)
			{
				this.looping_sounds.StartSound(GlobalAssets.GetSound("RedAlert_LP", false), true, false, true);
				this.showingRedAlert = true;
			}
		}
		else
		{
			this.showingRedAlert = false;
			this.looping_sounds.StopSound(GlobalAssets.GetSound("RedAlert_LP", false));
		}
		if (!this.showingRedAlert && !this.showingYellowAlert)
		{
			this.Reset();
		}
	}

	// Token: 0x0600B010 RID: 45072 RVA: 0x00423068 File Offset: 0x00421268
	public void Reset()
	{
		this.SetColor(this.defaultColor);
		this.showingRedAlert = false;
		this.showingYellowAlert = false;
		this.looping_sounds.StopSound(GlobalAssets.GetSound("RedAlert_LP", false));
		this.looping_sounds.StopSound(GlobalAssets.GetSound("YellowAlert_LP", false));
	}

	// Token: 0x04008ABF RID: 35519
	[SerializeField]
	private Image image;

	// Token: 0x04008AC0 RID: 35520
	public Color defaultColor;

	// Token: 0x04008AC1 RID: 35521
	public Color redAlertColor = new Color(1f, 0f, 0f, 0.3f);

	// Token: 0x04008AC2 RID: 35522
	public Color yellowAlertColor = new Color(1f, 1f, 0f, 0.3f);

	// Token: 0x04008AC3 RID: 35523
	public static Vignette Instance;

	// Token: 0x04008AC4 RID: 35524
	private LoopingSounds looping_sounds;

	// Token: 0x04008AC5 RID: 35525
	private bool showingRedAlert;

	// Token: 0x04008AC6 RID: 35526
	private bool showingYellowAlert;
}
