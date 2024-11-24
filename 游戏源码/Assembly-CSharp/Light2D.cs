using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001779 RID: 6009
[AddComponentMenu("KMonoBehaviour/scripts/Light2D")]
public class Light2D : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06007B94 RID: 31636 RVA: 0x000F14CD File Offset: 0x000EF6CD
	private T MaybeDirty<T>(T old_value, T new_value, ref bool dirty)
	{
		if (!EqualityComparer<T>.Default.Equals(old_value, new_value))
		{
			dirty = true;
			return new_value;
		}
		return old_value;
	}

	// Token: 0x170007C7 RID: 1991
	// (get) Token: 0x06007B95 RID: 31637 RVA: 0x000F14E3 File Offset: 0x000EF6E3
	// (set) Token: 0x06007B96 RID: 31638 RVA: 0x000F14F0 File Offset: 0x000EF6F0
	public global::LightShape shape
	{
		get
		{
			return this.pending_emitter_state.shape;
		}
		set
		{
			this.pending_emitter_state.shape = this.MaybeDirty<global::LightShape>(this.pending_emitter_state.shape, value, ref this.dirty_shape);
		}
	}

	// Token: 0x170007C8 RID: 1992
	// (get) Token: 0x06007B97 RID: 31639 RVA: 0x000F1515 File Offset: 0x000EF715
	// (set) Token: 0x06007B98 RID: 31640 RVA: 0x000F151D File Offset: 0x000EF71D
	public LightGridManager.LightGridEmitter emitter { get; private set; }

	// Token: 0x170007C9 RID: 1993
	// (get) Token: 0x06007B99 RID: 31641 RVA: 0x000F1526 File Offset: 0x000EF726
	// (set) Token: 0x06007B9A RID: 31642 RVA: 0x000F1533 File Offset: 0x000EF733
	public Color Color
	{
		get
		{
			return this.pending_emitter_state.colour;
		}
		set
		{
			this.pending_emitter_state.colour = value;
		}
	}

	// Token: 0x170007CA RID: 1994
	// (get) Token: 0x06007B9B RID: 31643 RVA: 0x000F1541 File Offset: 0x000EF741
	// (set) Token: 0x06007B9C RID: 31644 RVA: 0x000F154E File Offset: 0x000EF74E
	public int Lux
	{
		get
		{
			return this.pending_emitter_state.intensity;
		}
		set
		{
			this.pending_emitter_state.intensity = value;
		}
	}

	// Token: 0x170007CB RID: 1995
	// (get) Token: 0x06007B9D RID: 31645 RVA: 0x000F155C File Offset: 0x000EF75C
	// (set) Token: 0x06007B9E RID: 31646 RVA: 0x000F1569 File Offset: 0x000EF769
	public DiscreteShadowCaster.Direction LightDirection
	{
		get
		{
			return this.pending_emitter_state.direction;
		}
		set
		{
			this.pending_emitter_state.direction = this.MaybeDirty<DiscreteShadowCaster.Direction>(this.pending_emitter_state.direction, value, ref this.dirty_shape);
		}
	}

	// Token: 0x170007CC RID: 1996
	// (get) Token: 0x06007B9F RID: 31647 RVA: 0x000F158E File Offset: 0x000EF78E
	// (set) Token: 0x06007BA0 RID: 31648 RVA: 0x000F159B File Offset: 0x000EF79B
	public int Width
	{
		get
		{
			return this.pending_emitter_state.width;
		}
		set
		{
			this.pending_emitter_state.width = this.MaybeDirty<int>(this.pending_emitter_state.width, value, ref this.dirty_shape);
		}
	}

	// Token: 0x170007CD RID: 1997
	// (get) Token: 0x06007BA1 RID: 31649 RVA: 0x000F15C0 File Offset: 0x000EF7C0
	// (set) Token: 0x06007BA2 RID: 31650 RVA: 0x000F15CD File Offset: 0x000EF7CD
	public float Range
	{
		get
		{
			return this.pending_emitter_state.radius;
		}
		set
		{
			this.pending_emitter_state.radius = this.MaybeDirty<float>(this.pending_emitter_state.radius, value, ref this.dirty_shape);
		}
	}

	// Token: 0x170007CE RID: 1998
	// (get) Token: 0x06007BA3 RID: 31651 RVA: 0x000F15F2 File Offset: 0x000EF7F2
	// (set) Token: 0x06007BA4 RID: 31652 RVA: 0x000F15FF File Offset: 0x000EF7FF
	private int origin
	{
		get
		{
			return this.pending_emitter_state.origin;
		}
		set
		{
			this.pending_emitter_state.origin = this.MaybeDirty<int>(this.pending_emitter_state.origin, value, ref this.dirty_position);
		}
	}

	// Token: 0x170007CF RID: 1999
	// (get) Token: 0x06007BA5 RID: 31653 RVA: 0x000F1624 File Offset: 0x000EF824
	// (set) Token: 0x06007BA6 RID: 31654 RVA: 0x000F1631 File Offset: 0x000EF831
	public float FalloffRate
	{
		get
		{
			return this.pending_emitter_state.falloffRate;
		}
		set
		{
			this.pending_emitter_state.falloffRate = this.MaybeDirty<float>(this.pending_emitter_state.falloffRate, value, ref this.dirty_falloff);
		}
	}

	// Token: 0x170007D0 RID: 2000
	// (get) Token: 0x06007BA7 RID: 31655 RVA: 0x000F1656 File Offset: 0x000EF856
	// (set) Token: 0x06007BA8 RID: 31656 RVA: 0x000F165E File Offset: 0x000EF85E
	public float IntensityAnimation { get; set; }

	// Token: 0x170007D1 RID: 2001
	// (get) Token: 0x06007BA9 RID: 31657 RVA: 0x000F1667 File Offset: 0x000EF867
	// (set) Token: 0x06007BAA RID: 31658 RVA: 0x000F166F File Offset: 0x000EF86F
	public Vector2 Offset
	{
		get
		{
			return this._offset;
		}
		set
		{
			if (this._offset != value)
			{
				this._offset = value;
				this.origin = Grid.PosToCell(base.transform.GetPosition() + this._offset);
			}
		}
	}

	// Token: 0x170007D2 RID: 2002
	// (get) Token: 0x06007BAB RID: 31659 RVA: 0x000F16AC File Offset: 0x000EF8AC
	private bool isRegistered
	{
		get
		{
			return this.solidPartitionerEntry != HandleVector<int>.InvalidHandle;
		}
	}

	// Token: 0x06007BAC RID: 31660 RVA: 0x0031C814 File Offset: 0x0031AA14
	public Light2D()
	{
		this.emitter = new LightGridManager.LightGridEmitter();
		this.Range = 5f;
		this.Lux = 1000;
	}

	// Token: 0x06007BAD RID: 31661 RVA: 0x000F16BE File Offset: 0x000EF8BE
	protected override void OnPrefabInit()
	{
		base.Subscribe<Light2D>(-592767678, Light2D.OnOperationalChangedDelegate);
		if (this.disableOnStore)
		{
			base.Subscribe(856640610, new Action<object>(this.OnStore));
		}
		this.IntensityAnimation = 1f;
	}

	// Token: 0x06007BAE RID: 31662 RVA: 0x0031C870 File Offset: 0x0031AA70
	private void OnStore(object data)
	{
		global::Debug.Assert(this.disableOnStore, "Only Light2Ds that are disabled on storage should be subscribed to OnStore.");
		Storage storage = data as Storage;
		if (storage != null)
		{
			base.enabled = (storage.GetComponent<ItemPedestal>() != null || storage.GetComponent<MinionIdentity>() != null);
			return;
		}
		base.enabled = true;
	}

	// Token: 0x06007BAF RID: 31663 RVA: 0x0031C8C8 File Offset: 0x0031AAC8
	protected override void OnCmpEnable()
	{
		this.materialPropertyBlock = new MaterialPropertyBlock();
		base.OnCmpEnable();
		Components.Light2Ds.Add(this);
		if (base.isSpawned)
		{
			this.AddToScenePartitioner();
			this.emitter.Refresh(this.pending_emitter_state, true);
		}
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnMoved), "Light2D.OnMoved");
	}

	// Token: 0x06007BB0 RID: 31664 RVA: 0x000F16FC File Offset: 0x000EF8FC
	protected override void OnCmpDisable()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnMoved));
		Components.Light2Ds.Remove(this);
		base.OnCmpDisable();
		this.FullRemove();
	}

	// Token: 0x06007BB1 RID: 31665 RVA: 0x0031C934 File Offset: 0x0031AB34
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.origin = Grid.PosToCell(base.transform.GetPosition() + this.Offset);
		if (base.isActiveAndEnabled)
		{
			this.AddToScenePartitioner();
			this.emitter.Refresh(this.pending_emitter_state, true);
		}
	}

	// Token: 0x06007BB2 RID: 31666 RVA: 0x000F1731 File Offset: 0x000EF931
	protected override void OnCleanUp()
	{
		this.FullRemove();
	}

	// Token: 0x06007BB3 RID: 31667 RVA: 0x000F1739 File Offset: 0x000EF939
	private void OnMoved()
	{
		if (base.isSpawned)
		{
			this.FullRefresh();
		}
	}

	// Token: 0x06007BB4 RID: 31668 RVA: 0x000F1749 File Offset: 0x000EF949
	private HandleVector<int>.Handle AddToLayer(Extents ext, ScenePartitionerLayer layer)
	{
		return GameScenePartitioner.Instance.Add("Light2D", base.gameObject, ext, layer, new Action<object>(this.OnWorldChanged));
	}

	// Token: 0x06007BB5 RID: 31669 RVA: 0x0031C990 File Offset: 0x0031AB90
	private Extents ComputeExtents()
	{
		Vector2I vector2I = Grid.CellToXY(this.origin);
		int x = 0;
		int y = 0;
		int width = 0;
		int num = 0;
		global::LightShape shape = this.shape;
		if (shape > global::LightShape.Cone)
		{
			if (shape == global::LightShape.Quad)
			{
				width = this.Width;
				num = (int)this.Range;
				int num2 = (this.Width % 2 == 0) ? (this.Width / 2 - 1) : Mathf.FloorToInt((float)(this.Width - 1) * 0.5f);
				Vector2I vector2I2 = vector2I - DiscreteShadowCaster.TravelDirectionToOrtogonalDiractionVector(this.LightDirection) * num2;
				x = vector2I2.x;
				switch (this.LightDirection)
				{
				case DiscreteShadowCaster.Direction.North:
					y = vector2I2.y;
					goto IL_119;
				case DiscreteShadowCaster.Direction.South:
					y = vector2I2.y - num;
					goto IL_119;
				}
				y = vector2I2.y - DiscreteShadowCaster.TravelDirectionToOrtogonalDiractionVector(this.LightDirection).y * num2;
			}
		}
		else
		{
			int num3 = (int)this.Range;
			int num4 = num3 * 2;
			x = vector2I.x - num3;
			y = vector2I.y - num3;
			width = num4;
			num = ((this.shape == global::LightShape.Circle) ? num4 : num3);
		}
		IL_119:
		return new Extents(x, y, width, num);
	}

	// Token: 0x06007BB6 RID: 31670 RVA: 0x0031CAC0 File Offset: 0x0031ACC0
	private void AddToScenePartitioner()
	{
		Extents ext = this.ComputeExtents();
		this.solidPartitionerEntry = this.AddToLayer(ext, GameScenePartitioner.Instance.solidChangedLayer);
		this.liquidPartitionerEntry = this.AddToLayer(ext, GameScenePartitioner.Instance.liquidChangedLayer);
	}

	// Token: 0x06007BB7 RID: 31671 RVA: 0x000F176E File Offset: 0x000EF96E
	private void RemoveFromScenePartitioner()
	{
		if (this.isRegistered)
		{
			GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
			GameScenePartitioner.Instance.Free(ref this.liquidPartitionerEntry);
		}
	}

	// Token: 0x06007BB8 RID: 31672 RVA: 0x000F1798 File Offset: 0x000EF998
	private void MoveInScenePartitioner()
	{
		GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, this.ComputeExtents());
		GameScenePartitioner.Instance.UpdatePosition(this.liquidPartitionerEntry, this.ComputeExtents());
	}

	// Token: 0x06007BB9 RID: 31673 RVA: 0x000F17C6 File Offset: 0x000EF9C6
	private void EmitterRefresh()
	{
		this.emitter.Refresh(this.pending_emitter_state, true);
	}

	// Token: 0x06007BBA RID: 31674 RVA: 0x000F17DB File Offset: 0x000EF9DB
	[ContextMenu("Refresh")]
	public void FullRefresh()
	{
		if (!base.isSpawned || !base.isActiveAndEnabled)
		{
			return;
		}
		DebugUtil.DevAssert(this.isRegistered, "shouldn't be refreshing if we aren't spawned and enabled", null);
		this.RefreshShapeAndPosition();
		this.EmitterRefresh();
	}

	// Token: 0x06007BBB RID: 31675 RVA: 0x000F180C File Offset: 0x000EFA0C
	public void FullRemove()
	{
		this.RemoveFromScenePartitioner();
		this.emitter.RemoveFromGrid();
	}

	// Token: 0x06007BBC RID: 31676 RVA: 0x0031CB04 File Offset: 0x0031AD04
	public Light2D.RefreshResult RefreshShapeAndPosition()
	{
		if (!base.isSpawned)
		{
			return Light2D.RefreshResult.None;
		}
		if (!base.isActiveAndEnabled)
		{
			this.FullRemove();
			return Light2D.RefreshResult.Removed;
		}
		int num = Grid.PosToCell(base.transform.GetPosition() + this.Offset);
		if (!Grid.IsValidCell(num))
		{
			this.FullRemove();
			return Light2D.RefreshResult.Removed;
		}
		this.origin = num;
		if (this.dirty_shape)
		{
			this.RemoveFromScenePartitioner();
			this.AddToScenePartitioner();
		}
		else if (this.dirty_position)
		{
			this.MoveInScenePartitioner();
		}
		if (this.dirty_falloff)
		{
			this.EmitterRefresh();
		}
		this.dirty_shape = false;
		this.dirty_position = false;
		this.dirty_falloff = false;
		return Light2D.RefreshResult.Updated;
	}

	// Token: 0x06007BBD RID: 31677 RVA: 0x000F181F File Offset: 0x000EFA1F
	private void OnWorldChanged(object data)
	{
		this.FullRefresh();
	}

	// Token: 0x06007BBE RID: 31678 RVA: 0x0031CBAC File Offset: 0x0031ADAC
	public virtual List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, this.Range), UI.GAMEOBJECTEFFECTS.TOOLTIPS.EMITS_LIGHT, Descriptor.DescriptorType.Effect, false),
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT_LUX, this.Lux), UI.GAMEOBJECTEFFECTS.TOOLTIPS.EMITS_LIGHT_LUX, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x04005CAD RID: 23725
	public bool autoRespondToOperational = true;

	// Token: 0x04005CAE RID: 23726
	private bool dirty_shape;

	// Token: 0x04005CAF RID: 23727
	private bool dirty_position;

	// Token: 0x04005CB0 RID: 23728
	private bool dirty_falloff;

	// Token: 0x04005CB1 RID: 23729
	[SerializeField]
	private LightGridManager.LightGridEmitter.State pending_emitter_state = LightGridManager.LightGridEmitter.State.DEFAULT;

	// Token: 0x04005CB4 RID: 23732
	public float Angle;

	// Token: 0x04005CB5 RID: 23733
	public Vector2 Direction;

	// Token: 0x04005CB6 RID: 23734
	[SerializeField]
	private Vector2 _offset;

	// Token: 0x04005CB7 RID: 23735
	public bool drawOverlay;

	// Token: 0x04005CB8 RID: 23736
	public Color overlayColour;

	// Token: 0x04005CB9 RID: 23737
	public MaterialPropertyBlock materialPropertyBlock;

	// Token: 0x04005CBA RID: 23738
	private HandleVector<int>.Handle solidPartitionerEntry = HandleVector<int>.InvalidHandle;

	// Token: 0x04005CBB RID: 23739
	private HandleVector<int>.Handle liquidPartitionerEntry = HandleVector<int>.InvalidHandle;

	// Token: 0x04005CBC RID: 23740
	public bool disableOnStore;

	// Token: 0x04005CBD RID: 23741
	private static readonly EventSystem.IntraObjectHandler<Light2D> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Light2D>(delegate(Light2D light, object data)
	{
		if (light.autoRespondToOperational)
		{
			light.enabled = (bool)data;
		}
	});

	// Token: 0x0200177A RID: 6010
	public enum RefreshResult
	{
		// Token: 0x04005CBF RID: 23743
		None,
		// Token: 0x04005CC0 RID: 23744
		Removed,
		// Token: 0x04005CC1 RID: 23745
		Updated
	}
}
