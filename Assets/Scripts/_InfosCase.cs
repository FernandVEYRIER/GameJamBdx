//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class _InfosCase : MonoBehaviour
//{

//    public Material nature;
//    public Material human;
//    private int status = 1;
//    private bool colonise = false;
//    private int percent = 0;
//    private List<Transform> conneted_block = new List<Transform>();

//    private List<Item> activeItems = new List<Item>();

//    class Item
//    {
//        float duration;
//        int influence;
//        GameObject go;

//        public GameObject Go
//        {
//            get { return go; }
//            set { go = value; }
//        }

//        public float Duration
//        {
//            get { return duration; }
//            set { duration = value; }
//        }

//        public int Influence
//        {
//            get { return influence; }
//            set { influence = value; }
//        }

//        public Item(float _duration, int _influence)
//        {
//            duration = _duration;
//            influence = _influence;
//        }
//    }

//    // Bonus appliqué à la case
//    private int bonus = 0;

//    public int Bonus
//    {
//        get { return bonus; }
//    }

//    public int Status
//    {
//        get { return status; }
//        set { status = value; }
//    }
//    public List<Transform> Blocks
//    {
//        get { return conneted_block; }
//        set { conneted_block = value; }
//    }
//    public int Percent
//    {
//        get { return percent; }
//        set { percent = value; }
//    }
//    public bool Colonise
//    {
//        get { return colonise; }
//        set { GetComponent<MeshRenderer>().material = (value) ? nature : human; colonise = value; }
//    }

//    void Start()
//    {
//        GetComponent<MeshRenderer>().material = human;
//        //InvokeRepeating("growOrDie", 0, 1.0f);
//    }

//    public void SetBonus(CanvasManager.Item value)
//    {
//        activeItems.Add(new Item(value.Duration, value.Influence));
//        activeItems[activeItems.Count - 1].Go = (GameObject)Instantiate(value.ItemPrefab, this.transform.position, this.transform.rotation);
//        activeItems[activeItems.Count - 1].Go.transform.SetParent(this.transform);
//        bonus += value.Influence;
//    }

//    void Update()
//    {
//        // Pour chaque item sur cette case
//        for (int i = 0; i < activeItems.Count; i++)
//        {
//            // On fait diminuer le temps d'action de l'item
//            activeItems[i].Duration -= Time.deltaTime;
//            // S'il n'a plus d'influence on le vire
//            if (activeItems[i].Duration <= 0)
//            {
//                // Et on remove le prefab associé au buff
//                Destroy(activeItems[i].Go);
//                bonus -= activeItems[i].Influence;
//                activeItems.RemoveAt(i);
//            }
//        }
//    }

//    // Update is called every second
//    void growOrDie()
//    {

//        if (conneted_block.Count != 0)
//        {
//            if (GetComponent<SphereCollider>())
//            {
//                Destroy(GetComponent<SphereCollider>());
//                Destroy(GetComponent<Rigidbody>());
//            }
//            if (!transform.parent.parent.GetComponent<GenMap>().First)
//            {
//                int avg = 0;
//                foreach (Transform block in conneted_block)
//                {
//                    int dice = Random.Range(50, 100);
//                    if (block.GetComponent<InfosCase>().Percent > 50 && dice >= block.GetComponent<InfosCase>().Percent && colonise)
//                    {
//                        int grow = block.GetComponent<InfosCase>().Percent + percent / 2;
//                        if (grow > 100)
//                        {
//                            block.GetComponent<InfosCase>().Percent = 100;
//                        }
//                        else
//                        {
//                            block.GetComponent<InfosCase>().Percent = grow;
//                        }
//                        block.GetComponent<InfosCase>().Colonise = true;
//                        block.GetComponent<MeshRenderer>().material = nature;
//                    }
//                    //else
//                    //{
//                    //    int die = block.GetComponent<InfosCase>().Percent + percent / 2;
//                    //    if (die > 100)
//                    //    {
//                    //        block.GetComponent<InfosCase>().Percent = 100;
//                    //    }
//                    //    else
//                    //    {
//                    //        block.GetComponent<InfosCase>().Percent = die;
//                    //    }
//                    //    block.GetComponent<InfosCase>().Colonise = false;
//                    //    block.GetComponent<MeshRenderer>().material = human;
//                    //}
//                    avg += block.GetComponent<InfosCase>().Percent;
//                }
//                percent = (avg + percent) / (conneted_block.Count + 1);
//            }
//        }
//    }
//    void OnMouseOver()
//    {
//        if (CanvasManager.bIsPaused)
//        {
//            return;
//        }

//        GameObject selector = GameObject.FindGameObjectWithTag("Selector");
//        selector.transform.position = transform.position;
//        selector.transform.rotation = transform.rotation;
//        selector.GetComponent<Selector>().select(this);
//    }

//    public void setColonise(bool cube, Material mat)
//    {
//        GetComponent<MeshRenderer>().material = mat;
//    }

//    void OnTriggerEnter(Collider col)
//    {
//        if (col.CompareTag("Case"))
//        {
//            conneted_block.Add(col.transform);
//        }
//    }
//}
