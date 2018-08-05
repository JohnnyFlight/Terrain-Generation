using UnityEngine;
using System.Collections;

public class ProceduralQuad : MonoBehaviour {

	[SerializeField] private float m_length = 1.0f;
	[SerializeField] private float m_width = 2.0f;

	// Use this for initialization
	void Start ()
	{
		MeshBuilder meshBuilder = new MeshBuilder();
		
		meshBuilder.vertices.Add(new Vector3(0.0f, 0.0f, 0.0f));
		meshBuilder.uvs.Add(new Vector2(0.0f, 0.0f));
		meshBuilder.normals.Add(Vector3.up);
		
		meshBuilder.vertices.Add(new Vector3(0.0f, 0.0f, m_length));
		meshBuilder.uvs.Add(new Vector2(0.0f, 1.0f));
		meshBuilder.normals.Add(Vector3.up);
		
		meshBuilder.vertices.Add(new Vector3(m_width, 0.0f, m_length));
		meshBuilder.uvs.Add(new Vector2(1.0f, 1.0f));
		meshBuilder.normals.Add(Vector3.up);
		
		meshBuilder.vertices.Add(new Vector3(m_width, 0.0f, 0.0f));
		meshBuilder.uvs.Add(new Vector2(1.0f, 0.0f));
		meshBuilder.normals.Add(Vector3.up);

		meshBuilder.addTriangle(0, 1, 2);
		meshBuilder.addTriangle(0, 2, 3);

		//	Create the mesh
		MeshFilter filter = GetComponent<MeshFilter>();

		if (filter != null)
		{
			Debug.Log("Mesh Filter found");
			filter.sharedMesh = meshBuilder.CreateMesh();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
