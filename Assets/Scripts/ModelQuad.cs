using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelQuad : MonoBehaviour {

    public GameObject prefab;
	// Contient les props à mettre sur le terrain
	public GameObject[] terrainProps;

    private int size = 9;
    private int total_size = 486;
    private Quad[] quads = new Quad[486];
    private Quad[] DupliQuads = new Quad[486];
    private GameObject[] cubes = new GameObject[6];
    private bool first;
    public bool First
    {
        get { return first; }
        set { first = value; }
    }
    public void Start() {

        Build();
    }

    public void Build() {
        float unit = prefab.GetComponent<Renderer>().bounds.size.x;
        GameObject front = SetFace(unit);
        front.transform.parent = transform;

        GameObject top = SetFace(unit);
        top.transform.eulerAngles = new Vector3(90, 0, 0);
        top.transform.parent = transform;

        GameObject bottom = SetFace(unit);
        bottom.transform.eulerAngles = new Vector3(-90, 0, 0);
        bottom.transform.parent = transform;

        GameObject left = SetFace(unit);
        left.transform.eulerAngles = new Vector3(0, 90, 0);
        left.transform.parent = transform;

        GameObject right = SetFace(unit);
        right.transform.eulerAngles = new Vector3(0, -90, 0);
        right.transform.parent = transform;

        GameObject back = SetFace(unit);
        back.transform.eulerAngles = new Vector3(0, 180, 0);
        back.transform.parent = transform;
    }

	public void DestroyTerrain()
	{
		for (int i = transform.childCount - 1; i >= 0; i--)
		{
			DestroyImmediate(transform.GetChild(i).gameObject);
		}
	}

    private GameObject SetFace(float unit) {

        GameObject obj = new GameObject();
        float calc_center = - (unit / 2) * size - (unit / 2);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject tmp = (GameObject)Instantiate(prefab, new Vector3(unit * (1 + i) + calc_center, unit * (1 + j) + calc_center, (-unit / 2) * size), Quaternion.identity);
                Quad quad = new Quad();
                quad.Percent = Random.Range(0, 100);
                quad.ID = tmp.GetHashCode();
                quads[i + j] = quad;
                tmp.transform.parent = obj.transform;

				if (terrainProps.Length > 0)
				{
					int itemIndex = (terrainProps.Length - 1) * Mathf.RoundToInt(quad.Percent / 100f);
					tmp = (GameObject) Instantiate(terrainProps[itemIndex], tmp.transform.position, Quaternion.Euler(tmp.transform.right));
					tmp.transform.parent = obj.transform;
                }
            }
        }
        return obj;
    }

    public void SetSidesQuad() {

        for (int i = 0; i < total_size; i++)
        {
            Transform[] blocks = GameObject.Find(quads[i].ID.ToString()).GetComponent<InfosCase>().Blocks.ToArray();
            print(blocks.Length);
        }
        //this.enabled = false;
    }

    //private int[] MappingCube(int index){

    //    int cube = index / (size * size);
    //    int[] sides = new int[4];
        
            
    //    return sides;
    //}

    class Quad { 
        private bool colonised = false;
        private int percent;
        private int id;
        public int Percent
        {
            get { return percent; }
            set { percent = value; }
        }
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public bool Colonised
        {
            get { return colonised; }
            set { colonised = value; }
        }
    }	
}
