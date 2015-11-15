using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;
using LibNoise;

// TODO : fin

public class CanvasManager : MonoBehaviour {

	[HideInInspector]
	public Item selectedItem = null;

	public GameObject [] itemPrefabs;

	[SerializeField]
	private GameObject canvasCredit;

	[SerializeField]
	private GameObject infoBox;

	[SerializeField]
	private GameObject greetingBox;

	public GameObject beginBox;

	[SerializeField]
	private GameObject HUD;
	[SerializeField]
	private GameObject timer;

	[SerializeField]
	private GameObject[] itemButtons;

	[SerializeField]
	private GameObject[] itemBuyButtons;

	[SerializeField]
	private GameObject gauge;

	[SerializeField]
	private GameObject canvasPause;

	[SerializeField]
	private GameObject volumeSlider;

	[SerializeField]
	private GameObject canvasGameOver;

	private Dictionary<string, Item> inventory = new Dictionary<string, Item>();
	private float timeElapsed = 0;

	[HideInInspector]
	public static bool bIsPlaying;

	[HideInInspector]
	public float occupationAmount = 50;

	public int credits = 0;

	[HideInInspector]
	public static bool bIsPaused = false;

	// Use this for initialization
	void Start () 
	{
		HUD.SetActive(false);
		if (PlayerPrefs.GetInt("Display", 1) == 1)
		{
			greetingBox.SetActive(true);
			beginBox.SetActive(false);
		}
		else
		{
			greetingBox.SetActive(false);
			beginBox.SetActive(true);
		}
		infoBox.SetActive(false);
		canvasPause.SetActive(false);
		canvasGameOver.SetActive(false);

		volumeSlider.GetComponent<Slider>().value = Camera.main.GetComponent<AudioSource>().volume;

		// Initialise les objets
		inventory.Add("Seeds", new Item(10, 15, 37, 8, itemPrefabs[0]));
		inventory.Add ("Rain", new Item(5, 30, 25, 15, itemPrefabs[1]));
		inventory.Add ("Wind", new Item(3, 60, 18, 25,itemPrefabs[2]));
		inventory.Add ("Tornado", new Item(1000000, 90, 10, 50, itemPrefabs[3]));
		bIsPlaying = false;
		UpdateInventory();
	}

	public void ShowHUD(bool bIsDisplayed, bool bIsStart = true)
	{
		HUD.SetActive(bIsDisplayed);
		if (bIsStart)
		{
			beginBox.SetActive(!bIsDisplayed);
		}
		infoBox.SetActive(bIsDisplayed);
		UpdateInventory();
	}

	public void StartItemGeneration()
	{
		for (int i = 0; i < inventory.Count; i++)
		{
			StartCoroutine( GetItem(inventory.Keys.ElementAt(i), inventory[inventory.Keys.ElementAt(i)].Influence / 3) );
		}
	}

	IEnumerator GetItem(string itemIndex, int itemDelay)
	{
		while (bIsPlaying)
		{
			yield return new WaitForSeconds(itemDelay);
			inventory[itemIndex].Quantity++;
			UpdateInventory();
		}
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

	public void BuyItem(int itemIndex)
	{
		if (credits >= inventory[inventory.Keys.ElementAt(itemIndex)].Price)
		{
			credits -= inventory[inventory.Keys.ElementAt(itemIndex)].Price;
			inventory[inventory.Keys.ElementAt(itemIndex)].Quantity++;
		}
		UpdateInventory();
	}

	public void AddCredits(int ammount)
	{
		credits += ammount;
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
			if (i < itemBuyButtons.Length)
			{
				itemBuyButtons[i].GetComponentInChildren<Text>().text = "Buy\n(" + inventory[inventory.Keys.ElementAt(i)].Price + ")";
			}
		}
		// On met à jour les crédits
		canvasCredit.GetComponent<Text>().text = "Credits : " + credits;
	}

	public void SelectItem(int itemIndex)
	{
		if (itemIndex < inventory.Count && inventory[inventory.Keys.ElementAt(itemIndex)].Quantity > 0)
		{
			for (int i = 0; i < itemButtons.Length; i++)
			{
				Image curCol = itemButtons[i].GetComponent<Image>();
				curCol.color = new Color(1, 1, 1, 1);
			}
			selectedItem = inventory[inventory.Keys.ElementAt(itemIndex)];
			itemButtons[itemIndex].GetComponent<Image>().color = new Color(0, 0.6f, 0, 1);
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

			gauge.GetComponent<Image>().fillAmount = occupationAmount;

			if (Time.timeSinceLevelLoad > 10)
			{
				if (occupationAmount <= 0)
				{
					GameOver(false);
				}
				else if (occupationAmount >= 1)
				{
					GameOver(true);
				}
			}
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

	public void SetIntroBox(Toggle button)
	{
		PlayerPrefs.SetInt("Display", button.isOn ? 0 : 1);
	}

	public void GameOver(bool bHasWon)
	{
		ShowHUD(false, false);
		canvasGameOver.SetActive(true);
		if (bHasWon)
		{
			canvasGameOver.transform.GetChild(1).GetComponent<Text>().text = "Congratulations ! You managed to take back all the land you deserved, and won against human kind.\n\n Thanks for playing !";
		}
		else
		{
			canvasGameOver.transform.GetChild(1).GetComponent<Text>().text = "Sadly, humanity completely erased you from the map.\n\nThanks for playing !";
		}
		bIsPaused = true;
	}

	public void Quit()
	{
#if UNITY_WEBPLAYER
		Application.ExternalEval("window.close()");
#elif UNITY_STANDALONE
		Application.Quit();
#endif
	}

	public void ReloadLevel()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	public void SetPause()
	{
		if (!bIsPaused)
		{
			bIsPaused = true;
			canvasPause.SetActive(true);
			ShowHUD(false, false);
			Time.timeScale = 0;
		}
		else
		{
			canvasPause.SetActive(false);
			ShowHUD(true, false);
			bIsPaused = false;
			Time.timeScale = 1;
		}
	}

	public class Item
	{
		int quantity;
		int influence;
		float duration;
		int price;
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

		public int Price
		{
			get { return price; }
			set { price = value; }
		}

		public GameObject ItemPrefab
		{
			get { return itemPrefab; }
		}

		public Item(int _quantity, int _influence, int _duration, int _price, GameObject _itemPrefab)
		{
			quantity = _quantity;
			influence = _influence;
			duration = _duration;
			price = _price;
			itemPrefab = _itemPrefab;
		}
	}
}
