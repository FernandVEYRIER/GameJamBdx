using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class CanvasManager : MonoBehaviour {

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
	private Item selectedItem = null;
	private float timeElapsed = 0;

	[HideInInspector]
	public bool bIsPlaying;

	// Use this for initialization
	void Start () 
	{
		ShowHUD(false);

		// Initialise les objets
		inventory.Add("Seeds", new Item(5, 10));
		inventory.Add ("Rain", new Item(1, 30));
		inventory.Add ("Wind", new Item(0, 60));
		inventory.Add ("Cyclone", new Item(0, 90));
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
		if (selectedItem != null)
		Debug.Log(selectedItem.Influence);
	}

	public void UpdateInfoBox(int naturePercent)
	{
		int humanPercent = 100 - naturePercent;
		infoBox.GetComponent<Text>().text = "Human dominance : " + humanPercent + "%\n\n" + "Nature dominance : " + naturePercent + "%";
	}

	public void UpdateInfoBox()
	{
		infoBox.GetComponent<Text>().text = "";
	}

	class Item
	{
		int quantity;
		int influence;

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

		public Item(int _quantity, int _influence)
		{
			quantity = _quantity;
			influence = _influence;
		}
	}
}
