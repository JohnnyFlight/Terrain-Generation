using UnityEngine;
using System.Collections;

public class Fracturing : SeededHeightMap
{
	[SerializeField] private float m_offset_height = 1.0f;
	
	[SerializeField] private float m_decayRate = 0.999f;
	[SerializeField] private int m_iterations = 500;
	
	protected override void init()
	{
		init_seed();
	}

	public override void generate()
	{
		if (m_generated) return;

		set_seed();

		float offsetValue = m_offset_height;

		//	Define bounds of plane
		Vector2 topLeft = new Vector2(0.0f, 0.0f);
		Vector2 topRight = new Vector2((float)m_width, 0.0f);
		Vector2 bottomLeft = new Vector2(0.0f, (float)m_height);
		Vector2 bottomRight = new Vector2((float)m_width, (float)m_height);
		
		for (int r = 0; r < m_iterations; r++)
		{
			//	Pick a random point and a random vector
			Vector2 fracture_point = new Vector2(Random.Range(0.0f, (float)m_width), Random.Range(0.0f, (float)m_height));
			Vector2 fracture_line = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
			fracture_line.Normalize();
			
			//	Pick edge for point one
			int edge1 = Random.Range(0, 4);
			int edge2;

			//	Pick edge for point 2
			do
			{
				edge2 = Random.Range(0, 4);
			}
			while (edge2 == edge1);

			//	Select points
			Vector2 point1;
			Vector2 point2;

			switch (edge1)
			{
			case 0:
				//	Top edge
				point1 = Vector2.Lerp(topLeft, topRight, Random.Range(0.0f, 1.0f));
				break;
			case 1:
				//	Right edge
				point1 = Vector2.Lerp(topRight, bottomRight, Random.Range(0.0f, 1.0f));
				break;
			case 2:
				//	Bottom Edge
				point1 = Vector2.Lerp(bottomLeft, bottomRight, Random.Range(0.0f, 1.0f));
				break;
			default:
				//	Left edge
				point1 = Vector2.Lerp(topLeft, bottomLeft, Random.Range(0.0f, 1.0f));
				break;
			}

			switch (edge2)
			{
			case 0:
				//	Top edge
				point2 = Vector2.Lerp(topLeft, topRight, Random.Range(0.0f, 1.0f));
				break;
			case 1:
				//	Right edge
				point2 = Vector2.Lerp(topRight, bottomRight, Random.Range(0.0f, 1.0f));
				break;
			case 2:
				//	Bottom Edge
				point2 = Vector2.Lerp(bottomLeft, bottomRight, Random.Range(0.0f, 1.0f));
				break;
			default:
				//	Left edge
				point2 = Vector2.Lerp(topLeft, bottomLeft, Random.Range(0.0f, 1.0f));
				break;
			}

			fracture_point = point1;
			fracture_line = (point2 - point1).normalized;

			//	Define box for optimisation
			Vector2 boxTopLeft = new Vector2(Mathf.Min(point1.x, point2.x), Mathf.Min(point1.y, point2.y));
			Vector2 boxBottomRight = new Vector2(Mathf.Max(point1.x, point2.x), Mathf.Max(point1.y, point2.y));

			for (int i = 0; i <= m_width; i++)
			{
				for (int j = 0; j <= m_height; j++)
				{
					//	Check if conditions are met
					if (conditions_met(i, j, float.PositiveInfinity, this))
					{
						//	Taken from Wolfram Alpha:
						//	d = (a-p)-((a-p).n)n)
						Vector2 mesh_point = new Vector2((float)j, (float)i);

						//	Check if point is outside box
						//	Left or above -> Increase
						/*if (mesh_point.x < boxTopLeft.x)
						{
							m_map[i * (m_width + 1) + j] += offsetValue / 2.0f;
						}
						else if (mesh_point.y <  boxTopLeft.y)
						{
							m_map[i * (m_width + 1) + j] += offsetValue / 2.0f;
						}

						//	Right or below -> Decrease
						else if (mesh_point.x > boxBottomRight.x)
						{
							m_map[i * (m_width + 1) + j] -= offsetValue / 2.0f;
						}
						else if (mesh_point.y > boxBottomRight.y)
						{
							m_map[i * (m_width + 1) + j] -= offsetValue / 2.0f;
						}

						//	Else do the more expensive calculation
						else*/
						{
							Vector2 a = fracture_point;
							Vector2 b = fracture_point + fracture_line;
							Vector2 c = mesh_point;
							
							float distance = ((b.x - a.x)*(c.y - a.y) - (b.y - a.y)*(c.x - a.x));
							
							if (distance <= 0)
							{
								m_map[i * (m_width + 1) + j] += offsetValue / 2.0f;
							}
							else
							{
								m_map[i * (m_width + 1) + j] -= offsetValue / 2.0f;
							}
						}
					}
				}
			}
			offsetValue *= m_decayRate;
			
		}
	}
}