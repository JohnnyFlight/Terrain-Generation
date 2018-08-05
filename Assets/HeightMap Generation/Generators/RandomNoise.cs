using UnityEngine;
using System.Collections;

public class RandomNoise : SeededHeightMap
{
	[SerializeField] public float m_min_value = -1.0f;
	[SerializeField] public float m_max_value = 1.0f;

	protected override void init()
	{
		init_seed();
	}

	public override void generate()
	{
		if (m_generated) return;

		set_seed();

		//	For each point in the map
		for (int i = 0; i <= m_width; i++)
		{
			for (int j = 0; j <= m_height; j++)
			{
				//	Check if conditions met
				if (!conditions_met(i, j, float.PositiveInfinity, this)) return;

				//	Set to a random value within range
				set_value(i, j, Random.Range(m_min_value, m_max_value));
			}
		}
	}
}
