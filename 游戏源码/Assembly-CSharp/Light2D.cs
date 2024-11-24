using System.Collections.Generic;
using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Light2D")]
public class Light2D : KMonoBehaviour, IGameObjectEffectDescriptor
{
	public enum RefreshResult
	{
		None,
		Removed,
		Updated
	}

	public bool autoRespondToOperational = true;

	private bool dirty_shape;

	private bool dirty_position;

	private bool dirty_falloff;

	[SerializeField]
	private LightGridManager.LightGridEmitter.State pending_emitter_state = LightGridManager.LightGridEmitter.State.DEFAULT;

	public float Angle;

	public Vector2 Direction;

	[SerializeField]
	private Vector2 _offset;

	public bool drawOverlay;

	public Color overlayColour;

	public MaterialPropertyBlock materialPropertyBlock;

	private HandleVector<int>.Handle solidPartitionerEntry = HandleVector<int>.InvalidHandle;

	private HandleVector<int>.Handle liquidPartitionerEntry = HandleVector<int>.InvalidHandle;

	private static readonly EventSystem.IntraObjectHandler<Light2D> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Light2D>(delegate(Light2D light, object data)
	{
		if (light.autoRespondToOperational)
		{
			light.enabled = (bool)data;
		}
	});

	public LightShape shape
	{
		get
		{
			return pending_emitter_state.shape;
		}
		set
		{
			pending_emitter_state.shape = MaybeDirty(pending_emitter_state.shape, value, ref dirty_shape);
		}
	}

	public LightGridManager.LightGridEmitter emitter { get; private set; }

	public Color Color
	{
		get
		{
			return pending_emitter_state.colour;
		}
		set
		{
			pending_emitter_state.colour = value;
		}
	}

	public int Lux
	{
		get
		{
			return pending_emitter_state.intensity;
		}
		set
		{
			pending_emitter_state.intensity = value;
		}
	}

	public DiscreteShadowCaster.Direction LightDirection
	{
		get
		{
			return pending_emitter_state.direction;
		}
		set
		{
			pending_emitter_state.direction = MaybeDirty(pending_emitter_state.direction, value, ref dirty_shape);
		}
	}

	public int Width
	{
		get
		{
			return pending_emitter_state.width;
		}
		set
		{
			pending_emitter_state.width = MaybeDirty(pending_emitter_state.width, value, ref dirty_shape);
		}
	}

	public float Range
	{
		get
		{
			return pending_emitter_state.radius;
		}
		set
		{
			pending_emitter_state.radius = MaybeDirty(pending_emitter_state.radius, value, ref dirty_shape);
		}
	}

	private int origin
	{
		get
		{
			return pending_emitter_state.origin;
		}
		set
		{
			pending_emitter_state.origin = MaybeDirty(pending_emitter_state.origin, value, ref dirty_position);
		}
	}

	public float FalloffRate
	{
		get
		{
			return pending_emitter_state.falloffRate;
		}
		set
		{
			pending_emitter_state.falloffRate = MaybeDirty(pending_emitter_state.falloffRate, value, ref dirty_falloff);
		}
	}

	public float IntensityAnimation { get; set; }

	public Vector2 Offset
	{
		get
		{
			return _offset;
		}
		set
		{
			if (_offset != value)
			{
				_offset = value;
				origin = Grid.PosToCell(base.transform.GetPosition() + (Vector3)_offset);
			}
		}
	}

	private bool isRegistered => solidPartitionerEntry != HandleVector<int>.InvalidHandle;

	private T MaybeDirty<T>(T old_value, T new_value, ref bool dirty)
	{
		if (!EqualityComparer<T>.Default.Equals(old_value, new_value))
		{
			dirty = true;
			return new_value;
		}
		return old_value;
	}

	public Light2D()
	{
		emitter = new LightGridManager.LightGridEmitter();
		Range = 5f;
		Lux = 1000;
	}

	protected override void OnPrefabInit()
	{
		Subscribe(-592767678, OnOperationalChangedDelegate);
		IntensityAnimation = 1f;
	}

