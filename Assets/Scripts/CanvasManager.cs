using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class CanvasManager : MonoBehaviour {

	[HideInInspector]
	public Item selectedItem = null;

	public GameObject [] itemPrefabs;

	[SerializeField]
	private GameObject infoBox;
	[SerializeField]
	private GameObject beginBox;
	[SerializeField]
	private GameObject HUD;
	[SerializeField]
	private GameObject timer;

	[SerializeField]
	private GameObject[] itemButtons;

	private Dictionary<string, Item> inventory = new Dictionary<string, Item>();
	private float timeElapsed = 0;

	[HideInInspector]
	public static bool bIsPlaying;

	// Use this for initialization
	void Start () 
	{
		ShowHUD(false);

		// Initialise les objets
		inventory.Add("Seeds", new Item(10, 15, 20, itemPrefabs[0]));
		inventory.Add ("Rain", new Item(5, 30, 15, itemPrefabs[1]));
		inventory.Add ("Wind", new Item(3, 60, 10, itemPrefabs[2]));
		inventory.Add ("Tornado", new Item(100, 90, 6, itemPrefabs[3]));
		bIsPlaying = false;
		UpdateInventory();
	}

	public void ShowHUD(bool bIsDisplayed)
	{
		HUD.SetActive(bIsDisplayed);
		beginBox.SetActive(!bIsDisplayed);
		infoBox.SetActive(bIsDisplayed);
		UpdateInventory();
	}

	public void StartItemGeneration()
	{
		for (int i = 0; i < inventory.Count; i++)
		{
			StartCoroutine( GetItem(inventory.Keys.ElementAt(i), inventory[inventory.Keys.ElementAt(i)].Influence) 	);
		}
	}

	IEnumerator GetItem(string itemIndex, int itemDelay)
	{
		yield return new WaitForSeconds(itemDelay);
		inventory[itemIndex].Quantity++;
		UpdateInventory();
	}

	public bool UseItem()
	{
		bool bCouldUse = false;

		if (selectedItem != null)
		{
			if (selectedItem.Quantity > 0)
			{
				selectedItem.Quantity -= 1;
				bCouldUse = true;
			}
			else
			{
				selectedItem = null;
			}
		}
		UpdateInventory();
		return bCouldUse;
	}

	void UpdateInventory()
	{
		// Pour chaque item
		for (int i = 0; infoBox.activeSelf && i < inventory.Count; i++)
		{
			// On met à jour le texte
			if (i < itemButtons.Length)
			{
				itemButtons[i].GetComponentInChildren<Text>().text = inventory.Keys.ElementAt(i) + " (x" + inventory[inventory.Keys.ElementAt(i)].Quantity + ")";
			}
		}
	}

	public void SelectItem(int itemIndex)
	{
		if (itemIndex < inventory.Count && inventory[inventory.Keys.ElementAt(itemIndex)].Quantity > 0)
		{
			selectedItem = inventory[inventory.Keys.ElementAt(itemIndex)];
		}
		else
		{
			selectedItem = null;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (bIsPlaying)
		{
			TimeSpan timeDisp = TimeSpan.FromSeconds(timeElapsed);
			timer.GetComponent<Text>().text = timeDisp.ToString().Substring(0, timeDisp.ToString().Length - 8);
			timeElapsed += Time.deltaTime;
		}
		else
		{
			CancelInvoke();
		}
	}

	public void UpdateInfoBox(int naturePercent, int bonus)
	{
		int humanPercent = 100 - naturePercent;
		string toDisp = "Human dominance : " + humanPercent + "%\n\n" + "Nature dominance : " + naturePercent + "%";
		if (bonus != 0)
		{
			toDisp += "\n\nCurrent bonus : " + bonus + "%";
		}
		infoBox.GetComponent<Text>().text = toDisp;
	}

	public void UpdateInfoBox()
	{
		infoBox.GetComponent<Text>().text = "";
	}

	public class Item
	{
		int quantity;
		int influence;
		float duration;
		GameObject itemPrefab;

		public int Quantity
		{
			get { return quantity; }
			set { quantity = value; }
		}

		public int Influence
		{
			get { return influence; }
			set { influence = (value >= 0 && value <= 100) ? value : 0; }
		}

		public float Duration
		{
			get { return duration; }
			set { duration = value; influence = (value > 0) ? influence : 0; }
		}

		public GameObject ItemPrefab
		{
			get { return itemPrefab; }
		}

		public Item(int _quantity, int _influence, int _duration, GameObject _itemPrefab)
		{
			quantity = _quantity;
			influence = _influence;
			duration = _duration;
			itemPrefab = _itemPrefab;
		}
	}
}
