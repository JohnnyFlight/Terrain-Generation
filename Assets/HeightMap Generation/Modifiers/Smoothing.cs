using UnityEngine;
using System.Collections;

public class Smoothing : HeightMap
{
	[SerializeField] public HeightMap m_terrain;

	[SerializeField] public int m_passes = 1;
	[SerializeField] public bool m_include_self = false;

	protected override void init()
	{
		if (m_terrain) m_terrain.initialise(m_width, m_height);
	}

	public override void generate()
	{
		if (m_generated) return;

		if (!m_terrain) return;
		else m_terrain.generate();

		//	Create a temporary array to store values
		float[] temp = m_terrain.m_map;

		//	Handling non-positive pass numbers
		if (m_passes < 1)
		{
			m_map = temp;
			return;
		}

		//	For each pass
		for (int k = 0; k < m_passes; k++)
		{
			//	For each point in the array
			for (int i = 0; i <= m_width; i++)
			{
				for (int j = 0; j <= m_height; j++)
				{
					//	Check if conditions are met
					if (!conditions_met(i, j, get_value(i, j), this)) continue;

					//	Sum neighbouring values
					int neighbours = 0;
					float sum = 0.0f;
					for (int l = 0; l < 9; l++)
					{
						//	Starting from top-left (-1, -1)
						//	Go right, then down

						//	4 is the current point
						if (l == 4 && !m_include_self) continue;

						int adj_x = i + (-1 + (l % 3));
						int adj_y = j + (-1 + (l / 3));

						if (adj_x < 0 || adj_x > m_width) continue;
						if (adj_y < 0 || adj_y > m_height) continue;


						sum += temp[adj_x * (m_width + 1) + adj_y];
						neighbours++;
					}

					set_value(i, j, sum / (float)neighbours);
				}
			}

			//	If it's not the last pass
			if (k < m_passes - 1)
			{
				//	Copy self into temp
				temp = m_map;
			}
		}
	}
}
