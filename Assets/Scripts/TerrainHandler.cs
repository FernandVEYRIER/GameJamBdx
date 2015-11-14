using UnityEngine;
using System.Collections;

public class TerrainHandler : MonoBehaviour {
	
	[SerializeField]
	private float rotationVel = 50f;

	private Vector3 currentMousePosition;

	// Use this for initialization
	void Start () {
	
	}
	
	void Update () 
	{
		//this.transform.RotateAround(this.transform.position, new Vector3(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), 0), rotationVel);
		if (Input.GetMouseButton(0))
			this.transform.RotateAround(this.transform.position, new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X") , 0), Vector3.Distance(Input.mousePosition, currentMousePosition));
		currentMousePosition = Input.mousePosition;
	}
}
