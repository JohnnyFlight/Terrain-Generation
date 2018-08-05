using UnityEngine;
using System.Collections;

using DateTime = System.DateTime;

public class FractalNoise : HeightMap
{
	[SerializeField] private int m_seed = -1;
	[SerializeField] private int m_generated_seed;

	[SerializeField] public int m_iterations = 5;
	[SerializeField] public Vector2 m_noise_dimensions = new Vector2(10.0f, 10.0f);

	[SerializeField] public Vector2 m_noise_scale = new Vector2(2.0f, 2.0f);

	[SerializeField] public float m_offset_height = 1.0f;
	[SerializeField] public float m_offset_decay = 0.5f;

	protected override void init()
	{
		if (m_seed == -1)
			m_generated_seed = (int)DateTime.Now.Ticks;
		else
			m_generated_seed = m_seed;
	}

	public override void generate()
	{
		if (m_generated) return;

		Vector2 dimensions = m_noise_dimensions;
		float height = m_offset_height;

		//	For each iteration, add noise, then scale noise
		for (int k = 0; k < m_iterations; k++)
		{
			//	Adding noise
			for (int i = 0; i <= m_width; i++)
			{
				for (int j = 0; j <= m_height; j++)
				{
					//	Check if conditionals are satisfied
					if (conditions_met(i, j, float.PositiveInfinity, this))
					{
						float val = get_value(i, j);
						val += Mathf.PerlinNoise((dimensions.x / m_width) * i, (dimensions.y / m_height) * j) * height;
						set_value(i, j, val);
					}
				}
			}

			//	Scaling noise and decaying offset
			dimensions = new Vector2(dimensions.x * m_noise_scale.x, dimensions.y * m_noise_scale.y);
			height *= m_offset_decay;
		}
	}
}
