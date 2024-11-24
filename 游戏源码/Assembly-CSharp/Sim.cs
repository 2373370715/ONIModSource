using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02001B61 RID: 7009
public static class Sim
{
	// Token: 0x0600934F RID: 37711 RVA: 0x0010039E File Offset: 0x000FE59E
	public static bool IsRadiationEnabled()
	{
		return DlcManager.FeatureRadiationEnabled();
	}

	// Token: 0x06009350 RID: 37712 RVA: 0x001003A5 File Offset: 0x000FE5A5
	public static bool IsValidHandle(int h)
	{
		return h != -1 && h != -2;
	}

	// Token: 0x06009351 RID: 37713 RVA: 0x001003B5 File Offset: 0x000FE5B5
	public static int GetHandleIndex(int h)
	{
		return h & 16777215;
	}

	// Token: 0x06009352 RID: 37714
	[DllImport("SimDLL")]
	public static extern void SIM_Initialize(Sim.GAME_MessageHandler callback);

	// Token: 0x06009353 RID: 37715
	[DllImport("SimDLL")]
	public static extern void SIM_Shutdown();

	// Token: 0x06009354 RID: 37716
	[DllImport("SimDLL")]
	public unsafe static extern IntPtr SIM_HandleMessage(int sim_msg_id, int msg_length, byte* msg);

	// Token: 0x06009355 RID: 37717
	[DllImport("SimDLL")]
	private unsafe static extern byte* SIM_BeginSave(int* size, int x, int y);

	// Token: 0x06009356 RID: 37718
	[DllImport("SimDLL")]
	private static extern void SIM_EndSave();

	// Token: 0x06009357 RID: 37719
	[DllImport("SimDLL")]
	public static extern void SIM_DebugCrash();

	// Token: 0x06009358 RID: 37720 RVA: 0x0038D74C File Offset: 0x0038B94C
	public unsafe static IntPtr HandleMessage(SimMessageHashes sim_msg_id, int msg_length, byte[] msg)
	{
		IntPtr result;
		fixed (byte[] array = msg)
		{
			byte* msg2;
			if (msg == null || array.Length == 0)
			{
				msg2 = null;
			}
			else
			{
				msg2 = &array[0];
			}
			result = Sim.SIM_HandleMessage((int)sim_msg_id, msg_length, msg2);
		}
		return result;
	}

	// Token: 0x06009359 RID: 37721 RVA: 0x0038D77C File Offset: 0x0038B97C
	public unsafe static void Save(BinaryWriter writer, int x, int y)
	{
		int num;
		void* value = (void*)Sim.SIM_BeginSave(&num, x, y);
		byte[] array = new byte[num];
		Marshal.Copy((IntPtr)value, array, 0, num);
		Sim.SIM_EndSave();
		writer.Write(num);
		writer.Write(array);
	}

	// Token: 0x0600935A RID: 37722 RVA: 0x0038D7BC File Offset: 0x0038B9BC
	public unsafe static int LoadWorld(IReader reader)
	{
		int num = reader.ReadInt32();
		byte[] array;
		byte* msg;
		if ((array = reader.ReadBytes(num)) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		IntPtr value = Sim.SIM_HandleMessage(-672538170, num, msg);
		array = null;
		if (value == IntPtr.Zero)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x0600935B RID: 37723 RVA: 0x0038D80C File Offset: 0x0038BA0C
	public static void AllocateCells(int width, int height, bool headless = false)
	{
		using (MemoryStream memoryStream = new MemoryStream(8))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				binaryWriter.Write(width);
				binaryWriter.Write(height);
				bool value = Sim.IsRadiationEnabled();
				binaryWriter.Write(value);
				binaryWriter.Write(headless);
				binaryWriter.Flush();
				Sim.HandleMessage(SimMessageHashes.AllocateCells, (int)memoryStream.Length, memoryStream.GetBuffer());
			}
		}
	}

