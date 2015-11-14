using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenMap : MonoBehaviour {

    public uint size;
    public GameObject plan;
    private bool first = true;
    private List<GameObject> cubes = new List<GameObject>();
    private List<GameObject> dupli_cubes = new List<GameObject>();
    private SphereCollider[] all_spheres;
    private Rigidbody[] all_rigidbodies;
	
	private int [] statTab;

    public bool First
    {
        get { return first; }
        set { first = value; }
    }

    void Awake() {
    }

    private GameObject setFace(float unit) { 
        
        GameObject obj = new GameObject();
        for (uint j = 0; j < size; j++)
        {
            for (uint i = 0; i < size; i++)
            {
                GameObject tmp = (GameObject)Instantiate(plan, new Vector3(unit * (1 + i) - (unit / 2) * size - (unit / 2), unit * (1 + j) - (unit / 2) * size - (unit / 2), (-unit / 2) * size), Quaternion.identity);
                tmp.transform.parent = obj.transform;
                tmp.GetComponent<InfosCase>().Percent = Random.Range(0, 100);
                cubes.Add(tmp);
            }
        }
        return obj;
    }
	// Use this for initialization
	void Start () {
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

    public void makeCoffee()
    {
		statTab = new int[cubes.Count];
		StartCoroutine("growOrDie");
    }

	IEnumerator growOrDie()
	{
		for (int i = 0; i < size; i++)
		{
			List<Transform> blocks = cubes[i].GetComponent<InfosCase>().Blocks;
			if (blocks.Count != 0)
			{
				if (cubes[i].GetComponent<SphereCollider>())
				{
					all_spheres = FindObjectsOfType<SphereCollider>();
					all_rigidbodies = FindObjectsOfType<Rigidbody>();
					foreach (SphereCollider sphere_collider in all_spheres)
					{
						sphere_collider.enabled = false;
					}
					foreach (Rigidbody rigidbody in all_rigidbodies)
					{
						Destroy(rigidbody);
					}
				}
			}
		}

		while (!CanvasManager.bIsPlaying)
		{
			yield return new WaitForSeconds(0.1f);
		}

		// Si on est en partie
		while (CanvasManager.bIsPlaying)
		{
			int avg = 50;
			// Pour chaque case
			for (int i = 0; i < size; i++)
			{
				InfosCase blockInfo = cubes[i].GetComponent<InfosCase>();
				avg = 0;
				// On fait la moyenne des cases voisines
				foreach (Transform tr in blockInfo.Blocks)
				{
					avg += tr.GetComponent<InfosCase>().Percent;
				}
				avg /= blockInfo.Blocks.Count;
				// Si on domine, on gagne 1% + les bonus
				if (avg > 50)
				{
					blockInfo.Percent += Mathf.RoundToInt(1 + (int) (blockInfo.Bonus / 100f * 5));
				}
				// Sinon on perd 1 % moins les bonus qu'on a
				else
				{
					blockInfo.Percent -= Mathf.RoundToInt(1 - (int) (blockInfo.Bonus / 100f * 5));
				}

				// On reste entre 0 et 100
				blockInfo.Percent = Mathf.Clamp(blockInfo.Percent, 0, 100);

				// Eton met à jour les textures
				if (blockInfo.Percent > 50)
				{
					blockInfo.GetComponent<MeshRenderer>().material = blockInfo.nature;
				}
				else
				{
					blockInfo.GetComponent<MeshRenderer>().material = blockInfo.human;
				}
			}
			yield return new WaitForSeconds(2f);
		}
	}

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
