using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetNeighbors : MonoBehaviour {

    private List<GameObject> neighbors = new List<GameObject>();
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Map") || col.CompareTag("Road"))
            neighbors.Add(col.gameObject);
        if (neighbors.Count == 4) {
            transform.parent.GetComponent<InfosCase>().Neighbors = neighbors;
            Destroy(gameObject);
        }
    }
}
