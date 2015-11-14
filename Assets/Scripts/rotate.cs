using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //transform.RotateAround(Vector3.zero, new Vector3(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")), 1f);
        if (Input.GetMouseButton(1))
            transform.RotateAround(Vector3.zero, new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0), 1f);
	}
}
