using System;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/ProgressBar")]
public class ProgressBar : KMonoBehaviour
{
	public Image bar;

	private Func<float> updatePercentFull;

	private int overlayUpdateHandle = -1;

	public bool autoHide = true;

	private bool lastVisibilityValue = true;

	private bool hasBeenInitialize;

	public Color barColor
	{
		get
		{
			return bar.color;
		}
		set
		{
			bar.color = value;
		}
	}

	public float PercentFull
	{
		get
		{
			return bar.fillAmount;
		}
		set
		{
			bar.fillAmount = value;
		}
	}

	public void SetVisibility(bool visible)
	{
		lastVisibilityValue = visible;
		RefreshVisibility();
	}

	private void RefreshVisibility()
	{
		int myWorldId = base.gameObject.GetMyWorldId();
		bool flag = lastVisibilityValue;
		flag &= !hasBeenInitialize || myWorldId == ClusterManager.Instance.activeWorldId;
		flag &= !autoHide || SimDebugView.Instance == null || SimDebugView.Instance.GetMode() == OverlayModes.None.ID;
		base.gameObject.SetActive(flag);
		if (updatePercentFull == null || updatePercentFull.Target.IsNullOrDestroyed())
		{
			base.gameObject.SetActive(value: false);
		}
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		hasBeenInitialize = true;
		if (autoHide)
		{
			overlayUpdateHandle = Game.Instance.Subscribe(1798162660, OnOverlayChanged);
			if (SimDebugView.Instance != null && SimDebugView.Instance.GetMode() != OverlayModes.None.ID)
			{
				base.gameObject.SetActive(value: false);
			}
		}
		Game.Instance.Subscribe(1983128072, OnActiveWorldChanged);
		SetWorldActive(ClusterManager.Instance.activeWorldId);
		base.enabled = updatePercentFull != null;
		RefreshVisibility();
	}

	private void OnActiveWorldChanged(object data)
	{
		Tuple<int, int> tuple = (Tuple<int, int>)data;
		SetWorldActive(tuple.first);
	}

	private void SetWorldActive(int worldId)
	{
		RefreshVisibility();
	}

	public void SetUpdateFunc(Func<float> func)
	{
		updatePercentFull = func;
		base.enabled = updatePercentFull != null;
	}

	public virtual void Update()
	{
		if (updatePercentFull != null && !updatePercentFull.Target.IsNullOrDestroyed())
		{
			PercentFull = updatePercentFull();
		}
	}

	public virtual void OnOverlayChanged(object data = null)
	{
		RefreshVisibility();
	}

	public void Retarget(GameObject entity)
	{
		Vector3 position = entity.transform.GetPosition() + Vector3.down * 0.5f;
		Building component = entity.GetComponent<Building>();
		if (component != null)
		{
			position -= Vector3.right * 0.5f * (component.Def.WidthInCells % 2);
		}
		else
		{
			position -= Vector3.right * 0.5f;
		}
		base.transform.SetPosition(position);
	}

	protected override void OnCleanUp()
	{
		if (overlayUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(overlayUpdateHandle);
		}
		Game.Instance.Unsubscribe(1983128072, OnActiveWorldChanged);
		base.OnCleanUp();
	}

	private void OnBecameInvisible()
	{
		base.enabled = false;
	}

	private void OnBecameVisible()
	{
		base.enabled = true;
	}

	public static ProgressBar CreateProgressBar(GameObject entity, Func<float> updateFunc)
	{
		ProgressBar progressBar = Util.KInstantiateUI<ProgressBar>(ProgressBarsConfig.Instance.progressBarPrefab);
		progressBar.SetUpdateFunc(updateFunc);
		progressBar.transform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.transform);
		progressBar.name = ((entity != null) ? (entity.name + "_") : "") + " ProgressBar";
		progressBar.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("ProgressBar");
		progressBar.Update();
		progressBar.Retarget(entity);
		return progressBar;
	}
}
