using UnityEngine;
using System.Collections;

public class Scaling : HeightMap
{
	[SerializeField] public HeightMap terrain;
	[SerializeField] public float scale_factor = 1.0f;

	protected override void init()
	{
		//	Call initialise on attached heightmaps
		if (terrain) terrain.initialise(m_width, m_height);

		Debug.Log(m_width + " " + m_height);
	}

	public override void generate()
	{
		if (m_generated) return;

		//	Need to generate attached terrains first
		if (terrain) terrain.generate();

		for (int i = 0; i <= m_width; i++)
		{
			for (int j = 0; j <= m_height; j++)
			{
				if (!conditions_met(i, j, get_value(i, j), this)) continue;
				set_value(i, j, terrain.get_value(i, j) * scale_factor);
			}
		}

		m_generated = true;
	}
}
