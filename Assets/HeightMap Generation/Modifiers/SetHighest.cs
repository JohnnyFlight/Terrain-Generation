using UnityEngine;
using System.Collections;

public class SetHighest : HeightMap
{
	[SerializeField] public HeightMap m_terrain;
	[SerializeField] public float target_height = 0.0f;

	protected override void init()
	{
		if (m_terrain) m_terrain.initialise(m_width, m_height);
	}

	public override void generate()
	{
		if (!m_terrain) return;
		else m_terrain.generate();

		float highest_point = float.NegativeInfinity;

		//	Find highest point
		for (int i = 0; i <= m_width; i++)
		{
			for (int j = 0; j <= m_height; j++)
			{
				float val = m_terrain.get_value(i, j);
				if (val > highest_point)
				{
					highest_point = val;
				}
			}
		}

		//	Find difference from target height
		float diff = target_height - highest_point;

		//	Adjust every point by that amount
		for (int i = 0; i <= m_width; i++)
		{
			for (int j = 0; j <= m_height; j++)
			{
				if (!conditions_met(i, j, get_value(i, j), this)) continue;
				set_value(i, j, m_terrain.get_value(i, j) + diff);
			}
		}
	}
}
