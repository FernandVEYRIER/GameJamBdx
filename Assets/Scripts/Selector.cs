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
    void Start() {
    }

    void Update() {

        if (CanvasManager.bIsPaused)
        {
            return;
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        Debug.DrawRay(ray.origin, ray.direction, Color.green);
        bool res = Physics.Raycast(ray, out hit);
        if (!res)
        {
            canvas.UpdateInfoBox();
            status = false;
        }
        else if (res && Input.GetMouseButtonDown(0) && Camera.main.GetComponent<CreateMap>().First)
        {
			if (canvas.beginBox.activeSelf)
			{
                CreateMap map = Camera.main.GetComponent<CreateMap>();
				hit.transform.GetComponent<InfosCase>().Percent = 100;
	            map.First = false;
				map.makeCoffee();
				canvas.ShowHUD(true);
				CanvasManager.bIsPlaying = true;
				canvas.StartItemGeneration();
			}
        }
        else if (res)
        {
            print(hit.collider.tag);
			if (canvas.selectedItem != null && Input.GetMouseButtonDown(0))
			{
				if (canvas.UseItem())
				{
                    hit.transform.GetComponent<InfosCase>().SetBonus(canvas.selectedItem);
				}
			}
            canvas.UpdateInfoBox(hit.transform.GetComponent<InfosCase>().Percent, hit.transform.GetComponent<InfosCase>().Bonus);
        }
    }

    private int boolToInt(bool value) {
        return (value) ? 1 : 0;
    }
    public void select(InfosCase infos) {
        status = true;
    }
}
