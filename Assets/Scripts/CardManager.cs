using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class CardManager : MonoBehaviour {

	[SerializeField] DrawCards cardDrawer;
	[SerializeField] int dupliateStarCounter;
	[SerializeField] int[] cardsOfDrawnRarity;
	[SerializeField] float[] cardWeightIndex;
	[SerializeField] float[] drawChancesBasedOnWeight;
	HouseCreator houseCreator;
	GlobalCardDrawHandler globalCardDrawHandler;
	JsonData itemData;
	int counter = -1;
	int randomNumber;
	float randomFloat;
	int cardRarity;
	string json;
	bool runComplete = false;
	bool reset = false;
	float accumulatedChances = 0;
	[SerializeField] float accumulatedWeights;

	// Use this for initialization
	void Start () {
		globalCardDrawHandler = FindObjectOfType<GlobalCardDrawHandler>();
		houseCreator = FindObjectOfType<HouseCreator>();
		json = File.ReadAllText(Application.dataPath + "/CardInfo.json");
		itemData = JsonMapper.ToObject(json);
	}

	public void InitializeDuplicateStarCounter()
	{
		reset = true;
	}

	public int DrawCardFromIndexBasedOnRarity(int rarity)
	{
		if (reset)
		{
			dupliateStarCounter = 0;
			reset = false;
		}

		cardRarity = rarity;
		//print("cardRarity: " + cardRarity);

		for (int i = 0; i < houseCreator.houseCardNumberIndex.Length; i++)
		{
			if (houseCreator.houseCardRarityIndex[i] == cardRarity)
			{
				++counter;
			}
		}

		cardsOfDrawnRarity = new int[counter + 1];
		cardWeightIndex = new float[counter + 1];
		drawChancesBasedOnWeight = new float[counter + 1];
		accumulatedWeights = 0;
		counter = -1;

		for (int i = 0; i < houseCreator.houseCardNumberIndex.Length; i++)
		{
			if (houseCreator.houseCardRarityIndex[i] == cardRarity)
			{
				++counter;
				cardsOfDrawnRarity[counter] = houseCreator.houseCardNumberIndex[i];
				cardWeightIndex[counter] += houseCreator.houseCardWeightIndex[i];
				accumulatedWeights += cardWeightIndex[counter];
			}
			
		}

		for (int i = 0; i < cardsOfDrawnRarity.Length; i++)
		{
			if(cardWeightIndex[i] > 0)
			{
				drawChancesBasedOnWeight[i] = cardWeightIndex[i] / accumulatedWeights;
			}
			else
			{
				drawChancesBasedOnWeight[i] = 0;
			}
		}

		accumulatedChances = 0;
		randomFloat = Random.Range(0f, 1f);

		for (int i = 0; i < drawChancesBasedOnWeight.Length; i++)
		{
			if (randomFloat > accumulatedChances)
			{
				accumulatedChances += drawChancesBasedOnWeight[i];
			}
			if (randomFloat <= accumulatedChances)
			{
				if (!houseCreator.houseCardIsCollectedIndex[cardsOfDrawnRarity[i] - 1])
				{
					houseCreator.houseCardIsCollectedIndex[cardsOfDrawnRarity[i] - 1] = true;
					counter = -1;
					return cardsOfDrawnRarity[i] - 1;
				}
				else
				{
					dupliateStarCounter += cardRarity;
					globalCardDrawHandler.GetStarCounter(dupliateStarCounter);
					counter = -1;
					return cardsOfDrawnRarity[i] - 1;
				}
			}
		}
		Debug.LogError("No card of drawn rarity available");
		Debug.Break();
		return 99999;
	}
}
