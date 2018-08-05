using UnityEngine;
using System.Collections;

using DateTime = System.DateTime;

public class SeededHeightMap : HeightMap
{
	[SerializeField] public int m_seed = -1;
	[SerializeField] public int m_generated_seed;

	protected void init_seed()
	{
		if (m_seed == -1)
		{
			m_generated_seed = (int)DateTime.Now.Ticks;
		}
		else
		{
			m_generated_seed = m_seed;
		}
	}

	protected void set_seed()
	{
		Random.seed = m_generated_seed;
	}

	protected override void init()
	{

	}

	public override void generate()
	{

	}
}
