using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO: Ajuster la normale selon le terrain, donner des priorités de direction
public class IAMove : MonoBehaviour {

	public float velocity = 10;

	private Vector3 currentTarget = Vector3.zero;
	private Vector3 nextTarget = Vector3.zero;

	private bool bIsFirstCollision;

	private float startTime;
	private Vector3 startPos = Vector3.zero;
	
	void Start () 
	{
		bIsFirstCollision = true;
	}
	
	void Update () 
	{
		if (currentTarget != Vector3.zero)
		{
			if (startTime <= 1)
			{
				this.transform.position = Vector3.Lerp(startPos, currentTarget, startTime);
				startTime += Time.deltaTime / velocity;
			}
			else
			{
				this.transform.position = currentTarget;
				this.transform.rotation = Quaternion.LookRotation(nextTarget - currentTarget);
				currentTarget = nextTarget;
				startTime = 0;
				startPos = this.transform.position;
			}	
		}
	}

	void OnTriggerEnter(Collider col)
	{
		InfosCase terrain = col.gameObject.GetComponent<InfosCase>();
		List<Transform> dirs = new List<Transform>();

		// Si c'est la première collision on aligne l'objet
		if (bIsFirstCollision && col.tag == "Road")
		{
			this.transform.position = col.transform.position;

			foreach (GameObject go in terrain.Neighbors)
			{
				if (go.GetComponent<MeshRenderer>().material.name == "RoadFront (Instance)" || go.GetComponent<MeshRenderer>().material.name == "RoadMiddle (Instance)")
				{
					// On le fait regarder dans la direction qu'il prend
					this.transform.rotation = Quaternion.LookRotation(go.transform.position - col.transform.position);
					currentTarget = go.transform.position;
					startTime = 0;
					startPos = this.transform.position;
					break;
				}
			}

			bIsFirstCollision = false;
		}
		// Sinon on garde en mémoire le prochain checkpoint
		else if (!bIsFirstCollision && col.tag == "Road")	
		{
			foreach (GameObject go in terrain.Neighbors)
			{
				if (go.GetComponent<MeshRenderer>().material.name == "RoadFront (Instance)" || go.GetComponent<MeshRenderer>().material.name == "RoadMiddle (Instance)")
				{
					dirs.Add(go.transform);
				}
			}
			if (dirs.Count > 0)
			{
				nextTarget = dirs[Random.Range(0, dirs.Count)].transform.position;
				Debug.Log("GO normal = " + dirs[0].GetComponent<MeshFilter>().mesh.normals[0] + " my up vector = " + this.transform.up);
				Vector3 myFwd = transform.forward;
				Vector3 otherFwd = nextTarget - currentTarget;
				float dotRes = Vector3.Dot(myFwd, otherFwd);
				Debug.Log("My fwd = " + myFwd + " Dot = " + dotRes);
				// Si on va faire demi tour, ou tourner à gauche
				if (dotRes == -1)
				{
					// Ajout d'un offset
//						currentTarget += transform.forward * (col.GetComponent<MeshRenderer>().bounds.size.x / 2f - this.GetComponent<BoxCollider>().size.z / 2f);
//						Debug.Log(col.GetComponent<MeshRenderer>().bounds.size.x / 2f);
//						startTime -= 0.15f;
				}
			}
		}
	}
}
