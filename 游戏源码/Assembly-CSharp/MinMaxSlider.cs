using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002060 RID: 8288
[AddComponentMenu("KMonoBehaviour/scripts/MinMaxSlider")]
public class MinMaxSlider : KMonoBehaviour
{
	// Token: 0x17000B3B RID: 2875
	// (get) Token: 0x0600B062 RID: 45154 RVA: 0x00112B17 File Offset: 0x00110D17
	// (set) Token: 0x0600B063 RID: 45155 RVA: 0x00112B1F File Offset: 0x00110D1F
	public MinMaxSlider.Mode mode { get; private set; }

	// Token: 0x0600B064 RID: 45156 RVA: 0x0042429C File Offset: 0x0042249C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ToolTip component = base.transform.parent.gameObject.GetComponent<ToolTip>();
		if (component != null)
		{
			UnityEngine.Object.DestroyImmediate(this.toolTip);
			this.toolTip = component;
		}
		this.minSlider.value = this.currentMinValue;
		this.maxSlider.value = this.currentMaxValue;
		this.minSlider.interactable = this.interactable;
		this.maxSlider.interactable = this.interactable;
		this.minSlider.maxValue = this.maxLimit;
		this.maxSlider.maxValue = this.maxLimit;
		this.minSlider.minValue = this.minLimit;
		this.maxSlider.minValue = this.minLimit;
		this.minSlider.direction = (this.maxSlider.direction = this.direction);
		if (this.isOverPowered != null)
		{
			this.isOverPowered.enabled = false;
		}
		this.minSlider.gameObject.SetActive(false);
		if (this.mode != MinMaxSlider.Mode.Single)
		{
			this.minSlider.gameObject.SetActive(true);
		}
		if (this.extraSlider != null)
		{
			this.extraSlider.value = this.currentExtraValue;
			this.extraSlider.wholeNumbers = (this.minSlider.wholeNumbers = (this.maxSlider.wholeNumbers = this.wholeNumbers));
			this.extraSlider.direction = this.direction;
			this.extraSlider.interactable = this.interactable;
			this.extraSlider.maxValue = this.maxLimit;
			this.extraSlider.minValue = this.minLimit;
			this.extraSlider.gameObject.SetActive(false);
			if (this.mode == MinMaxSlider.Mode.Triple)
			{
				this.extraSlider.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x0600B065 RID: 45157 RVA: 0x0042448C File Offset: 0x0042268C
	public void SetIcon(Image newIcon)
	{
		this.icon = newIcon;
		this.icon.gameObject.transform.SetParent(base.transform);
		this.icon.gameObject.transform.SetAsFirstSibling();
		this.icon.rectTransform().anchoredPosition = Vector2.zero;
	}

	// Token: 0x0600B066 RID: 45158 RVA: 0x004244E8 File Offset: 0x004226E8
	public void SetMode(MinMaxSlider.Mode mode)
	{
		this.mode = mode;
		if (mode == MinMaxSlider.Mode.Single && this.extraSlider != null)
		{
			this.extraSlider.gameObject.SetActive(false);
			this.extraSlider.handleRect.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600B067 RID: 45159 RVA: 0x00112B28 File Offset: 0x00110D28
	private void SetAnchor(RectTransform trans, Vector2 min, Vector2 max)
	{
		trans.anchorMin = min;
		trans.anchorMax = max;
	}

	// Token: 0x0600B068 RID: 45160 RVA: 0x00424534 File Offset: 0x00422734
	public void SetMinMaxValue(float currentMin, float currentMax, float min, float max)
	{
		this.minSlider.value = currentMin;
		this.currentMinValue = currentMin;
		this.maxSlider.value = currentMax;
		this.currentMaxValue = currentMax;
		this.minLimit = min;
		this.maxLimit = max;
		this.minSlider.minValue = this.minLimit;
		this.maxSlider.minValue = this.minLimit;
		this.minSlider.maxValue = this.maxLimit;
		this.maxSlider.maxValue = this.maxLimit;
		if (this.extraSlider != null)
		{
			this.extraSlider.minValue = this.minLimit;
			this.extraSlider.maxValue = this.maxLimit;
		}
	}

	// Token: 0x0600B069 RID: 45161 RVA: 0x00112B38 File Offset: 0x00110D38
	public void SetExtraValue(float current)
	{
		this.extraSlider.value = current;
		this.toolTip.toolTip = base.transform.parent.name + ": " + current.ToString("F2");
	}

	// Token: 0x0600B06A RID: 45162 RVA: 0x004245F0 File Offset: 0x004227F0
	public void SetMaxValue(float current, float max)
	{
		float num = current / max * 100f;
		if (this.isOverPowered != null)
		{
			this.isOverPowered.enabled = (num > 100f);
		}
		this.maxSlider.value = Mathf.Min(100f, num);
		if (this.toolTip != null)
		{
			this.toolTip.toolTip = string.Concat(new string[]
			{
				base.transform.parent.name,
				": ",
				current.ToString("F2"),
				"/",
				max.ToString("F2")
			});
		}
	}

	// Token: 0x0600B06B RID: 45163 RVA: 0x004246A4 File Offset: 0x004228A4
	private void Update()
	{
		if (!this.interactable)
		{
			return;
		}
		this.minSlider.value = Mathf.Clamp(this.currentMinValue, this.minLimit, this.currentMinValue);
		this.maxSlider.value = Mathf.Max(this.minSlider.value, Mathf.Clamp(this.currentMaxValue, Mathf.Max(this.minSlider.value, this.minLimit), this.maxLimit));
		if (this.direction == Slider.Direction.LeftToRight || this.direction == Slider.Direction.RightToLeft)
		{
			this.minRect.anchorMax = new Vector2(this.minSlider.value / this.maxLimit, this.minRect.anchorMax.y);
			this.maxRect.anchorMax = new Vector2(this.maxSlider.value / this.maxLimit, this.maxRect.anchorMax.y);
			this.maxRect.anchorMin = new Vector2(this.minSlider.value / this.maxLimit, this.maxRect.anchorMin.y);
			return;
		}
		this.minRect.anchorMax = new Vector2(this.minRect.anchorMin.x, this.minSlider.value / this.maxLimit);
		this.maxRect.anchorMin = new Vector2(this.maxRect.anchorMin.x, this.minSlider.value / this.maxLimit);
	}

	// Token: 0x0600B06C RID: 45164 RVA: 0x00424830 File Offset: 0x00422A30
	public void OnMinValueChanged(float ignoreThis)
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockRange)
		{
			this.currentMaxValue = Mathf.Min(Mathf.Max(this.minLimit, this.minSlider.value) + this.range, this.maxLimit);
			this.currentMinValue = Mathf.Max(this.minLimit, Mathf.Min(this.maxSlider.value, this.currentMaxValue - this.range));
		}
		else
		{
			this.currentMinValue = Mathf.Clamp(this.minSlider.value, this.minLimit, Mathf.Min(this.maxSlider.value, this.currentMaxValue));
		}
		if (this.onMinChange != null)
		{
			this.onMinChange(this);
		}
	}

	// Token: 0x0600B06D RID: 45165 RVA: 0x004248F4 File Offset: 0x00422AF4
	public void OnMaxValueChanged(float ignoreThis)
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockRange)
		{
			this.currentMinValue = Mathf.Max(this.maxSlider.value - this.range, this.minLimit);
			this.currentMaxValue = Mathf.Max(this.minSlider.value, Mathf.Clamp(this.maxSlider.value, Mathf.Max(this.currentMinValue + this.range, this.minLimit), this.maxLimit));
		}
		else
		{
			this.currentMaxValue = Mathf.Max(this.minSlider.value, Mathf.Clamp(this.maxSlider.value, Mathf.Max(this.minSlider.value, this.minLimit), this.maxLimit));
		}
		if (this.onMaxChange != null)
		{
			this.onMaxChange(this);
		}
	}

