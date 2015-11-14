using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour {

	[SerializeField]
	private CanvasManager canvas;

    private bool status = false;
    public bool Status
    {
        get { return status; }
        set { status = value; }
    }
    public Material[] mats;
    MeshRenderer child_1;
    MeshRenderer child_2;
    MeshRenderer child_3;
    MeshRenderer child_4;
    void Start() {
        child_1 = transform.GetChild(0).GetComponent<MeshRenderer>();
        child_2 = transform.GetChild(1).GetComponent<MeshRenderer>();
        child_3 = transform.GetChild(2).GetComponent<MeshRenderer>();
        child_4 = transform.GetChild(3).GetComponent<MeshRenderer>();
        changeStatus();
    }

    void Update() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        Debug.DrawRay(ray.origin, ray.direction, Color.green);
        bool res = Physics.Raycast(ray, out hit);
        if (!res && child_1.enabled)
        {
            status = false;
            changeStatus();
        }
        else if (res && Input.GetMouseButtonDown(0) && GameObject.FindGameObjectWithTag("Map").GetComponent<GenMap>().First)
        {
            GenMap map = GameObject.FindGameObjectWithTag("Map").GetComponent<GenMap>();
            hit.transform.GetComponent<InfosCase>().Colonise = true;
            map.First = false;
            map.makeCoffee();
			canvas.ShowHUD(true);
			CanvasManager.bIsPlaying = true;
			canvas.StartItemGeneration();
        }
        else if (res)
        {
			if (canvas.selectedItem != null && Input.GetMouseButtonDown(0))
			{
				GameObject go = (GameObject) Instantiate(canvas.selectedItem.ItemPrefab, hit.transform.position + 5 * hit.normal, hit.transform.rotation);
				go.transform.SetParent(hit.transform);
				hit.transform.GetComponent<InfosCase>().SetBonus(canvas.selectedItem);
				canvas.UseItem();
			}
			canvas.UpdateInfoBox(hit.transform.GetComponent<InfosCase>().Percent, hit.transform.GetComponent<InfosCase>().Bonus);
        }
		else if (!res)
		{
			canvas.UpdateInfoBox();
		}
    }

    private void changeStatus() {
        child_1.enabled = status;
        child_2.enabled = status;
        child_3.enabled = status;
        child_4.enabled = status;
    }

    private int boolToInt(bool value) {
        return (value) ? 1 : 0;
    }
    public void select(InfosCase infos) {

        int stat = boolToInt(infos.Colonise);
        child_1.material = mats[stat];
        child_2.material = mats[stat];
        child_3.material = mats[stat];
        child_4.material = mats[stat];
        status = true;
        changeStatus();
    }
}
