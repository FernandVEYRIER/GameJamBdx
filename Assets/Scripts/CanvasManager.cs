using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class CanvasManager : MonoBehaviour {

	[SerializeField]
	private GameObject infoBox;

	[SerializeField]
	private GameObject[] itemButtons;

	private Dictionary<string, Item> inventory = new Dictionary<string, Item>();

	// Use this for initialization
	void Start () 
	{
		// Initialise les objets
		inventory.Add("Seeds", new Item(5, 10));
		inventory.Add ("Rain", new Item(1, 30));
		inventory.Add ("Wind", new Item(0, 60));
		inventory.Add ("Cyclone", new Item(0, 90));
		UpdateInventory();
	}

	void UpdateInventory()
	{
		// Pour chaque item
		for (int i = 0; i < inventory.Count; i++)
		{
			// On met à jour le texte
			if (i < itemButtons.Length)
			{
				itemButtons[i].GetComponentInChildren<Text>().text = inventory.Keys.ElementAt(i) + " (x" + inventory[inventory.Keys.ElementAt(i)].Quantity + ")";
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
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
