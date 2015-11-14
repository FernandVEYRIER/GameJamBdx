using UnityEngine;
using System.Collections;

public class ModelQuad : MonoBehaviour {

    public GameObject prefab;
    private int size = 9;
    private Quad[] quads = new Quad[486];
    private Quad[] DupliQuads = new Quad[486];
    private GameObject[] cubes = new GameObject[6];

    public void Start() {

        Build();
    }

    private void Build() {
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
                quads[i + j] = quad;
                tmp.transform.parent = obj.transform;
            }
        }
        return obj;
    }

    private int[] MappingCube(int index){
        
        int[] sides = new int[4];
        if (quads[index + 1] != null) {
            sides[0] = index + 1;
        }
            
        return sides;
    }

    class Quad { 
        private bool colonised = false;
        private int percent;
        public int Percent
        {
            get { return percent; }
            set { percent = value; }
        }
        public bool Colonised
        {
            get { return colonised; }
            set { colonised = value; }
        }
    }	
}
