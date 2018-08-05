using UnityEngine;
using System.Collections;

public class Parabolas : SeededHeightMap
{
	[SerializeField] public int m_iterations = 100;
	[SerializeField] public float m_radius = 5.0f;
	[SerializeField] public float m_radius_decay = 0.99f;

	protected override void init()
	{
		init_seed();
	}

	public override void generate()
	{
		if (m_generated) return;

		set_seed();

		//	Set initial radius
		float radius = m_radius;

		//	For each iteration
		for (int k = 0; k < m_iterations; k++)
		{
			//	Scale everything down into the range 0 <= n <= 1

			//	Select a random point on the plane
			Vector2 parabola_centre = new Vector2(Random.Range(0.0f, (float)m_width), Random.Range(0.0f, (float)m_height));

			//	Define enclosing box
			Vector2 b0 = new Vector2(parabola_centre.x - radius, parabola_centre.y - radius);
			Vector2 b1 = new Vector2(parabola_centre.x + radius, parabola_centre.y + radius);

			//	For each point in the heightmap
			for (int i = 0; i <= m_width; i++)
			{
				for (int j = 0; j <= m_height; j++)
				{
					//	Check if conditions met
					if (!conditions_met(i, j, float.PositiveInfinity, this)) continue;

					//	Convert i, j to a point
					Vector2 point = new Vector2((float)i, (float)j);

					//	See if the point is in the enclosing box
					//	Break out if point is beyond bounds of box
					if (point.x < b0.x) continue;
					else if (point.x > b1.x) break;
					else if (point.y < b0.y) continue;
					else if (point.y > b1.y) break;
					else
					{
						//	See if the point is in the parabola
						//	z = r^2  - ( (x2 - x1)^2 + (y2 - y1)^2 )
						float offset = radius* radius - ((point.x - parabola_centre.x) * (point.x - parabola_centre.x) + (point.y - parabola_centre.y) * (point.y - parabola_centre.y));
						if (offset > 0.0f)
						{
							set_value(i, j, get_value(i, j) + offset);
						}
					}
				}
			}

			//	Decay radius
			radius *= m_radius_decay;
		}
	}
}
