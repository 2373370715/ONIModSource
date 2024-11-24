using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000914 RID: 2324
public abstract class KAnimControllerBase : MonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x06002947 RID: 10567 RVA: 0x001D5A50 File Offset: 0x001D3C50
	protected KAnimControllerBase()
	{
		this.previousFrame = -1;
		this.currentFrame = -1;
		this.PlaySpeedMultiplier = 1f;
		this.synchronizer = new KAnimSynchronizer(this);
		this.layering = new KAnimLayering(this, this.fgLayer);
		this.isVisible = true;
	}

	// Token: 0x06002948 RID: 10568
	public abstract KAnim.Anim GetAnim(int index);

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x06002949 RID: 10569 RVA: 0x000BAD4E File Offset: 0x000B8F4E
	// (set) Token: 0x0600294A RID: 10570 RVA: 0x000BAD56 File Offset: 0x000B8F56
	public string debugName { get; private set; }

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x0600294B RID: 10571 RVA: 0x000BAD5F File Offset: 0x000B8F5F
	// (set) Token: 0x0600294C RID: 10572 RVA: 0x000BAD67 File Offset: 0x000B8F67
	public KAnim.Build curBuild { get; protected set; }

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x0600294D RID: 10573 RVA: 0x001D5B5C File Offset: 0x001D3D5C
	// (remove) Token: 0x0600294E RID: 10574 RVA: 0x001D5B94 File Offset: 0x001D3D94
	public event Action<Color32> OnOverlayColourChanged;

	// Token: 0x17000135 RID: 309
	// (get) Token: 0x0600294F RID: 10575 RVA: 0x000BAD70 File Offset: 0x000B8F70
	// (set) Token: 0x06002950 RID: 10576 RVA: 0x000BAD78 File Offset: 0x000B8F78
	public new bool enabled
	{
		get
		{
			return this._enabled;
		}
		set
		{
			this._enabled = value;
			if (!this.hasAwakeRun)
			{
				return;
			}
			if (this._enabled)
			{
				this.Enable();
				return;
			}
			this.Disable();
		}
	}

	// Token: 0x17000136 RID: 310
	// (get) Token: 0x06002951 RID: 10577 RVA: 0x000BAD9F File Offset: 0x000B8F9F
	public bool HasBatchInstanceData
	{
		get
		{
			return this.batchInstanceData != null;
		}
	}

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x06002952 RID: 10578 RVA: 0x000BADAA File Offset: 0x000B8FAA
	// (set) Token: 0x06002953 RID: 10579 RVA: 0x000BADB2 File Offset: 0x000B8FB2
	public SymbolInstanceGpuData symbolInstanceGpuData { get; protected set; }

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x06002954 RID: 10580 RVA: 0x000BADBB File Offset: 0x000B8FBB
	// (set) Token: 0x06002955 RID: 10581 RVA: 0x000BADC3 File Offset: 0x000B8FC3
	public SymbolOverrideInfoGpuData symbolOverrideInfoGpuData { get; protected set; }

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x06002956 RID: 10582 RVA: 0x000BADCC File Offset: 0x000B8FCC
	// (set) Token: 0x06002957 RID: 10583 RVA: 0x001D5BCC File Offset: 0x001D3DCC
	public Color32 TintColour
	{
		get
		{
			return this.batchInstanceData.GetTintColour();
		}
		set
		{
			if (this.batchInstanceData != null && this.batchInstanceData.SetTintColour(value))
			{
				this.SetDirty();
				this.SuspendUpdates(false);
				if (this.OnTintChanged != null)
				{
					this.OnTintChanged(value);
				}
			}
		}
	}

	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06002958 RID: 10584 RVA: 0x000BADDE File Offset: 0x000B8FDE
	// (set) Token: 0x06002959 RID: 10585 RVA: 0x000BADF0 File Offset: 0x000B8FF0
	public Color32 HighlightColour
	{
		get
		{
			return this.batchInstanceData.GetHighlightcolour();
		}
		set
		{
			if (this.batchInstanceData.SetHighlightColour(value))
			{
				this.SetDirty();
				this.SuspendUpdates(false);
				if (this.OnHighlightChanged != null)
				{
					this.OnHighlightChanged(value);
				}
			}
		}
	}

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x0600295A RID: 10586 RVA: 0x000BAE2B File Offset: 0x000B902B
	// (set) Token: 0x0600295B RID: 10587 RVA: 0x000BAE38 File Offset: 0x000B9038
	public Color OverlayColour
	{
		get
		{
			return this.batchInstanceData.GetOverlayColour();
		}
		set
		{
			if (this.batchInstanceData.SetOverlayColour(value))
			{
				this.SetDirty();
				this.SuspendUpdates(false);
				if (this.OnOverlayColourChanged != null)
				{
					this.OnOverlayColourChanged(value);
				}
			}
		}
	}

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x0600295C RID: 10588 RVA: 0x001D5C1C File Offset: 0x001D3E1C
	// (remove) Token: 0x0600295D RID: 10589 RVA: 0x001D5C54 File Offset: 0x001D3E54
	public event KAnimControllerBase.KAnimEvent onAnimEnter;

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x0600295E RID: 10590 RVA: 0x001D5C8C File Offset: 0x001D3E8C
	// (remove) Token: 0x0600295F RID: 10591 RVA: 0x001D5CC4 File Offset: 0x001D3EC4
	public event KAnimControllerBase.KAnimEvent onAnimComplete;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06002960 RID: 10592 RVA: 0x001D5CFC File Offset: 0x001D3EFC
	// (remove) Token: 0x06002961 RID: 10593 RVA: 0x001D5D34 File Offset: 0x001D3F34
	public event Action<int> onLayerChanged;

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x06002962 RID: 10594 RVA: 0x000BAE6E File Offset: 0x000B906E
	// (set) Token: 0x06002963 RID: 10595 RVA: 0x000BAE76 File Offset: 0x000B9076
	public int previousFrame { get; protected set; }

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x06002964 RID: 10596 RVA: 0x000BAE7F File Offset: 0x000B907F
	// (set) Token: 0x06002965 RID: 10597 RVA: 0x000BAE87 File Offset: 0x000B9087
	public int currentFrame { get; protected set; }

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x06002966 RID: 10598 RVA: 0x001D5D6C File Offset: 0x001D3F6C
	public HashedString currentAnim
	{
		get
		{
			if (this.curAnim == null)
			{
				return default(HashedString);
			}
			return this.curAnim.hash;
		}
	}

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x06002968 RID: 10600 RVA: 0x000BAE99 File Offset: 0x000B9099
	// (set) Token: 0x06002967 RID: 10599 RVA: 0x000BAE90 File Offset: 0x000B9090
	public float PlaySpeedMultiplier { get; set; }

	// Token: 0x06002969 RID: 10601 RVA: 0x000BAEA1 File Offset: 0x000B90A1
	public void SetFGLayer(Grid.SceneLayer layer)
	{
		this.fgLayer = layer;
		this.GetLayering();
		if (this.layering != null)
		{
			this.layering.SetLayer(this.fgLayer);
		}
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x0600296A RID: 10602 RVA: 0x000BAECA File Offset: 0x000B90CA
	// (set) Token: 0x0600296B RID: 10603 RVA: 0x000BAED2 File Offset: 0x000B90D2
	public KAnim.PlayMode PlayMode
	{
		get
		{
			return this.mode;
		}
		set
		{
			this.mode = value;
		}
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x0600296C RID: 10604 RVA: 0x000BAEDB File Offset: 0x000B90DB
	// (set) Token: 0x0600296D RID: 10605 RVA: 0x000BAEE3 File Offset: 0x000B90E3
	public bool FlipX
	{
		get
		{
			return this.flipX;
		}
		set
		{
			this.flipX = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.SetDirty();
		}
	}

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x0600296E RID: 10606 RVA: 0x000BAF05 File Offset: 0x000B9105
	// (set) Token: 0x0600296F RID: 10607 RVA: 0x000BAF0D File Offset: 0x000B910D
	public bool FlipY
	{
		get
		{
			return this.flipY;
		}
		set
		{
			this.flipY = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.SetDirty();
		}
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x06002970 RID: 10608 RVA: 0x000BAF2F File Offset: 0x000B912F
	// (set) Token: 0x06002971 RID: 10609 RVA: 0x000BAF37 File Offset: 0x000B9137
	public Vector3 Offset
	{
		get
		{
			return this.offset;
		}
		set
		{
			this.offset = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.DeRegister();
			this.Register();
			this.RefreshVisibilityListener();
			this.SetDirty();
		}
	}

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06002972 RID: 10610 RVA: 0x000BAF6B File Offset: 0x000B916B
	// (set) Token: 0x06002973 RID: 10611 RVA: 0x000BAF73 File Offset: 0x000B9173
	public float Rotation
	{
		get
		{
			return this.rotation;
		}
		set
		{
			this.rotation = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.SetDirty();
		}
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06002974 RID: 10612 RVA: 0x000BAF95 File Offset: 0x000B9195
	// (set) Token: 0x06002975 RID: 10613 RVA: 0x000BAF9D File Offset: 0x000B919D
	public Vector3 Pivot
	{
		get
		{
			return this.pivot;
		}
		set
		{
			this.pivot = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.SetDirty();
		}
	}

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x06002976 RID: 10614 RVA: 0x000BAFBF File Offset: 0x000B91BF
	public Vector3 PositionIncludingOffset
	{
		get
		{
			return base.transform.GetPosition() + this.Offset;
		}
	}

	// Token: 0x06002977 RID: 10615 RVA: 0x000BAFD7 File Offset: 0x000B91D7
	public KAnimBatchGroup.MaterialType GetMaterialType()
	{
		return this.materialType;
	}

	// Token: 0x06002978 RID: 10616 RVA: 0x001D5D98 File Offset: 0x001D3F98
	public Vector3 GetWorldPivot()
	{
		Vector3 position = base.transform.GetPosition();
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		if (component != null)
		{
			position.x += component.offset.x;
			position.y += component.offset.y - component.size.y / 2f;
		}
		return position;
	}

	// Token: 0x06002979 RID: 10617 RVA: 0x000BAFDF File Offset: 0x000B91DF
	public KAnim.Anim GetCurrentAnim()
	{
		return this.curAnim;
	}

	// Token: 0x0600297A RID: 10618 RVA: 0x000BAFE7 File Offset: 0x000B91E7
	public KAnimHashedString GetBuildHash()
	{
		if (this.curBuild == null)
		{
			return KAnimBatchManager.NO_BATCH;
		}
		return this.curBuild.fileHash;
	}

	// Token: 0x0600297B RID: 10619 RVA: 0x000BB007 File Offset: 0x000B9207
	protected float GetDuration()
	{
		if (this.curAnim != null)
		{
			return (float)this.curAnim.numFrames / this.curAnim.frameRate;
		}
		return 0f;
	}

	// Token: 0x0600297C RID: 10620 RVA: 0x001D5E00 File Offset: 0x001D4000
	protected int GetFrameIdxFromOffset(int offset)
	{
		int result = -1;
		if (this.curAnim != null)
		{
			result = offset + this.curAnim.firstFrameIdx;
		}
		return result;
	}

	// Token: 0x0600297D RID: 10621 RVA: 0x001D5E28 File Offset: 0x001D4028
	public int GetFrameIdx(float time, bool absolute)
	{
		int result = -1;
		if (this.curAnim != null)
		{
			result = this.curAnim.GetFrameIdx(this.mode, time) + (absolute ? this.curAnim.firstFrameIdx : 0);
		}
		return result;
	}

	// Token: 0x0600297E RID: 10622 RVA: 0x000BB02F File Offset: 0x000B922F
	public bool IsStopped()
	{
		return this.stopped;
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x0600297F RID: 10623 RVA: 0x000BAFDF File Offset: 0x000B91DF
	public KAnim.Anim CurrentAnim
	{
		get
		{
			return this.curAnim;
		}
	}

	// Token: 0x06002980 RID: 10624 RVA: 0x000BB037 File Offset: 0x000B9237
	public KAnimSynchronizer GetSynchronizer()
	{
		return this.synchronizer;
	}

	// Token: 0x06002981 RID: 10625 RVA: 0x000BB03F File Offset: 0x000B923F
	public KAnimLayering GetLayering()
	{
		if (this.layering == null && this.fgLayer != Grid.SceneLayer.NoLayer)
		{
			this.layering = new KAnimLayering(this, this.fgLayer);
		}
		return this.layering;
	}

	// Token: 0x06002982 RID: 10626 RVA: 0x000BAECA File Offset: 0x000B90CA
	public KAnim.PlayMode GetMode()
	{
		return this.mode;
	}

	// Token: 0x06002983 RID: 10627 RVA: 0x000BB06B File Offset: 0x000B926B
	public static string GetModeString(KAnim.PlayMode mode)
	{
		switch (mode)
		{
		case KAnim.PlayMode.Loop:
			return "Loop";
		case KAnim.PlayMode.Once:
			return "Once";
		case KAnim.PlayMode.Paused:
			return "Paused";
		default:
			return "Unknown";
		}
	}

	// Token: 0x06002984 RID: 10628 RVA: 0x000BB098 File Offset: 0x000B9298
	public float GetPlaySpeed()
	{
		return this.playSpeed;
	}

	// Token: 0x06002985 RID: 10629 RVA: 0x000BB0A0 File Offset: 0x000B92A0
	public void SetElapsedTime(float value)
	{
		this.elapsedTime = value;
	}

	// Token: 0x06002986 RID: 10630 RVA: 0x000BB0A9 File Offset: 0x000B92A9
	public float GetElapsedTime()
	{
		return this.elapsedTime;
	}

	// Token: 0x06002987 RID: 10631
	protected abstract void SuspendUpdates(bool suspend);

	// Token: 0x06002988 RID: 10632
	protected abstract void OnStartQueuedAnim();

	// Token: 0x06002989 RID: 10633
	public abstract void SetDirty();

	// Token: 0x0600298A RID: 10634
	protected abstract void RefreshVisibilityListener();

	// Token: 0x0600298B RID: 10635
	protected abstract void DeRegister();

	// Token: 0x0600298C RID: 10636
	protected abstract void Register();

	// Token: 0x0600298D RID: 10637
	protected abstract void OnAwake();

	// Token: 0x0600298E RID: 10638
	protected abstract void OnStart();

	// Token: 0x0600298F RID: 10639
	protected abstract void OnStop();

	// Token: 0x06002990 RID: 10640
	protected abstract void Enable();

	// Token: 0x06002991 RID: 10641
	protected abstract void Disable();

	// Token: 0x06002992 RID: 10642
	protected abstract void UpdateFrame(float t);

	// Token: 0x06002993 RID: 10643
	public abstract Matrix2x3 GetTransformMatrix();

	// Token: 0x06002994 RID: 10644
	public abstract Matrix2x3 GetSymbolLocalTransform(HashedString symbol, out bool symbolVisible);

	// Token: 0x06002995 RID: 10645
	public abstract void UpdateAllHiddenSymbols();

	// Token: 0x06002996 RID: 10646
	public abstract void UpdateHiddenSymbol(KAnimHashedString specificSymbol);

	// Token: 0x06002997 RID: 10647
	public abstract void UpdateHiddenSymbolSet(HashSet<KAnimHashedString> specificSymbols);

	// Token: 0x06002998 RID: 10648
	public abstract void TriggerStop();

	// Token: 0x06002999 RID: 10649 RVA: 0x000BB0B1 File Offset: 0x000B92B1
	public virtual void SetLayer(int layer)
	{
		if (this.onLayerChanged != null)
		{
			this.onLayerChanged(layer);
		}
	}

	// Token: 0x0600299A RID: 10650 RVA: 0x001D5E68 File Offset: 0x001D4068
	public Vector3 GetPivotSymbolPosition()
	{
		bool flag = false;
		Matrix4x4 symbolTransform = this.GetSymbolTransform(KAnimControllerBase.snaptoPivot, out flag);
		Vector3 position = base.transform.GetPosition();
		if (flag)
		{
			position = new Vector3(symbolTransform[0, 3], symbolTransform[1, 3], symbolTransform[2, 3]);
		}
		return position;
	}

	// Token: 0x0600299B RID: 10651 RVA: 0x000BB0C7 File Offset: 0x000B92C7
	public virtual Matrix4x4 GetSymbolTransform(HashedString symbol, out bool symbolVisible)
	{
		symbolVisible = false;
		return Matrix4x4.identity;
	}

	// Token: 0x0600299C RID: 10652 RVA: 0x001D5EB8 File Offset: 0x001D40B8
	private void Awake()
	{
		this.aem = Singleton<AnimEventManager>.Instance;
		this.debugName = base.name;
		this.SetFGLayer(this.fgLayer);
		this.OnAwake();
		if (!string.IsNullOrEmpty(this.initialAnim))
		{
			this.SetDirty();
			this.Play(this.initialAnim, this.initialMode, 1f, 0f);
		}
		this.hasAwakeRun = true;
	}

	// Token: 0x0600299D RID: 10653 RVA: 0x000BB0D1 File Offset: 0x000B92D1
	private void Start()
	{
		this.OnStart();
	}

	// Token: 0x0600299E RID: 10654 RVA: 0x001D5F2C File Offset: 0x001D412C
	protected virtual void OnDestroy()
	{
		this.animFiles = null;
		this.curAnim = null;
		this.curBuild = null;
		this.synchronizer = null;
		this.layering = null;
		this.animQueue = null;
		this.overrideAnims = null;
		this.anims = null;
		this.synchronizer = null;
		this.layering = null;
		this.overrideAnimFiles = null;
	}

	// Token: 0x0600299F RID: 10655 RVA: 0x000BB0D9 File Offset: 0x000B92D9
	protected void AnimEnter(HashedString hashed_name)
	{
		if (this.onAnimEnter != null)
		{
			this.onAnimEnter(hashed_name);
		}
	}

	// Token: 0x060029A0 RID: 10656 RVA: 0x000BB0EF File Offset: 0x000B92EF
	public void Play(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		if (!this.stopped)
		{
			this.Stop();
		}
		this.Queue(anim_name, mode, speed, time_offset);
	}

	// Token: 0x060029A1 RID: 10657 RVA: 0x001D5F88 File Offset: 0x001D4188
	public void Play(HashedString[] anim_names, KAnim.PlayMode mode = KAnim.PlayMode.Once)
	{
		if (!this.stopped)
		{
			this.Stop();
		}
		for (int i = 0; i < anim_names.Length - 1; i++)
		{
			this.Queue(anim_names[i], KAnim.PlayMode.Once, 1f, 0f);
		}
		global::Debug.Assert(anim_names.Length != 0, "Play was called with an empty anim array");
		this.Queue(anim_names[anim_names.Length - 1], mode, 1f, 0f);
	}

	// Token: 0x060029A2 RID: 10658 RVA: 0x001D5FF8 File Offset: 0x001D41F8
	public void Queue(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		this.animQueue.Enqueue(new KAnimControllerBase.AnimData
		{
			anim = anim_name,
			mode = mode,
			speed = speed,
			timeOffset = time_offset
		});
		this.mode = ((mode == KAnim.PlayMode.Paused) ? KAnim.PlayMode.Paused : KAnim.PlayMode.Once);
		if (this.aem != null)
		{
			this.aem.SetMode(this.eventManagerHandle, this.mode);
		}
		if (this.animQueue.Count == 1 && this.stopped)
		{
			this.StartQueuedAnim();
		}
	}

	// Token: 0x060029A3 RID: 10659 RVA: 0x000BB10A File Offset: 0x000B930A
	public void QueueAndSyncTransition(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		this.SyncTransition();
		this.Queue(anim_name, mode, speed, time_offset);
	}

	// Token: 0x060029A4 RID: 10660 RVA: 0x000BB11D File Offset: 0x000B931D
	public void SyncTransition()
	{
		this.elapsedTime %= Mathf.Max(float.Epsilon, this.GetDuration());
	}

	// Token: 0x060029A5 RID: 10661 RVA: 0x000BB13C File Offset: 0x000B933C
	public void ClearQueue()
	{
		this.animQueue.Clear();
	}

	// Token: 0x060029A6 RID: 10662 RVA: 0x001D6084 File Offset: 0x001D4284
	private void Restart(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		if (this.curBuild == null)
		{
			string[] array = new string[5];
			array[0] = "[";
			array[1] = base.gameObject.name;
			array[2] = "] Missing build while trying to play anim [";
			int num = 3;
			HashedString hashedString = anim_name;
			array[num] = hashedString.ToString();
			array[4] = "]";
			global::Debug.LogWarning(string.Concat(array), base.gameObject);
			return;
		}
		Queue<KAnimControllerBase.AnimData> queue = new Queue<KAnimControllerBase.AnimData>();
		queue.Enqueue(new KAnimControllerBase.AnimData
		{
			anim = anim_name,
			mode = mode,
			speed = speed,
			timeOffset = time_offset
		});
		while (this.animQueue.Count > 0)
		{
			queue.Enqueue(this.animQueue.Dequeue());
		}
		this.animQueue = queue;
		if (this.animQueue.Count == 1 && this.stopped)
		{
			this.StartQueuedAnim();
		}
	}

	// Token: 0x060029A7 RID: 10663 RVA: 0x001D6164 File Offset: 0x001D4364
	protected void StartQueuedAnim()
	{
		this.StopAnimEventSequence();
		this.previousFrame = -1;
		this.currentFrame = -1;
		this.SuspendUpdates(false);
		this.stopped = false;
		this.OnStartQueuedAnim();
		KAnimControllerBase.AnimData animData = this.animQueue.Dequeue();
		while (animData.mode == KAnim.PlayMode.Loop && this.animQueue.Count > 0)
		{
			animData = this.animQueue.Dequeue();
		}
		KAnimControllerBase.AnimLookupData animLookupData;
		if (this.overrideAnims == null || !this.overrideAnims.TryGetValue(animData.anim, out animLookupData))
		{
			if (!this.anims.TryGetValue(animData.anim, out animLookupData))
			{
				bool flag = true;
				if (this.showWhenMissing != null)
				{
					this.showWhenMissing.SetActive(true);
				}
				if (flag)
				{
					this.TriggerStop();
					return;
				}
			}
			else if (this.showWhenMissing != null)
			{
				this.showWhenMissing.SetActive(false);
			}
		}
		this.curAnim = this.GetAnim(animLookupData.animIndex);
		int num = 0;
		if (animData.mode == KAnim.PlayMode.Loop && this.randomiseLoopedOffset)
		{
			num = UnityEngine.Random.Range(0, this.curAnim.numFrames - 1);
		}
		this.prevAnimFrame = -1;
		this.curAnimFrameIdx = this.GetFrameIdxFromOffset(num);
		this.currentFrame = this.curAnimFrameIdx;
		this.mode = animData.mode;
		this.playSpeed = animData.speed * this.PlaySpeedMultiplier;
		this.SetElapsedTime((float)num / this.curAnim.frameRate + animData.timeOffset);
		this.synchronizer.Sync();
		this.StartAnimEventSequence();
		this.AnimEnter(animData.anim);
	}

	// Token: 0x060029A8 RID: 10664 RVA: 0x000BB149 File Offset: 0x000B9349
	public bool GetSymbolVisiblity(KAnimHashedString symbol)
	{
		return !this.hiddenSymbolsSet.Contains(symbol);
	}

	// Token: 0x060029A9 RID: 10665 RVA: 0x000BB15A File Offset: 0x000B935A
	public void SetSymbolVisiblity(KAnimHashedString symbol, bool is_visible)
	{
		if (is_visible)
		{
			this.hiddenSymbolsSet.Remove(symbol);
		}
		else if (!this.hiddenSymbolsSet.Contains(symbol))
		{
			this.hiddenSymbolsSet.Add(symbol);
		}
		if (this.curBuild != null)
		{
			this.UpdateHiddenSymbol(symbol);
		}
	}

	// Token: 0x060029AA RID: 10666 RVA: 0x001D62E8 File Offset: 0x001D44E8
	public void BatchSetSymbolsVisiblity(HashSet<KAnimHashedString> symbols, bool is_visible)
	{
		foreach (KAnimHashedString item in symbols)
		{
			if (is_visible)
			{
				this.hiddenSymbolsSet.Remove(item);
			}
			else if (!this.hiddenSymbolsSet.Contains(item))
			{
				this.hiddenSymbolsSet.Add(item);
			}
		}
		if (this.curBuild != null)
		{
			this.UpdateHiddenSymbolSet(symbols);
		}
	}

	// Token: 0x060029AB RID: 10667 RVA: 0x001D636C File Offset: 0x001D456C
	public void AddAnimOverrides(KAnimFile kanim_file, float priority = 0f)
	{
		if (kanim_file == null)
		{
			global::Debug.LogError(string.Format("AddAnimOverrides tried to add a null override to {0} at position {1}", base.gameObject.name, base.transform.position));
		}
		if (kanim_file.GetData().build != null && kanim_file.GetData().build.symbols.Length != 0)
		{
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			DebugUtil.Assert(component != null, "Anim overrides containing additional symbols require a symbol override controller.");
			component.AddBuildOverride(kanim_file.GetData(), 0);
		}
		this.overrideAnimFiles.Add(new KAnimControllerBase.OverrideAnimFileData
		{
			priority = priority,
			file = kanim_file
		});
		this.overrideAnimFiles.Sort((KAnimControllerBase.OverrideAnimFileData a, KAnimControllerBase.OverrideAnimFileData b) => b.priority.CompareTo(a.priority));
		this.RebuildOverrides(kanim_file);
	}

	// Token: 0x060029AC RID: 10668 RVA: 0x001D6444 File Offset: 0x001D4644
	public void RemoveAnimOverrides(KAnimFile kanim_file)
	{
		if (kanim_file == null)
		{
			global::Debug.LogError(string.Format("RemoveAnimOverrides tried to add a null override to {0} at position {1}", base.gameObject.name, base.transform.position));
		}
		if (kanim_file.GetData().build != null && kanim_file.GetData().build.symbols.Length != 0)
		{
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			DebugUtil.Assert(component != null, "Anim overrides containing additional symbols require a symbol override controller.");
			component.TryRemoveBuildOverride(kanim_file.GetData(), 0);
		}
		for (int i = 0; i < this.overrideAnimFiles.Count; i++)
		{
			if (this.overrideAnimFiles[i].file == kanim_file)
			{
				this.overrideAnimFiles.RemoveAt(i);
				break;
			}
		}
		this.RebuildOverrides(kanim_file);
	}

	// Token: 0x060029AD RID: 10669 RVA: 0x001D650C File Offset: 0x001D470C
	private void RebuildOverrides(KAnimFile kanim_file)
	{
		bool flag = false;
		this.overrideAnims.Clear();
		for (int i = 0; i < this.overrideAnimFiles.Count; i++)
		{
			KAnimControllerBase.OverrideAnimFileData overrideAnimFileData = this.overrideAnimFiles[i];
			KAnimFileData data = overrideAnimFileData.file.GetData();
			for (int j = 0; j < data.animCount; j++)
			{
				KAnim.Anim anim = data.GetAnim(j);
				if (anim.animFile.hashName != data.hashName)
				{
					global::Debug.LogError(string.Format("How did we get an anim from another file? [{0}] != [{1}] for anim [{2}]", data.name, anim.animFile.name, j));
				}
				KAnimControllerBase.AnimLookupData value = default(KAnimControllerBase.AnimLookupData);
				value.animIndex = anim.index;
				HashedString hashedString = new HashedString(anim.name);
				if (!this.overrideAnims.ContainsKey(hashedString))
				{
					this.overrideAnims[hashedString] = value;
				}
				if (this.curAnim != null && this.curAnim.hash == hashedString && overrideAnimFileData.file == kanim_file)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.Restart(this.curAnim.name, this.mode, this.playSpeed, 0f);
		}
	}

	// Token: 0x060029AE RID: 10670 RVA: 0x001D665C File Offset: 0x001D485C
	public bool HasAnimation(HashedString anim_name)
	{
		bool flag = anim_name.IsValid;
		if (flag)
		{
			bool flag2 = this.anims.ContainsKey(anim_name);
			bool flag3 = !flag2 && this.overrideAnims.ContainsKey(anim_name);
			flag = (flag2 || flag3);
		}
		return flag;
	}

	// Token: 0x060029AF RID: 10671 RVA: 0x001D6698 File Offset: 0x001D4898
	public bool HasAnimationFile(KAnimHashedString anim_file_name)
	{
		KAnimFile kanimFile = null;
		return this.TryGetAnimationFile(anim_file_name, out kanimFile);
	}

	// Token: 0x060029B0 RID: 10672 RVA: 0x001D66B0 File Offset: 0x001D48B0
	public bool TryGetAnimationFile(KAnimHashedString anim_file_name, out KAnimFile match)
	{
		match = null;
		if (!anim_file_name.IsValid())
		{
			return false;
		}
		KAnimFileData kanimFileData = null;
		int num = 0;
		int num2 = this.overrideAnimFiles.Count - 1;
		int num3 = (int)((float)this.overrideAnimFiles.Count * 0.5f);
		while (num3 > 0 && match == null && num < num3)
		{
			if (this.overrideAnimFiles[num].file != null)
			{
				kanimFileData = this.overrideAnimFiles[num].file.GetData();
			}
			if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
			{
				match = this.overrideAnimFiles[num].file;
				break;
			}
			if (this.overrideAnimFiles[num2].file != null)
			{
				kanimFileData = this.overrideAnimFiles[num2].file.GetData();
			}
			if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
			{
				match = this.overrideAnimFiles[num2].file;
			}
			num++;
			num2--;
		}
		if (match == null && this.overrideAnimFiles.Count % 2 != 0)
		{
			if (this.overrideAnimFiles[num].file != null)
			{
				kanimFileData = this.overrideAnimFiles[num].file.GetData();
			}
			if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
			{
				match = this.overrideAnimFiles[num].file;
			}
		}
		kanimFileData = null;
		if (match == null && this.animFiles != null)
		{
			num = 0;
			num2 = this.animFiles.Length - 1;
			num3 = (int)((float)this.animFiles.Length * 0.5f);
			while (num3 > 0 && match == null && num < num3)
			{
				if (this.animFiles[num] != null)
				{
					kanimFileData = this.animFiles[num].GetData();
				}
				if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
				{
					match = this.animFiles[num];
					break;
				}
				if (this.animFiles[num2] != null)
				{
					kanimFileData = this.animFiles[num2].GetData();
				}
				if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
				{
					match = this.animFiles[num2];
				}
				num++;
				num2--;
			}
			if (match == null && this.animFiles.Length % 2 != 0)
			{
				if (this.animFiles[num] != null)
				{
					kanimFileData = this.animFiles[num].GetData();
				}
				if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
				{
					match = this.animFiles[num];
				}
			}
		}
		return match != null;
	}

	// Token: 0x060029B1 RID: 10673 RVA: 0x001D698C File Offset: 0x001D4B8C
	public void AddAnims(KAnimFile anim_file)
	{
		KAnimFileData data = anim_file.GetData();
		if (data == null)
		{
			global::Debug.LogError("AddAnims() Null animfile data");
			return;
		}
		this.maxSymbols = Mathf.Max(this.maxSymbols, data.maxVisSymbolFrames);
		for (int i = 0; i < data.animCount; i++)
		{
			KAnim.Anim anim = data.GetAnim(i);
			if (anim.animFile.hashName != data.hashName)
			{
				global::Debug.LogErrorFormat("How did we get an anim from another file? [{0}] != [{1}] for anim [{2}]", new object[]
				{
					data.name,
					anim.animFile.name,
					i
				});
			}
			this.anims[anim.hash] = new KAnimControllerBase.AnimLookupData
			{
				animIndex = anim.index
			};
		}
		if (this.usingNewSymbolOverrideSystem && data.buildIndex != -1 && data.build.symbols != null && data.build.symbols.Length != 0)
		{
			base.GetComponent<SymbolOverrideController>().AddBuildOverride(anim_file.GetData(), -1);
		}
	}

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x060029B2 RID: 10674 RVA: 0x000BB198 File Offset: 0x000B9398
	// (set) Token: 0x060029B3 RID: 10675 RVA: 0x001D6A90 File Offset: 0x001D4C90
	public KAnimFile[] AnimFiles
	{
		get
		{
			return this.animFiles;
		}
		set
		{
			DebugUtil.AssertArgs(value.Length != 0, new object[]
			{
				"Controller has no anim files.",
				base.gameObject
			});
			DebugUtil.AssertArgs(value[0] != null, new object[]
			{
				"First anim file needs to be non-null.",
				base.gameObject
			});
			DebugUtil.AssertArgs(value[0].IsBuildLoaded, new object[]
			{
				"First anim file needs to be the build file.",
				base.gameObject
			});
			for (int i = 0; i < value.Length; i++)
			{
				DebugUtil.AssertArgs(value[i] != null, new object[]
				{
					"Anim file is null",
					base.gameObject
				});
			}
			this.animFiles = new KAnimFile[value.Length];
			for (int j = 0; j < value.Length; j++)
			{
				this.animFiles[j] = value[j];
			}
		}
	}

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x060029B4 RID: 10676 RVA: 0x000BB1A0 File Offset: 0x000B93A0
	public IReadOnlyList<KAnimControllerBase.OverrideAnimFileData> OverrideAnimFiles
	{
		get
		{
			return this.overrideAnimFiles;
		}
	}

	// Token: 0x060029B5 RID: 10677 RVA: 0x001D6B64 File Offset: 0x001D4D64
	public void Stop()
	{
		if (this.curAnim != null)
		{
			this.StopAnimEventSequence();
		}
		this.animQueue.Clear();
		this.stopped = true;
		if (this.onAnimComplete != null)
		{
			this.onAnimComplete((this.curAnim == null) ? HashedString.Invalid : this.curAnim.hash);
		}
		this.OnStop();
	}

	// Token: 0x060029B6 RID: 10678 RVA: 0x001D6BC4 File Offset: 0x001D4DC4
	public void StopAndClear()
	{
		if (!this.stopped)
		{
			this.Stop();
		}
		this.bounds.center = Vector3.zero;
		this.bounds.extents = Vector3.zero;
		if (this.OnUpdateBounds != null)
		{
			this.OnUpdateBounds(this.bounds);
		}
	}

	// Token: 0x060029B7 RID: 10679 RVA: 0x000BB1A8 File Offset: 0x000B93A8
	public float GetPositionPercent()
	{
		return this.GetElapsedTime() / this.GetDuration();
	}

	// Token: 0x060029B8 RID: 10680 RVA: 0x001D6C18 File Offset: 0x001D4E18
	public void SetPositionPercent(float percent)
	{
		if (this.curAnim == null)
		{
			return;
		}
		this.SetElapsedTime(percent * (float)this.curAnim.numFrames / this.curAnim.frameRate);
		int frameIdx = this.curAnim.GetFrameIdx(this.mode, this.elapsedTime);
		if (this.currentFrame != frameIdx)
		{
			this.SetDirty();
			this.UpdateAnimEventSequenceTime();
			this.SuspendUpdates(false);
		}
	}

	// Token: 0x060029B9 RID: 10681 RVA: 0x001D6C84 File Offset: 0x001D4E84
	protected void StartAnimEventSequence()
	{
		if (!this.layering.GetIsForeground() && this.aem != null)
		{
			this.eventManagerHandle = this.aem.PlayAnim(this, this.curAnim, this.mode, this.elapsedTime, this.visibilityType == KAnimControllerBase.VisibilityType.Always);
		}
	}

	// Token: 0x060029BA RID: 10682 RVA: 0x000BB1B7 File Offset: 0x000B93B7
	protected void UpdateAnimEventSequenceTime()
	{
		if (this.eventManagerHandle.IsValid() && this.aem != null)
		{
			this.aem.SetElapsedTime(this.eventManagerHandle, this.elapsedTime);
		}
	}

	// Token: 0x060029BB RID: 10683 RVA: 0x001D6CD4 File Offset: 0x001D4ED4
	protected void StopAnimEventSequence()
	{
		if (this.eventManagerHandle.IsValid() && this.aem != null)
		{
			if (!this.stopped && this.mode != KAnim.PlayMode.Paused)
			{
				this.SetElapsedTime(this.aem.GetElapsedTime(this.eventManagerHandle));
			}
			this.aem.StopAnim(this.eventManagerHandle);
			this.eventManagerHandle = HandleVector<int>.InvalidHandle;
		}
	}

	// Token: 0x060029BC RID: 10684 RVA: 0x000BB1E5 File Offset: 0x000B93E5
	protected void DestroySelf()
	{
		if (this.onDestroySelf != null)
		{
			this.onDestroySelf(base.gameObject);
			return;
		}
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060029BD RID: 10685 RVA: 0x000BB20C File Offset: 0x000B940C
	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
		this.hiddenSymbols.Clear();
		this.hiddenSymbols = new List<KAnimHashedString>(this.hiddenSymbolsSet);
	}

	// Token: 0x060029BE RID: 10686 RVA: 0x000BB22A File Offset: 0x000B942A
	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		this.hiddenSymbolsSet = new HashSet<KAnimHashedString>(this.hiddenSymbols);
		this.hiddenSymbols.Clear();
	}

	// Token: 0x04001B89 RID: 7049
	[NonSerialized]
	public GameObject showWhenMissing;

	// Token: 0x04001B8A RID: 7050
	[SerializeField]
	public KAnimBatchGroup.MaterialType materialType;

	// Token: 0x04001B8B RID: 7051
	[SerializeField]
	public string initialAnim;

	// Token: 0x04001B8C RID: 7052
	[SerializeField]
	public KAnim.PlayMode initialMode = KAnim.PlayMode.Once;

	// Token: 0x04001B8D RID: 7053
	[SerializeField]
	protected KAnimFile[] animFiles = new KAnimFile[0];

	// Token: 0x04001B8E RID: 7054
	[SerializeField]
	protected Vector3 offset;

	// Token: 0x04001B8F RID: 7055
	[SerializeField]
	protected Vector3 pivot;

	// Token: 0x04001B90 RID: 7056
	[SerializeField]
	protected float rotation;

	// Token: 0x04001B91 RID: 7057
	[SerializeField]
	public bool destroyOnAnimComplete;

	// Token: 0x04001B92 RID: 7058
	[SerializeField]
	public bool inactiveDisable;

	// Token: 0x04001B93 RID: 7059
	[SerializeField]
	protected bool flipX;

	// Token: 0x04001B94 RID: 7060
	[SerializeField]
	protected bool flipY;

	// Token: 0x04001B95 RID: 7061
	[SerializeField]
	public bool forceUseGameTime;

	// Token: 0x04001B96 RID: 7062
	public string defaultAnim;

	// Token: 0x04001B98 RID: 7064
	protected KAnim.Anim curAnim;

	// Token: 0x04001B99 RID: 7065
	protected int curAnimFrameIdx = -1;

	// Token: 0x04001B9A RID: 7066
	protected int prevAnimFrame = -1;

	// Token: 0x04001B9B RID: 7067
	public bool usingNewSymbolOverrideSystem;

	// Token: 0x04001B9D RID: 7069
	protected HandleVector<int>.Handle eventManagerHandle = HandleVector<int>.InvalidHandle;

	// Token: 0x04001B9E RID: 7070
	protected List<KAnimControllerBase.OverrideAnimFileData> overrideAnimFiles = new List<KAnimControllerBase.OverrideAnimFileData>();

	// Token: 0x04001B9F RID: 7071
	protected DeepProfiler DeepProfiler = new DeepProfiler(false);

	// Token: 0x04001BA0 RID: 7072
	public bool randomiseLoopedOffset;

	// Token: 0x04001BA1 RID: 7073
	protected float elapsedTime;

	// Token: 0x04001BA2 RID: 7074
	protected float playSpeed = 1f;

	// Token: 0x04001BA3 RID: 7075
	protected KAnim.PlayMode mode = KAnim.PlayMode.Once;

	// Token: 0x04001BA4 RID: 7076
	protected bool stopped = true;

	// Token: 0x04001BA5 RID: 7077
	public float animHeight = 1f;

	// Token: 0x04001BA6 RID: 7078
	public float animWidth = 1f;

	// Token: 0x04001BA7 RID: 7079
	protected bool isVisible;

	// Token: 0x04001BA8 RID: 7080
	protected Bounds bounds;

	// Token: 0x04001BA9 RID: 7081
	public Action<Bounds> OnUpdateBounds;

	// Token: 0x04001BAA RID: 7082
	public Action<Color> OnTintChanged;

	// Token: 0x04001BAB RID: 7083
	public Action<Color> OnHighlightChanged;

	// Token: 0x04001BAD RID: 7085
	protected KAnimSynchronizer synchronizer;

	// Token: 0x04001BAE RID: 7086
	protected KAnimLayering layering;

	// Token: 0x04001BAF RID: 7087
	[SerializeField]
	protected bool _enabled = true;

	// Token: 0x04001BB0 RID: 7088
	protected bool hasEnableRun;

	// Token: 0x04001BB1 RID: 7089
	protected bool hasAwakeRun;

	// Token: 0x04001BB2 RID: 7090
	protected KBatchedAnimInstanceData batchInstanceData;

	// Token: 0x04001BB5 RID: 7093
	public KAnimControllerBase.VisibilityType visibilityType;

	// Token: 0x04001BB9 RID: 7097
	public Action<GameObject> onDestroySelf;

	// Token: 0x04001BBC RID: 7100
	[SerializeField]
	protected List<KAnimHashedString> hiddenSymbols = new List<KAnimHashedString>();

	// Token: 0x04001BBD RID: 7101
	[SerializeField]
	protected HashSet<KAnimHashedString> hiddenSymbolsSet = new HashSet<KAnimHashedString>();

	// Token: 0x04001BBE RID: 7102
	protected Dictionary<HashedString, KAnimControllerBase.AnimLookupData> anims = new Dictionary<HashedString, KAnimControllerBase.AnimLookupData>();

	// Token: 0x04001BBF RID: 7103
	protected Dictionary<HashedString, KAnimControllerBase.AnimLookupData> overrideAnims = new Dictionary<HashedString, KAnimControllerBase.AnimLookupData>();

	// Token: 0x04001BC0 RID: 7104
	protected Queue<KAnimControllerBase.AnimData> animQueue = new Queue<KAnimControllerBase.AnimData>();

	// Token: 0x04001BC1 RID: 7105
	protected int maxSymbols;

	// Token: 0x04001BC3 RID: 7107
	public Grid.SceneLayer fgLayer = Grid.SceneLayer.NoLayer;

	// Token: 0x04001BC4 RID: 7108
	protected AnimEventManager aem;

	// Token: 0x04001BC5 RID: 7109
	private static HashedString snaptoPivot = new HashedString("snapTo_pivot");

	// Token: 0x02000915 RID: 2325
	public struct OverrideAnimFileData
	{
		// Token: 0x04001BC6 RID: 7110
		public float priority;

		// Token: 0x04001BC7 RID: 7111
		public KAnimFile file;
	}

	// Token: 0x02000916 RID: 2326
	public struct AnimLookupData
	{
		// Token: 0x04001BC8 RID: 7112
		public int animIndex;
	}

	// Token: 0x02000917 RID: 2327
	public struct AnimData
	{
		// Token: 0x04001BC9 RID: 7113
		public HashedString anim;

		// Token: 0x04001BCA RID: 7114
		public KAnim.PlayMode mode;

		// Token: 0x04001BCB RID: 7115
		public float speed;

		// Token: 0x04001BCC RID: 7116
		public float timeOffset;
	}

	// Token: 0x02000918 RID: 2328
	public enum VisibilityType
	{
		// Token: 0x04001BCE RID: 7118
		Default,
		// Token: 0x04001BCF RID: 7119
		OffscreenUpdate,
		// Token: 0x04001BD0 RID: 7120
		Always
	}

	// Token: 0x02000919 RID: 2329
	// (Invoke) Token: 0x060029C1 RID: 10689
	public delegate void KAnimEvent(HashedString name);
}
