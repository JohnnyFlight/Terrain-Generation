/*
 * 	Note: This is a very naive implementation of the algorithm containing (width+1)*(height+1) square root operations for each generation
 * 		  The purpose of this implementation is to test the viability of the algorithm before committing to a more complex implementation
 */

/*
 *	Example Terrain idea: Archipelago
		1:	Worley Noise
		2:	Scale -1.0f
		3:	Clamp -0.35f (Gets rid of unsightly peaks)
		4:	Smooth (Maybe several passes)
		5:	Generate Fractal Noise
		6:	Add Fractal noise (if lower than 0.5f if I implement conditionals)
		7:	Smooth again
	
	Another example: Mountain Range
		1: Worley Noise (200 points)
		2: Scale -1.0f
		3: Normalise
		4: Power Scale 4.0f
		5: Scale 3.0f

		NB: This leads to a range where all the mountains are the same height
			Maybe add a small amount of noise before normalising?
 */

using UnityEngine;
using System.Collections;

public class WorleyNoise : SeededHeightMap
{
	[SerializeField] public int m_number_feature_points = 10;

	private Vector2[] m_feature_points;

	protected override void init()
	{
		init_seed();

		m_feature_points = new Vector2[m_number_feature_points];
	}

	public override void generate()
	{
		if (m_generated) return;

		set_seed();

		//	Generate feature points within plane
		for (int i = 0; i < m_number_feature_points; i++)
		{
			m_feature_points[i] = new Vector2(Random.Range(0.0f, (float)m_width), Random.Range(0.0f, (float)m_height));
		}

		//	For each point in the heightmap
		for (int i = 0; i <= m_width; i++)
		{
			for (int j = 0; j <= m_height; j++)
			{

				//	Check if condition is met
				if (!conditions_met(i, j, float.PositiveInfinity, this)) continue;

				//	Calculate noise value
				//	For each feature point
				Vector2 point = new Vector2((float)i, (float)j);
				float shortest_distance_squared = float.PositiveInfinity;
				foreach (Vector2 feature in m_feature_points)
				{
					//	Calculate distance to point
					float distance_squared = (point.x - feature.x) * (point.x - feature.x) + (point.y - feature.y) * (point.y - feature.y);
					if (distance_squared < shortest_distance_squared)
					{
						shortest_distance_squared = distance_squared;
					}

					//	Set value to square rooted shortest distance
					set_value(i, j, Mathf.Sqrt(shortest_distance_squared));
				}
			}
		}
	}
}
