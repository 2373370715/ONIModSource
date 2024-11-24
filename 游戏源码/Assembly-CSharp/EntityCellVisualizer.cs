using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A48 RID: 2632
public class EntityCellVisualizer : KMonoBehaviour
{
	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x0600305D RID: 12381 RVA: 0x000BF717 File Offset: 0x000BD917
	public BuildingCellVisualizerResources Resources
	{
		get
		{
			return BuildingCellVisualizerResources.Instance();
		}
	}

	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x0600305E RID: 12382 RVA: 0x000BCAC8 File Offset: 0x000BACC8
	protected int CenterCell
	{
		get
		{
			return Grid.PosToCell(this);
		}
	}

	// Token: 0x0600305F RID: 12383 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void DefinePorts()
	{
	}

	// Token: 0x06003060 RID: 12384 RVA: 0x000BF71E File Offset: 0x000BD91E
	protected override void OnPrefabInit()
	{
		this.LoadDiseaseIcon();
		this.DefinePorts();
	}

	// Token: 0x06003061 RID: 12385 RVA: 0x000BF72C File Offset: 0x000BD92C
	public void ConnectedEventWithDelay(float delay, int connectionCount, int cell, string soundName)
	{
		base.StartCoroutine(this.ConnectedDelay(delay, connectionCount, cell, soundName));
	}

	// Token: 0x06003062 RID: 12386 RVA: 0x000BF740 File Offset: 0x000BD940
	private IEnumerator ConnectedDelay(float delay, int connectionCount, int cell, string soundName)
	{
		float startTime = Time.realtimeSinceStartup;
		float currentTime = startTime;
		while (currentTime < startTime + delay)
		{
			currentTime += Time.unscaledDeltaTime;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.ConnectedEvent(cell);
		string sound = GlobalAssets.GetSound(soundName, false);
		if (sound != null)
		{
			Vector3 position = base.transform.GetPosition();
			position.z = 0f;
			EventInstance instance = SoundEvent.BeginOneShot(sound, position, 1f, false);
			instance.setParameterByName("connectedCount", (float)connectionCount, false);
			SoundEvent.EndOneShot(instance);
		}
		yield break;
	}

	// Token: 0x06003063 RID: 12387 RVA: 0x001FB97C File Offset: 0x001F9B7C
	private int ComputeCell(CellOffset cellOffset)
	{
		CellOffset offset = cellOffset;
		if (this.rotatable != null)
		{
			offset = this.rotatable.GetRotatedCellOffset(cellOffset);
		}
		return Grid.OffsetCell(Grid.PosToCell(base.gameObject), offset);
	}

	// Token: 0x06003064 RID: 12388 RVA: 0x001FB9B8 File Offset: 0x001F9BB8
	public void ConnectedEvent(int cell)
	{
		foreach (EntityCellVisualizer.PortEntry portEntry in this.ports)
		{
			if (this.ComputeCell(portEntry.cellOffset) == cell && portEntry.visualizer != null)
			{
				SizePulse pulse = portEntry.visualizer.AddComponent<SizePulse>();
				pulse.speed = 20f;
				pulse.multiplier = 0.75f;
				pulse.updateWhenPaused = true;
				SizePulse pulse2 = pulse;
				pulse2.onComplete = (System.Action)Delegate.Combine(pulse2.onComplete, new System.Action(delegate()
				{
					UnityEngine.Object.Destroy(pulse);
				}));
			}
		}
	}

	// Token: 0x06003065 RID: 12389 RVA: 0x000BF76C File Offset: 0x000BD96C
	public virtual void AddPort(EntityCellVisualizer.Ports type, CellOffset cell)
	{
		this.AddPort(type, cell, Color.white);
	}

	// Token: 0x06003066 RID: 12390 RVA: 0x000BF77B File Offset: 0x000BD97B
	public virtual void AddPort(EntityCellVisualizer.Ports type, CellOffset cell, Color tint)
	{
		this.AddPort(type, cell, tint, tint, 1.5f, false);
	}

	// Token: 0x06003067 RID: 12391 RVA: 0x000BF78D File Offset: 0x000BD98D
	public virtual void AddPort(EntityCellVisualizer.Ports type, CellOffset cell, Color connectedTint, Color disconnectedTint, float scale = 1.5f, bool hideBG = false)
	{
		this.ports.Add(new EntityCellVisualizer.PortEntry(type, cell, connectedTint, disconnectedTint, scale, hideBG));
		this.addedPorts |= type;
	}

	// Token: 0x06003068 RID: 12392 RVA: 0x001FBA94 File Offset: 0x001F9C94
	protected override void OnCleanUp()
	{
		foreach (EntityCellVisualizer.PortEntry portEntry in this.ports)
		{
			if (portEntry.visualizer != null)
			{
				UnityEngine.Object.Destroy(portEntry.visualizer);
			}
		}
		GameObject[] array = new GameObject[]
		{
			this.switchVisualizer,
			this.wireVisualizerAlpha,
			this.wireVisualizerBeta
		};
		for (int i = 0; i < array.Length; i++)
		{
			UnityEngine.Object.Destroy(array[i]);
		}
		base.OnCleanUp();
	}

	// Token: 0x06003069 RID: 12393 RVA: 0x000BF7B6 File Offset: 0x000BD9B6
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (this.icons == null)
		{
			this.icons = new Dictionary<GameObject, Image>();
		}
		Components.EntityCellVisualizers.Add(this);
	}

