using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateMap : MonoBehaviour
{

    public GameObject quad_prefabs;
    public Texture2D tex;
    public Color ignore_color;
    public Color road_color;
    public Color building_color;
    public GameObject[] buildings;
    public GameObject nuclear;
    public GameObject farm;
    public road[] roads;
    public CanvasManager canvas;
    private GameObject[] faces = new GameObject[6];
    private GameObject cube;
	// Use this for initialization
    private const int FRONT = 0;
    private const int BACK = 1;
    private const int LEFT = 2;
    private const int RIGHT = 3;
    private const int TOP = 4;
    private const int BOTTOM = 5;
    private List<GameObject> destroy_list = new List<GameObject>();
    private bool first = true;
    [System.Serializable]
    public class road {
        public Mesh mesh;
        public Material mat;
    }

    public bool First
    {
        get { return first; }
        set { first = value; }
    }

	void Start () {

        Build();
	}

    public void Build() {
        Color col;
        cube = new GameObject();
        cube.name = "MAP";
        cube.AddComponent<rotate>();
        faces[FRONT] = new GameObject();
        faces[FRONT].name = "FRONT";

        faces[BACK] = new GameObject();
        faces[BACK].name = "BACK";

        faces[LEFT] = new GameObject();
        faces[LEFT].name = "LEFT";

        faces[RIGHT] = new GameObject();
        faces[RIGHT].name = "RIGHT";

        faces[TOP] = new GameObject();
        faces[TOP].name = "TOP";

        faces[BOTTOM] = new GameObject();
        faces[BOTTOM].name = "BOTTOM";

        Vector3 middle = new Vector3(tex.width / 2f - 0.5f, tex.height / 2f + tex.width / 3f / 2f - 0.5f, tex.width / 3f / 2f);

        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                col = tex.GetPixel(i, j);
                if (col != ignore_color)
                {
                    GameObject quad = (GameObject)Instantiate(quad_prefabs, new Vector3(i - middle.x, j - middle.y, -middle.z), Quaternion.identity);
                    quad.GetComponent<MeshRenderer>().material.color = tex.GetPixel(i, j);
                    if (i < 9 && j > 17 && j < 27)
                    {
                        quad.transform.parent = faces[LEFT].transform;
                    }
                    else if (i > 8 && i < 18)
                    {
                        if (j < 9)
                            quad.transform.parent = faces[BACK].transform;
                        else if (j > 8 && j < 18)
                            quad.transform.parent = faces[BOTTOM].transform;
                        else if (j > 17 && j < 27)
                            quad.transform.parent = faces[FRONT].transform;
                        else
                            quad.transform.parent = faces[TOP].transform;
                    }
                    else
                    {
                        quad.transform.parent = faces[RIGHT].transform;
                    }
                }
            }
        }
        for (int i = 0; i < 6; i++)
        {
            if (i == LEFT)
                faces[i].transform.RotateAround(new Vector3(-middle.z, 0, -middle.z), Vector3.up, 90);
            if (i == RIGHT)
                faces[i].transform.RotateAround(new Vector3(middle.z, 0, -middle.z), Vector3.up, -90);
            if (i == TOP)
                faces[i].transform.RotateAround(new Vector3(0, middle.z, -middle.z), Vector3.right, 90);
            if (i == BOTTOM)
                faces[i].transform.RotateAround(new Vector3(0, -middle.z, -middle.z), Vector3.right, -90);
            if (i == BACK)
                faces[i].transform.RotateAround(new Vector3(0, -middle.z * 2, 0), Vector3.right, 180);
            faces[i].transform.parent = cube.transform;
        }
        Transform current;
        Color current_color;
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < faces[i].transform.childCount; j++)
            {
                current = faces[i].transform.GetChild(j);
                current_color = current.GetComponent<MeshRenderer>().material.color;
                if (current_color == road_color)
                {
                    current.GetComponent<MeshFilter>().mesh = roads[0].mesh;
                    current.GetComponent<MeshCollider>().sharedMesh = roads[0].mesh;
                    current.GetComponent<MeshRenderer>().material = roads[0].mat;
                    current.localScale += new Vector3(0, 0, -2);
                    current.tag = "Road";
                    current.GetComponent<InfosCase>().Percent = 40;
                }
                else if (current_color == building_color)
                {
                    current.GetComponent<MeshRenderer>().material.color = Color.grey;
                    GameObject obj = (GameObject)Instantiate(buildings[Random.Range(0, buildings.Length)], current.position, Quaternion.Euler(current.eulerAngles.x, current.eulerAngles.y, current.eulerAngles.z + Random.Range(-90, 90)));
                    obj.transform.parent = current;
                    current.GetComponent<InfosCase>().Percent = 30;
                } else if (current_color == Color.green) {
                    current.GetComponent<InfosCase>().Percent = 80;
                }
                else if (current_color == Color.cyan)
                {
                    GameObject obj = (GameObject)Instantiate(nuclear, current.position, Quaternion.identity);
                    obj.transform.parent = current;
                    current.GetComponent<InfosCase>().Percent = 10;
                }
                else if (current_color == Color.blue)
                {
                    //current.GetComponent<MeshRenderer>().material.color = Color.grey;
                    GameObject obj = (GameObject)Instantiate(farm, current.position, current.rotation);
                    obj.transform.parent = current;
                    current.GetComponent<InfosCase>().Percent = 25;
                }
                else {
                    current.GetComponent<InfosCase>().Percent = current_color.b * 100;
                }
            }
        }
        foreach (GameObject quad in destroy_list)
        {
            Destroy(quad);
        }
        Invoke("RebuildRoad", 0.5f);
    }

    public void DestroyTerrain() {

        DestroyImmediate(cube);
    }
	
	// Update is called once per frame
	void Update () {
	}

    private void RebuildRoad() {
        Transform current;
        
        List<road_construc> vecs = new List<road_construc>();
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < faces[i].transform.childCount; j++) //for (int j = 40; j < 41; j++)//
            {
                current = faces[i].transform.GetChild(j);
                List<GameObject> neighbors = current.GetComponent<InfosCase>().Neighbors;
                List<GameObject> currents = new List<GameObject>();
                for (int h = 0; h < 4; h++)
			    {
                    if (neighbors[h].CompareTag("Road")) {
                        currents.Add(neighbors[h]);
                    }
                    if (current.GetComponent<MeshRenderer>().material.color == Color.cyan) {
                        neighbors[h].GetComponent<InfosCase>().Percent = 20;
                    }
                    else if (current.GetComponent<MeshRenderer>().material.color == Color.blue)
                    {
                        neighbors[h].GetComponent<InfosCase>().Percent = 25;
                    }
			    }
                //neighbors[0].GetComponent<MeshRenderer>().material.color = Color.green;
                //neighbors[1].GetComponent<MeshRenderer>().material.color = Color.cyan;
                //neighbors[2].GetComponent<MeshRenderer>().material.color = Color.yellow;
                //neighbors[3].GetComponent<MeshRenderer>().material.color = Color.red;
                if (current.GetComponent<MeshRenderer>().material.color == Color.cyan || current.GetComponent<MeshRenderer>().material.color == Color.blue)
                    current.GetComponent<MeshRenderer>().material.color = Color.grey;
                if (currents.Count == 4) {
                    current.GetComponent<MeshFilter>().mesh = roads[1].mesh;
                    current.GetComponent<MeshRenderer>().material = roads[1].mat;
                }
                else if (currents.Count == 2)
                {
                    if ((i != RIGHT && i != LEFT) && (current.transform.position.x == currents[1].transform.position.x && current.transform.position.x == currents[0].transform.position.x))
                        vecs.Add(new road_construc(current.transform.eulerAngles + new Vector3(0, 0, 90), current));
                }
            }
        }
        foreach (var vec in vecs)
        {
            vec.Tr.eulerAngles = vec.Vec;
        }
    }

    class road_construc {
        Vector3 vec;
        Transform tr;
        public Vector3 Vec
        {
            get { return vec; }
            set { vec = value; }
        }
        public Transform Tr
        {
            get { return tr; }
            set { tr = value; }
        }
        public road_construc(Vector3 v, Transform o) {
            vec = v;
            tr = o;
        }
    }

    public void makeCoffee()
    {
        //statTab = new int[cubes.Count];
        StartCoroutine("growOrDie");
    }

    IEnumerator growOrDie()
    {
        while (!CanvasManager.bIsPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }
        while (CanvasManager.bIsPlaying)
        {
            for (int i = 0; i < 6; i++)
            {
                float avg = 50;
                float previousPercent = 0;
                float natureOccupation = 0;
                for (int j = 0; j < faces[i].transform.childCount; j++)
                {
                    InfosCase blockInfo = faces[i].transform.GetChild(j).GetComponent<InfosCase>();
                    previousPercent = blockInfo.Percent;
                    avg = 0;
                    // On fait la moyenne des cases voisines
                    foreach (GameObject tr in blockInfo.Neighbors)
                    {
                        avg += tr.transform.GetComponent<InfosCase>().Percent;
                    }
                    avg /= blockInfo.Neighbors.Count;
                    // Si on domine, on gagne 1% + les bonus
                    if (avg > 50)
                    {
                        blockInfo.Percent += Mathf.RoundToInt(1 + (int)(blockInfo.Bonus / 100f * 5));
                    }
                    // Sinon on perd 1 % moins les bonus qu'on a
                    else
                    {
                        blockInfo.Percent -= Mathf.RoundToInt(1 - (int)(blockInfo.Bonus / 100f * 5));
                    }
                    blockInfo.Percent = Mathf.Clamp(blockInfo.Percent, 0, 100);

                    // Et on met à jour les textures
                    if (blockInfo.Percent > 50)
                    {
                        if (previousPercent <= 50)
                        {
                            canvas.AddCredits(30);
                        }
                        natureOccupation += 1;
                        blockInfo.GetComponent<MeshRenderer>().material = blockInfo.nature;
                        //blockInfo.transform.GetChild(0).GetComponentInChildren<MeshRenderer>().enabled = false;
                    }
                    else
                    {
                        //blockInfo.GetComponent<MeshRenderer>().material = blockInfo.human;
                        //blockInfo.transform.GetChild(0).GetComponentInChildren<MeshRenderer>().enabled = true;
                    }
                }
                natureOccupation /= (float)6 * faces[0].transform.childCount;
                canvas.occupationAmount = natureOccupation;
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