	// Token: 0x0600935C RID: 37724 RVA: 0x0038D7BC File Offset: 0x0038B9BC
	public unsafe static int Load(IReader reader)
	{
		int num = reader.ReadInt32();
		byte[] array;
		byte* msg;
		if ((array = reader.ReadBytes(num)) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		IntPtr value = Sim.SIM_HandleMessage(-672538170, num, msg);
		array = null;
		if (value == IntPtr.Zero)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x0600935D RID: 37725 RVA: 0x0038D89C File Offset: 0x0038BA9C
	public unsafe static void Start()
	{
		Sim.GameDataUpdate* ptr = (Sim.GameDataUpdate*)((void*)Sim.SIM_HandleMessage(-931446686, 0, null));
		Grid.elementIdx = ptr->elementIdx;
		Grid.temperature = ptr->temperature;
		Grid.radiation = ptr->radiation;
		Grid.mass = ptr->mass;
		Grid.properties = ptr->properties;
		Grid.strengthInfo = ptr->strengthInfo;
		Grid.insulation = ptr->insulation;
		Grid.diseaseIdx = ptr->diseaseIdx;
		Grid.diseaseCount = ptr->diseaseCount;
		Grid.AccumulatedFlowValues = ptr->accumulatedFlow;
		PropertyTextures.externalFlowTex = ptr->propertyTextureFlow;
		PropertyTextures.externalLiquidTex = ptr->propertyTextureLiquid;
		PropertyTextures.externalExposedToSunlight = ptr->propertyTextureExposedToSunlight;
		Grid.InitializeCells();
	}

	// Token: 0x0600935E RID: 37726 RVA: 0x001003BE File Offset: 0x000FE5BE
	public static void Shutdown()
	{
		Sim.SIM_Shutdown();
		Grid.mass = null;
	}

	// Token: 0x0600935F RID: 37727
	[DllImport("SimDLL")]
	public unsafe static extern char* SYSINFO_Acquire();

	// Token: 0x06009360 RID: 37728
	[DllImport("SimDLL")]
	public static extern void SYSINFO_Release();

	// Token: 0x06009361 RID: 37729 RVA: 0x0038D950 File Offset: 0x0038BB50
	public unsafe static int DLL_MessageHandler(int message_id, IntPtr data)
	{
		if (message_id == 0)
		{
			Sim.DLLExceptionHandlerMessage* ptr = (Sim.DLLExceptionHandlerMessage*)((void*)data);
			string stack_trace = Marshal.PtrToStringAnsi(ptr->callstack);
			string dmp_filename = Marshal.PtrToStringAnsi(ptr->dmpFilename);
			KCrashReporter.ReportSimDLLCrash("SimDLL Crash Dump", stack_trace, dmp_filename);
			return 0;
		}
		if (message_id == 1)
		{
			Sim.DLLReportMessageMessage* ptr2 = (Sim.DLLReportMessageMessage*)((void*)data);
			string msg = "SimMessage: " + Marshal.PtrToStringAnsi(ptr2->message);
			string stack_trace2;
			if (ptr2->callstack != IntPtr.Zero)
			{
				stack_trace2 = Marshal.PtrToStringAnsi(ptr2->callstack);
			}
			else
			{
				string str = Marshal.PtrToStringAnsi(ptr2->file);
				int line = ptr2->line;
				stack_trace2 = str + ":" + line.ToString();
			}
			KCrashReporter.ReportSimDLLCrash(msg, stack_trace2, null);
			return 0;
		}
		return -1;
	}

	// Token: 0x04006F99 RID: 28569
	public const int InvalidHandle = -1;

	// Token: 0x04006F9A RID: 28570
	public const int QueuedRegisterHandle = -2;

	// Token: 0x04006F9B RID: 28571
	public const byte InvalidDiseaseIdx = 255;

	// Token: 0x04006F9C RID: 28572
	public const ushort InvalidElementIdx = 65535;

	// Token: 0x04006F9D RID: 28573
	public const byte SpaceZoneID = 255;

	// Token: 0x04006F9E RID: 28574
	public const byte SolidZoneID = 0;

	// Token: 0x04006F9F RID: 28575
	public const int ChunkEdgeSize = 32;

	// Token: 0x04006FA0 RID: 28576
	public const float StateTransitionEnergy = 3f;

	// Token: 0x04006FA1 RID: 28577
	public const float ZeroDegreesCentigrade = 273.15f;

	// Token: 0x04006FA2 RID: 28578
	public const float StandardTemperature = 293.15f;

	// Token: 0x04006FA3 RID: 28579
	public const float StandardMeltingPointOffset = 10f;

	// Token: 0x04006FA4 RID: 28580
	public const float StandardPressure = 101.3f;

	// Token: 0x04006FA5 RID: 28581
	public const float Epsilon = 0.0001f;

	// Token: 0x04006FA6 RID: 28582
	public const float MaxTemperature = 10000f;

	// Token: 0x04006FA7 RID: 28583
	public const float MinTemperature = 0f;

	// Token: 0x04006FA8 RID: 28584
	public const float MaxRadiation = 9000000f;

	// Token: 0x04006FA9 RID: 28585
	public const float MinRadiation = 0f;

	// Token: 0x04006FAA RID: 28586
	public const float MaxMass = 10000f;

	// Token: 0x04006FAB RID: 28587
	public const float MinMass = 1.0001f;

	// Token: 0x04006FAC RID: 28588
	private const int PressureUpdateInterval = 1;

	// Token: 0x04006FAD RID: 28589
	private const int TemperatureUpdateInterval = 1;

	// Token: 0x04006FAE RID: 28590
	private const int LiquidUpdateInterval = 1;

	// Token: 0x04006FAF RID: 28591
	private const int LifeUpdateInterval = 1;

	// Token: 0x04006FB0 RID: 28592
	public const byte ClearSkyGridValue = 253;

	// Token: 0x04006FB1 RID: 28593
	public const int PACKING_ALIGNMENT = 4;

	// Token: 0x02001B62 RID: 7010
	// (Invoke) Token: 0x06009363 RID: 37731
	public delegate int GAME_MessageHandler(int message_id, IntPtr data);

	// Token: 0x02001B63 RID: 7011
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DLLExceptionHandlerMessage
	{
		// Token: 0x04006FB2 RID: 28594
		public IntPtr callstack;

		// Token: 0x04006FB3 RID: 28595
		public IntPtr dmpFilename;
	}

	// Token: 0x02001B64 RID: 7012
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DLLReportMessageMessage
	{
		// Token: 0x04006FB4 RID: 28596
		public IntPtr callstack;

		// Token: 0x04006FB5 RID: 28597
		public IntPtr message;

		// Token: 0x04006FB6 RID: 28598
		public IntPtr file;

		// Token: 0x04006FB7 RID: 28599
		public int line;
	}

	// Token: 0x02001B65 RID: 7013
	private enum GameHandledMessages
	{
		// Token: 0x04006FB9 RID: 28601
		ExceptionHandler,
		// Token: 0x04006FBA RID: 28602
		ReportMessage
	}

	// Token: 0x02001B66 RID: 7014
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PhysicsData
	{
		// Token: 0x06009366 RID: 37734 RVA: 0x001003CC File Offset: 0x000FE5CC
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.temperature);
			writer.Write(this.mass);
			writer.Write(this.pressure);
		}

		// Token: 0x04006FBB RID: 28603
		public float temperature;

		// Token: 0x04006FBC RID: 28604
		public float mass;

		// Token: 0x04006FBD RID: 28605
		public float pressure;
	}

