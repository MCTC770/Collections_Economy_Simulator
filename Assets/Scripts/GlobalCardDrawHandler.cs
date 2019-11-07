using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCardDrawHandler : MonoBehaviour {

	[SerializeField] int numberOfDays;
	[SerializeField] float[] setCardRarities = new float[5];
	[SerializeField] float[] packsPerDay = new float[5];
	[SerializeField] PackCreator[] packSelection;

	//[SerializeField] PackCreator[] packsDrawn;
	PackCreator[] packsForTheDay;
	DrawCards cardDrawer;
	CardManager cardManager;

	float currentCardNum;
	int[] endNum;
	int dayCounter;
	bool[] packDistribution;

	int[] cardsOfRarityInPack;
	float[] defaultCardRarities;

	int totalPacksThisDay;
	int packRarityTracker;
	int currentRarity;
	int rarityOfCurrentlyDrawnCard;
	string[] fixedPacksPerDay;
	float[] packsPerDayLeft;
	int[] numberOfPacksForTheDay = new int[5];
	int[] numberOfPacksForTheDayInUse;
	string[] numberOfPacksForTheDayString;
	[SerializeField] int[,] cardsPerDayTracker;

	// Use this for initialization
	void Start ()
	{
		cardsPerDayTracker = new int[numberOfDays, packSelection.Length];
		fixedPacksPerDay = new string[packsPerDay.Length];
		for (int i = 0; i < packsPerDay.Length; i++)
		{
			fixedPacksPerDay[i] = packsPerDay[i].ToString();
		}

		endNum = new int[packsPerDay.Length];
		packDistribution = new bool[numberOfDays];
		cardDrawer = FindObjectOfType<DrawCards>();
		cardManager = FindObjectOfType<CardManager>();
		defaultCardRarities = new float[setCardRarities.Length];
		cardsOfRarityInPack = new int[5];

		for (int i = 0; i < setCardRarities.Length; i++)
		{
			defaultCardRarities[i] = setCardRarities[i];
		}
		GetCardValues();

		ResetPackDistributionArray();
	}

	private void ResetPackDistributionArray()
	{
		for (int i = 0; i < packDistribution.Length; i++)
		{
			packDistribution[i] = false;
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A))
		{
			SelectPacksIndividually();
		}

		if (Input.GetKeyDown(KeyCode.B))
		{
			AllocatePacksForTheDay();
		}
	}

	void AllocatePacksForTheDay()
	{
		packsPerDayLeft = new float[packsPerDay.Length];
		for (int i = 0; i < packsPerDayLeft.Length; i++)
		{
			packsPerDayLeft[i] = float.Parse(fixedPacksPerDay[i]);
		}

		for (int i = 0; i < packsPerDay.Length; i++)
		{
			dayCounter = 0;

			while (packsPerDayLeft[i] >= 1)
			{
				numberOfPacksForTheDay[i] += 1;
				packsPerDayLeft[i] -= 1;
			}

			if (packsPerDayLeft[i] > 0)
			{
				currentCardNum = packsPerDayLeft[i] * numberOfDays;
				endNum[i] = 0;

				while (currentCardNum >= 1)
				{
					endNum[i] += 1;
					currentCardNum -= 1;
				}
				if (currentCardNum > 0)
				{
					float randomNum = Random.Range(0f, 1f);
					if (randomNum >= currentCardNum)
					{
						endNum[i] += 1;
					}
				}
			}
		}

		numberOfPacksForTheDayString = new string[numberOfPacksForTheDay.Length];
		numberOfPacksForTheDayInUse = new int[numberOfPacksForTheDay.Length];

		for (int k = 0; k < numberOfPacksForTheDay.Length; k++)
		{
			numberOfPacksForTheDayString[k] = numberOfPacksForTheDay[k].ToString();
			numberOfPacksForTheDayInUse[k] = int.Parse(numberOfPacksForTheDayString[k]);
		}

		for (int i = 0; i < packsPerDayLeft.Length; i++)
		{
			ResetPackDistributionArray();

			while (endNum[i] > 0)
			{
				int randomNum = Random.Range(0, numberOfDays);
				while (packDistribution[randomNum] == true)
				{
					randomNum = Random.Range(0, numberOfDays);
				}
				packDistribution[randomNum] = true;
				endNum[i] -= 1;
			}

			for (int j = 0; j < numberOfDays; j++)
			{
				numberOfPacksForTheDayInUse[i] = int.Parse(numberOfPacksForTheDayString[i]);

				if (packDistribution[j] == true)
				{
					numberOfPacksForTheDayInUse[i] += 1;
				}

				cardsPerDayTracker[j, i] = numberOfPacksForTheDayInUse[i];
				if (i == 4)
				{
					Debug.Log("cardsPerDayTracker[" + j + ", 0]: " + cardsPerDayTracker[j, 0]);
					Debug.Log("cardsPerDayTracker[" + j + ", 1]: " + cardsPerDayTracker[j, 1]);
					Debug.Log("cardsPerDayTracker[" + j + ", 2]: " + cardsPerDayTracker[j, 2]);
					Debug.Log("cardsPerDayTracker[" + j + ", 3]: " + cardsPerDayTracker[j, 3]);
					Debug.Log("cardsPerDayTracker[" + j + ", 4]: " + cardsPerDayTracker[j, 4]);
				}
			}
		}

		for (int j = 0; j < numberOfDays; j++)
		{
			totalPacksThisDay = 0;

			for (int k = 0; k < packSelection.Length; k++)
			{
				totalPacksThisDay += cardsPerDayTracker[j, k];
			}

			packsForTheDay = new PackCreator[totalPacksThisDay];

			for (int k = 0; k < totalPacksThisDay; k++)
			{
				for (int l = 0; l < packSelection.Length; l++)
				{
					packRarityTracker = cardsPerDayTracker[j, l];

					for (int m = 0; m < packRarityTracker; m++)
					{
						packsForTheDay[k] = packSelection[l];
						k++;

						if (k >= totalPacksThisDay)
						{
							k = totalPacksThisDay - 1;
						}
					}
				}
			}
			SelectPacksIndividually();
		}
	}

	void SelectPacksIndividually()
	{
		for (int i = 0; i < packsForTheDay.Length; i++)
		{
			cardsOfRarityInPack[0] = packsForTheDay[i].numberOf1StarCards;
			cardsOfRarityInPack[1] = packsForTheDay[i].numberOf2StarCards;
			cardsOfRarityInPack[2] = packsForTheDay[i].numberOf3StarCards;
			cardsOfRarityInPack[3] = packsForTheDay[i].numberOf4StarCards;
			cardsOfRarityInPack[4] = packsForTheDay[i].numberOf5StarCards;

			DrawCardsIncurrentPack();
		}
	}

	void DrawCardsIncurrentPack()
	{
		for (int i = 0; i < cardsOfRarityInPack.Length; i++)
		{
			while (cardsOfRarityInPack[i] > 0)
			{

				Debug.Log("cardsOfRarityInPack[0]: " + cardsOfRarityInPack[0]);
				Debug.Log("cardsOfRarityInPack[1]: " + cardsOfRarityInPack[1]);
				Debug.Log("cardsOfRarityInPack[2]: " + cardsOfRarityInPack[2]);
				Debug.Log("cardsOfRarityInPack[3]: " + cardsOfRarityInPack[3]);
				Debug.Log("cardsOfRarityInPack[4]: " + cardsOfRarityInPack[4]);

				DrawChancePerMinimumCardRarity(i);
				rarityOfCurrentlyDrawnCard = cardDrawer.DrawARarity();
				cardManager.DrawCardFromIndexBasedOnRarity(rarityOfCurrentlyDrawnCard);
				cardsOfRarityInPack[i] -= 1;
			}
		}
	}

	void DrawChancePerMinimumCardRarity(int rarity)
	{
		GetCardValues();
		for (int k = 0; k < setCardRarities.Length; k++)
		{
			if (rarity > k)
			{
				setCardRarities[k] = 0;
			}
		}

		cardDrawer.SetCardRarities(setCardRarities[0], setCardRarities[1], setCardRarities[2], setCardRarities[3], setCardRarities[4]);

		Debug.Log("setCardRarities[0]: " + setCardRarities[0]);
		Debug.Log("setCardRarities[1]: " + setCardRarities[1]);
		Debug.Log("setCardRarities[2]: " + setCardRarities[2]);
		Debug.Log("setCardRarities[3]: " + setCardRarities[3]);
		Debug.Log("setCardRarities[4]: " + setCardRarities[4]);

	}

	private void GetCardValues()
	{
		for (int i = 0; i < setCardRarities.Length; i++)
		{
			setCardRarities[i] = defaultCardRarities[i];
		}
	}
}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCardDrawHandler : MonoBehaviour {

	[SerializeField] int runTimeInDays;
	[SerializeField] float[] setCardRarities = new float[5];
	[SerializeField] PackCreator[] packIndex = new PackCreator[5];
	[SerializeField] float[] numberOfPacksDrawnPerTier = new float[5];

	DrawCards cardDrawer;
	CardManager cardManager;

	int[] cardsOfRarityInPack;
	float[] defaultCardRarities;

	int rarityOfCurrentlyDrawnCard;
	float totalAmountOfPacksFromCurrentRarity;
	int dailyPacks;

	// Use this for initialization
	void Start () {

		cardDrawer = FindObjectOfType<DrawCards>();
		cardManager = FindObjectOfType<CardManager>();
		defaultCardRarities = new float[setCardRarities.Length];
		cardsOfRarityInPack = new int[5];

		for (int i = 0; i < setCardRarities.Length; i++)
		{
			defaultCardRarities[i] = setCardRarities[i];
		}
		GetCardValues();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A))
		{
			SelectPacksIndividually();
		}
	}

	void SelectPacksIndividually()
	{
		for (int i = 0; i < numberOfPacksDrawnPerTier.Length; i++)
		{
			cardsOfRarityInPack[0] = packIndex[i].numberOf1StarCards;
			cardsOfRarityInPack[1] = packIndex[i].numberOf2StarCards;
			cardsOfRarityInPack[2] = packIndex[i].numberOf3StarCards;
			cardsOfRarityInPack[3] = packIndex[i].numberOf4StarCards;
			cardsOfRarityInPack[4] = packIndex[i].numberOf5StarCards;

			while (numberOfPacksDrawnPerTier[i] >= 1)
			{
				dailyPacks = 1;
				DrawCardsInCurrentPack();
				numberOfPacksDrawnPerTier[i] -= 1;
			}

			totalAmountOfPacksFromCurrentRarity = numberOfPacksDrawnPerTier[i] * runTimeInDays;
			//TODO: If number is a float, define by chance if the remaining rarity results in pack (e.g. 20.5 should be 20 guaranteed + 50% chance for a 21st) 
			//TODO: Distribute number accross days evenly. If number is a float, distribute remaining cards evenly

			DrawCardsInCurrentPack();
		}
	}

	void DrawCardsInCurrentPack()
	{
		for (int i = 0; i < cardsOfRarityInPack.Length; i++)
		{
			while (cardsOfRarityInPack[i] > 0)
			{

				Debug.Log("cardsOfRarityInPack[0]: " + cardsOfRarityInPack[0]);
				Debug.Log("cardsOfRarityInPack[1]: " + cardsOfRarityInPack[1]);
				Debug.Log("cardsOfRarityInPack[2]: " + cardsOfRarityInPack[2]);
				Debug.Log("cardsOfRarityInPack[3]: " + cardsOfRarityInPack[3]);
				Debug.Log("cardsOfRarityInPack[4]: " + cardsOfRarityInPack[4]);

				DrawChancePerMinimumCardRarity(i);
				rarityOfCurrentlyDrawnCard = cardDrawer.DrawARarity();
				cardManager.DrawCardFromIndexBasedOnRarity(rarityOfCurrentlyDrawnCard);
				cardsOfRarityInPack[i] -= 1;
			}
		}
	}

	void DrawChancePerMinimumCardRarity(int rarity)
	{
		GetCardValues();
		for (int k = 0; k < setCardRarities.Length; k++)
		{
			if (rarity > k)
			{
				setCardRarities[k] = 0;
			}
		}

		cardDrawer.SetCardRarities(setCardRarities[0], setCardRarities[1], setCardRarities[2], setCardRarities[3], setCardRarities[4]);

		Debug.Log("setCardRarities[0]: " + setCardRarities[0]);
		Debug.Log("setCardRarities[1]: " + setCardRarities[1]);
		Debug.Log("setCardRarities[2]: " + setCardRarities[2]);
		Debug.Log("setCardRarities[3]: " + setCardRarities[3]);
		Debug.Log("setCardRarities[4]: " + setCardRarities[4]);

	}

	private void GetCardValues()
	{
		for (int i = 0; i < setCardRarities.Length; i++)
		{
			setCardRarities[i] = defaultCardRarities[i];
		}
	}
}*/