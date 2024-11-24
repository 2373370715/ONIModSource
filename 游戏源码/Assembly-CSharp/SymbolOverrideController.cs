using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/SymbolOverrideController")]
public class SymbolOverrideController : KMonoBehaviour
{
	[Serializable]
	public struct SymbolEntry
	{
		public HashedString targetSymbol;

		[NonSerialized]
		public KAnim.Build.Symbol sourceSymbol;

		public HashedString sourceSymbolId;

		public HashedString sourceSymbolBatchTag;

		public int priority;
	}

	private struct SymbolToOverride
	{
		public KAnim.Build.Symbol sourceSymbol;

		public HashedString targetSymbol;

		public KBatchGroupData data;

		public int atlasIdx;
	}

	private class BatchGroupInfo
	{
		public KAnim.Build build;

		public int atlasIdx;

		public KBatchGroupData data;
	}

	public bool applySymbolOverridesEveryFrame;

	[SerializeField]
	private List<SymbolEntry> symbolOverrides = new List<SymbolEntry>();

	private KAnimBatch.AtlasList atlases;

	private KBatchedAnimController animController;

	private FaceGraph faceGraph;

	private bool requiresSorting;

	public SymbolEntry[] GetSymbolOverrides => symbolOverrides.ToArray();

	public int version { get; private set; }

	protected override void OnPrefabInit()
	{
		animController = GetComponent<KBatchedAnimController>();
		DebugUtil.Assert(GetComponent<KBatchedAnimController>() != null, "SymbolOverrideController requires KBatchedAnimController");
		DebugUtil.Assert(GetComponent<KBatchedAnimController>().usingNewSymbolOverrideSystem, "SymbolOverrideController requires usingNewSymbolOverrideSystem to be set to true. Try adding the component by calling: SymbolOverrideControllerUtil.AddToPrefab");
		for (int i = 0; i < symbolOverrides.Count; i++)
		{
			SymbolEntry value = symbolOverrides[i];
			value.sourceSymbol = KAnimBatchManager.Instance().GetBatchGroupData(value.sourceSymbolBatchTag).GetSymbol(value.sourceSymbolId);
			symbolOverrides[i] = value;
		}
		atlases = new KAnimBatch.AtlasList(0, KAnimBatchManager.MaxAtlasesByMaterialType[(int)animController.materialType]);
		faceGraph = GetComponent<FaceGraph>();
	}

	public int AddSymbolOverride(HashedString target_symbol, KAnim.Build.Symbol source_symbol, int priority = 0)
	{
		if (source_symbol == null)
		{
			throw new Exception("NULL source symbol when overriding: " + target_symbol.ToString());
		}
		SymbolEntry symbolEntry = default(SymbolEntry);
		symbolEntry.targetSymbol = target_symbol;
		symbolEntry.sourceSymbol = source_symbol;
		symbolEntry.sourceSymbolId = new HashedString(source_symbol.hash.HashValue);
		symbolEntry.sourceSymbolBatchTag = source_symbol.build.batchTag;
		symbolEntry.priority = priority;
		SymbolEntry symbolEntry2 = symbolEntry;
		int num = GetSymbolOverrideIdx(target_symbol, priority);
		if (num >= 0)
		{
			symbolOverrides[num] = symbolEntry2;
		}
		else
		{
			num = symbolOverrides.Count;
			symbolOverrides.Add(symbolEntry2);
		}
		MarkDirty();
		return num;
	}

	public bool RemoveSymbolOverride(HashedString target_symbol, int priority = 0)
	{
		for (int i = 0; i < symbolOverrides.Count; i++)
		{
			SymbolEntry symbolEntry = symbolOverrides[i];
			if (symbolEntry.targetSymbol == target_symbol && symbolEntry.priority == priority)
			{
				symbolOverrides.RemoveAt(i);
				MarkDirty();
				return true;
			}
		}
		return false;
	}

	public void RemoveAllSymbolOverrides(int priority = 0)
	{
		symbolOverrides.RemoveAll((SymbolEntry x) => x.priority >= priority);
		MarkDirty();
	}

