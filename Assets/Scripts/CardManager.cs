using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class CardManager : MonoBehaviour {

	[SerializeField] DrawCards cardDrawer;
	[SerializeField] int dupliateStarCounter;
	[SerializeField] int[] cardsOfDrawnRarity;
	HouseCreator houseCreator;
	JsonData itemData;
	int counter = -1;
	int randomNumber;
	int cardRarity;
	string json;
	bool runComplete = false;

	// Use this for initialization
	void Start () {
		houseCreator = FindObjectOfType<HouseCreator>();
		json = File.ReadAllText(Application.dataPath + "/CardInfo.json");
		itemData = JsonMapper.ToObject(json);
	}

	public void DrawCardFromIndexBasedOnRarity(int rarity)
	{
		cardRarity = rarity;
		print("cardRarity: " + cardRarity);

		for (int i = 0; i < houseCreator.houseCardNumberIndex.Length; i++)
		{
			if (itemData["index"][i][1].ToString() == cardRarity.ToString())
			{
				++counter;
			}
		}

		cardsOfDrawnRarity = new int[counter + 1];
		counter = -1;

		for (int i = 0; i < houseCreator.houseCardNumberIndex.Length; i++)
		{
			if (itemData["index"][i][1].ToString() == cardRarity.ToString())
			{
				++counter;
				cardsOfDrawnRarity[counter] = houseCreator.houseCardNumberIndex[i];
			}
		}

		int randomNumber = Random.Range(0, counter + 1);

		if (!houseCreator.houseCardIsCollectedIndex[cardsOfDrawnRarity[randomNumber] - 1])
		{
			houseCreator.houseCardIsCollectedIndex[cardsOfDrawnRarity[randomNumber] - 1] = true;
			print("cardsOfDrawnRarity[randomNumber]: " + (cardsOfDrawnRarity[randomNumber] - 1));
		}
		else
		{
			print("duplicate! " + cardRarity + " Stars!");
			print("cardsOfDrawnRarity[randomNumber]: " + (cardsOfDrawnRarity[randomNumber] - 1));
			dupliateStarCounter += cardRarity;
		}

		counter = -1;
	}
}
