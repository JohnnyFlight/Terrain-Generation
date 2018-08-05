using UnityEngine;
using System.Collections;

public class Maximum : HeightMap
{
	[SerializeField] public HeightMap t1;
	[SerializeField] public HeightMap t2;

	protected override void init()
	{
		if (t1) t1.initialise(m_width, m_height);
		if (t2) t2.initialise(m_width, m_height);
	}

	public override void generate ()
	{
		if (m_generated) return;
		if (!t1 && !t2) return;

		if (t1) t1.generate();
		if (t2) t2.generate();

		//	For each point, if value is lower than clamp value, set it to clamp value
		for (int i = 0; i <= m_width; i++)
		{
			for (int j = 0; j <= m_height; j++)
			{
				if (!conditions_met(i, j, get_value(i, j), this)) continue;

				float val1 = 0.0f;
				if (t1)
					val1 = t1.get_value(i, j);

				float val2 = 0.0f;
				if (t2)
					val2 = t2.get_value(i, j);

				if (val1 < val2)
					set_value(i, j, val2);
				else
					set_value(i, j, val1);
			}
		}
	}
}
