using UnityEngine;
using System.Collections;

public class coucou : MonoBehaviour {

    public Transform target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Debug.DrawLine(transform.position, target.position, Color.green);
        if (Physics.Linecast(transform.position, target.position, out hit))
        {
            if (hit.collider.tag == "Player") {
                if (hit.distance < 3) {
                    Debug.Log("coucou");
                } else {
                    Debug.Log("trop loing");
                }
            }
        }
	}
}
