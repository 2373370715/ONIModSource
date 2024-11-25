using System;
using System.Collections.Generic;
using ProcGenGame;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/CavityVisualizer")]
public class CavityVisualizer : KMonoBehaviour
{
		protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		foreach (TerrainCell key in MobSpawning.NaturalCavities.Keys)
		{
			foreach (HashSet<int> hashSet in MobSpawning.NaturalCavities[key])
			{
				foreach (int item in hashSet)
				{
					this.cavityCells.Add(item);
				}
			}
		}
	}

		private void OnDrawGizmosSelected()
	{
		if (this.drawCavity)
		{
			Color[] array = new Color[]
			{
				Color.blue,
				Color.yellow
			};
			int num = 0;
			foreach (TerrainCell key in MobSpawning.NaturalCavities.Keys)
			{
				Gizmos.color = array[num % array.Length];
				Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.125f);
				num++;
				foreach (HashSet<int> hashSet in MobSpawning.NaturalCavities[key])
				{
					foreach (int cell in hashSet)
					{
						Gizmos.DrawCube(Grid.CellToPos(cell) + (Vector3.right / 2f + Vector3.up / 2f), Vector3.one);
					}
				}
			}
		}
		if (this.spawnCells != null && this.drawSpawnCells)
		{
			Gizmos.color = new Color(0f, 1f, 0f, 0.15f);
			foreach (int cell2 in this.spawnCells)
			{
				Gizmos.DrawCube(Grid.CellToPos(cell2) + (Vector3.right / 2f + Vector3.up / 2f), Vector3.one);
			}
		}
	}

		public List<int> cavityCells = new List<int>();

		public List<int> spawnCells = new List<int>();

		public bool drawCavity = true;

		public bool drawSpawnCells = true;
}
