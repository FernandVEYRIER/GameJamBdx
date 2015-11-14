using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject cache;
    private bool first = true;
	// Use this for initialization
	void Start () {
        //Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
        GameObject selector = GameObject.FindGameObjectWithTag("Selector");
        if (Input.GetMouseButtonDown(0) && selector.GetComponent<Selector>().Status && first)
        {
            first = false;
            cache.SetActive(false);
            //Time.timeScale = 1;
        }
        else if (first) {
            cache.transform.position = new Vector3(selector.transform.position.x, selector.transform.position.y, selector.transform.position.z - 0.5f);
            cache.transform.rotation = selector.transform.rotation;
        }
	}
}
