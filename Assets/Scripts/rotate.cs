using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //transform.RotateAround(Vector3.zero, new Vector3(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")), 1f);
        transform.RotateAround(Vector3.zero, new Vector3(Random.Range(0, 0.1f), Random.Range(0, 0.1f)), 1f);
	}
}
