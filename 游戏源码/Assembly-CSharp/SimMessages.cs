using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Database;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;
using STRINGS;

// Token: 0x02001B8B RID: 7051
public static class SimMessages
{
	// Token: 0x06009378 RID: 37752 RVA: 0x0038E0A4 File Offset: 0x0038C2A4
	public unsafe static void AddElementConsumer(int gameCell, ElementConsumer.Configuration configuration, SimHashes element, byte radius, int cb_handle)
	{
		Debug.Assert(Grid.IsValidCell(gameCell));
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		ushort elementIndex = ElementLoader.GetElementIndex(element);
		SimMessages.AddElementConsumerMessage* ptr = stackalloc SimMessages.AddElementConsumerMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddElementConsumerMessage))];
		ptr->cellIdx = gameCell;
		ptr->configuration = (byte)configuration;
		ptr->elementIdx = elementIndex;
		ptr->radius = radius;
		ptr->callbackIdx = cb_handle;
		Sim.SIM_HandleMessage(2024405073, sizeof(SimMessages.AddElementConsumerMessage), (byte*)ptr);
	}

	// Token: 0x06009379 RID: 37753 RVA: 0x0038E110 File Offset: 0x0038C310
	public unsafe static void SetElementConsumerData(int sim_handle, int cell, float consumptionRate)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			return;
		}
		SimMessages.SetElementConsumerDataMessage* ptr = stackalloc SimMessages.SetElementConsumerDataMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetElementConsumerDataMessage))];
		ptr->handle = sim_handle;
		ptr->cell = cell;
		ptr->consumptionRate = consumptionRate;
		Sim.SIM_HandleMessage(1575539738, sizeof(SimMessages.SetElementConsumerDataMessage), (byte*)ptr);
	}

	// Token: 0x0600937A RID: 37754 RVA: 0x0038E15C File Offset: 0x0038C35C
	public unsafe static void RemoveElementConsumer(int cb_handle, int sim_handle)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.RemoveElementConsumerMessage* ptr = stackalloc SimMessages.RemoveElementConsumerMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveElementConsumerMessage))];
		ptr->callbackIdx = cb_handle;
		ptr->handle = sim_handle;
		Sim.SIM_HandleMessage(894417742, sizeof(SimMessages.RemoveElementConsumerMessage), (byte*)ptr);
	}

	// Token: 0x0600937B RID: 37755 RVA: 0x0038E1AC File Offset: 0x0038C3AC
	public unsafe static void AddElementEmitter(float max_pressure, int on_registered, int on_blocked = -1, int on_unblocked = -1)
	{
		SimMessages.AddElementEmitterMessage* ptr = stackalloc SimMessages.AddElementEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddElementEmitterMessage))];
		ptr->maxPressure = max_pressure;
		ptr->callbackIdx = on_registered;
		ptr->onBlockedCB = on_blocked;
		ptr->onUnblockedCB = on_unblocked;
		Sim.SIM_HandleMessage(-505471181, sizeof(SimMessages.AddElementEmitterMessage), (byte*)ptr);
	}

	// Token: 0x0600937C RID: 37756 RVA: 0x0038E1F4 File Offset: 0x0038C3F4
	public unsafe static void ModifyElementEmitter(int sim_handle, int game_cell, int max_depth, SimHashes element, float emit_interval, float emit_mass, float emit_temperature, float max_pressure, byte disease_idx, int disease_count)
	{
		Debug.Assert(Grid.IsValidCell(game_cell));
		if (!Grid.IsValidCell(game_cell))
		{
			return;
		}
		ushort elementIndex = ElementLoader.GetElementIndex(element);
		SimMessages.ModifyElementEmitterMessage* ptr = stackalloc SimMessages.ModifyElementEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyElementEmitterMessage))];
		ptr->handle = sim_handle;
		ptr->cellIdx = game_cell;
		ptr->emitInterval = emit_interval;
		ptr->emitMass = emit_mass;
		ptr->emitTemperature = emit_temperature;
		ptr->maxPressure = max_pressure;
		ptr->elementIdx = elementIndex;
		ptr->maxDepth = (byte)max_depth;
		ptr->diseaseIdx = disease_idx;
		ptr->diseaseCount = disease_count;
		Sim.SIM_HandleMessage(403589164, sizeof(SimMessages.ModifyElementEmitterMessage), (byte*)ptr);
	}

	// Token: 0x0600937D RID: 37757 RVA: 0x0038E288 File Offset: 0x0038C488
	public unsafe static void RemoveElementEmitter(int cb_handle, int sim_handle)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.RemoveElementEmitterMessage* ptr = stackalloc SimMessages.RemoveElementEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveElementEmitterMessage))];
		ptr->callbackIdx = cb_handle;
		ptr->handle = sim_handle;
		Sim.SIM_HandleMessage(-1524118282, sizeof(SimMessages.RemoveElementEmitterMessage), (byte*)ptr);
	}

	// Token: 0x0600937E RID: 37758 RVA: 0x0038E2D8 File Offset: 0x0038C4D8
	public unsafe static void AddRadiationEmitter(int on_registered, int game_cell, short emitRadiusX, short emitRadiusY, float emitRads, float emitRate, float emitSpeed, float emitDirection, float emitAngle, RadiationEmitter.RadiationEmitterType emitType)
	{
		SimMessages.AddRadiationEmitterMessage* ptr = stackalloc SimMessages.AddRadiationEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddRadiationEmitterMessage))];
		ptr->callbackIdx = on_registered;
		ptr->cell = game_cell;
		ptr->emitRadiusX = emitRadiusX;
		ptr->emitRadiusY = emitRadiusY;
		ptr->emitRads = emitRads;
		ptr->emitRate = emitRate;
		ptr->emitSpeed = emitSpeed;
		ptr->emitDirection = emitDirection;
		ptr->emitAngle = emitAngle;
		ptr->emitType = (int)emitType;
		Sim.SIM_HandleMessage(-1505895314, sizeof(SimMessages.AddRadiationEmitterMessage), (byte*)ptr);
	}

	// Token: 0x0600937F RID: 37759 RVA: 0x0038E350 File Offset: 0x0038C550
	public unsafe static void ModifyRadiationEmitter(int sim_handle, int game_cell, short emitRadiusX, short emitRadiusY, float emitRads, float emitRate, float emitSpeed, float emitDirection, float emitAngle, RadiationEmitter.RadiationEmitterType emitType)
	{
		if (!Grid.IsValidCell(game_cell))
		{
			return;
		}
		SimMessages.ModifyRadiationEmitterMessage* ptr = stackalloc SimMessages.ModifyRadiationEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyRadiationEmitterMessage))];
		ptr->handle = sim_handle;
		ptr->cell = game_cell;
		ptr->callbackIdx = -1;
		ptr->emitRadiusX = emitRadiusX;
		ptr->emitRadiusY = emitRadiusY;
		ptr->emitRads = emitRads;
		ptr->emitRate = emitRate;
		ptr->emitSpeed = emitSpeed;
		ptr->emitDirection = emitDirection;
		ptr->emitAngle = emitAngle;
		ptr->emitType = (int)emitType;
		Sim.SIM_HandleMessage(-503965465, sizeof(SimMessages.ModifyRadiationEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06009380 RID: 37760 RVA: 0x0038E3D8 File Offset: 0x0038C5D8
	public unsafe static void RemoveRadiationEmitter(int cb_handle, int sim_handle)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.RemoveRadiationEmitterMessage* ptr = stackalloc SimMessages.RemoveRadiationEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveRadiationEmitterMessage))];
		ptr->callbackIdx = cb_handle;
		ptr->handle = sim_handle;
		Sim.SIM_HandleMessage(-704259919, sizeof(SimMessages.RemoveRadiationEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06009381 RID: 37761 RVA: 0x0038E428 File Offset: 0x0038C628
	public unsafe static void AddElementChunk(int gameCell, SimHashes element, float mass, float temperature, float surface_area, float thickness, float ground_transfer_scale, int cb_handle)
	{
		Debug.Assert(Grid.IsValidCell(gameCell));
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		if (mass * temperature > 0f)
		{
			ushort elementIndex = ElementLoader.GetElementIndex(element);
			SimMessages.AddElementChunkMessage* ptr = stackalloc SimMessages.AddElementChunkMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddElementChunkMessage))];
			ptr->gameCell = gameCell;
			ptr->callbackIdx = cb_handle;
			ptr->mass = mass;
			ptr->temperature = temperature;
			ptr->surfaceArea = surface_area;
			ptr->thickness = thickness;
			ptr->groundTransferScale = ground_transfer_scale;
			ptr->elementIdx = elementIndex;
			Sim.SIM_HandleMessage(1445724082, sizeof(SimMessages.AddElementChunkMessage), (byte*)ptr);
		}
	}

	// Token: 0x06009382 RID: 37762 RVA: 0x0038E4B4 File Offset: 0x0038C6B4
	public unsafe static void RemoveElementChunk(int sim_handle, int cb_handle)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.RemoveElementChunkMessage* ptr = stackalloc SimMessages.RemoveElementChunkMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveElementChunkMessage))];
		ptr->callbackIdx = cb_handle;
		ptr->handle = sim_handle;
		Sim.SIM_HandleMessage(-912908555, sizeof(SimMessages.RemoveElementChunkMessage), (byte*)ptr);
	}

	// Token: 0x06009383 RID: 37763 RVA: 0x0038E504 File Offset: 0x0038C704
	public unsafe static void SetElementChunkData(int sim_handle, float temperature, float heat_capacity)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			return;
		}
		SimMessages.SetElementChunkDataMessage* ptr = stackalloc SimMessages.SetElementChunkDataMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetElementChunkDataMessage))];
		ptr->handle = sim_handle;
		ptr->temperature = temperature;
		ptr->heatCapacity = heat_capacity;
		Sim.SIM_HandleMessage(-435115907, sizeof(SimMessages.SetElementChunkDataMessage), (byte*)ptr);
	}

	// Token: 0x06009384 RID: 37764 RVA: 0x0038E550 File Offset: 0x0038C750
	public unsafe static void MoveElementChunk(int sim_handle, int cell)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.MoveElementChunkMessage* ptr = stackalloc SimMessages.MoveElementChunkMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.MoveElementChunkMessage))];
		ptr->handle = sim_handle;
		ptr->gameCell = cell;
		Sim.SIM_HandleMessage(-374911358, sizeof(SimMessages.MoveElementChunkMessage), (byte*)ptr);
	}

	// Token: 0x06009385 RID: 37765 RVA: 0x0038E5A0 File Offset: 0x0038C7A0
	public unsafe static void ModifyElementChunkEnergy(int sim_handle, float delta_kj)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.ModifyElementChunkEnergyMessage* ptr = stackalloc SimMessages.ModifyElementChunkEnergyMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyElementChunkEnergyMessage))];
		ptr->handle = sim_handle;
		ptr->deltaKJ = delta_kj;
		Sim.SIM_HandleMessage(1020555667, sizeof(SimMessages.ModifyElementChunkEnergyMessage), (byte*)ptr);
	}

	// Token: 0x06009386 RID: 37766 RVA: 0x0038E5F0 File Offset: 0x0038C7F0
	public unsafe static void ModifyElementChunkTemperatureAdjuster(int sim_handle, float temperature, float heat_capacity, float thermal_conductivity)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.ModifyElementChunkAdjusterMessage* ptr = stackalloc SimMessages.ModifyElementChunkAdjusterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyElementChunkAdjusterMessage))];
		ptr->handle = sim_handle;
		ptr->temperature = temperature;
		ptr->heatCapacity = heat_capacity;
		ptr->thermalConductivity = thermal_conductivity;
		Sim.SIM_HandleMessage(-1387601379, sizeof(SimMessages.ModifyElementChunkAdjusterMessage), (byte*)ptr);
	}

	// Token: 0x06009387 RID: 37767 RVA: 0x0038E64C File Offset: 0x0038C84C
	public unsafe static void AddBuildingHeatExchange(Extents extents, float mass, float temperature, float thermal_conductivity, float operating_kw, ushort elem_idx, int callbackIdx = -1)
	{
		if (!Grid.IsValidCell(Grid.XYToCell(extents.x, extents.y)))
		{
			return;
		}
		int num = Grid.XYToCell(extents.x + extents.width, extents.y + extents.height);
		if (!Grid.IsValidCell(num))
		{
			Debug.LogErrorFormat("Invalid Cell [{0}] Extents [{1},{2}] [{3},{4}]", new object[]
			{
				num,
				extents.x,
				extents.y,
				extents.width,
				extents.height
			});
		}
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		SimMessages.AddBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.AddBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddBuildingHeatExchangeMessage))];
		ptr->callbackIdx = callbackIdx;
		ptr->elemIdx = elem_idx;
		ptr->mass = mass;
		ptr->temperature = temperature;
		ptr->thermalConductivity = thermal_conductivity;
		ptr->overheatTemperature = float.MaxValue;
		ptr->operatingKilowatts = operating_kw;
		ptr->minX = extents.x;
		ptr->minY = extents.y;
		ptr->maxX = extents.x + extents.width;
		ptr->maxY = extents.y + extents.height;
		Sim.SIM_HandleMessage(1739021608, sizeof(SimMessages.AddBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x06009388 RID: 37768 RVA: 0x0038E788 File Offset: 0x0038C988
	public unsafe static void ModifyBuildingHeatExchange(int sim_handle, Extents extents, float mass, float temperature, float thermal_conductivity, float overheat_temperature, float operating_kw, ushort element_idx)
	{
		int cell = Grid.XYToCell(extents.x, extents.y);
		Debug.Assert(Grid.IsValidCell(cell));
		if (!Grid.IsValidCell(cell))
		{
			return;
		}
		int cell2 = Grid.XYToCell(extents.x + extents.width, extents.y + extents.height);
		Debug.Assert(Grid.IsValidCell(cell2));
		if (!Grid.IsValidCell(cell2))
		{
			return;
		}
		SimMessages.ModifyBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.ModifyBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyBuildingHeatExchangeMessage))];
		ptr->callbackIdx = sim_handle;
		ptr->elemIdx = element_idx;
		ptr->mass = mass;
		ptr->temperature = temperature;
		ptr->thermalConductivity = thermal_conductivity;
		ptr->overheatTemperature = overheat_temperature;
		ptr->operatingKilowatts = operating_kw;
		ptr->minX = extents.x;
		ptr->minY = extents.y;
		ptr->maxX = extents.x + extents.width;
		ptr->maxY = extents.y + extents.height;
		Sim.SIM_HandleMessage(1818001569, sizeof(SimMessages.ModifyBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x06009389 RID: 37769 RVA: 0x0038E87C File Offset: 0x0038CA7C
	public unsafe static void RemoveBuildingHeatExchange(int sim_handle, int callbackIdx = -1)
	{
		SimMessages.RemoveBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.RemoveBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveBuildingHeatExchangeMessage))];
		Debug.Assert(Sim.IsValidHandle(sim_handle));
		ptr->handle = sim_handle;
		ptr->callbackIdx = callbackIdx;
		Sim.SIM_HandleMessage(-456116629, sizeof(SimMessages.RemoveBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x0600938A RID: 37770 RVA: 0x0038E8C0 File Offset: 0x0038CAC0
	public unsafe static void ModifyBuildingEnergy(int sim_handle, float delta_kj, float min_temperature, float max_temperature)
	{
		SimMessages.ModifyBuildingEnergyMessage* ptr = stackalloc SimMessages.ModifyBuildingEnergyMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyBuildingEnergyMessage))];
		Debug.Assert(Sim.IsValidHandle(sim_handle));
		ptr->handle = sim_handle;
		ptr->deltaKJ = delta_kj;
		ptr->minTemperature = min_temperature;
		ptr->maxTemperature = max_temperature;
		Sim.SIM_HandleMessage(-1348791658, sizeof(SimMessages.ModifyBuildingEnergyMessage), (byte*)ptr);
	}

	// Token: 0x0600938B RID: 37771 RVA: 0x0038E914 File Offset: 0x0038CB14
	public unsafe static void RegisterBuildingToBuildingHeatExchange(int structureTemperatureHandler, int callbackIdx = -1)
	{
		SimMessages.RegisterBuildingToBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.RegisterBuildingToBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RegisterBuildingToBuildingHeatExchangeMessage))];
		ptr->structureTemperatureHandler = structureTemperatureHandler;
		ptr->callbackIdx = callbackIdx;
		Sim.SIM_HandleMessage(-1338718217, sizeof(SimMessages.RegisterBuildingToBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x0600938C RID: 37772 RVA: 0x0038E950 File Offset: 0x0038CB50
	public unsafe static void AddBuildingToBuildingHeatExchange(int selfHandler, int buildingInContact, int cellsInContact)
	{
		SimMessages.AddBuildingToBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.AddBuildingToBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddBuildingToBuildingHeatExchangeMessage))];
		ptr->selfHandler = selfHandler;
		ptr->buildingInContactHandle = buildingInContact;
		ptr->cellsInContact = cellsInContact;
		Sim.SIM_HandleMessage(-1586724321, sizeof(SimMessages.AddBuildingToBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x0600938D RID: 37773 RVA: 0x0038E990 File Offset: 0x0038CB90
	public unsafe static void RemoveBuildingInContactFromBuildingToBuildingHeatExchange(int selfHandler, int buildingToRemove)
	{
		SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage))];
		ptr->selfHandler = selfHandler;
		ptr->buildingNoLongerInContactHandler = buildingToRemove;
		Sim.SIM_HandleMessage(-1993857213, sizeof(SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x0600938E RID: 37774 RVA: 0x0038E9CC File Offset: 0x0038CBCC
	public unsafe static void RemoveBuildingToBuildingHeatExchange(int selfHandler, int callback = -1)
	{
		SimMessages.RemoveBuildingToBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.RemoveBuildingToBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveBuildingToBuildingHeatExchangeMessage))];
		ptr->callbackIdx = callback;
		ptr->selfHandler = selfHandler;
		Sim.SIM_HandleMessage(697100730, sizeof(SimMessages.RemoveBuildingToBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x0600938F RID: 37775 RVA: 0x0038EA08 File Offset: 0x0038CC08
	public unsafe static void AddDiseaseEmitter(int callbackIdx)
	{
		SimMessages.AddDiseaseEmitterMessage* ptr = stackalloc SimMessages.AddDiseaseEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddDiseaseEmitterMessage))];
		ptr->callbackIdx = callbackIdx;
		Sim.SIM_HandleMessage(1486783027, sizeof(SimMessages.AddDiseaseEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06009390 RID: 37776 RVA: 0x0038EA3C File Offset: 0x0038CC3C
	public unsafe static void ModifyDiseaseEmitter(int sim_handle, int cell, byte range, byte disease_idx, float emit_interval, int emit_count)
	{
		Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.ModifyDiseaseEmitterMessage* ptr = stackalloc SimMessages.ModifyDiseaseEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyDiseaseEmitterMessage))];
		ptr->handle = sim_handle;
		ptr->gameCell = cell;
		ptr->maxDepth = range;
		ptr->diseaseIdx = disease_idx;
		ptr->emitInterval = emit_interval;
		ptr->emitCount = emit_count;
		Sim.SIM_HandleMessage(-1899123924, sizeof(SimMessages.ModifyDiseaseEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06009391 RID: 37777 RVA: 0x0038EAA0 File Offset: 0x0038CCA0
	public unsafe static void RemoveDiseaseEmitter(int cb_handle, int sim_handle)
	{
		Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.RemoveDiseaseEmitterMessage* ptr = stackalloc SimMessages.RemoveDiseaseEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveDiseaseEmitterMessage))];
		ptr->handle = sim_handle;
		ptr->callbackIdx = cb_handle;
		Sim.SIM_HandleMessage(468135926, sizeof(SimMessages.RemoveDiseaseEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06009392 RID: 37778 RVA: 0x0038EAE4 File Offset: 0x0038CCE4
	public unsafe static void SetSavedOptionValue(SimMessages.SimSavedOptions option, int zero_or_one)
	{
		SimMessages.SetSavedOptionsMessage* ptr = stackalloc SimMessages.SetSavedOptionsMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetSavedOptionsMessage))];
		if (zero_or_one == 0)
		{
			SimMessages.SetSavedOptionsMessage* ptr2 = ptr;
			ptr2->clearBits = (ptr2->clearBits | (byte)option);
			ptr->setBits = 0;
		}
		else
		{
			ptr->clearBits = 0;
			SimMessages.SetSavedOptionsMessage* ptr3 = ptr;
			ptr3->setBits = (ptr3->setBits | (byte)option);
		}
		Sim.SIM_HandleMessage(1154135737, sizeof(SimMessages.SetSavedOptionsMessage), (byte*)ptr);
	}

	// Token: 0x06009393 RID: 37779 RVA: 0x0038EB3C File Offset: 0x0038CD3C
	private static void WriteKleiString(this BinaryWriter writer, string str)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		writer.Write(bytes.Length);
		if (bytes.Length != 0)
		{
			writer.Write(bytes);
		}
	}

	// Token: 0x06009394 RID: 37780 RVA: 0x0038EB6C File Offset: 0x0038CD6C
	public unsafe static void CreateSimElementsTable(List<Element> elements)
	{
		MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(Sim.Element)) * elements.Count);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		Debug.Assert(elements.Count < 65535, "SimDLL internals assume there are fewer than 65535 elements");
		binaryWriter.Write(elements.Count);
		for (int i = 0; i < elements.Count; i++)
		{
			Sim.Element element = new Sim.Element(elements[i], elements);
			element.Write(binaryWriter);
		}
		for (int j = 0; j < elements.Count; j++)
		{
			binaryWriter.WriteKleiString(UI.StripLinkFormatting(elements[j].name));
		}
		byte[] buffer = memoryStream.GetBuffer();
		byte[] array;
		byte* msg;
		if ((array = buffer) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		Sim.SIM_HandleMessage(1108437482, buffer.Length, msg);
		array = null;
	}

	// Token: 0x06009395 RID: 37781 RVA: 0x0038EC5C File Offset: 0x0038CE5C
	public unsafe static void CreateDiseaseTable(Diseases diseases)
	{
		MemoryStream memoryStream = new MemoryStream(1024);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(diseases.Count);
		List<Element> elements = ElementLoader.elements;
		binaryWriter.Write(elements.Count);
		for (int i = 0; i < diseases.Count; i++)
		{
			Disease disease = diseases[i];
			binaryWriter.WriteKleiString(UI.StripLinkFormatting(disease.Name));
			binaryWriter.Write(disease.id.GetHashCode());
			binaryWriter.Write(disease.strength);
			disease.temperatureRange.Write(binaryWriter);
			disease.temperatureHalfLives.Write(binaryWriter);
			disease.pressureRange.Write(binaryWriter);
			disease.pressureHalfLives.Write(binaryWriter);
			binaryWriter.Write(disease.radiationKillRate);
			for (int j = 0; j < elements.Count; j++)
			{
				ElemGrowthInfo elemGrowthInfo = disease.elemGrowthInfo[j];
				elemGrowthInfo.Write(binaryWriter);
			}
		}
		byte[] array;
		byte* msg;
		if ((array = memoryStream.GetBuffer()) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		Sim.SIM_HandleMessage(825301935, (int)memoryStream.Length, msg);
		array = null;
	}

	// Token: 0x06009396 RID: 37782 RVA: 0x0038ED98 File Offset: 0x0038CF98
	public unsafe static void DefineWorldOffsets(List<SimMessages.WorldOffsetData> worldOffsets)
	{
		MemoryStream memoryStream = new MemoryStream(1024);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(worldOffsets.Count);
		foreach (SimMessages.WorldOffsetData worldOffsetData in worldOffsets)
		{
			binaryWriter.Write(worldOffsetData.worldOffsetX);
			binaryWriter.Write(worldOffsetData.worldOffsetY);
			binaryWriter.Write(worldOffsetData.worldSizeX);
			binaryWriter.Write(worldOffsetData.worldSizeY);
		}
		byte[] array;
		byte* msg;
		if ((array = memoryStream.GetBuffer()) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		Sim.SIM_HandleMessage(-895846551, (int)memoryStream.Length, msg);
		array = null;
	}

	// Token: 0x06009397 RID: 37783 RVA: 0x0038EE68 File Offset: 0x0038D068
	public static void SimDataInitializeFromCells(int width, int height, Sim.Cell[] cells, float[] bgTemp, Sim.DiseaseCell[] dc, bool headless)
	{
		MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(Sim.Cell)) * width * height + Marshal.SizeOf(typeof(float)) * width * height + Marshal.SizeOf(typeof(Sim.DiseaseCell)) * width * height);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(width);
		binaryWriter.Write(height);
		bool value = Sim.IsRadiationEnabled();
		binaryWriter.Write(value);
		binaryWriter.Write(headless);
		int num = width * height;
		for (int i = 0; i < num; i++)
		{
			cells[i].Write(binaryWriter);
		}
		for (int j = 0; j < num; j++)
		{
			binaryWriter.Write(bgTemp[j]);
		}
		for (int k = 0; k < num; k++)
		{
			dc[k].Write(binaryWriter);
		}
		byte[] buffer = memoryStream.GetBuffer();
		Sim.HandleMessage(SimMessageHashes.SimData_InitializeFromCells, buffer.Length, buffer);
	}

	// Token: 0x06009398 RID: 37784 RVA: 0x0038EF74 File Offset: 0x0038D174
	public static void SimDataResizeGridAndInitializeVacuumCells(Vector2I grid_size, int width, int height, int x_offset, int y_offset)
	{
		MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(int)));
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(grid_size.x);
		binaryWriter.Write(grid_size.y);
		binaryWriter.Write(width);
		binaryWriter.Write(height);
		binaryWriter.Write(x_offset);
		binaryWriter.Write(y_offset);
		byte[] buffer = memoryStream.GetBuffer();
		Sim.HandleMessage(SimMessageHashes.SimData_ResizeAndInitializeVacuumCells, buffer.Length, buffer);
	}

	// Token: 0x06009399 RID: 37785 RVA: 0x0038EFF4 File Offset: 0x0038D1F4
	public static void SimDataFreeCells(int width, int height, int x_offset, int y_offset)
	{
		MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(int)));
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(width);
		binaryWriter.Write(height);
		binaryWriter.Write(x_offset);
		binaryWriter.Write(y_offset);
		byte[] buffer = memoryStream.GetBuffer();
		Sim.HandleMessage(SimMessageHashes.SimData_FreeCells, buffer.Length, buffer);
	}

	// Token: 0x0600939A RID: 37786 RVA: 0x0038F05C File Offset: 0x0038D25C
	public unsafe static void Dig(int gameCell, int callbackIdx = -1, bool skipEvent = false)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.DigMessage* ptr = stackalloc SimMessages.DigMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.DigMessage))];
		ptr->cellIdx = gameCell;
		ptr->callbackIdx = callbackIdx;
		ptr->skipEvent = skipEvent;
		Sim.SIM_HandleMessage(833038498, sizeof(SimMessages.DigMessage), (byte*)ptr);
	}

	// Token: 0x0600939B RID: 37787 RVA: 0x0038F0A8 File Offset: 0x0038D2A8
	public unsafe static void SetInsulation(int gameCell, float value)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.SetCellFloatValueMessage* ptr = stackalloc SimMessages.SetCellFloatValueMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetCellFloatValueMessage))];
		ptr->cellIdx = gameCell;
		ptr->value = value;
		Sim.SIM_HandleMessage(-898773121, sizeof(SimMessages.SetCellFloatValueMessage), (byte*)ptr);
	}

	// Token: 0x0600939C RID: 37788 RVA: 0x0038F0EC File Offset: 0x0038D2EC
	public unsafe static void SetStrength(int gameCell, int weight, float strengthMultiplier)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.SetCellFloatValueMessage* ptr = stackalloc SimMessages.SetCellFloatValueMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetCellFloatValueMessage))];
		ptr->cellIdx = gameCell;
		int num = (int)(strengthMultiplier * 4f) & 127;
		int num2 = (weight & 1) << 7 | num;
		ptr->value = (float)((byte)num2);
		Sim.SIM_HandleMessage(1593243982, sizeof(SimMessages.SetCellFloatValueMessage), (byte*)ptr);
	}

	// Token: 0x0600939D RID: 37789 RVA: 0x0038F144 File Offset: 0x0038D344
	public unsafe static void SetCellProperties(int gameCell, byte properties)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.CellPropertiesMessage* ptr = stackalloc SimMessages.CellPropertiesMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellPropertiesMessage))];
		ptr->cellIdx = gameCell;
		ptr->properties = properties;
		ptr->set = 1;
		Sim.SIM_HandleMessage(-469311643, sizeof(SimMessages.CellPropertiesMessage), (byte*)ptr);
	}

	// Token: 0x0600939E RID: 37790 RVA: 0x0038F190 File Offset: 0x0038D390
	public unsafe static void ClearCellProperties(int gameCell, byte properties)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.CellPropertiesMessage* ptr = stackalloc SimMessages.CellPropertiesMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellPropertiesMessage))];
		ptr->cellIdx = gameCell;
		ptr->properties = properties;
		ptr->set = 0;
		Sim.SIM_HandleMessage(-469311643, sizeof(SimMessages.CellPropertiesMessage), (byte*)ptr);
	}

	// Token: 0x0600939F RID: 37791 RVA: 0x0038F1DC File Offset: 0x0038D3DC
	public unsafe static void ModifyCell(int gameCell, ushort elementIdx, float temperature, float mass, byte disease_idx, int disease_count, SimMessages.ReplaceType replace_type = SimMessages.ReplaceType.None, bool do_vertical_solid_displacement = false, int callbackIdx = -1)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		Element element = ElementLoader.elements[(int)elementIdx];
		if (element.maxMass == 0f && mass > element.maxMass)
		{
			Debug.LogWarningFormat("Invalid cell modification (mass greater than element maximum): Cell={0}, EIdx={1}, T={2}, M={3}, {4} max mass = {5}", new object[]
			{
				gameCell,
				elementIdx,
				temperature,
				mass,
				element.id,
				element.maxMass
			});
			mass = element.maxMass;
		}
		if (temperature < 0f || temperature > 10000f)
		{
			Debug.LogWarningFormat("Invalid cell modification (temp out of bounds): Cell={0}, EIdx={1}, T={2}, M={3}, {4} default temp = {5}", new object[]
			{
				gameCell,
				elementIdx,
				temperature,
				mass,
				element.id,
				element.defaultValues.temperature
			});
			temperature = element.defaultValues.temperature;
		}
		if (temperature == 0f && mass > 0f)
		{
			Debug.LogWarningFormat("Invalid cell modification (zero temp with non-zero mass): Cell={0}, EIdx={1}, T={2}, M={3}, {4} default temp = {5}", new object[]
			{
				gameCell,
				elementIdx,
				temperature,
				mass,
				element.id,
				element.defaultValues.temperature
			});
			temperature = element.defaultValues.temperature;
		}
		SimMessages.ModifyCellMessage* ptr = stackalloc SimMessages.ModifyCellMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyCellMessage))];
		ptr->cellIdx = gameCell;
		ptr->callbackIdx = callbackIdx;
		ptr->temperature = temperature;
		ptr->mass = mass;
		ptr->elementIdx = elementIdx;
		ptr->replaceType = (byte)replace_type;
		ptr->diseaseIdx = disease_idx;
		ptr->diseaseCount = disease_count;
		ptr->addSubType = (do_vertical_solid_displacement ? 0 : 1);
		Sim.SIM_HandleMessage(-1252920804, sizeof(SimMessages.ModifyCellMessage), (byte*)ptr);
	}

	// Token: 0x060093A0 RID: 37792 RVA: 0x0038F3BC File Offset: 0x0038D5BC
	public unsafe static void ModifyDiseaseOnCell(int gameCell, byte disease_idx, int disease_delta)
	{
		SimMessages.CellDiseaseModification* ptr = stackalloc SimMessages.CellDiseaseModification[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellDiseaseModification))];
		ptr->cellIdx = gameCell;
		ptr->diseaseIdx = disease_idx;
		ptr->diseaseCount = disease_delta;
		Sim.SIM_HandleMessage(-1853671274, sizeof(SimMessages.CellDiseaseModification), (byte*)ptr);
	}

	// Token: 0x060093A1 RID: 37793 RVA: 0x0038F3FC File Offset: 0x0038D5FC
	public unsafe static void ModifyRadiationOnCell(int gameCell, float radiationDelta, int callbackIdx = -1)
	{
		SimMessages.CellRadiationModification* ptr = stackalloc SimMessages.CellRadiationModification[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellRadiationModification))];
		ptr->cellIdx = gameCell;
		ptr->radiationDelta = radiationDelta;
		ptr->callbackIdx = callbackIdx;
		Sim.SIM_HandleMessage(-1914877797, sizeof(SimMessages.CellRadiationModification), (byte*)ptr);
	}

	// Token: 0x060093A2 RID: 37794 RVA: 0x0038F43C File Offset: 0x0038D63C
	public unsafe static void ModifyRadiationParams(RadiationParams type, float value)
	{
		SimMessages.RadiationParamsModification* ptr = stackalloc SimMessages.RadiationParamsModification[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RadiationParamsModification))];
		ptr->RadiationParamsType = (int)type;
		ptr->value = value;
		Sim.SIM_HandleMessage(377112707, sizeof(SimMessages.RadiationParamsModification), (byte*)ptr);
	}

	// Token: 0x060093A3 RID: 37795 RVA: 0x00100459 File Offset: 0x000FE659
	public static ushort GetElementIndex(SimHashes element)
	{
		return ElementLoader.GetElementIndex(element);
	}

	// Token: 0x060093A4 RID: 37796 RVA: 0x0038F478 File Offset: 0x0038D678
	public unsafe static void ConsumeMass(int gameCell, SimHashes element, float mass, byte radius, int callbackIdx = -1)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		ushort elementIndex = ElementLoader.GetElementIndex(element);
		SimMessages.MassConsumptionMessage* ptr = stackalloc SimMessages.MassConsumptionMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.MassConsumptionMessage))];
		ptr->cellIdx = gameCell;
		ptr->callbackIdx = callbackIdx;
		ptr->mass = mass;
		ptr->elementIdx = elementIndex;
		ptr->radius = radius;
		Sim.SIM_HandleMessage(1727657959, sizeof(SimMessages.MassConsumptionMessage), (byte*)ptr);
	}

	// Token: 0x060093A5 RID: 37797 RVA: 0x0038F4D8 File Offset: 0x0038D6D8
	public unsafe static void EmitMass(int gameCell, ushort element_idx, float mass, float temperature, byte disease_idx, int disease_count, int callbackIdx = -1)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.MassEmissionMessage* ptr = stackalloc SimMessages.MassEmissionMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.MassEmissionMessage))];
		ptr->cellIdx = gameCell;
		ptr->callbackIdx = callbackIdx;
		ptr->mass = mass;
		ptr->temperature = temperature;
		ptr->elementIdx = element_idx;
		ptr->diseaseIdx = disease_idx;
		ptr->diseaseCount = disease_count;
		Sim.SIM_HandleMessage(797274363, sizeof(SimMessages.MassEmissionMessage), (byte*)ptr);
	}

	// Token: 0x060093A6 RID: 37798 RVA: 0x0038F540 File Offset: 0x0038D740
	public unsafe static void ConsumeDisease(int game_cell, float percent_to_consume, int max_to_consume, int callback_idx)
	{
		if (!Grid.IsValidCell(game_cell))
		{
			return;
		}
		SimMessages.ConsumeDiseaseMessage* ptr = stackalloc SimMessages.ConsumeDiseaseMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ConsumeDiseaseMessage))];
		ptr->callbackIdx = callback_idx;
		ptr->gameCell = game_cell;
		ptr->percentToConsume = percent_to_consume;
		ptr->maxToConsume = max_to_consume;
		Sim.SIM_HandleMessage(-1019841536, sizeof(SimMessages.ConsumeDiseaseMessage), (byte*)ptr);
	}

	// Token: 0x060093A7 RID: 37799 RVA: 0x0038F590 File Offset: 0x0038D790
	public static void AddRemoveSubstance(int gameCell, SimHashes new_element, CellAddRemoveSubstanceEvent ev, float mass, float temperature, byte disease_idx, int disease_count, bool do_vertical_solid_displacement = true, int callbackIdx = -1)
	{
		ushort elementIndex = SimMessages.GetElementIndex(new_element);
		SimMessages.AddRemoveSubstance(gameCell, elementIndex, ev, mass, temperature, disease_idx, disease_count, do_vertical_solid_displacement, callbackIdx);
	}

	// Token: 0x060093A8 RID: 37800 RVA: 0x0038F5B8 File Offset: 0x0038D7B8
	public static void AddRemoveSubstance(int gameCell, ushort elementIdx, CellAddRemoveSubstanceEvent ev, float mass, float temperature, byte disease_idx, int disease_count, bool do_vertical_solid_displacement = true, int callbackIdx = -1)
	{
		if (elementIdx == 65535)
		{
			return;
		}
		Element element = ElementLoader.elements[(int)elementIdx];
		float temperature2 = (temperature != -1f) ? temperature : element.defaultValues.temperature;
		SimMessages.ModifyCell(gameCell, elementIdx, temperature2, mass, disease_idx, disease_count, SimMessages.ReplaceType.None, do_vertical_solid_displacement, callbackIdx);
	}

	// Token: 0x060093A9 RID: 37801 RVA: 0x0038F608 File Offset: 0x0038D808
	public static void ReplaceElement(int gameCell, SimHashes new_element, CellElementEvent ev, float mass, float temperature = -1f, byte diseaseIdx = 255, int diseaseCount = 0, int callbackIdx = -1)
	{
		ushort elementIndex = SimMessages.GetElementIndex(new_element);
		if (elementIndex != 65535)
		{
			Element element = ElementLoader.elements[(int)elementIndex];
			float temperature2 = (temperature != -1f) ? temperature : element.defaultValues.temperature;
			SimMessages.ModifyCell(gameCell, elementIndex, temperature2, mass, diseaseIdx, diseaseCount, SimMessages.ReplaceType.Replace, false, callbackIdx);
		}
	}

	// Token: 0x060093AA RID: 37802 RVA: 0x0038F65C File Offset: 0x0038D85C
	public static void ReplaceAndDisplaceElement(int gameCell, SimHashes new_element, CellElementEvent ev, float mass, float temperature = -1f, byte disease_idx = 255, int disease_count = 0, int callbackIdx = -1)
	{
		ushort elementIndex = SimMessages.GetElementIndex(new_element);
		if (elementIndex != 65535)
		{
			Element element = ElementLoader.elements[(int)elementIndex];
			float temperature2 = (temperature != -1f) ? temperature : element.defaultValues.temperature;
			SimMessages.ModifyCell(gameCell, elementIndex, temperature2, mass, disease_idx, disease_count, SimMessages.ReplaceType.ReplaceAndDisplace, false, callbackIdx);
		}
	}

	// Token: 0x060093AB RID: 37803 RVA: 0x0038F6B0 File Offset: 0x0038D8B0
	public unsafe static void ModifyEnergy(int gameCell, float kilojoules, float max_temperature, SimMessages.EnergySourceID id)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		if (max_temperature <= 0f)
		{
			Debug.LogError("invalid max temperature for cell energy modification");
			return;
		}
		SimMessages.ModifyCellEnergyMessage* ptr = stackalloc SimMessages.ModifyCellEnergyMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyCellEnergyMessage))];
		ptr->cellIdx = gameCell;
		ptr->kilojoules = kilojoules;
		ptr->maxTemperature = max_temperature;
		ptr->id = (int)id;
		Sim.SIM_HandleMessage(818320644, sizeof(SimMessages.ModifyCellEnergyMessage), (byte*)ptr);
	}

	// Token: 0x060093AC RID: 37804 RVA: 0x0038F714 File Offset: 0x0038D914
	public static void ModifyMass(int gameCell, float mass, byte disease_idx, int disease_count, CellModifyMassEvent ev, float temperature = -1f, SimHashes element = SimHashes.Vacuum)
	{
		if (element != SimHashes.Vacuum)
		{
			ushort elementIndex = SimMessages.GetElementIndex(element);
			if (elementIndex != 65535)
			{
				if (temperature == -1f)
				{
					temperature = ElementLoader.elements[(int)elementIndex].defaultValues.temperature;
				}
				SimMessages.ModifyCell(gameCell, elementIndex, temperature, mass, disease_idx, disease_count, SimMessages.ReplaceType.None, false, -1);
				return;
			}
		}
		else
		{
			SimMessages.ModifyCell(gameCell, 0, temperature, mass, disease_idx, disease_count, SimMessages.ReplaceType.None, false, -1);
		}
	}

	// Token: 0x060093AD RID: 37805 RVA: 0x0038F77C File Offset: 0x0038D97C
	public unsafe static void CreateElementInteractions(SimMessages.ElementInteraction[] interactions)
	{
		fixed (SimMessages.ElementInteraction[] array = interactions)
		{
			SimMessages.ElementInteraction* interactions2;
			if (interactions == null || array.Length == 0)
			{
				interactions2 = null;
			}
			else
			{
				interactions2 = &array[0];
			}
			SimMessages.CreateElementInteractionsMsg* ptr = stackalloc SimMessages.CreateElementInteractionsMsg[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CreateElementInteractionsMsg))];
			ptr->numInteractions = interactions.Length;
			ptr->interactions = interactions2;
			Sim.SIM_HandleMessage(-930289787, sizeof(SimMessages.CreateElementInteractionsMsg), (byte*)ptr);
		}
	}

	// Token: 0x060093AE RID: 37806 RVA: 0x0038F7D4 File Offset: 0x0038D9D4
	public unsafe static void NewGameFrame(float elapsed_seconds, List<Game.SimActiveRegion> activeRegions)
	{
		Debug.Assert(activeRegions.Count > 0, "NewGameFrame cannot be called with zero activeRegions");
		Sim.NewGameFrame* ptr = stackalloc Sim.NewGameFrame[checked(unchecked((UIntPtr)activeRegions.Count) * (UIntPtr)sizeof(Sim.NewGameFrame))];
		Sim.NewGameFrame* ptr2 = ptr;
		foreach (Game.SimActiveRegion simActiveRegion in activeRegions)
		{
			Pair<Vector2I, Vector2I> region = simActiveRegion.region;
			region.first = new Vector2I(MathUtil.Clamp(0, Grid.WidthInCells - 1, simActiveRegion.region.first.x), MathUtil.Clamp(0, Grid.HeightInCells - 1, simActiveRegion.region.first.y));
			region.second = new Vector2I(MathUtil.Clamp(0, Grid.WidthInCells, simActiveRegion.region.second.x), MathUtil.Clamp(0, Grid.HeightInCells - 1, simActiveRegion.region.second.y));
			ptr2->elapsedSeconds = elapsed_seconds;
			ptr2->minX = region.first.x;
			ptr2->minY = region.first.y;
			ptr2->maxX = region.second.x;
			ptr2->maxY = region.second.y;
			ptr2->currentSunlightIntensity = simActiveRegion.currentSunlightIntensity;
			ptr2->currentCosmicRadiationIntensity = simActiveRegion.currentCosmicRadiationIntensity;
			ptr2++;
		}
		Sim.SIM_HandleMessage(-775326397, sizeof(Sim.NewGameFrame) * activeRegions.Count, (byte*)ptr);
	}

	// Token: 0x060093AF RID: 37807 RVA: 0x0038F970 File Offset: 0x0038DB70
	public unsafe static void SetDebugProperties(Sim.DebugProperties properties)
	{
		Sim.DebugProperties* ptr = stackalloc Sim.DebugProperties[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(Sim.DebugProperties))];
		*ptr = properties;
		ptr->buildingTemperatureScale = properties.buildingTemperatureScale;
		ptr->buildingToBuildingTemperatureScale = properties.buildingToBuildingTemperatureScale;
		Sim.SIM_HandleMessage(-1683118492, sizeof(Sim.DebugProperties), (byte*)ptr);
	}

	// Token: 0x060093B0 RID: 37808 RVA: 0x0038F9BC File Offset: 0x0038DBBC
	public unsafe static void ModifyCellWorldZone(int cell, byte zone_id)
	{
		SimMessages.CellWorldZoneModification* ptr = stackalloc SimMessages.CellWorldZoneModification[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellWorldZoneModification))];
		ptr->cell = cell;
		ptr->zoneID = zone_id;
		Sim.SIM_HandleMessage(-449718014, sizeof(SimMessages.CellWorldZoneModification), (byte*)ptr);
	}

	// Token: 0x040070B4 RID: 28852
	public const int InvalidCallback = -1;

	// Token: 0x040070B5 RID: 28853
	public const float STATE_TRANSITION_TEMPERATURE_BUFER = 3f;

	// Token: 0x02001B8C RID: 7052
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct AddElementConsumerMessage
	{
		// Token: 0x040070B6 RID: 28854
		public int cellIdx;

		// Token: 0x040070B7 RID: 28855
		public int callbackIdx;

		// Token: 0x040070B8 RID: 28856
		public byte radius;

		// Token: 0x040070B9 RID: 28857
		public byte configuration;

		// Token: 0x040070BA RID: 28858
		public ushort elementIdx;
	}

	// Token: 0x02001B8D RID: 7053
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetElementConsumerDataMessage
	{
		// Token: 0x040070BB RID: 28859
		public int handle;

		// Token: 0x040070BC RID: 28860
		public int cell;

		// Token: 0x040070BD RID: 28861
		public float consumptionRate;
	}

	// Token: 0x02001B8E RID: 7054
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RemoveElementConsumerMessage
	{
		// Token: 0x040070BE RID: 28862
		public int handle;

		// Token: 0x040070BF RID: 28863
		public int callbackIdx;
	}

	// Token: 0x02001B8F RID: 7055
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct AddElementEmitterMessage
	{
		// Token: 0x040070C0 RID: 28864
		public float maxPressure;

		// Token: 0x040070C1 RID: 28865
		public int callbackIdx;

		// Token: 0x040070C2 RID: 28866
		public int onBlockedCB;

		// Token: 0x040070C3 RID: 28867
		public int onUnblockedCB;
	}

	// Token: 0x02001B90 RID: 7056
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyElementEmitterMessage
	{
		// Token: 0x040070C4 RID: 28868
		public int handle;

		// Token: 0x040070C5 RID: 28869
		public int cellIdx;

		// Token: 0x040070C6 RID: 28870
		public float emitInterval;

		// Token: 0x040070C7 RID: 28871
		public float emitMass;

		// Token: 0x040070C8 RID: 28872
		public float emitTemperature;

		// Token: 0x040070C9 RID: 28873
		public float maxPressure;

		// Token: 0x040070CA RID: 28874
		public int diseaseCount;

		// Token: 0x040070CB RID: 28875
		public ushort elementIdx;

		// Token: 0x040070CC RID: 28876
		public byte maxDepth;

		// Token: 0x040070CD RID: 28877
		public byte diseaseIdx;
	}

	// Token: 0x02001B91 RID: 7057
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RemoveElementEmitterMessage
	{
		// Token: 0x040070CE RID: 28878
		public int handle;

		// Token: 0x040070CF RID: 28879
		public int callbackIdx;
	}

	// Token: 0x02001B92 RID: 7058
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct AddRadiationEmitterMessage
	{
		// Token: 0x040070D0 RID: 28880
		public int callbackIdx;

		// Token: 0x040070D1 RID: 28881
		public int cell;

		// Token: 0x040070D2 RID: 28882
		public short emitRadiusX;

		// Token: 0x040070D3 RID: 28883
		public short emitRadiusY;

		// Token: 0x040070D4 RID: 28884
		public float emitRads;

		// Token: 0x040070D5 RID: 28885
		public float emitRate;

		// Token: 0x040070D6 RID: 28886
		public float emitSpeed;

		// Token: 0x040070D7 RID: 28887
		public float emitDirection;

		// Token: 0x040070D8 RID: 28888
		public float emitAngle;

		// Token: 0x040070D9 RID: 28889
		public int emitType;
	}

	// Token: 0x02001B93 RID: 7059
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyRadiationEmitterMessage
	{
		// Token: 0x040070DA RID: 28890
		public int handle;

		// Token: 0x040070DB RID: 28891
		public int cell;

		// Token: 0x040070DC RID: 28892
		public int callbackIdx;

		// Token: 0x040070DD RID: 28893
		public short emitRadiusX;

		// Token: 0x040070DE RID: 28894
		public short emitRadiusY;

		// Token: 0x040070DF RID: 28895
		public float emitRads;

		// Token: 0x040070E0 RID: 28896
		public float emitRate;

		// Token: 0x040070E1 RID: 28897
		public float emitSpeed;

		// Token: 0x040070E2 RID: 28898
		public float emitDirection;

		// Token: 0x040070E3 RID: 28899
		public float emitAngle;

		// Token: 0x040070E4 RID: 28900
		public int emitType;
	}

	// Token: 0x02001B94 RID: 7060
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RemoveRadiationEmitterMessage
	{
		// Token: 0x040070E5 RID: 28901
		public int handle;

		// Token: 0x040070E6 RID: 28902
		public int callbackIdx;
	}

	// Token: 0x02001B95 RID: 7061
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct AddElementChunkMessage
	{
		// Token: 0x040070E7 RID: 28903
		public int gameCell;

		// Token: 0x040070E8 RID: 28904
		public int callbackIdx;

		// Token: 0x040070E9 RID: 28905
		public float mass;

		// Token: 0x040070EA RID: 28906
		public float temperature;

		// Token: 0x040070EB RID: 28907
		public float surfaceArea;

		// Token: 0x040070EC RID: 28908
		public float thickness;

		// Token: 0x040070ED RID: 28909
		public float groundTransferScale;

		// Token: 0x040070EE RID: 28910
		public ushort elementIdx;

		// Token: 0x040070EF RID: 28911
		public byte pad0;

		// Token: 0x040070F0 RID: 28912
		public byte pad1;
	}

	// Token: 0x02001B96 RID: 7062
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RemoveElementChunkMessage
	{
		// Token: 0x040070F1 RID: 28913
		public int handle;

		// Token: 0x040070F2 RID: 28914
		public int callbackIdx;
	}

	// Token: 0x02001B97 RID: 7063
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetElementChunkDataMessage
	{
		// Token: 0x040070F3 RID: 28915
		public int handle;

		// Token: 0x040070F4 RID: 28916
		public float temperature;

		// Token: 0x040070F5 RID: 28917
		public float heatCapacity;
	}

	// Token: 0x02001B98 RID: 7064
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct MoveElementChunkMessage
	{
		// Token: 0x040070F6 RID: 28918
		public int handle;

		// Token: 0x040070F7 RID: 28919
		public int gameCell;
	}

	// Token: 0x02001B99 RID: 7065
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyElementChunkEnergyMessage
	{
		// Token: 0x040070F8 RID: 28920
		public int handle;

		// Token: 0x040070F9 RID: 28921
		public float deltaKJ;
	}

	// Token: 0x02001B9A RID: 7066
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyElementChunkAdjusterMessage
	{
		// Token: 0x040070FA RID: 28922
		public int handle;

		// Token: 0x040070FB RID: 28923
		public float temperature;

		// Token: 0x040070FC RID: 28924
		public float heatCapacity;

		// Token: 0x040070FD RID: 28925
		public float thermalConductivity;
	}

	// Token: 0x02001B9B RID: 7067
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct AddBuildingHeatExchangeMessage
	{
		// Token: 0x040070FE RID: 28926
		public int callbackIdx;

		// Token: 0x040070FF RID: 28927
		public ushort elemIdx;

		// Token: 0x04007100 RID: 28928
		public byte pad0;

		// Token: 0x04007101 RID: 28929
		public byte pad1;

		// Token: 0x04007102 RID: 28930
		public float mass;

		// Token: 0x04007103 RID: 28931
		public float temperature;

		// Token: 0x04007104 RID: 28932
		public float thermalConductivity;

		// Token: 0x04007105 RID: 28933
		public float overheatTemperature;

		// Token: 0x04007106 RID: 28934
		public float operatingKilowatts;

		// Token: 0x04007107 RID: 28935
		public int minX;

		// Token: 0x04007108 RID: 28936
		public int minY;

		// Token: 0x04007109 RID: 28937
		public int maxX;

		// Token: 0x0400710A RID: 28938
		public int maxY;
	}

	// Token: 0x02001B9C RID: 7068
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ModifyBuildingHeatExchangeMessage
	{
		// Token: 0x0400710B RID: 28939
		public int callbackIdx;

		// Token: 0x0400710C RID: 28940
		public ushort elemIdx;

		// Token: 0x0400710D RID: 28941
		public byte pad0;

		// Token: 0x0400710E RID: 28942
		public byte pad1;

		// Token: 0x0400710F RID: 28943
		public float mass;

		// Token: 0x04007110 RID: 28944
		public float temperature;

		// Token: 0x04007111 RID: 28945
		public float thermalConductivity;

		// Token: 0x04007112 RID: 28946
		public float overheatTemperature;

		// Token: 0x04007113 RID: 28947
		public float operatingKilowatts;

		// Token: 0x04007114 RID: 28948
		public int minX;

		// Token: 0x04007115 RID: 28949
		public int minY;

		// Token: 0x04007116 RID: 28950
		public int maxX;

		// Token: 0x04007117 RID: 28951
		public int maxY;
	}

	// Token: 0x02001B9D RID: 7069
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ModifyBuildingEnergyMessage
	{
		// Token: 0x04007118 RID: 28952
		public int handle;

		// Token: 0x04007119 RID: 28953
		public float deltaKJ;

		// Token: 0x0400711A RID: 28954
		public float minTemperature;

		// Token: 0x0400711B RID: 28955
		public float maxTemperature;
	}

	// Token: 0x02001B9E RID: 7070
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RemoveBuildingHeatExchangeMessage
	{
		// Token: 0x0400711C RID: 28956
		public int handle;

		// Token: 0x0400711D RID: 28957
		public int callbackIdx;
	}

	// Token: 0x02001B9F RID: 7071
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RegisterBuildingToBuildingHeatExchangeMessage
	{
		// Token: 0x0400711E RID: 28958
		public int callbackIdx;

		// Token: 0x0400711F RID: 28959
		public int structureTemperatureHandler;
	}

	// Token: 0x02001BA0 RID: 7072
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct AddBuildingToBuildingHeatExchangeMessage
	{
		// Token: 0x04007120 RID: 28960
		public int selfHandler;

		// Token: 0x04007121 RID: 28961
		public int buildingInContactHandle;

		// Token: 0x04007122 RID: 28962
		public int cellsInContact;
	}

	// Token: 0x02001BA1 RID: 7073
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage
	{
		// Token: 0x04007123 RID: 28963
		public int selfHandler;

		// Token: 0x04007124 RID: 28964
		public int buildingNoLongerInContactHandler;
	}

	// Token: 0x02001BA2 RID: 7074
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RemoveBuildingToBuildingHeatExchangeMessage
	{
		// Token: 0x04007125 RID: 28965
		public int callbackIdx;

		// Token: 0x04007126 RID: 28966
		public int selfHandler;
	}

	// Token: 0x02001BA3 RID: 7075
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct AddDiseaseEmitterMessage
	{
		// Token: 0x04007127 RID: 28967
		public int callbackIdx;
	}

	// Token: 0x02001BA4 RID: 7076
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ModifyDiseaseEmitterMessage
	{
		// Token: 0x04007128 RID: 28968
		public int handle;

		// Token: 0x04007129 RID: 28969
		public int gameCell;

		// Token: 0x0400712A RID: 28970
		public byte diseaseIdx;

		// Token: 0x0400712B RID: 28971
		public byte maxDepth;

		// Token: 0x0400712C RID: 28972
		private byte pad0;

		// Token: 0x0400712D RID: 28973
		private byte pad1;

		// Token: 0x0400712E RID: 28974
		public float emitInterval;

		// Token: 0x0400712F RID: 28975
		public int emitCount;
	}

	// Token: 0x02001BA5 RID: 7077
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RemoveDiseaseEmitterMessage
	{
		// Token: 0x04007130 RID: 28976
		public int handle;

		// Token: 0x04007131 RID: 28977
		public int callbackIdx;
	}

	// Token: 0x02001BA6 RID: 7078
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetSavedOptionsMessage
	{
		// Token: 0x04007132 RID: 28978
		public byte clearBits;

		// Token: 0x04007133 RID: 28979
		public byte setBits;
	}

	// Token: 0x02001BA7 RID: 7079
	public enum SimSavedOptions : byte
	{
		// Token: 0x04007135 RID: 28981
		ENABLE_DIAGONAL_FALLING_SAND = 1
	}

	// Token: 0x02001BA8 RID: 7080
	public struct WorldOffsetData
	{
		// Token: 0x04007136 RID: 28982
		public int worldOffsetX;

		// Token: 0x04007137 RID: 28983
		public int worldOffsetY;

		// Token: 0x04007138 RID: 28984
		public int worldSizeX;

		// Token: 0x04007139 RID: 28985
		public int worldSizeY;
	}

	// Token: 0x02001BA9 RID: 7081
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct DigMessage
	{
		// Token: 0x0400713A RID: 28986
		public int cellIdx;

		// Token: 0x0400713B RID: 28987
		public int callbackIdx;

		// Token: 0x0400713C RID: 28988
		public bool skipEvent;
	}

	// Token: 0x02001BAA RID: 7082
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetCellFloatValueMessage
	{
		// Token: 0x0400713D RID: 28989
		public int cellIdx;

		// Token: 0x0400713E RID: 28990
		public float value;
	}

	// Token: 0x02001BAB RID: 7083
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct CellPropertiesMessage
	{
		// Token: 0x0400713F RID: 28991
		public int cellIdx;

		// Token: 0x04007140 RID: 28992
		public byte properties;

		// Token: 0x04007141 RID: 28993
		public byte set;

		// Token: 0x04007142 RID: 28994
		public byte pad0;

		// Token: 0x04007143 RID: 28995
		public byte pad1;
	}

	// Token: 0x02001BAC RID: 7084
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetInsulationValueMessage
	{
		// Token: 0x04007144 RID: 28996
		public int cellIdx;

		// Token: 0x04007145 RID: 28997
		public float value;
	}

	// Token: 0x02001BAD RID: 7085
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyCellMessage
	{
		// Token: 0x04007146 RID: 28998
		public int cellIdx;

		// Token: 0x04007147 RID: 28999
		public int callbackIdx;

		// Token: 0x04007148 RID: 29000
		public float temperature;

		// Token: 0x04007149 RID: 29001
		public float mass;

		// Token: 0x0400714A RID: 29002
		public int diseaseCount;

		// Token: 0x0400714B RID: 29003
		public ushort elementIdx;

		// Token: 0x0400714C RID: 29004
		public byte replaceType;

		// Token: 0x0400714D RID: 29005
		public byte diseaseIdx;

		// Token: 0x0400714E RID: 29006
		public byte addSubType;
	}

	// Token: 0x02001BAE RID: 7086
	public enum ReplaceType
	{
		// Token: 0x04007150 RID: 29008
		None,
		// Token: 0x04007151 RID: 29009
		Replace,
		// Token: 0x04007152 RID: 29010
		ReplaceAndDisplace
	}

	// Token: 0x02001BAF RID: 7087
	private enum AddSolidMassSubType
	{
		// Token: 0x04007154 RID: 29012
		DoVerticalDisplacement,
		// Token: 0x04007155 RID: 29013
		OnlyIfSameElement
	}

	// Token: 0x02001BB0 RID: 7088
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct CellDiseaseModification
	{
		// Token: 0x04007156 RID: 29014
		public int cellIdx;

		// Token: 0x04007157 RID: 29015
		public byte diseaseIdx;

		// Token: 0x04007158 RID: 29016
		public byte pad0;

		// Token: 0x04007159 RID: 29017
		public byte pad1;

		// Token: 0x0400715A RID: 29018
		public byte pad2;

		// Token: 0x0400715B RID: 29019
		public int diseaseCount;
	}

	// Token: 0x02001BB1 RID: 7089
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RadiationParamsModification
	{
		// Token: 0x0400715C RID: 29020
		public int RadiationParamsType;

		// Token: 0x0400715D RID: 29021
		public float value;
	}

	// Token: 0x02001BB2 RID: 7090
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct CellRadiationModification
	{
		// Token: 0x0400715E RID: 29022
		public int cellIdx;

		// Token: 0x0400715F RID: 29023
		public float radiationDelta;

		// Token: 0x04007160 RID: 29024
		public int callbackIdx;
	}

	// Token: 0x02001BB3 RID: 7091
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct MassConsumptionMessage
	{
		// Token: 0x04007161 RID: 29025
		public int cellIdx;

		// Token: 0x04007162 RID: 29026
		public int callbackIdx;

		// Token: 0x04007163 RID: 29027
		public float mass;

		// Token: 0x04007164 RID: 29028
		public ushort elementIdx;

		// Token: 0x04007165 RID: 29029
		public byte radius;
	}

	// Token: 0x02001BB4 RID: 7092
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct MassEmissionMessage
	{
		// Token: 0x04007166 RID: 29030
		public int cellIdx;

		// Token: 0x04007167 RID: 29031
		public int callbackIdx;

		// Token: 0x04007168 RID: 29032
		public float mass;

		// Token: 0x04007169 RID: 29033
		public float temperature;

		// Token: 0x0400716A RID: 29034
		public int diseaseCount;

		// Token: 0x0400716B RID: 29035
		public ushort elementIdx;

		// Token: 0x0400716C RID: 29036
		public byte diseaseIdx;
	}

	// Token: 0x02001BB5 RID: 7093
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ConsumeDiseaseMessage
	{
		// Token: 0x0400716D RID: 29037
		public int gameCell;

		// Token: 0x0400716E RID: 29038
		public int callbackIdx;

		// Token: 0x0400716F RID: 29039
		public float percentToConsume;

		// Token: 0x04007170 RID: 29040
		public int maxToConsume;
	}

	// Token: 0x02001BB6 RID: 7094
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyCellEnergyMessage
	{
		// Token: 0x04007171 RID: 29041
		public int cellIdx;

		// Token: 0x04007172 RID: 29042
		public float kilojoules;

		// Token: 0x04007173 RID: 29043
		public float maxTemperature;

		// Token: 0x04007174 RID: 29044
		public int id;
	}

	// Token: 0x02001BB7 RID: 7095
	public enum EnergySourceID
	{
		// Token: 0x04007176 RID: 29046
		DebugHeat = 1000,
		// Token: 0x04007177 RID: 29047
		DebugCool,
		// Token: 0x04007178 RID: 29048
		FierySkin,
		// Token: 0x04007179 RID: 29049
		Overheatable,
		// Token: 0x0400717A RID: 29050
		LiquidCooledFan,
		// Token: 0x0400717B RID: 29051
		ConduitTemperatureManager,
		// Token: 0x0400717C RID: 29052
		Excavator,
		// Token: 0x0400717D RID: 29053
		HeatBulb,
		// Token: 0x0400717E RID: 29054
		WarmBlooded,
		// Token: 0x0400717F RID: 29055
		StructureTemperature,
		// Token: 0x04007180 RID: 29056
		Burner,
		// Token: 0x04007181 RID: 29057
		VacuumRadiator
	}

	// Token: 0x02001BB8 RID: 7096
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct VisibleCells
	{
		// Token: 0x04007182 RID: 29058
		public Vector2I min;

		// Token: 0x04007183 RID: 29059
		public Vector2I max;
	}

	// Token: 0x02001BB9 RID: 7097
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct WakeCellMessage
	{
		// Token: 0x04007184 RID: 29060
		public int gameCell;
	}

	// Token: 0x02001BBA RID: 7098
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ElementInteraction
	{
		// Token: 0x04007185 RID: 29061
		public uint interactionType;

		// Token: 0x04007186 RID: 29062
		public ushort elemIdx1;

		// Token: 0x04007187 RID: 29063
		public ushort elemIdx2;

		// Token: 0x04007188 RID: 29064
		public ushort elemResultIdx;

		// Token: 0x04007189 RID: 29065
		public byte pad0;

		// Token: 0x0400718A RID: 29066
		public byte pad1;

		// Token: 0x0400718B RID: 29067
		public float minMass;

		// Token: 0x0400718C RID: 29068
		public float interactionProbability;

		// Token: 0x0400718D RID: 29069
		public float elem1MassDestructionPercent;

		// Token: 0x0400718E RID: 29070
		public float elem2MassRequiredMultiplier;

		// Token: 0x0400718F RID: 29071
		public float elemResultMassCreationMultiplier;
	}

	// Token: 0x02001BBB RID: 7099
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct CreateElementInteractionsMsg
	{
		// Token: 0x04007190 RID: 29072
		public int numInteractions;

		// Token: 0x04007191 RID: 29073
		public unsafe SimMessages.ElementInteraction* interactions;
	}

	// Token: 0x02001BBC RID: 7100
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PipeChange
	{
		// Token: 0x04007192 RID: 29074
		public int cell;

		// Token: 0x04007193 RID: 29075
		public byte layer;

		// Token: 0x04007194 RID: 29076
		public byte pad0;

		// Token: 0x04007195 RID: 29077
		public byte pad1;

		// Token: 0x04007196 RID: 29078
		public byte pad2;

		// Token: 0x04007197 RID: 29079
		public float mass;

		// Token: 0x04007198 RID: 29080
		public float temperature;

		// Token: 0x04007199 RID: 29081
		public int elementHash;
	}

	// Token: 0x02001BBD RID: 7101
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CellWorldZoneModification
	{
		// Token: 0x0400719A RID: 29082
		public int cell;

		// Token: 0x0400719B RID: 29083
		public byte zoneID;
	}
}
