using UnityEngine;
using System.Collections;

public class Normalise : HeightMap
{
	[SerializeField] public HeightMap terrain;

	protected override void init()
	{
		if (terrain) terrain.initialise(m_width, m_height);
	}

	public override void generate()
	{
		if (m_generated) return;

		if (terrain) terrain.generate();
		else return;

		float lowest = float.PositiveInfinity;
		float highest = float.NegativeInfinity;

		//	Get highest and lowest values
		for (int i = 0; i <= m_width; i++)
		{
			for (int j = 0; j <= m_height; j++)
			{
				float val = terrain.get_value(i, j);
				if (val < lowest)
					lowest = val;
				else if (val > highest)
					highest = val;
			}
		}

		//	Subtract every point by lowest and divide by highest-lowest
		for (int i = 0; i <= m_width; i++)
		{
			for (int j = 0; j <= m_height; j++)
			{
				if (!conditions_met(i, j, get_value(i, j), this)) continue;
				set_value(i, j, ((terrain.get_value(i, j) - lowest) / (highest - lowest)));
			}
		}
	}
}
