using UnityEngine;
using System.Collections;

public class GrowOrDie : MonoBehaviour {

    private uint size;
	// Use this for initialization
	void Start () {

        size = GetComponent<GenMap>().size;
        size = (size * size) * 6;
        InvokeRepeating("makeCoffee", 0, 3.0f);
	}
	
	// Update is called once per frame
    private void makeCoffee()
    {
        for (uint i = 0; i < size; i++)
        {

        }
	}
}
