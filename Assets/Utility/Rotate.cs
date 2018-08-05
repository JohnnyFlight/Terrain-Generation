using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{
	[SerializeField] private Vector3 m_axis = Vector3.up;
	[SerializeField] private float m_speed = 0.25f; // Rotations per second
	
	// Update is called once per frame
	void Update ()
	{
		this.transform.RotateAround(this.transform.position, m_axis, 360.0f * m_speed * Time.deltaTime);
	}
}
