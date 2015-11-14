using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfosCase : MonoBehaviour {

    public Material nature;
    public Material human;
    private int status = 1;
    private bool colonise = false;
    private int percent = 0;
    private List<Transform> conneted_block = new List<Transform>();
    public int Status
    {
        get { return status; }
        set { status = value; }
    }
    public List<Transform> Blocks
    {
        get { return conneted_block; }
        set { conneted_block = value; }
    }
    public int Percent
    {
        get { return percent; }
        set { percent = value; }
    }
    public bool Colonise
    {
        get { return colonise; }
        set { GetComponent<MeshRenderer>().material = (value)?nature:human;colonise = value; }
    }
	// Use this for initialization
	void Start () {
        GetComponent<MeshRenderer>().material = human;
        //InvokeRepeating("growOrDie", 0, 1.0f);
	}
	
	// Update is called every second
	void growOrDie () {

        if (conneted_block.Count != 0) {
            if (GetComponent<SphereCollider>())
            {
                Destroy(GetComponent<SphereCollider>());
                Destroy(GetComponent<Rigidbody>());
            }
            if (!transform.parent.parent.GetComponent<GenMap>().First)
            {
                int avg = 0;
                foreach (Transform block in conneted_block)
                {
                    int dice = Random.Range(50, 100);
                    if (block.GetComponent<InfosCase>().Percent > 50 && dice >= block.GetComponent<InfosCase>().Percent && colonise)
                    {
                        int grow = block.GetComponent<InfosCase>().Percent + percent / 2;
                        if (grow > 100)
                        {
                            block.GetComponent<InfosCase>().Percent = 100;
                        }
                        else
                        {
                            block.GetComponent<InfosCase>().Percent = grow;
                        }
                        block.GetComponent<InfosCase>().Colonise = true;
                        block.GetComponent<MeshRenderer>().material = nature;
                    }
                    //else
                    //{
                    //    int die = block.GetComponent<InfosCase>().Percent + percent / 2;
                    //    if (die > 100)
                    //    {
                    //        block.GetComponent<InfosCase>().Percent = 100;
                    //    }
                    //    else
                    //    {
                    //        block.GetComponent<InfosCase>().Percent = die;
                    //    }
                    //    block.GetComponent<InfosCase>().Colonise = false;
                    //    block.GetComponent<MeshRenderer>().material = human;
                    //}
                    avg += block.GetComponent<InfosCase>().Percent;
                }
                percent = (avg + percent) / (conneted_block.Count + 1);
            }
        }
	}
    void OnMouseOver() {
        GameObject selector = GameObject.FindGameObjectWithTag("Selector");
        selector.transform.position = transform.position;
        selector.transform.rotation = transform.rotation;
        selector.GetComponent<Selector>().select(this);
    }

    public void setColonise(bool cube, Material mat) {
        GetComponent<MeshRenderer>().material = mat;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Case")) {
            conneted_block.Add(col.transform);
        }
    }
}