	// Token: 0x0600B06E RID: 45166 RVA: 0x004249D4 File Offset: 0x00422BD4
	public void Lock(bool shouldLock)
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockType == MinMaxSlider.LockingType.Drag)
		{
			this.lockRange = shouldLock;
			this.range = this.maxSlider.value - this.minSlider.value;
			this.mousePos = KInputManager.GetMousePos();
		}
	}

	// Token: 0x0600B06F RID: 45167 RVA: 0x00424A24 File Offset: 0x00422C24
	public void ToggleLock()
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockType == MinMaxSlider.LockingType.Toggle)
		{
			this.lockRange = !this.lockRange;
			if (this.lockRange)
			{
				this.range = this.maxSlider.value - this.minSlider.value;
			}
		}
	}

	// Token: 0x0600B070 RID: 45168 RVA: 0x00424A78 File Offset: 0x00422C78
	public void OnDrag()
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockRange && this.lockType == MinMaxSlider.LockingType.Drag)
		{
			float num = KInputManager.GetMousePos().x - this.mousePos.x;
			if (this.direction == Slider.Direction.TopToBottom || this.direction == Slider.Direction.BottomToTop)
			{
				num = KInputManager.GetMousePos().y - this.mousePos.y;
			}
			this.currentMinValue = Mathf.Max(this.currentMinValue + num, this.minLimit);
			this.mousePos = KInputManager.GetMousePos();
		}
	}

	// Token: 0x04008B2E RID: 35630
	public MinMaxSlider.LockingType lockType = MinMaxSlider.LockingType.Drag;

	// Token: 0x04008B30 RID: 35632
	public bool lockRange;

	// Token: 0x04008B31 RID: 35633
	public bool interactable = true;

	// Token: 0x04008B32 RID: 35634
	public float minLimit;

	// Token: 0x04008B33 RID: 35635
	public float maxLimit = 100f;

	// Token: 0x04008B34 RID: 35636
	public float range = 50f;

	// Token: 0x04008B35 RID: 35637
	public float barWidth = 10f;

	// Token: 0x04008B36 RID: 35638
	public float barHeight = 100f;

	// Token: 0x04008B37 RID: 35639
	public float currentMinValue = 10f;

	// Token: 0x04008B38 RID: 35640
	public float currentMaxValue = 90f;

	// Token: 0x04008B39 RID: 35641
	public float currentExtraValue = 50f;

	// Token: 0x04008B3A RID: 35642
	public Slider.Direction direction;

	// Token: 0x04008B3B RID: 35643
	public bool wholeNumbers = true;

	// Token: 0x04008B3C RID: 35644
	public Action<MinMaxSlider> onMinChange;

	// Token: 0x04008B3D RID: 35645
	public Action<MinMaxSlider> onMaxChange;

	// Token: 0x04008B3E RID: 35646
	public Slider minSlider;

	// Token: 0x04008B3F RID: 35647
	public Slider maxSlider;

	// Token: 0x04008B40 RID: 35648
	public Slider extraSlider;

	// Token: 0x04008B41 RID: 35649
	public RectTransform minRect;

	// Token: 0x04008B42 RID: 35650
	public RectTransform maxRect;

	// Token: 0x04008B43 RID: 35651
	public RectTransform bgFill;

	// Token: 0x04008B44 RID: 35652
	public RectTransform mgFill;

	// Token: 0x04008B45 RID: 35653
	public RectTransform fgFill;

	// Token: 0x04008B46 RID: 35654
	public Text title;

	// Token: 0x04008B47 RID: 35655
	[MyCmpGet]
	public ToolTip toolTip;

	// Token: 0x04008B48 RID: 35656
	public Image icon;

	// Token: 0x04008B49 RID: 35657
	public Image isOverPowered;

	// Token: 0x04008B4A RID: 35658
	private Vector3 mousePos;

	// Token: 0x02002061 RID: 8289
	public enum LockingType
	{
		// Token: 0x04008B4C RID: 35660
		Toggle,
		// Token: 0x04008B4D RID: 35661
		Drag
	}

	// Token: 0x02002062 RID: 8290
	public enum Mode
	{
		// Token: 0x04008B4F RID: 35663
		Single,
		// Token: 0x04008B50 RID: 35664
		Double,
		// Token: 0x04008B51 RID: 35665
		Triple
	}
}
