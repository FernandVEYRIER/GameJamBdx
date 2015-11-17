using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GenMap : MonoBehaviour {

	public CanvasManager canvas;

	public GameObject [] terrainProps;

	public PropPrefab [] prefabProps;

	[Serializable]
	public class PropPrefab
	{
		public Mesh mesh;
		public Material material;
	}

    public uint size;
    public GameObject plan;
    private bool first = true;
    private List<GameObject> cubes = new List<GameObject>();
    private List<GameObject> dupli_cubes = new List<GameObject>();
    private SphereCollider[] all_spheres;
    private Rigidbody[] all_rigidbodies;

    public bool First
    {
        get { return first; }
        set { first = value; }
    }

    void Awake() {
    }

    private GameObject setFace(float unit) { 
        
		LibNoise.Unity.Generator.Perlin pl = new LibNoise.Unity.Generator.Perlin();
		pl.Lacunarity = 1;
		pl.OctaveCount = 2;
		pl.Persistence = 1.96f;
		pl.Seed = 1;
		pl.Quality = LibNoise.Unity.QualityMode.High;
		pl.Frequency = 1 / 80.0f;

        GameObject obj = new GameObject();
        for (uint j = 0; j < size; j++)
        {
            for (uint i = 0; i < size; i++)
            {
                GameObject tmp = (GameObject)Instantiate(plan, new Vector3(unit * (1 + i) - (unit / 2) * size - (unit / 2), unit * (1 + j) - (unit / 2) * size - (unit / 2), (-unit / 2) * size), Quaternion.identity);
                tmp.transform.parent = obj.transform;
				tmp.GetComponent<InfosCase>().Percent = (int)(((pl.GetValue(tmp.transform.position) + 1.7f) / 3.0f ) * 100);
                cubes.Add(tmp);

				if (terrainProps.Length > 0)
				{
					GameObject go = (GameObject) Instantiate(terrainProps[0], tmp.transform.position, Quaternion.Euler(tmp.transform.right));
					go.transform.parent = tmp.transform;
					if (tmp.GetComponent<InfosCase>().Percent >= 50)
					{
						go.GetComponentInChildren<MeshRenderer>().enabled = false;

//						go.GetComponentInChildren<MeshRenderer>().material = prefabProps[0].material;
//						go.GetComponentInChildren<MeshFilter>().mesh = prefabProps[0].mesh;
					}
					else
					{
						go.GetComponentInChildren<MeshRenderer>().enabled = true;
					}
				}

                
//                if (terrainProps.Length > 0)
//				{
//					int yolo = (terrainProps.Length - 1) * Mathf.RoundToInt(tmp.GetComponent<InfosCase>().Percent / 100f);
//					GameObject go = (GameObject) Instantiate(terrainProps[yolo], tmp.transform.position, Quaternion.Euler(tmp.transform.right));
//					go.transform.parent = tmp.transform;
//				}
            }
        }
        return obj;
    }
	// Use this for initialization
	void Start () {
		Build();
	}

	public void Build()
	{
		float unit = plan.GetComponent<Renderer>().bounds.size.x;
		GameObject front = setFace(unit);
		front.transform.parent = transform;
		GameObject top = setFace(unit);
		top.transform.eulerAngles = new Vector3(90, 0, 0);
		top.transform.parent = transform;
		GameObject bottom = setFace(unit);
		bottom.transform.eulerAngles = new Vector3(-90, 0, 0);
		bottom.transform.parent = transform;
		GameObject right = setFace(unit);
		right.transform.eulerAngles = new Vector3(0, 90, 0);
		right.transform.parent = transform;
		GameObject left = setFace(unit);
		left.transform.eulerAngles = new Vector3(0, -90, 0);
		left.transform.parent = transform;
		GameObject back = setFace(unit);
		back.transform.eulerAngles = new Vector3(0, 180, 0);
		back.transform.parent = transform;
		
		size = (size * size) * 6;
		cubes.ForEach((item) => { dupli_cubes.Add(item); });
		//        for (int i = 0; i < cubes.Count; i++)
		//        {
		//            print(dupli_cubes[i].GetHashCode());
		//            print(cubes[i].GetHashCode());
		//        }
	}

	public void DestroyTerrain()
	{
		for (int i = transform.childCount - 1; i >= 0; i--)
		{
			DestroyImmediate(transform.GetChild(i).gameObject);
		}
	}

    public void makeCoffee()
    {
		StartCoroutine("growOrDie");
    }

    //IEnumerator growOrDie()
    //{
    //    for (int i = 0; i < size; i++)
    //    {
    //        List<Transform> blocks = cubes[i].GetComponent<InfosCase>().Blocks;
    //        if (blocks.Count != 0)
    //        {
    //            if (cubes[i].GetComponent<SphereCollider>())
    //            {
    //                all_spheres = FindObjectsOfType<SphereCollider>();
    //                all_rigidbodies = FindObjectsOfType<Rigidbody>();
    //                foreach (SphereCollider sphere_collider in all_spheres)
    //                {
    //                    sphere_collider.enabled = false;
    //                }
    //                foreach (Rigidbody rigidbody in all_rigidbodies)
    //                {
    //                    Destroy(rigidbody);
    //                }
    //            }
    //        }
    //    }

    //    while (!CanvasManager.bIsPlaying)
    //    {
    //        yield return new WaitForSeconds(0.1f);
    //    }

    //    // Si on est en partie
    //    while (CanvasManager.bIsPlaying)
    //    {
    //        int avg = 50;
    //        int previousPercent = 0;
    //        float natureOccupation = 0;
    //        // Pour chaque case
    //        for (int i = 0; i < size; i++)
    //        {
    //            InfosCase blockInfo = cubes[i].GetComponent<InfosCase>();
    //            previousPercent = blockInfo.Percent;
    //            avg = 0;
    //            // On fait la moyenne des cases voisines
    //            foreach (Transform tr in blockInfo.Blocks)
    //            {
    //                avg += tr.GetComponent<InfosCase>().Percent;
    //            }
    //            avg /= blockInfo.Blocks.Count;
    //            // Si on domine, on gagne 1% + les bonus
    //            if (avg > 50)
    //            {
    //                blockInfo.Percent += Mathf.RoundToInt(1 + (int) (blockInfo.Bonus / 100f * 5));
    //            }
    //            // Sinon on perd 1 % moins les bonus qu'on a
    //            else
    //            {
    //                blockInfo.Percent -= Mathf.RoundToInt(1 - (int) (blockInfo.Bonus / 100f * 5));
    //            }

    //            // On reste entre 0 et 100
    //            blockInfo.Percent = Mathf.Clamp(blockInfo.Percent, 0, 100);

    //            // Et on met à jour les textures
    //            if (blockInfo.Percent > 50)
    //            {
    //                if (previousPercent <= 50)
    //                {
    //                    canvas.AddCredits(30);
    //                }
    //                natureOccupation += 1;
    //                blockInfo.GetComponent<MeshRenderer>().material = blockInfo.nature;
    //                blockInfo.transform.GetChild(0).GetComponentInChildren<MeshRenderer>().enabled = false;
    //            }
    //            else
    //            {
    //                blockInfo.GetComponent<MeshRenderer>().material = blockInfo.human;
    //                blockInfo.transform.GetChild (0).GetComponentInChildren<MeshRenderer>().enabled = true;
    //            }
    //        }
    //        natureOccupation /= (float)size;
    //        canvas.occupationAmount = natureOccupation;
    //        yield return new WaitForSeconds(1f);
    //    }
    //}

//    IEnumerator growOrDie()
//    {
//
//        while (true)
//        {
//            for (int i = 0; i < size; i++)
//            {
//                List<Transform> blocks = cubes[i].GetComponent<InfosCase>().Blocks;
//                if (blocks.Count != 0)
//                {
//                    if (cubes[i].GetComponent<SphereCollider>())
//                    {
//                        all_spheres = FindObjectsOfType<SphereCollider>();
//                        all_rigidbodies = FindObjectsOfType<Rigidbody>();
//                        foreach (SphereCollider sphere_collider in all_spheres)
//                        {
//                            sphere_collider.enabled = false;
//                        }
//                        foreach (Rigidbody rigidbody in all_rigidbodies)
//                        {
//                            Destroy(rigidbody);
//                        }
//                    }
//                    if (!first)
//                    {
//                        int avg = 0;
//                        for (int j = 0; j < blocks.Count; j++)
//                        {
//                            int dice = Random.Range(50, 100);
//                            InfosCase infos_blocks = blocks[j].GetComponent<InfosCase>();
//                            if (infos_blocks.Percent > 50 && dice >= infos_blocks.Percent && cubes[i].GetComponent<InfosCase>().Colonise)
//                            {
//                                int grow = infos_blocks.Percent + cubes[i].GetComponent<InfosCase>().Percent / 2;
//                                if (grow > 100)
//                                {
//                                    infos_blocks.Percent = 100;
//                                }
//                                else
//                                {
//                                    infos_blocks.Percent = grow;
//                                }
//                                infos_blocks.Colonise = true;
//                                blocks[j].GetComponent<MeshRenderer>().material = cubes[i].GetComponent<InfosCase>().nature;
//                                yield return new WaitForSeconds(0.5f);
//                            }
//                            avg += infos_blocks.Percent;
//                        }
//                        cubes[i].GetComponent<InfosCase>().Percent = (avg + cubes[i].GetComponent<InfosCase>().Percent) / (blocks.Count + 1);
//                    }
//                }
//            }
//        }
//    }
}
