/*
 * Code from: http://www.gamasutra.com/blogs/JayelindaSuridge/20130903/199457/Modelling_by_numbers_Part_One_A.php
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshBuilder
{
	private List<Vector3> m_vertices = new List<Vector3>();
	public List<Vector3> vertices
	{
		get { return m_vertices; }
	}

	private List<Vector3> m_normals = new List<Vector3>();
	public List<Vector3> normals
	{
		get { return m_normals; }
	}

	private List<Vector2> m_uvs = new List<Vector2>();
	public List<Vector2> uvs
	{
		get { return m_uvs; }
	}

	private List<int> m_indices = new List<int>();

	public void addTriangle(int index0, int index1, int index2)
	{
		m_indices.Add(index0);
		m_indices.Add(index1);
		m_indices.Add(index2);
	}

	public Mesh CreateMesh()
	{
		Mesh mesh = new Mesh();

		mesh.vertices = m_vertices.ToArray();
		mesh.triangles = m_indices.ToArray();

		//	Don't use normals unless there is the right number
		if (m_normals.Count == m_vertices.Count)
		{
			mesh.normals = m_normals.ToArray();
		}

		//	Don't use UVs unless there is the right number
		if (m_uvs.Count == m_vertices.Count)
		{
			mesh.uv = m_uvs.ToArray();
		}

		mesh.RecalculateBounds();

		return mesh;
	}
}
