using UnityEngine;
using System.Collections;

public class HighClamp : HeightMap
{
	[SerializeField] public HeightMap terrain;
	[SerializeField] public float clamp_value = 0.0f;

	protected override void init()
	{
		if (terrain) terrain.initialise(m_width, m_height);
	}

	public override void generate ()
	{
		if (m_generated) return;
		if (terrain) terrain.generate();
		else return;

		//	For each point, if value is lower than clamp value, set it to clamp value
		for (int i = 0; i <= m_width; i++)
		{
			for (int j = 0; j <= m_height; j++)
			{
				float val = terrain.get_value(i, j);
				if (val > clamp_value)
					set_value(i, j, clamp_value);
				else
					set_value(i, j, val);
			}
		}
	}
}
