using UnityEngine;
using System.Collections;

public abstract class Conditional : MonoBehaviour
{
	//	If offset value is +inf, value comparisons should return true
	public abstract bool satisfies(int i, int j, float offset, HeightMap map);
}