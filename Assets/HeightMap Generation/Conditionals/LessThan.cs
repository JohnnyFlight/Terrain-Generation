using UnityEngine;
using System.Collections;

public class LessThan : Conditional
{
	[SerializeField] protected float value = 0.0f;

	public override bool satisfies(int i, int j, float offset, HeightMap map)
	{
		if (float.IsPositiveInfinity(offset)) return true;
		return offset < value;
	}
}
