using UnityEngine;
using System.Collections;

using DateTime = System.DateTime; // For file saving

public class Enclosing : ProceduralTerrain
{
	//	"Buffer" added to bottom of terrain
	[SerializeField] public float m_thickness = 1.0f;
	[SerializeField] public bool m_enabled = false;

	public override void render()
	{
		m_meshBuilder = new MeshBuilder();

		float lowest_point = float.PositiveInfinity;

		//	Add height map
		for (int i = 0; i <= m_segments_across; i++)
		{
			float z = (float)((m_height / m_segments_down) * i) - (float)(m_height) / 2.0f;
			float v = (1.0f / m_segments_across) * i;

			for (int j = 0; j <= m_segments_down; j++)
			{
				//	Check lowest point
				float point = m_map.get_value(i, j);
				if (point < lowest_point)
					lowest_point = point;


				float x = (float)((m_width / m_segments_across) * j) - (float)(m_width) / 2.0f;
				float u = (1.0f / m_segments_down) * j;

				Vector3 offset = new Vector3(x, point, z);
				Vector2 uv = new Vector2(u, v);
				bool buildTriangles = i > 0 && j > 0;

				BuildQuadForGrid(m_meshBuilder, offset, uv, buildTriangles, m_segments_across + 1);
			}
		}

		if (m_enabled)
		{
			//	Add base
			for (int i = 0; i <= m_segments_across; i++)
			{
				float z = (float)((m_height / m_segments_down) * i) - (float)(m_height) / 2.0f;
				float v = (1.0f / m_segments_across) * i;

				for (int j = 0; j <= m_segments_down; j++)
				{


					float x = (float)((m_width / m_segments_across) * j) - (float)(m_width) / 2.0f;
					float u = (1.0f / m_segments_down) * j;

					Vector3 offset = new Vector3(x, lowest_point - m_thickness, z);
					Vector2 uv = new Vector2(u, v);
					bool buildTriangles = i > 0 && j > 0;

					BuildQuadForGrid(m_meshBuilder, offset, uv, buildTriangles, m_segments_across + 1, true);
				}
			}

			//	Offset for base verts
			int base_offset = (m_segments_across + 1) * (m_segments_down + 1);

			//	Add top/bottom edges
			for (int i = 0; i < m_segments_across; i++)
			{
				//	Add triangles for top edge
				//	Determine indices
				int index0 = i;
				int index1 = i + 1;
				int index2 = base_offset + i;
				int index3 = base_offset + i + 1;

				m_meshBuilder.addTriangle(index2, index0, index1);
				m_meshBuilder.addTriangle(index3, index2, index1);

				//	Add triangles for bottom edge
				index0 = base_offset - m_segments_across + i - 1;
				index1 = base_offset - m_segments_across + i;
				index2 = 2 * base_offset - m_segments_across + i - 1;
				index3 = 2 * base_offset - m_segments_across + i;

				m_meshBuilder.addTriangle(index0, index2, index1);
				m_meshBuilder.addTriangle(index2, index3, index1);
			}

			//	Add left/right edges
			for (int i = 0; i < m_segments_down; i++)
			{
				//	Add triangles for top edge
				//	Determine indices
				int index0 = i * (m_segments_across + 1);
				int index1 = (i + 1) * (m_segments_across + 1);
				int index2 = base_offset + i * (m_segments_across + 1);
				int index3 = base_offset + (i + 1) * (m_segments_across + 1);

				m_meshBuilder.addTriangle(index0, index2, index1);
				m_meshBuilder.addTriangle(index2, index3, index1);

				//	Add triangles for bottom edge
				index0 = (i + 1) * (m_segments_across + 1) - 1;
				index1 = (i + 2) * (m_segments_across + 1) - 1;
				index2 = base_offset + (i + 1) * (m_segments_across + 1) - 1;
				index3 = base_offset + (i + 2) * (m_segments_across + 1) - 1;

				m_meshBuilder.addTriangle(index2, index0, index1);
				m_meshBuilder.addTriangle(index3, index2, index1);
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

	public void saveToFile()
	{
		MeshFilter filter = GetComponent<MeshFilter>();
		if (filter)
		{
			string filename = "terrain " + DateTime.UtcNow.ToString() + ".obj";
			//	Replace '/', ':' and ' ' with '_'
			filename = filename.Replace(' ', '_');
			filename = filename.Replace('/', '_');
			filename = filename.Replace(':', '_');

			Debug.Log(filename);

			ObjExporter.MeshToFile(filter, filename);
		}
		else
			Debug.Log("No MeshFilter attached.");
	}
}
