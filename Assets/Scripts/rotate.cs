using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {


    private Vector3 currentMousePosition;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //transform.RotateAround(Vector3.zero, new Vector3(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")), 1f);
        if (Input.GetMouseButton(1))
            this.transform.RotateAround(this.transform.position, new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0), Vector3.Distance(Input.mousePosition, currentMousePosition));
        currentMousePosition = Input.mousePosition;
	}
}
