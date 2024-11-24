using System;
using UnityEngine;

public class MotdData_Box
{
	public string category;

	public string guid;

	public long startTime;

	public long finishTime;

	public string title;

	public string text;

	public string image;

	public string href;

	public Texture2D resolvedImage;

	public bool resolvedImageIsFromDisk;

	public bool ShouldDisplay()
	{
		long num = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		if (num < startTime || finishTime < num)
		{
			return false;
		}
		return true;
	}
}
