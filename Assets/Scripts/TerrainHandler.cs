using UnityEngine;
using System.Collections;

public class TerrainHandler : MonoBehaviour {
	
	[SerializeField]
	private float rotationVel = 50f;

	// Use this for initialization
	void Start () {
	
	}
	
	void Update () 
	{
		this.transform.RotateAround(this.transform.position, new Vector3(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), 0), rotationVel);
	}
}
