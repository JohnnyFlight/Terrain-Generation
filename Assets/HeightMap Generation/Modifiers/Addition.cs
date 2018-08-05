using UnityEngine;
using System.Collections;

/*
 * 		Note on Conditionals:
 * 				t2 value will only be added to t1 if t1 offset meets condition
 * 				If either heightmap does not exist, then conditions are not checked
 */

public class Addition : HeightMap
{
	[SerializeField] public HeightMap t1;
	[SerializeField] public HeightMap t2;

	protected override void init()
	{
		//	Call initialise on attached heightmaps
		if (t1) t1.initialise(m_width, m_height);
		if (t2) t2.initialise(m_width, m_height);

		Debug.Log(m_width + " " + m_height);
	}

	public override void generate()
	{
		if (m_generated) return;

		//	Need to generate attached terrains first
		if (t1) t1.generate();
		if (t2) t2.generate();

		for (int i = 0; i <= m_width; i++)
		{
			for (int j = 0; j <= m_height; j++)
			{
				float value;
				//	If neither terrain exists just create a flat plane
				if (!t1 && !t2)
				{
					value = 0.0f;
				}
				//	If both exist add them together
				else if (t1 && t2)
				{
					//	Check if t1 meets conditions
					value = t1.get_value(i, j);
					if (conditions_met(i, j, value, t1))
					{
						value += t2.get_value(i, j);
					}
				}
				//	If only one exists set map to that one
				else
				{
					if (t1) value = t1.get_value(i ,j);
					else value = t2.get_value(i, j);
				}

				set_value(i, j, value);
			}
		}

		m_generated = true;
	}
}
