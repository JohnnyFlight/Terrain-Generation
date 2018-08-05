using UnityEngine;
using System.Collections;

public class GreaterThanOrEqual : Conditional
{
	[SerializeField] protected float value = 0.0f;

	public override bool satisfies(int i, int j, float offset, HeightMap map)
	{
		if (float.IsPositiveInfinity(offset)) return true;
		return offset >= value;
	}
}