	public int GetSymbolOverrideIdx(HashedString target_symbol, int priority = 0)
	{
		for (int i = 0; i < symbolOverrides.Count; i++)
		{
			SymbolEntry symbolEntry = symbolOverrides[i];
			if (symbolEntry.targetSymbol == target_symbol && symbolEntry.priority == priority)
			{
				return i;
			}
		}
		return -1;
	}

	public int GetAtlasIdx(Texture2D atlas)
	{
		return atlases.GetAtlasIdx(atlas);
	}

	public void ApplyOverrides()
	{
		if (requiresSorting)
		{
			symbolOverrides.Sort((SymbolEntry x, SymbolEntry y) => x.priority - y.priority);
			requiresSorting = false;
		}
		KAnimBatch batch = animController.GetBatch();
		DebugUtil.Assert(batch != null);
		KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(animController.batchGroupID);
		int count = batch.atlases.Count;
		atlases.Clear(count);
		DictionaryPool<HashedString, Pair<int, int>, SymbolOverrideController>.PooledDictionary pooledDictionary = DictionaryPool<HashedString, Pair<int, int>, SymbolOverrideController>.Allocate();
		ListPool<SymbolEntry, SymbolOverrideController>.PooledList pooledList = ListPool<SymbolEntry, SymbolOverrideController>.Allocate();
		for (int i = 0; i < symbolOverrides.Count; i++)
		{
			SymbolEntry symbolEntry = symbolOverrides[i];
			if (pooledDictionary.TryGetValue(symbolEntry.targetSymbol, out var value))
			{
				int first = value.first;
				if (symbolEntry.priority > first)
				{
					int second = value.second;
					pooledDictionary[symbolEntry.targetSymbol] = new Pair<int, int>(symbolEntry.priority, second);
					pooledList[second] = symbolEntry;
				}
			}
			else
			{
				pooledDictionary[symbolEntry.targetSymbol] = new Pair<int, int>(symbolEntry.priority, pooledList.Count);
				pooledList.Add(symbolEntry);
			}
		}
		DictionaryPool<KAnim.Build, BatchGroupInfo, SymbolOverrideController>.PooledDictionary pooledDictionary2 = DictionaryPool<KAnim.Build, BatchGroupInfo, SymbolOverrideController>.Allocate();
		for (int j = 0; j < pooledList.Count; j++)
		{
			SymbolEntry symbolEntry2 = pooledList[j];
			if (!pooledDictionary2.TryGetValue(symbolEntry2.sourceSymbol.build, out var value2))
			{
				value2 = new BatchGroupInfo
				{
					build = symbolEntry2.sourceSymbol.build,
					data = KAnimBatchManager.Instance().GetBatchGroupData(symbolEntry2.sourceSymbol.build.batchTag)
				};
				Texture2D texture = symbolEntry2.sourceSymbol.build.GetTexture(0);
				int num = batch.atlases.GetAtlasIdx(texture);
				if (num < 0)
				{
					num = atlases.Add(texture);
				}
				value2.atlasIdx = num;
				pooledDictionary2[value2.build] = value2;
			}
			KAnim.Build.Symbol symbol = batchGroupData.GetSymbol(symbolEntry2.targetSymbol);
			if (symbol != null)
			{
				animController.SetSymbolOverrides(symbol.firstFrameIdx, symbol.numFrames, value2.atlasIdx, value2.data, symbolEntry2.sourceSymbol.firstFrameIdx, symbolEntry2.sourceSymbol.numFrames);
			}
		}
		pooledDictionary2.Recycle();
		pooledList.Recycle();
		pooledDictionary.Recycle();
		if (faceGraph != null)
		{
			faceGraph.ApplyShape();
		}
	}

	public void ApplyAtlases()
	{
		KAnimBatch batch = animController.GetBatch();
		atlases.Apply(batch.matProperties);
	}

	public KAnimBatch.AtlasList GetAtlasList()
	{
		return atlases;
	}

	public void MarkDirty()
	{
		if (animController != null)
		{
			animController.SetDirty();
		}
		int num = version + 1;
		version = num;
		requiresSorting = true;
	}
}