	// Token: 0x02001B67 RID: 7015
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Cell
	{
		// Token: 0x06009367 RID: 37735 RVA: 0x0038DA08 File Offset: 0x0038BC08
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.elementIdx);
			writer.Write(0);
			writer.Write(this.insulation);
			writer.Write(0);
			writer.Write(this.pad0);
			writer.Write(this.pad1);
			writer.Write(this.pad2);
			writer.Write(this.temperature);
			writer.Write(this.mass);
		}

		// Token: 0x06009368 RID: 37736 RVA: 0x001003F5 File Offset: 0x000FE5F5
		public void SetValues(global::Element elem, List<global::Element> elements)
		{
			this.SetValues(elem, elem.defaultValues, elements);
		}

		// Token: 0x06009369 RID: 37737 RVA: 0x0038DA7C File Offset: 0x0038BC7C
		public void SetValues(global::Element elem, Sim.PhysicsData pd, List<global::Element> elements)
		{
			this.elementIdx = (ushort)elements.IndexOf(elem);
			this.temperature = pd.temperature;
			this.mass = pd.mass;
			this.insulation = byte.MaxValue;
			DebugUtil.Assert(this.temperature > 0f || this.mass == 0f, "A non-zero mass cannot have a <= 0 temperature");
		}

		// Token: 0x0600936A RID: 37738 RVA: 0x0038DAE4 File Offset: 0x0038BCE4
		public void SetValues(ushort new_elem_idx, float new_temperature, float new_mass)
		{
			this.elementIdx = new_elem_idx;
			this.temperature = new_temperature;
			this.mass = new_mass;
			this.insulation = byte.MaxValue;
			DebugUtil.Assert(this.temperature > 0f || this.mass == 0f, "A non-zero mass cannot have a <= 0 temperature");
		}

		// Token: 0x04006FBE RID: 28606
		public ushort elementIdx;

		// Token: 0x04006FBF RID: 28607
		public byte properties;

		// Token: 0x04006FC0 RID: 28608
		public byte insulation;

		// Token: 0x04006FC1 RID: 28609
		public byte strengthInfo;

		// Token: 0x04006FC2 RID: 28610
		public byte pad0;

		// Token: 0x04006FC3 RID: 28611
		public byte pad1;

		// Token: 0x04006FC4 RID: 28612
		public byte pad2;

		// Token: 0x04006FC5 RID: 28613
		public float temperature;

		// Token: 0x04006FC6 RID: 28614
		public float mass;

		// Token: 0x02001B68 RID: 7016
		public enum Properties
		{
			// Token: 0x04006FC8 RID: 28616
			GasImpermeable = 1,
			// Token: 0x04006FC9 RID: 28617
			LiquidImpermeable,
			// Token: 0x04006FCA RID: 28618
			SolidImpermeable = 4,
			// Token: 0x04006FCB RID: 28619
			Unbreakable = 8,
			// Token: 0x04006FCC RID: 28620
			Transparent = 16,
			// Token: 0x04006FCD RID: 28621
			Opaque = 32,
			// Token: 0x04006FCE RID: 28622
			NotifyOnMelt = 64,
			// Token: 0x04006FCF RID: 28623
			ConstructedTile = 128
		}
	}

	// Token: 0x02001B69 RID: 7017
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Element
	{
		// Token: 0x0600936B RID: 37739 RVA: 0x0038DB38 File Offset: 0x0038BD38
		public Element(global::Element e, List<global::Element> elements)
		{
			this.id = e.id;
			this.state = (byte)e.state;
			if (e.HasTag(GameTags.Unstable))
			{
				this.state |= 8;
			}
			int num = elements.FindIndex((global::Element ele) => ele.id == e.lowTempTransitionTarget);
			int num2 = elements.FindIndex((global::Element ele) => ele.id == e.highTempTransitionTarget);
			this.lowTempTransitionIdx = (ushort)((num >= 0) ? num : 65535);
			this.highTempTransitionIdx = (ushort)((num2 >= 0) ? num2 : 65535);
			this.elementsTableIdx = (ushort)elements.IndexOf(e);
			this.specificHeatCapacity = e.specificHeatCapacity;
			this.thermalConductivity = e.thermalConductivity;
			this.solidSurfaceAreaMultiplier = e.solidSurfaceAreaMultiplier;
			this.liquidSurfaceAreaMultiplier = e.liquidSurfaceAreaMultiplier;
			this.gasSurfaceAreaMultiplier = e.gasSurfaceAreaMultiplier;
			this.molarMass = e.molarMass;
			this.strength = e.strength;
			this.flow = e.flow;
			this.viscosity = e.viscosity;
			this.minHorizontalFlow = e.minHorizontalFlow;
			this.minVerticalFlow = e.minVerticalFlow;
			this.maxMass = e.maxMass;
			this.lowTemp = e.lowTemp;
			this.highTemp = e.highTemp;
			this.highTempTransitionOreID = e.highTempTransitionOreID;
			this.highTempTransitionOreMassConversion = e.highTempTransitionOreMassConversion;
			this.lowTempTransitionOreID = e.lowTempTransitionOreID;
			this.lowTempTransitionOreMassConversion = e.lowTempTransitionOreMassConversion;
			this.sublimateIndex = (ushort)elements.FindIndex((global::Element ele) => ele.id == e.sublimateId);
			this.convertIndex = (ushort)elements.FindIndex((global::Element ele) => ele.id == e.convertId);
			this.pack0 = 0;
			if (e.substance == null)
			{
				this.colour = 0U;
			}
			else
			{
				Color32 color = e.substance.colour;
				this.colour = (uint)((int)color.a << 24 | (int)color.b << 16 | (int)color.g << 8 | (int)color.r);
			}
			this.sublimateFX = e.sublimateFX;
			this.sublimateRate = e.sublimateRate;
			this.sublimateEfficiency = e.sublimateEfficiency;
			this.sublimateProbability = e.sublimateProbability;
			this.offGasProbability = e.offGasPercentage;
			this.lightAbsorptionFactor = e.lightAbsorptionFactor;
			this.radiationAbsorptionFactor = e.radiationAbsorptionFactor;
			this.radiationPer1000Mass = e.radiationPer1000Mass;
			this.defaultValues = e.defaultValues;
		}

		// Token: 0x0600936C RID: 37740 RVA: 0x0038DE48 File Offset: 0x0038C048
		public void Write(BinaryWriter writer)
		{
			writer.Write((int)this.id);
			writer.Write(this.lowTempTransitionIdx);
			writer.Write(this.highTempTransitionIdx);
			writer.Write(this.elementsTableIdx);
			writer.Write(this.state);
			writer.Write(this.pack0);
			writer.Write(this.specificHeatCapacity);
			writer.Write(this.thermalConductivity);
			writer.Write(this.molarMass);
			writer.Write(this.solidSurfaceAreaMultiplier);
			writer.Write(this.liquidSurfaceAreaMultiplier);
			writer.Write(this.gasSurfaceAreaMultiplier);
			writer.Write(this.flow);
			writer.Write(this.viscosity);
			writer.Write(this.minHorizontalFlow);
			writer.Write(this.minVerticalFlow);
			writer.Write(this.maxMass);
			writer.Write(this.lowTemp);
			writer.Write(this.highTemp);
			writer.Write(this.strength);
			writer.Write((int)this.lowTempTransitionOreID);
			writer.Write(this.lowTempTransitionOreMassConversion);
			writer.Write((int)this.highTempTransitionOreID);
			writer.Write(this.highTempTransitionOreMassConversion);
			writer.Write(this.sublimateIndex);
			writer.Write(this.convertIndex);
			writer.Write(this.colour);
			writer.Write((int)this.sublimateFX);
			writer.Write(this.sublimateRate);
			writer.Write(this.sublimateEfficiency);
			writer.Write(this.sublimateProbability);
			writer.Write(this.offGasProbability);
			writer.Write(this.lightAbsorptionFactor);
			writer.Write(this.radiationAbsorptionFactor);
			writer.Write(this.radiationPer1000Mass);
			this.defaultValues.Write(writer);
		}

		// Token: 0x04006FD0 RID: 28624
		public SimHashes id;

		// Token: 0x04006FD1 RID: 28625
		public ushort lowTempTransitionIdx;

		// Token: 0x04006FD2 RID: 28626
		public ushort highTempTransitionIdx;

		// Token: 0x04006FD3 RID: 28627
		public ushort elementsTableIdx;

		// Token: 0x04006FD4 RID: 28628
		public byte state;

		// Token: 0x04006FD5 RID: 28629
		public byte pack0;

		// Token: 0x04006FD6 RID: 28630
		public float specificHeatCapacity;

		// Token: 0x04006FD7 RID: 28631
		public float thermalConductivity;

		// Token: 0x04006FD8 RID: 28632
		public float molarMass;

		// Token: 0x04006FD9 RID: 28633
		public float solidSurfaceAreaMultiplier;

		// Token: 0x04006FDA RID: 28634
		public float liquidSurfaceAreaMultiplier;

		// Token: 0x04006FDB RID: 28635
		public float gasSurfaceAreaMultiplier;

		// Token: 0x04006FDC RID: 28636
		public float flow;

		// Token: 0x04006FDD RID: 28637
		public float viscosity;

		// Token: 0x04006FDE RID: 28638
		public float minHorizontalFlow;

		// Token: 0x04006FDF RID: 28639
		public float minVerticalFlow;

		// Token: 0x04006FE0 RID: 28640
		public float maxMass;

		// Token: 0x04006FE1 RID: 28641
		public float lowTemp;

		// Token: 0x04006FE2 RID: 28642
		public float highTemp;

		// Token: 0x04006FE3 RID: 28643
		public float strength;

		// Token: 0x04006FE4 RID: 28644
		public SimHashes lowTempTransitionOreID;

		// Token: 0x04006FE5 RID: 28645
		public float lowTempTransitionOreMassConversion;

		// Token: 0x04006FE6 RID: 28646
		public SimHashes highTempTransitionOreID;

		// Token: 0x04006FE7 RID: 28647
		public float highTempTransitionOreMassConversion;

		// Token: 0x04006FE8 RID: 28648
		public ushort sublimateIndex;

		// Token: 0x04006FE9 RID: 28649
		public ushort convertIndex;

		// Token: 0x04006FEA RID: 28650
		public uint colour;

		// Token: 0x04006FEB RID: 28651
		public SpawnFXHashes sublimateFX;

		// Token: 0x04006FEC RID: 28652
		public float sublimateRate;

		// Token: 0x04006FED RID: 28653
		public float sublimateEfficiency;

		// Token: 0x04006FEE RID: 28654
		public float sublimateProbability;

		// Token: 0x04006FEF RID: 28655
		public float offGasProbability;

		// Token: 0x04006FF0 RID: 28656
		public float lightAbsorptionFactor;

		// Token: 0x04006FF1 RID: 28657
		public float radiationAbsorptionFactor;

		// Token: 0x04006FF2 RID: 28658
		public float radiationPer1000Mass;

		// Token: 0x04006FF3 RID: 28659
		public Sim.PhysicsData defaultValues;
	}

	// Token: 0x02001B6B RID: 7019
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DiseaseCell
	{
		// Token: 0x06009372 RID: 37746 RVA: 0x0038E01C File Offset: 0x0038C21C
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.diseaseIdx);
			writer.Write(this.reservedInfestationTickCount);
			writer.Write(this.pad1);
			writer.Write(this.pad2);
			writer.Write(this.elementCount);
			writer.Write(this.reservedAccumulatedError);
		}

		// Token: 0x04006FF5 RID: 28661
		public byte diseaseIdx;

		// Token: 0x04006FF6 RID: 28662
		private byte reservedInfestationTickCount;

		// Token: 0x04006FF7 RID: 28663
		private byte pad1;

		// Token: 0x04006FF8 RID: 28664
		private byte pad2;

		// Token: 0x04006FF9 RID: 28665
		public int elementCount;

		// Token: 0x04006FFA RID: 28666
		private float reservedAccumulatedError;

		// Token: 0x04006FFB RID: 28667
		public static readonly Sim.DiseaseCell Invalid = new Sim.DiseaseCell
		{
			diseaseIdx = byte.MaxValue,
			elementCount = 0
		};
	}

	// Token: 0x02001B6C RID: 7020
	// (Invoke) Token: 0x06009375 RID: 37749
	public delegate void GAME_Callback();

	// Token: 0x02001B6D RID: 7021
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SolidInfo
	{
		// Token: 0x04006FFC RID: 28668
		public int cellIdx;

		// Token: 0x04006FFD RID: 28669
		public int isSolid;
	}

	// Token: 0x02001B6E RID: 7022
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct LiquidChangeInfo
	{
		// Token: 0x04006FFE RID: 28670
		public int cellIdx;
	}

	// Token: 0x02001B6F RID: 7023
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SolidSubstanceChangeInfo
	{
		// Token: 0x04006FFF RID: 28671
		public int cellIdx;
	}

	// Token: 0x02001B70 RID: 7024
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SubstanceChangeInfo
	{
		// Token: 0x04007000 RID: 28672
		public int cellIdx;

		// Token: 0x04007001 RID: 28673
		public ushort oldElemIdx;

		// Token: 0x04007002 RID: 28674
		public ushort newElemIdx;
	}

	// Token: 0x02001B71 RID: 7025
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CallbackInfo
	{
		// Token: 0x04007003 RID: 28675
		public int callbackIdx;
	}

	// Token: 0x02001B72 RID: 7026
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GameDataUpdate
	{
		// Token: 0x04007004 RID: 28676
		public int numFramesProcessed;

		// Token: 0x04007005 RID: 28677
		public unsafe ushort* elementIdx;

		// Token: 0x04007006 RID: 28678
		public unsafe float* temperature;

		// Token: 0x04007007 RID: 28679
		public unsafe float* mass;

		// Token: 0x04007008 RID: 28680
		public unsafe byte* properties;

		// Token: 0x04007009 RID: 28681
		public unsafe byte* insulation;

		// Token: 0x0400700A RID: 28682
		public unsafe byte* strengthInfo;

		// Token: 0x0400700B RID: 28683
		public unsafe float* radiation;

		// Token: 0x0400700C RID: 28684
		public unsafe byte* diseaseIdx;

		// Token: 0x0400700D RID: 28685
		public unsafe int* diseaseCount;

		// Token: 0x0400700E RID: 28686
		public int numSolidInfo;

		// Token: 0x0400700F RID: 28687
		public unsafe Sim.SolidInfo* solidInfo;

		// Token: 0x04007010 RID: 28688
		public int numLiquidChangeInfo;

		// Token: 0x04007011 RID: 28689
		public unsafe Sim.LiquidChangeInfo* liquidChangeInfo;

		// Token: 0x04007012 RID: 28690
		public int numSolidSubstanceChangeInfo;

		// Token: 0x04007013 RID: 28691
		public unsafe Sim.SolidSubstanceChangeInfo* solidSubstanceChangeInfo;

		// Token: 0x04007014 RID: 28692
		public int numSubstanceChangeInfo;

		// Token: 0x04007015 RID: 28693
		public unsafe Sim.SubstanceChangeInfo* substanceChangeInfo;

		// Token: 0x04007016 RID: 28694
		public int numCallbackInfo;

		// Token: 0x04007017 RID: 28695
		public unsafe Sim.CallbackInfo* callbackInfo;

		// Token: 0x04007018 RID: 28696
		public int numSpawnFallingLiquidInfo;

		// Token: 0x04007019 RID: 28697
		public unsafe Sim.SpawnFallingLiquidInfo* spawnFallingLiquidInfo;

		// Token: 0x0400701A RID: 28698
		public int numDigInfo;

		// Token: 0x0400701B RID: 28699
		public unsafe Sim.SpawnOreInfo* digInfo;

		// Token: 0x0400701C RID: 28700
		public int numSpawnOreInfo;

		// Token: 0x0400701D RID: 28701
		public unsafe Sim.SpawnOreInfo* spawnOreInfo;

		// Token: 0x0400701E RID: 28702
		public int numSpawnFXInfo;

		// Token: 0x0400701F RID: 28703
		public unsafe Sim.SpawnFXInfo* spawnFXInfo;

		// Token: 0x04007020 RID: 28704
		public int numUnstableCellInfo;

		// Token: 0x04007021 RID: 28705
		public unsafe Sim.UnstableCellInfo* unstableCellInfo;

		// Token: 0x04007022 RID: 28706
		public int numWorldDamageInfo;

		// Token: 0x04007023 RID: 28707
		public unsafe Sim.WorldDamageInfo* worldDamageInfo;

		// Token: 0x04007024 RID: 28708
		public int numBuildingTemperatures;

		// Token: 0x04007025 RID: 28709
		public unsafe Sim.BuildingTemperatureInfo* buildingTemperatures;

		// Token: 0x04007026 RID: 28710
		public int numMassConsumedCallbacks;

		// Token: 0x04007027 RID: 28711
		public unsafe Sim.MassConsumedCallback* massConsumedCallbacks;

		// Token: 0x04007028 RID: 28712
		public int numMassEmittedCallbacks;

		// Token: 0x04007029 RID: 28713
		public unsafe Sim.MassEmittedCallback* massEmittedCallbacks;

		// Token: 0x0400702A RID: 28714
		public int numDiseaseConsumptionCallbacks;

		// Token: 0x0400702B RID: 28715
		public unsafe Sim.DiseaseConsumptionCallback* diseaseConsumptionCallbacks;

		// Token: 0x0400702C RID: 28716
		public int numComponentStateChangedMessages;

		// Token: 0x0400702D RID: 28717
		public unsafe Sim.ComponentStateChangedMessage* componentStateChangedMessages;

		// Token: 0x0400702E RID: 28718
		public int numRemovedMassEntries;

		// Token: 0x0400702F RID: 28719
		public unsafe Sim.ConsumedMassInfo* removedMassEntries;

		// Token: 0x04007030 RID: 28720
		public int numEmittedMassEntries;

		// Token: 0x04007031 RID: 28721
		public unsafe Sim.EmittedMassInfo* emittedMassEntries;

		// Token: 0x04007032 RID: 28722
		public int numElementChunkInfos;

		// Token: 0x04007033 RID: 28723
		public unsafe Sim.ElementChunkInfo* elementChunkInfos;

		// Token: 0x04007034 RID: 28724
		public int numElementChunkMeltedInfos;

		// Token: 0x04007035 RID: 28725
		public unsafe Sim.MeltedInfo* elementChunkMeltedInfos;

		// Token: 0x04007036 RID: 28726
		public int numBuildingOverheatInfos;

		// Token: 0x04007037 RID: 28727
		public unsafe Sim.MeltedInfo* buildingOverheatInfos;

		// Token: 0x04007038 RID: 28728
		public int numBuildingNoLongerOverheatedInfos;

		// Token: 0x04007039 RID: 28729
		public unsafe Sim.MeltedInfo* buildingNoLongerOverheatedInfos;

		// Token: 0x0400703A RID: 28730
		public int numBuildingMeltedInfos;

		// Token: 0x0400703B RID: 28731
		public unsafe Sim.MeltedInfo* buildingMeltedInfos;

		// Token: 0x0400703C RID: 28732
		public int numCellMeltedInfos;

		// Token: 0x0400703D RID: 28733
		public unsafe Sim.CellMeltedInfo* cellMeltedInfos;

		// Token: 0x0400703E RID: 28734
		public int numDiseaseEmittedInfos;

		// Token: 0x0400703F RID: 28735
		public unsafe Sim.DiseaseEmittedInfo* diseaseEmittedInfos;

		// Token: 0x04007040 RID: 28736
		public int numDiseaseConsumedInfos;

		// Token: 0x04007041 RID: 28737
		public unsafe Sim.DiseaseConsumedInfo* diseaseConsumedInfos;

		// Token: 0x04007042 RID: 28738
		public int numRadiationConsumedCallbacks;

		// Token: 0x04007043 RID: 28739
		public unsafe Sim.ConsumedRadiationCallback* radiationConsumedCallbacks;

		// Token: 0x04007044 RID: 28740
		public unsafe float* accumulatedFlow;

		// Token: 0x04007045 RID: 28741
		public IntPtr propertyTextureFlow;

		// Token: 0x04007046 RID: 28742
		public IntPtr propertyTextureLiquid;

		// Token: 0x04007047 RID: 28743
		public IntPtr propertyTextureExposedToSunlight;
	}

	// Token: 0x02001B73 RID: 7027
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SpawnFallingLiquidInfo
	{
		// Token: 0x04007048 RID: 28744
		public int cellIdx;

		// Token: 0x04007049 RID: 28745
		public ushort elemIdx;

		// Token: 0x0400704A RID: 28746
		public byte diseaseIdx;

		// Token: 0x0400704B RID: 28747
		public byte pad0;

		// Token: 0x0400704C RID: 28748
		public float mass;

		// Token: 0x0400704D RID: 28749
		public float temperature;

		// Token: 0x0400704E RID: 28750
		public int diseaseCount;
	}

	// Token: 0x02001B74 RID: 7028
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SpawnOreInfo
	{
		// Token: 0x0400704F RID: 28751
		public int cellIdx;

		// Token: 0x04007050 RID: 28752
		public ushort elemIdx;

		// Token: 0x04007051 RID: 28753
		public byte diseaseIdx;

		// Token: 0x04007052 RID: 28754
		private byte pad0;

		// Token: 0x04007053 RID: 28755
		public float mass;

		// Token: 0x04007054 RID: 28756
		public float temperature;

		// Token: 0x04007055 RID: 28757
		public int diseaseCount;
	}

	// Token: 0x02001B75 RID: 7029
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SpawnFXInfo
	{
		// Token: 0x04007056 RID: 28758
		public int cellIdx;

		// Token: 0x04007057 RID: 28759
		public int fxHash;

		// Token: 0x04007058 RID: 28760
		public float rotation;
	}

	// Token: 0x02001B76 RID: 7030
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct UnstableCellInfo
	{
		// Token: 0x04007059 RID: 28761
		public int cellIdx;

		// Token: 0x0400705A RID: 28762
		public ushort elemIdx;

		// Token: 0x0400705B RID: 28763
		public byte fallingInfo;

		// Token: 0x0400705C RID: 28764
		public byte diseaseIdx;

		// Token: 0x0400705D RID: 28765
		public float mass;

		// Token: 0x0400705E RID: 28766
		public float temperature;

		// Token: 0x0400705F RID: 28767
		public int diseaseCount;

		// Token: 0x02001B77 RID: 7031
		public enum FallingInfo
		{
			// Token: 0x04007061 RID: 28769
			StartedFalling,
			// Token: 0x04007062 RID: 28770
			StoppedFalling
		}
	}

	// Token: 0x02001B78 RID: 7032
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct NewGameFrame
	{
		// Token: 0x04007063 RID: 28771
		public float elapsedSeconds;

		// Token: 0x04007064 RID: 28772
		public int minX;

		// Token: 0x04007065 RID: 28773
		public int minY;

		// Token: 0x04007066 RID: 28774
		public int maxX;

		// Token: 0x04007067 RID: 28775
		public int maxY;

		// Token: 0x04007068 RID: 28776
		public float currentSunlightIntensity;

		// Token: 0x04007069 RID: 28777
		public float currentCosmicRadiationIntensity;
	}

	// Token: 0x02001B79 RID: 7033
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct WorldDamageInfo
	{
		// Token: 0x0400706A RID: 28778
		public int gameCell;

		// Token: 0x0400706B RID: 28779
		public int damageSourceOffset;
	}

	// Token: 0x02001B7A RID: 7034
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PipeTemperatureChange
	{
		// Token: 0x0400706C RID: 28780
		public int cellIdx;

		// Token: 0x0400706D RID: 28781
		public float temperature;
	}

	// Token: 0x02001B7B RID: 7035
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MassConsumedCallback
	{
		// Token: 0x0400706E RID: 28782
		public int callbackIdx;

		// Token: 0x0400706F RID: 28783
		public ushort elemIdx;

		// Token: 0x04007070 RID: 28784
		public byte diseaseIdx;

		// Token: 0x04007071 RID: 28785
		private byte pad0;

		// Token: 0x04007072 RID: 28786
		public float mass;

		// Token: 0x04007073 RID: 28787
		public float temperature;

		// Token: 0x04007074 RID: 28788
		public int diseaseCount;
	}

	// Token: 0x02001B7C RID: 7036
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MassEmittedCallback
	{
		// Token: 0x04007075 RID: 28789
		public int callbackIdx;

		// Token: 0x04007076 RID: 28790
		public ushort elemIdx;

		// Token: 0x04007077 RID: 28791
		public byte suceeded;

		// Token: 0x04007078 RID: 28792
		public byte diseaseIdx;

		// Token: 0x04007079 RID: 28793
		public float mass;

		// Token: 0x0400707A RID: 28794
		public float temperature;

		// Token: 0x0400707B RID: 28795
		public int diseaseCount;
	}

	// Token: 0x02001B7D RID: 7037
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DiseaseConsumptionCallback
	{
		// Token: 0x0400707C RID: 28796
		public int callbackIdx;

		// Token: 0x0400707D RID: 28797
		public byte diseaseIdx;

		// Token: 0x0400707E RID: 28798
		private byte pad0;

		// Token: 0x0400707F RID: 28799
		private byte pad1;

		// Token: 0x04007080 RID: 28800
		private byte pad2;

		// Token: 0x04007081 RID: 28801
		public int diseaseCount;
	}

	// Token: 0x02001B7E RID: 7038
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ComponentStateChangedMessage
	{
		// Token: 0x04007082 RID: 28802
		public int callbackIdx;

		// Token: 0x04007083 RID: 28803
		public int simHandle;
	}

	// Token: 0x02001B7F RID: 7039
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DebugProperties
	{
		// Token: 0x04007084 RID: 28804
		public float buildingTemperatureScale;

		// Token: 0x04007085 RID: 28805
		public float buildingToBuildingTemperatureScale;

		// Token: 0x04007086 RID: 28806
		public float biomeTemperatureLerpRate;

		// Token: 0x04007087 RID: 28807
		public byte isDebugEditing;

		// Token: 0x04007088 RID: 28808
		public byte pad0;

		// Token: 0x04007089 RID: 28809
		public byte pad1;

		// Token: 0x0400708A RID: 28810
		public byte pad2;
	}

	// Token: 0x02001B80 RID: 7040
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct EmittedMassInfo
	{
		// Token: 0x0400708B RID: 28811
		public ushort elemIdx;

		// Token: 0x0400708C RID: 28812
		public byte diseaseIdx;

		// Token: 0x0400708D RID: 28813
		public byte pad0;

		// Token: 0x0400708E RID: 28814
		public float mass;

		// Token: 0x0400708F RID: 28815
		public float temperature;

		// Token: 0x04007090 RID: 28816
		public int diseaseCount;
	}

	// Token: 0x02001B81 RID: 7041
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ConsumedMassInfo
	{
		// Token: 0x04007091 RID: 28817
		public int simHandle;

		// Token: 0x04007092 RID: 28818
		public ushort removedElemIdx;

		// Token: 0x04007093 RID: 28819
		public byte diseaseIdx;

		// Token: 0x04007094 RID: 28820
		private byte pad0;

		// Token: 0x04007095 RID: 28821
		public float mass;

		// Token: 0x04007096 RID: 28822
		public float temperature;

		// Token: 0x04007097 RID: 28823
		public int diseaseCount;
	}

	// Token: 0x02001B82 RID: 7042
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ConsumedDiseaseInfo
	{
		// Token: 0x04007098 RID: 28824
		public int simHandle;

		// Token: 0x04007099 RID: 28825
		public byte diseaseIdx;

		// Token: 0x0400709A RID: 28826
		private byte pad0;

		// Token: 0x0400709B RID: 28827
		private byte pad1;

		// Token: 0x0400709C RID: 28828
		private byte pad2;

		// Token: 0x0400709D RID: 28829
		public int diseaseCount;
	}

	// Token: 0x02001B83 RID: 7043
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ElementChunkInfo
	{
		// Token: 0x0400709E RID: 28830
		public float temperature;

		// Token: 0x0400709F RID: 28831
		public float deltaKJ;
	}

	// Token: 0x02001B84 RID: 7044
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MeltedInfo
	{
		// Token: 0x040070A0 RID: 28832
		public int handle;
	}

	// Token: 0x02001B85 RID: 7045
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CellMeltedInfo
	{
		// Token: 0x040070A1 RID: 28833
		public int gameCell;
	}

	// Token: 0x02001B86 RID: 7046
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct BuildingTemperatureInfo
	{
		// Token: 0x040070A2 RID: 28834
		public int handle;

		// Token: 0x040070A3 RID: 28835
		public float temperature;
	}

	// Token: 0x02001B87 RID: 7047
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct BuildingConductivityData
	{
		// Token: 0x040070A4 RID: 28836
		public float temperature;

		// Token: 0x040070A5 RID: 28837
		public float heatCapacity;

		// Token: 0x040070A6 RID: 28838
		public float thermalConductivity;
	}

	// Token: 0x02001B88 RID: 7048
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DiseaseEmittedInfo
	{
		// Token: 0x040070A7 RID: 28839
		public byte diseaseIdx;

		// Token: 0x040070A8 RID: 28840
		private byte pad0;

		// Token: 0x040070A9 RID: 28841
		private byte pad1;

		// Token: 0x040070AA RID: 28842
		private byte pad2;

		// Token: 0x040070AB RID: 28843
		public int count;
	}

	// Token: 0x02001B89 RID: 7049
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DiseaseConsumedInfo
	{
		// Token: 0x040070AC RID: 28844
		public byte diseaseIdx;

		// Token: 0x040070AD RID: 28845
		private byte pad0;

		// Token: 0x040070AE RID: 28846
		private byte pad1;

		// Token: 0x040070AF RID: 28847
		private byte pad2;

		// Token: 0x040070B0 RID: 28848
		public int count;
	}

	// Token: 0x02001B8A RID: 7050
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ConsumedRadiationCallback
	{
		// Token: 0x040070B1 RID: 28849
		public int callbackIdx;

		// Token: 0x040070B2 RID: 28850
		public int gameCell;

		// Token: 0x040070B3 RID: 28851
		public float radiation;
	}
}
