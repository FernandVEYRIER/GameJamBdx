using UnityEngine;
using System.Collections;

public class Cars : MonoBehaviour {

    public float speed;
    public bool invert;
    private bool init = false;
    public float borne;
    public string axis;
    private GameObject[] points = new GameObject[4];
    private int index = 0;
	// Use this for initialization
	void Start () {

        points[0] = new GameObject();
        points[1] = new GameObject();
        points[2] = new GameObject();
        points[3] = new GameObject();
        if (invert)
        {
            if (axis == "x") {
                points[3].transform.position = new Vector3(transform.position.x, borne, borne);
                points[2].transform.position = new Vector3(transform.position.x, -borne, borne);
                points[1].transform.position = new Vector3(transform.position.x, -borne, -borne);
                points[0].transform.position = new Vector3(transform.position.x, borne, -borne);
            }
            else if (axis == "z") {
                points[3].transform.position = new Vector3(borne, borne, transform.position.z);
                points[2].transform.position = new Vector3(borne, -borne, transform.position.z);
                points[1].transform.position = new Vector3(-borne, -borne, transform.position.z);
                points[0].transform.position = new Vector3(-borne, borne, transform.position.z);
            }
            else if (axis == "y")
            {
                points[3].transform.position = new Vector3(borne, transform.position.y, -borne);
                points[2].transform.position = new Vector3(borne, transform.position.y, borne);
                points[1].transform.position = new Vector3(-borne, transform.position.y, borne);
                points[0].transform.position = new Vector3(-borne, transform.position.y, -borne);
            }
            
        }
        else
        {
            if (axis == "x") {
                points[0].transform.position = new Vector3(transform.position.x, borne, borne);
                points[1].transform.position = new Vector3(transform.position.x, -borne, borne);
                points[2].transform.position = new Vector3(transform.position.x, -borne, -borne);
                points[3].transform.position = new Vector3(transform.position.x, borne, -borne);
            }
            else if (axis == "z")
            {
                points[0].transform.position = new Vector3(borne, borne, transform.position.z);
                points[1].transform.position = new Vector3(borne, -borne, transform.position.z);
                points[2].transform.position = new Vector3(-borne, -borne, transform.position.z);
                points[3].transform.position = new Vector3(-borne, borne, transform.position.z);
            } else if (axis == "y") {
                points[0].transform.position = new Vector3(borne, transform.position.y, -borne);
                points[1].transform.position = new Vector3(borne, transform.position.y, borne);
                points[2].transform.position = new Vector3(-borne, transform.position.y, borne);
                points[3].transform.position = new Vector3(-borne, transform.position.y, -borne);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        
        if (!init && GameObject.Find("MAP").transform) {
            Transform parent = GameObject.Find("MAP").transform;
            transform.parent = parent;
            points[0].transform.parent = parent;
            points[1].transform.parent = parent;
            points[2].transform.parent = parent;
            points[3].transform.parent = parent;
            init = true;
        }
        if (init) {
            Vector3 previous = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, points[index].transform.position, speed * Time.deltaTime);
            if (previous == transform.position)
            {
                transform.rotation *= Quaternion.Euler(new Vector3(0, 90, 0));
                if (index == 3)
                    index = 0;
                else
                    index++;
            }
        }
	}
}