	// Token: 0x0600306A RID: 12394 RVA: 0x000BF7DC File Offset: 0x000BD9DC
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		Components.EntityCellVisualizers.Remove(this);
	}

	// Token: 0x0600306B RID: 12395 RVA: 0x001FBB38 File Offset: 0x001F9D38
	public void DrawIcons(HashedString mode)
	{
		EntityCellVisualizer.Ports ports = (EntityCellVisualizer.Ports)0;
		if (base.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
		{
			ports = (EntityCellVisualizer.Ports)0;
		}
		else if (mode == OverlayModes.Power.ID)
		{
			ports = (EntityCellVisualizer.Ports.PowerIn | EntityCellVisualizer.Ports.PowerOut);
		}
		else if (mode == OverlayModes.GasConduits.ID)
		{
			ports = (EntityCellVisualizer.Ports.GasIn | EntityCellVisualizer.Ports.GasOut);
		}
		else if (mode == OverlayModes.LiquidConduits.ID)
		{
			ports = (EntityCellVisualizer.Ports.LiquidIn | EntityCellVisualizer.Ports.LiquidOut);
		}
		else if (mode == OverlayModes.SolidConveyor.ID)
		{
			ports = (EntityCellVisualizer.Ports.SolidIn | EntityCellVisualizer.Ports.SolidOut);
		}
		else if (mode == OverlayModes.Radiation.ID)
		{
			ports = (EntityCellVisualizer.Ports.HighEnergyParticleIn | EntityCellVisualizer.Ports.HighEnergyParticleOut);
		}
		else if (mode == OverlayModes.Disease.ID)
		{
			ports = (EntityCellVisualizer.Ports.DiseaseIn | EntityCellVisualizer.Ports.DiseaseOut);
		}
		else if (mode == OverlayModes.Temperature.ID || mode == OverlayModes.HeatFlow.ID)
		{
			ports = (EntityCellVisualizer.Ports.HeatSource | EntityCellVisualizer.Ports.HeatSink);
		}
		bool flag = false;
		foreach (EntityCellVisualizer.PortEntry portEntry in this.ports)
		{
			if ((portEntry.type & ports) == portEntry.type)
			{
				this.DrawUtilityIcon(portEntry);
				flag = true;
			}
			else if (portEntry.visualizer != null && portEntry.visualizer.activeInHierarchy)
			{
				portEntry.visualizer.SetActive(false);
			}
		}
		if (mode == OverlayModes.Power.ID)
		{
			if (!flag)
			{
				Switch component = base.GetComponent<Switch>();
				if (component != null)
				{
					int cell = Grid.PosToCell(base.transform.GetPosition());
					Color32 c = component.IsHandlerOn() ? this.Resources.switchColor : this.Resources.switchOffColor;
					this.DrawUtilityIcon(cell, this.Resources.switchIcon, ref this.switchVisualizer, c, 1f, false);
					return;
				}
				WireUtilityNetworkLink component2 = base.GetComponent<WireUtilityNetworkLink>();
				if (component2 != null)
				{
					int cell2;
					int cell3;
					component2.GetCells(out cell2, out cell3);
					this.DrawUtilityIcon(cell2, (Game.Instance.circuitManager.GetCircuitID(cell2) == ushort.MaxValue) ? this.Resources.electricityBridgeIcon : this.Resources.electricityConnectedIcon, ref this.wireVisualizerAlpha, this.Resources.electricityInputColor, 1f, false);
					this.DrawUtilityIcon(cell3, (Game.Instance.circuitManager.GetCircuitID(cell3) == ushort.MaxValue) ? this.Resources.electricityBridgeIcon : this.Resources.electricityConnectedIcon, ref this.wireVisualizerBeta, this.Resources.electricityInputColor, 1f, false);
					return;
				}
			}
		}
		else
		{
			foreach (GameObject gameObject in new GameObject[]
			{
				this.switchVisualizer,
				this.wireVisualizerAlpha,
				this.wireVisualizerBeta
			})
			{
				if (gameObject != null && gameObject.activeInHierarchy)
				{
					gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x0600306C RID: 12396 RVA: 0x001FBE1C File Offset: 0x001FA01C
	private Sprite GetSpriteForPortType(EntityCellVisualizer.Ports type, bool connected)
	{
		if (type <= EntityCellVisualizer.Ports.SolidOut)
		{
			if (type <= EntityCellVisualizer.Ports.LiquidIn)
			{
				switch (type)
				{
				case EntityCellVisualizer.Ports.PowerIn:
					if (!connected)
					{
						return this.Resources.electricityInputIcon;
					}
					return this.Resources.electricityBridgeConnectedIcon;
				case EntityCellVisualizer.Ports.PowerOut:
					if (!connected)
					{
						return this.Resources.electricityOutputIcon;
					}
					return this.Resources.electricityBridgeConnectedIcon;
				case EntityCellVisualizer.Ports.PowerIn | EntityCellVisualizer.Ports.PowerOut:
					break;
				case EntityCellVisualizer.Ports.GasIn:
					return this.Resources.gasInputIcon;
				default:
					if (type == EntityCellVisualizer.Ports.GasOut)
					{
						return this.Resources.gasOutputIcon;
					}
					if (type == EntityCellVisualizer.Ports.LiquidIn)
					{
						return this.Resources.liquidInputIcon;
					}
					break;
				}
			}
			else
			{
				if (type == EntityCellVisualizer.Ports.LiquidOut)
				{
					return this.Resources.liquidOutputIcon;
				}
				if (type == EntityCellVisualizer.Ports.SolidIn)
				{
					return this.Resources.liquidInputIcon;
				}
				if (type == EntityCellVisualizer.Ports.SolidOut)
				{
					return this.Resources.liquidOutputIcon;
				}
			}
		}
		else if (type <= EntityCellVisualizer.Ports.DiseaseIn)
		{
			if (type == EntityCellVisualizer.Ports.HighEnergyParticleIn)
			{
				return this.Resources.highEnergyParticleInputIcon;
			}
			if (type == EntityCellVisualizer.Ports.HighEnergyParticleOut)
			{
				return this.GetIconForHighEnergyOutput();
			}
			if (type == EntityCellVisualizer.Ports.DiseaseIn)
			{
				return this.diseaseSourceSprite;
			}
		}
		else
		{
			if (type == EntityCellVisualizer.Ports.DiseaseOut)
			{
				return this.diseaseSourceSprite;
			}
			if (type == EntityCellVisualizer.Ports.HeatSource)
			{
				return this.Resources.heatSourceIcon;
			}
			if (type == EntityCellVisualizer.Ports.HeatSink)
			{
				return this.Resources.heatSinkIcon;
			}
		}
		return null;
	}

	// Token: 0x0600306D RID: 12397 RVA: 0x001FBF90 File Offset: 0x001FA190
	protected virtual void DrawUtilityIcon(EntityCellVisualizer.PortEntry port)
	{
		int cell = this.ComputeCell(port.cellOffset);
		bool flag = true;
		bool connected = true;
		EntityCellVisualizer.Ports type = port.type;
		if (type <= EntityCellVisualizer.Ports.GasOut)
		{
			if (type - EntityCellVisualizer.Ports.PowerIn > 1)
			{
				if (type == EntityCellVisualizer.Ports.GasIn || type == EntityCellVisualizer.Ports.GasOut)
				{
					flag = (null != Grid.Objects[cell, 12]);
				}
			}
			else
			{
				bool flag2 = base.GetComponent<Building>() as BuildingPreview != null;
				BuildingEnabledButton component = base.GetComponent<BuildingEnabledButton>();
				connected = (!flag2 && Game.Instance.circuitManager.GetCircuitID(cell) != ushort.MaxValue);
				flag = (flag2 || (component != null && component.IsEnabled));
			}
		}
		else if (type <= EntityCellVisualizer.Ports.LiquidOut)
		{
			if (type == EntityCellVisualizer.Ports.LiquidIn || type == EntityCellVisualizer.Ports.LiquidOut)
			{
				flag = (null != Grid.Objects[cell, 16]);
			}
		}
		else if (type == EntityCellVisualizer.Ports.SolidIn || type == EntityCellVisualizer.Ports.SolidOut)
		{
			flag = (null != Grid.Objects[cell, 20]);
		}
		this.DrawUtilityIcon(cell, this.GetSpriteForPortType(port.type, connected), ref port.visualizer, flag ? port.connectedTint : port.disconnectedTint, port.scale, port.hideBG);
	}

	// Token: 0x0600306E RID: 12398 RVA: 0x001FC0CC File Offset: 0x001FA2CC
	protected virtual void LoadDiseaseIcon()
	{
		DiseaseVisualization.Info info = Assets.instance.DiseaseVisualization.GetInfo(this.DiseaseCellVisName);
		if (info.name != null)
		{
			this.diseaseSourceSprite = Assets.instance.DiseaseVisualization.overlaySprite;
			this.diseaseSourceColour = GlobalAssets.Instance.colorSet.GetColorByName(info.overlayColourName);
		}
	}

	// Token: 0x0600306F RID: 12399 RVA: 0x001FC12C File Offset: 0x001FA32C
	protected virtual Sprite GetIconForHighEnergyOutput()
	{
		IHighEnergyParticleDirection component = base.GetComponent<IHighEnergyParticleDirection>();
		Sprite result = this.Resources.highEnergyParticleOutputIcons[0];
		if (component != null)
		{
			int directionIndex = EightDirectionUtil.GetDirectionIndex(component.Direction);
			result = this.Resources.highEnergyParticleOutputIcons[directionIndex];
		}
		return result;
	}

	// Token: 0x06003070 RID: 12400 RVA: 0x001FC16C File Offset: 0x001FA36C
	private void DrawUtilityIcon(int cell, Sprite icon_img, ref GameObject visualizerObj, Color tint, float scaleMultiplier = 1.5f, bool hideBG = false)
	{
		Vector3 position = Grid.CellToPosCCC(cell, Grid.SceneLayer.Building);
		if (visualizerObj == null)
		{
			visualizerObj = global::Util.KInstantiate(Assets.UIPrefabs.ResourceVisualizer, GameScreenManager.Instance.worldSpaceCanvas, null);
			visualizerObj.transform.SetAsFirstSibling();
			this.icons.Add(visualizerObj, visualizerObj.transform.GetChild(0).GetComponent<Image>());
		}
		if (!visualizerObj.gameObject.activeInHierarchy)
		{
			visualizerObj.gameObject.SetActive(true);
		}
		visualizerObj.GetComponent<Image>().enabled = !hideBG;
		this.icons[visualizerObj].raycastTarget = this.enableRaycast;
		this.icons[visualizerObj].sprite = icon_img;
		visualizerObj.transform.GetChild(0).gameObject.GetComponent<Image>().color = tint;
		visualizerObj.transform.SetPosition(position);
		if (visualizerObj.GetComponent<SizePulse>() == null)
		{
			visualizerObj.transform.localScale = Vector3.one * scaleMultiplier;
		}
	}

	// Token: 0x06003071 RID: 12401 RVA: 0x001FC280 File Offset: 0x001FA480
	public Image GetPowerOutputIcon()
	{
		foreach (EntityCellVisualizer.PortEntry portEntry in this.ports)
		{
			if (portEntry.type == EntityCellVisualizer.Ports.PowerOut)
			{
				return (portEntry.visualizer != null) ? portEntry.visualizer.transform.GetChild(0).GetComponent<Image>() : null;
			}
		}
		return null;
	}

	// Token: 0x06003072 RID: 12402 RVA: 0x001FC304 File Offset: 0x001FA504
	public Image GetPowerInputIcon()
	{
		foreach (EntityCellVisualizer.PortEntry portEntry in this.ports)
		{
			if (portEntry.type == EntityCellVisualizer.Ports.PowerIn)
			{
				return (portEntry.visualizer != null) ? portEntry.visualizer.transform.GetChild(0).GetComponent<Image>() : null;
			}
		}
		return null;
	}

	// Token: 0x04002091 RID: 8337
	protected List<EntityCellVisualizer.PortEntry> ports = new List<EntityCellVisualizer.PortEntry>();

	// Token: 0x04002092 RID: 8338
	public EntityCellVisualizer.Ports addedPorts;

	// Token: 0x04002093 RID: 8339
	private GameObject switchVisualizer;

	// Token: 0x04002094 RID: 8340
	private GameObject wireVisualizerAlpha;

	// Token: 0x04002095 RID: 8341
	private GameObject wireVisualizerBeta;

	// Token: 0x04002096 RID: 8342
	public const EntityCellVisualizer.Ports HEAT_PORTS = EntityCellVisualizer.Ports.HeatSource | EntityCellVisualizer.Ports.HeatSink;

	// Token: 0x04002097 RID: 8343
	public const EntityCellVisualizer.Ports POWER_PORTS = EntityCellVisualizer.Ports.PowerIn | EntityCellVisualizer.Ports.PowerOut;

	// Token: 0x04002098 RID: 8344
	public const EntityCellVisualizer.Ports GAS_PORTS = EntityCellVisualizer.Ports.GasIn | EntityCellVisualizer.Ports.GasOut;

	// Token: 0x04002099 RID: 8345
	public const EntityCellVisualizer.Ports LIQUID_PORTS = EntityCellVisualizer.Ports.LiquidIn | EntityCellVisualizer.Ports.LiquidOut;

	// Token: 0x0400209A RID: 8346
	public const EntityCellVisualizer.Ports SOLID_PORTS = EntityCellVisualizer.Ports.SolidIn | EntityCellVisualizer.Ports.SolidOut;

	// Token: 0x0400209B RID: 8347
	public const EntityCellVisualizer.Ports ENERGY_PARTICLES_PORTS = EntityCellVisualizer.Ports.HighEnergyParticleIn | EntityCellVisualizer.Ports.HighEnergyParticleOut;

	// Token: 0x0400209C RID: 8348
	public const EntityCellVisualizer.Ports DISEASE_PORTS = EntityCellVisualizer.Ports.DiseaseIn | EntityCellVisualizer.Ports.DiseaseOut;

	// Token: 0x0400209D RID: 8349
	public const EntityCellVisualizer.Ports MATTER_PORTS = EntityCellVisualizer.Ports.GasIn | EntityCellVisualizer.Ports.GasOut | EntityCellVisualizer.Ports.LiquidIn | EntityCellVisualizer.Ports.LiquidOut | EntityCellVisualizer.Ports.SolidIn | EntityCellVisualizer.Ports.SolidOut;

	// Token: 0x0400209E RID: 8350
	protected Sprite diseaseSourceSprite;

	// Token: 0x0400209F RID: 8351
	protected Color32 diseaseSourceColour;

	// Token: 0x040020A0 RID: 8352
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x040020A1 RID: 8353
	protected bool enableRaycast = true;

	// Token: 0x040020A2 RID: 8354
	protected Dictionary<GameObject, Image> icons;

	// Token: 0x040020A3 RID: 8355
	public string DiseaseCellVisName = DUPLICANTSTATS.STANDARD.Secretions.PEE_DISEASE;

	// Token: 0x02000A49 RID: 2633
	[Flags]
	public enum Ports
	{
		// Token: 0x040020A5 RID: 8357
		PowerIn = 1,
		// Token: 0x040020A6 RID: 8358
		PowerOut = 2,
		// Token: 0x040020A7 RID: 8359
		GasIn = 4,
		// Token: 0x040020A8 RID: 8360
		GasOut = 8,
		// Token: 0x040020A9 RID: 8361
		LiquidIn = 16,
		// Token: 0x040020AA RID: 8362
		LiquidOut = 32,
		// Token: 0x040020AB RID: 8363
		SolidIn = 64,
		// Token: 0x040020AC RID: 8364
		SolidOut = 128,
		// Token: 0x040020AD RID: 8365
		HighEnergyParticleIn = 256,
		// Token: 0x040020AE RID: 8366
		HighEnergyParticleOut = 512,
		// Token: 0x040020AF RID: 8367
		DiseaseIn = 1024,
		// Token: 0x040020B0 RID: 8368
		DiseaseOut = 2048,
		// Token: 0x040020B1 RID: 8369
		HeatSource = 4096,
		// Token: 0x040020B2 RID: 8370
		HeatSink = 8192
	}

	// Token: 0x02000A4A RID: 2634
	protected class PortEntry
	{
		// Token: 0x06003074 RID: 12404 RVA: 0x000BF81E File Offset: 0x000BDA1E
		public PortEntry(EntityCellVisualizer.Ports type, CellOffset cellOffset, Color connectedTint, Color disconnectedTint, float scale, bool hideBG)
		{
			this.type = type;
			this.cellOffset = cellOffset;
			this.visualizer = null;
			this.connectedTint = connectedTint;
			this.disconnectedTint = disconnectedTint;
			this.scale = scale;
			this.hideBG = hideBG;
		}

		// Token: 0x040020B3 RID: 8371
		public EntityCellVisualizer.Ports type;

		// Token: 0x040020B4 RID: 8372
		public CellOffset cellOffset;

		// Token: 0x040020B5 RID: 8373
		public GameObject visualizer;

		// Token: 0x040020B6 RID: 8374
		public Color connectedTint;

		// Token: 0x040020B7 RID: 8375
		public Color disconnectedTint;

		// Token: 0x040020B8 RID: 8376
		public float scale;

		// Token: 0x040020B9 RID: 8377
		public bool hideBG;
	}
}
