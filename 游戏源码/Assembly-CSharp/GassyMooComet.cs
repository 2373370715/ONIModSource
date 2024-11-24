using System;
using UnityEngine;

public class GassyMooComet : Comet
{
	public const float MOO_ANGLE = 15f;

	public Vector3 mooSpawnImpactOffset = new Vector3(-0.5f, 0f, 0f);

	private bool? initialFlipState;

	public void SetCustomInitialFlip(bool state)
	{
		initialFlipState = state;
	}

	public override void RandomizeVelocity()
	{
		bool flag = false;
		byte id = Grid.WorldIdx[Grid.PosToCell(base.gameObject.transform.position)];
		WorldContainer world = ClusterManager.Instance.GetWorld(id);
		if (!(world == null))
		{
			int num = world.WorldOffset.x + world.Width / 2;
			if (Grid.PosToXY(base.gameObject.transform.position).x > num)
			{
				flag = true;
			}
			if (initialFlipState.HasValue)
			{
				flag = initialFlipState.Value;
			}
			float f = (flag ? (-75f) : (-105f)) * (float)Math.PI / 180f;
			float num2 = UnityEngine.Random.Range(spawnVelocity.x, spawnVelocity.y);
			velocity = new Vector2((0f - Mathf.Cos(f)) * num2, Mathf.Sin(f) * num2);
			GetComponent<KBatchedAnimController>().FlipX = flag;
		}
	}

	protected override void SpawnCraterPrefabs()
	{
		KBatchedAnimController animController = GetComponent<KBatchedAnimController>();
		animController.Play("landing");
		animController.onAnimComplete += delegate
		{
			if (craterPrefabs != null && craterPrefabs.Length != 0)
			{
				byte world = Grid.WorldIdx[Grid.PosToCell(base.gameObject.transform.position)];
				float x = 0f;
				int cell = Grid.PosToCell(base.transform.GetPosition());
				int num = Grid.OffsetCell(cell, 0, 1);
				int num2 = Grid.OffsetCell(cell, 0, -1);
				cell = ((!Grid.IsValidCellInWorld(num, world)) ? num2 : num);
				if (Grid.Solid[cell])
				{
					bool flipX = animController.FlipX;
					int num3 = Grid.OffsetCell(cell, -1, 0);
					int num4 = Grid.OffsetCell(cell, 2, 0);
					if (!flipX && Grid.IsValidCell(num3) && !Grid.Solid[num3])
					{
						cell = num3;
					}
					else if (flipX && Grid.IsValidCell(num4) && !Grid.Solid[num4])
					{
						cell = num4;
					}
				}
				else
				{
					x = base.gameObject.transform.position.x - Mathf.Floor(base.gameObject.transform.position.x);
				}
				Vector3 position = Grid.CellToPos(cell) + new Vector3(x, 0f, Grid.GetLayerZ(Grid.SceneLayer.Creatures));
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(craterPrefabs[UnityEngine.Random.Range(0, craterPrefabs.Length)]), position);
				Vector3 vector = gameObject.transform.position + mooSpawnImpactOffset;
				if (!Grid.Solid[Grid.PosToCell(vector)])
				{
					gameObject.transform.position = vector;
				}
				gameObject.GetComponent<KBatchedAnimController>().FlipX = animController.FlipX;
				gameObject.SetActive(value: true);
			}
			Util.KDestroyGameObject(base.gameObject);
		};
	}
}
