/*
 *	Intended to tell a terrain to generate, then render the heightmap
 */

using UnityEngine;
using System.Collections;
using System.IO;

using DateTime = System.DateTime; // For file saving

public class ProceduralTerrain : MonoBehaviour
{
	[SerializeField] protected float m_width = 1.0f;
	[SerializeField] protected float m_height = 1.0f;
	
	[SerializeField] protected int m_segments_across = 100;
	[SerializeField] protected int m_segments_down = 100;

	[SerializeField] protected HeightMap m_map;
	
	protected MeshBuilder m_meshBuilder;

	private float m_timer = 0.0f;
	private int m_counter = 0;
	private float m_multiplier = 0.95f;

	// Use this for initialization
	void Start ()
	{
		reset();
	}

	void Update()
	{
		return;

		//	Trying to edit points within a mesh
		m_timer += Time.deltaTime;

		if (m_timer >= 0.5f)
		{
			m_timer -= 0.5f;
			//	Should check if mesh filer exists first
			MeshFilter filter = GetComponent<MeshFilter>();

			if (filter)
			{
				//	Flip the y-position of each vertex
				for (int i = 0; i < m_meshBuilder.vertices.Count; i++)
				{
					Vector3 v = m_meshBuilder.vertices[i];
					v.y *= m_multiplier;
					m_meshBuilder.vertices[i] = v;
				}

				//	Remake mesh and assign
				Mesh mesh = m_meshBuilder.CreateMesh();
				mesh.RecalculateNormals();
				filter.sharedMesh = mesh;
			}

			m_counter += 1;
			if (m_counter > 100)
			{
				m_counter = 0;
				m_multiplier = 1.0f / m_multiplier;
			}
		}
	}

	public virtual void render()
	{
		m_meshBuilder = new MeshBuilder();

		for (int i = 0; i <= m_segments_across; i++)
		{
			float z = (float)((m_height / m_segments_down) * i) - (float)(m_height) / 2.0f;
			float v = (1.0f / m_segments_across) * i;

			for (int j = 0; j <= m_segments_down; j++)
			{
				float x = (float)((m_width / m_segments_across) * j) - (float)(m_width) / 2.0f;
				float u = (1.0f / m_segments_down) * j;

				Vector3 offset = new Vector3(x, m_map.m_map[i * (m_segments_across + 1) + j], z);
				Vector2 uv = new Vector2(u, v);
				bool buildTriangles = i > 0 && j > 0;

				BuildQuadForGrid(m_meshBuilder, offset, uv, buildTriangles, m_segments_across + 1);
			}
		}

		//	Create the mesh
		MeshFilter filter = GetComponent<MeshFilter>();
		if (filter)
		{
			Mesh mesh = m_meshBuilder.CreateMesh();
			mesh.RecalculateNormals();
			filter.sharedMesh = mesh;
		}
	}

	public void reset()
	{
		m_map.initialise(m_segments_across, m_segments_down);
		m_map.generate();
		
		render();
	}

	public void refresh()
	{
		render();
	}

	public void next()
	{
		//	Get all heightmap components
		HeightMap[] maps = GetComponents<HeightMap>();

		int counter = 0;
		foreach (HeightMap map in maps)
		{
			counter++;
			//	If you're at the current HM
			if (m_map.GetInstanceID() == map.GetInstanceID())
			{
				//	Check if it's the last one in the list
				if (counter == maps.Length)
				{
					counter = 0;
				}

				//	Set current HM to the one at counter
				m_map = maps[counter];
				break;
			}
		}

		//	Refresh to show change
		refresh();
	}

	public void previous()
	{
		//	Get all heightmap components
		HeightMap[] maps = GetComponents<HeightMap>();

		int counter = 0;
		foreach (HeightMap map in maps)
		{
			//	If you're at the current HM
			if (m_map.GetInstanceID() == map.GetInstanceID())
			{
				//	Check if it's the first one in the list
				if (counter == 0)
				{
					counter = maps.Length - 1;
				}

				//	Set current HM to the one at counter
				m_map = maps[counter-1];
				break;
			}

			counter++;
		}

		//	Refresh to show change
		refresh();
	}

	public void save_map_PNG()
	{
		//	Check if HeightMap is attached
		if (!m_map) return;

		//	Create texture from HeightMap
		Texture2D tex = m_map.convertToTexture();

		//	Convert to PNG
		byte[] bytes = tex.EncodeToPNG();

		string filename = "heightmap " + DateTime.UtcNow.ToString() + ".png";
		//	Replace '/', ':' and ' ' with '_'
		filename = filename.Replace(' ', '_');
		filename = filename.Replace('/', '_');
		filename = filename.Replace(':', '_');

		//	Write to file
		File.WriteAllBytes(Application.dataPath + "/" + filename, bytes);
	}

	public void save_normal_map_PNG()
	{
		if (!m_map) return;

		Texture2D tex = m_map.createNormalMap();

		byte[] bytes = tex.EncodeToPNG();

		string filename = "normalmap " + DateTime.UtcNow.ToString() + ".png";
		//	Replace '/', ':' and ' ' with '_'
		filename = filename.Replace(' ', '_');
		filename = filename.Replace('/', '_');
		filename = filename.Replace(':', '_');

		//	Write to file
		File.WriteAllBytes(Application.dataPath + "/" + filename, bytes);
	}

	protected void BuildQuad(MeshBuilder meshBuilder, Vector3 offset)
	{
		meshBuilder.vertices.Add(new Vector3(0.0f, 0.0f, 0.0f) + offset);
		meshBuilder.uvs.Add(new Vector2(0.0f, 0.0f));
		meshBuilder.normals.Add(Vector3.up);
		
		meshBuilder.vertices.Add(new Vector3(0.0f, 0.0f, m_height) + offset);
		meshBuilder.uvs.Add(new Vector2(0.0f, 1.0f));
		meshBuilder.normals.Add(Vector3.up);
		
		meshBuilder.vertices.Add(new Vector3(m_width, 0.0f, m_height) + offset);
		meshBuilder.uvs.Add(new Vector2(1.0f, 1.0f));
		meshBuilder.normals.Add(Vector3.up);
		
		meshBuilder.vertices.Add(new Vector3(m_width, 0.0f, 0.0f) + offset);
		meshBuilder.uvs.Add(new Vector2(1.0f, 0.0f));
		meshBuilder.normals.Add(Vector3.up);

		//	Working on the last 4 verts
		int baseIndex = meshBuilder.vertices.Count - 4;

		meshBuilder.addTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
		meshBuilder.addTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
	}

	protected void BuildQuadForGrid(MeshBuilder meshBuilder, Vector3 position, Vector2 uv,
	                      bool buildTriangles, int vertsPerRow, bool reverse = false)
	{
		meshBuilder.vertices.Add(position);
		meshBuilder.uvs.Add(uv);

		if (buildTriangles)
		{
			int baseIndex = meshBuilder.vertices.Count - 1;

			int index0 = baseIndex;
			int index1 = baseIndex - 1;
			int index2 = baseIndex - vertsPerRow;
			int index3 = baseIndex - vertsPerRow - 1;

			if (reverse)
			{
				meshBuilder.addTriangle(index2, index0, index1);
				meshBuilder.addTriangle(index3, index2, index1);
			}
			else
			{
				meshBuilder.addTriangle(index0, index2, index1);
				meshBuilder.addTriangle(index2, index3, index1);
			}
		}
	}

}