using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfosCase : MonoBehaviour {

    public Material nature;
    public Material human;
    private List<GameObject> neighbors = new List<GameObject>();
    private float percent;
    private List<Item> activeItems = new List<Item>();
    public List<GameObject> Neighbors
    {
        get { return neighbors; }
        set { neighbors = value; }
    }
    public float Percent
    {
        get { return percent; }
        set { percent = value; }
    }

    class Item
    {
        float duration;
        int influence;
        GameObject go;

        public GameObject Go
        {
            get { return go; }
            set { go = value; }
        }

        public float Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public int Influence
        {
            get { return influence; }
            set { influence = value; }
        }

        public Item(float _duration, int _influence)
        {
            duration = _duration;
            influence = _influence;
        }
    }

    // Bonus appliqué à la case
    private int bonus = 0;

    public int Bonus
    {
        get { return bonus; }
    }
    public void SetBonus(CanvasManager.Item value)
    {
        activeItems.Add(new Item(value.Duration, value.Influence));
        activeItems[activeItems.Count - 1].Go = (GameObject)Instantiate(value.ItemPrefab, this.transform.position, this.transform.rotation);
        activeItems[activeItems.Count - 1].Go.transform.SetParent(this.transform);
        bonus += value.Influence;
    }

    void Update()
    {
        // Pour chaque item sur cette case
        for (int i = 0; i < activeItems.Count; i++)
        {
            // On fait diminuer le temps d'action de l'item
            activeItems[i].Duration -= Time.deltaTime;
            // S'il n'a plus d'influence on le vire
            if (activeItems[i].Duration <= 0)
            {
                // Et on remove le prefab associé au buff
                Destroy(activeItems[i].Go);
                bonus -= activeItems[i].Influence;
                activeItems.RemoveAt(i);
            }
        }
    }

    void OnMouseOver() {
        if (CanvasManager.bIsPaused)
        {
            return;
        }

        GameObject selector = GameObject.FindGameObjectWithTag("Selector");
        selector.GetComponent<MeshRenderer>().material.color = new Color((100 - percent) / 100f, percent / 100f, 0, 0.4f);
        selector.transform.position = transform.position;
        selector.transform.rotation = transform.rotation;
        selector.GetComponent<Selector>().select(this);
    }
}