	protected override void OnCmpEnable()
	{
		materialPropertyBlock = new MaterialPropertyBlock();
		base.OnCmpEnable();
		Components.Light2Ds.Add(this);
		if (base.isSpawned)
		{
			AddToScenePartitioner();
			emitter.Refresh(pending_emitter_state, force: true);
		}
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, OnMoved, "Light2D.OnMoved");
	}

	protected override void OnCmpDisable()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, OnMoved);
		Components.Light2Ds.Remove(this);
		base.OnCmpDisable();
		FullRemove();
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		origin = Grid.PosToCell(base.transform.GetPosition() + (Vector3)Offset);
		if (base.isActiveAndEnabled)
		{
			AddToScenePartitioner();
			emitter.Refresh(pending_emitter_state, force: true);
		}
	}

	protected override void OnCleanUp()
	{
		FullRemove();
	}

	private void OnMoved()
	{
		if (base.isSpawned)
		{
			FullRefresh();
		}
	}

	private HandleVector<int>.Handle AddToLayer(Extents ext, ScenePartitionerLayer layer)
	{
		return GameScenePartitioner.Instance.Add("Light2D", base.gameObject, ext, layer, OnWorldChanged);
	}

	private Extents ComputeExtents()
	{
		Vector2I vector2I = Grid.CellToXY(origin);
		int x = 0;
		int y = 0;
		int width = 0;
		int num = 0;
		switch (shape)
		{
		case LightShape.Circle:
		case LightShape.Cone:
		{
			int num3 = (int)Range;
			int num4 = num3 * 2;
			x = vector2I.x - num3;
			y = vector2I.y - num3;
			width = num4;
			num = ((shape == LightShape.Circle) ? num4 : num3);
			break;
		}
		case LightShape.Quad:
		{
			width = Width;
			num = (int)Range;
			int num2 = ((Width % 2 == 0) ? (Width / 2 - 1) : Mathf.FloorToInt((float)(Width - 1) * 0.5f));
			Vector2I vector2I2 = vector2I - DiscreteShadowCaster.TravelDirectionToOrtogonalDiractionVector(LightDirection) * num2;
			x = vector2I2.x;
			y = LightDirection switch
			{
				DiscreteShadowCaster.Direction.North => vector2I2.y, 
				DiscreteShadowCaster.Direction.South => vector2I2.y - num, 
				_ => vector2I2.y - DiscreteShadowCaster.TravelDirectionToOrtogonalDiractionVector(LightDirection).y * num2, 
			};
			break;
		}
		}
		return new Extents(x, y, width, num);
	}

	private void AddToScenePartitioner()
	{
		Extents ext = ComputeExtents();
		solidPartitionerEntry = AddToLayer(ext, GameScenePartitioner.Instance.solidChangedLayer);
		liquidPartitionerEntry = AddToLayer(ext, GameScenePartitioner.Instance.liquidChangedLayer);
	}

	private void RemoveFromScenePartitioner()
	{
		if (isRegistered)
		{
			GameScenePartitioner.Instance.Free(ref solidPartitionerEntry);
			GameScenePartitioner.Instance.Free(ref liquidPartitionerEntry);
		}
	}

	private void MoveInScenePartitioner()
	{
		GameScenePartitioner.Instance.UpdatePosition(solidPartitionerEntry, ComputeExtents());
		GameScenePartitioner.Instance.UpdatePosition(liquidPartitionerEntry, ComputeExtents());
	}

	private void EmitterRefresh()
	{
		emitter.Refresh(pending_emitter_state, force: true);
	}

	[ContextMenu("Refresh")]
	public void FullRefresh()
	{
		if (base.isSpawned && base.isActiveAndEnabled)
		{
			DebugUtil.DevAssert(isRegistered, "shouldn't be refreshing if we aren't spawned and enabled");
			RefreshShapeAndPosition();
			EmitterRefresh();
		}
	}

	public void FullRemove()
	{
		RemoveFromScenePartitioner();
		emitter.RemoveFromGrid();
	}

	public RefreshResult RefreshShapeAndPosition()
	{
		if (!base.isSpawned)
		{
			return RefreshResult.None;
		}
		if (!base.isActiveAndEnabled)
		{
			FullRemove();
			return RefreshResult.Removed;
		}
		int cell = Grid.PosToCell(base.transform.GetPosition() + (Vector3)Offset);
		if (!Grid.IsValidCell(cell))
		{
			FullRemove();
			return RefreshResult.Removed;
		}
		origin = cell;
		if (dirty_shape)
		{
			RemoveFromScenePartitioner();
			AddToScenePartitioner();
		}
		else if (dirty_position)
		{
			MoveInScenePartitioner();
		}
		if (dirty_falloff)
		{
			EmitterRefresh();
		}
		dirty_shape = false;
		dirty_position = false;
		dirty_falloff = false;
		return RefreshResult.Updated;
	}

	private void OnWorldChanged(object data)
	{
		FullRefresh();
	}

	public virtual List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, Range), UI.GAMEOBJECTEFFECTS.TOOLTIPS.EMITS_LIGHT),
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT_LUX, Lux), UI.GAMEOBJECTEFFECTS.TOOLTIPS.EMITS_LIGHT_LUX)
		};
	}
}
