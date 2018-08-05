using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public abstract class HeightMap : MonoBehaviour
{
	public int m_width { get; set; }
	public int m_height { get; set; }

	public float[] m_map { get; set; }

	[SerializeField] protected Conditional[] m_conditionals;

	protected bool m_generated = false;

	// Use this for initialization
	public void initialise(int w, int h)
	{
		m_generated = false;

		m_width = w;
		m_height = h;

		m_map = new float[(m_width+1)*(m_height+1)];

		//Debug.Log("Created height map");

		init();
	}

	public void reset()
	{
		m_generated = false;
	}

	public float get_value(int x, int y)
	{
		return m_map[x*(m_width+1)+y];
	}

	public void set_value(int x, int y, float value)
	{
		m_map[x*(m_width+1)+y] = value;
	}

	protected bool conditions_met(int i, int j, float offset, HeightMap map)
	{
		foreach (Conditional c in m_conditionals)
		{
			//	Ensure c actually exists
			if (c)
			if (!c.satisfies(i, j, offset, map))
				return false;
		}

		return true;
	}

	public Texture2D convertToTexture()
	{
		//	Create Texture
		Texture2D tex = new Texture2D(m_width + 1, m_height + 1, TextureFormat.RGBA32, false);

		float max_height = float.NegativeInfinity;
		float min_height = float.PositiveInfinity;

		//	Find max and min heights in map
		for (int i = 0; i <= m_width; i++)
		{
			for (int j = 0; j <= m_height; j++)
			{
				float val = get_value(i, j);
				if (val > max_height) max_height = val;
				if (val < min_height) min_height = val;
			}
		}

		//	For each offset, subtract height and divide by max - min height
		for (int i = 0; i <= m_width; i++)
		{
			for (int j = 0; j <= m_height; j++)
			{
				float val = get_value(i, j);
				val = (val - min_height) / (max_height - min_height);
				tex.SetPixel(i, j, new Color(val, val, val));
			}
		}

		//	Apply pixel changes
		tex.Apply();

		//	Return finished texture
		return tex;
	}

	public Texture2D createNormalMap()
	{
		//	Create texture 1 smaller than HM in each direction
		Texture2D tex = new Texture2D(m_width, m_height, TextureFormat.RGB24, false);

		float lowest = float.PositiveInfinity;

		//	Get lowest point
		for (int i = 0; i < m_width; i++)
		{
			for (int j = 0; j < m_height; j++)
			{
				float val = get_value(i, j);
				if (val < lowest) lowest = val;
			}
		}

		//	Loop through elements
		for (int i = 0; i < m_width; i++)
		{
			for (int j = 0; j < m_height; j++)
			{
				//	Calculate normal of this cell
				//	Turn each offset into a 3d point
				Vector3 p00 = new Vector3(0.0f, 0.0f, get_value(i, j) - lowest);
				Vector3 p10 = new Vector3(1.0f, 0.0f, get_value(i + 1, j) - lowest);
				Vector3 p01 = new Vector3(0.0f, 1.0f, get_value(i, j + 1) - lowest);
				Vector3 p11 = new Vector3(1.0f, 1.0f, get_value(i + 1, j + 1) - lowest);

				//	Calculate normals of each set of each angle created by points
				Vector3 n00 = Vector3.Cross(p01 - p00, p10 - p00);
				Vector3 n10 = Vector3.Cross(p00 - p10, p11 - p10);
				Vector3 n01 = Vector3.Cross(p11 - p01, p00 - p01);
				Vector3 n11 = Vector3.Cross(p10 - p11, p01 - p11);

				//	Average the normals
				Vector3 normal = (n00 + n10 + n01 + n11) / 4.0f;

				//	Normalise the resultant vector
				normal.Normalize();

				//	Prepare resultant normal for being mapped
				normal /= 2.0f;
				normal += Vector3.one / 2.0f;

				//	Write normal to texture
				tex.SetPixel(i, j, new Color(1.0f - normal.x, 1.0f - normal.y, 1.0f - normal.z));
			}
		}

		//	Apply texture additions
		tex.Apply();

		return tex;
	}

	protected abstract void init();

	public abstract void generate();
}
