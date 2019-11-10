using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCardDrawHandler : MonoBehaviour {

	[SerializeField] int numberOfSimulations;
	[SerializeField] int numberOfDays;
	[Space (12)]
	[SerializeField] float[] setCardRarities = new float[5];
	[SerializeField] float[] packsPerDay = new float[5];
	[SerializeField] PackCreator[] packSelection;

	string defaultNumberOfSimulations;
	string defaultNumberOfDays;
	string[] defaultSetCardRarities = new string[5];
	string[] defaultPacksPerDay = new string[5];

	PackCreator[] packsForTheDay;
	DrawCards cardDrawer;
	CardManager cardManager;
	HouseCreator houseCreator;
	CsvCreator csvCreator;
	CardWeightManager cardWeightManager;

	float currentCardNum;
	int[] endNum;
	bool[] packDistribution;

	int[] cardsOfRarityInPack;
	float[] defaultCardRarities;

	int[] drawnCardIndex;

	int starCounter;
	int totalPacksThisDay;
	int packRarityTracker;
	int currentRarity;
	int rarityOfCurrentlyDrawnCard;
	int getIndexOfDrawnCard;
	bool runCompleted = false;

	string[] fixedPacksPerDay;
	float[] packsPerDayLeft;
	float[] weight;
	int[] numberOfPacksForTheDay = new int[5];
	int[] numberOfPacksForTheDayInUse;
	string[] numberOfPacksForTheDayString;
	int[,] cardsPerDayTracker;

	// Use this for initialization
	void Start ()
	{

		houseCreator = FindObjectOfType<HouseCreator>();
		cardDrawer = FindObjectOfType<DrawCards>();
		cardManager = FindObjectOfType<CardManager>();
		csvCreator = FindObjectOfType<CsvCreator>();
		cardWeightManager = FindObjectOfType<CardWeightManager>();

		starCounter = 0;

		defaultNumberOfSimulations = numberOfSimulations.ToString();
		defaultNumberOfDays = numberOfDays.ToString();
		for (int i = 0; i < setCardRarities.Length; i++)
		{
			defaultSetCardRarities[i] = setCardRarities[i].ToString();
		}
		for (int i = 0; i < packsPerDay.Length; i++)
		{
			defaultPacksPerDay[i] = packsPerDay[i].ToString();
		}

		//InitializeValues();
		//ResetPackDistributionArray();
	}

	public void GetStarCounter(int stars)
	{
		starCounter = stars;
	}

	public void SetWeight(float[] getWeights)
	{
		weight = getWeights;
	}

	private void InitializeValues()
	{
		totalPacksThisDay = 0;
		numberOfSimulations = int.Parse(defaultNumberOfSimulations);
		numberOfDays = int.Parse(defaultNumberOfDays);
		for (int i = 0; i < setCardRarities.Length; i++)
		{
			setCardRarities[i] = float.Parse(defaultSetCardRarities[i]);
		}
		for (int i = 0; i < packsPerDay.Length; i++)
		{
			packsPerDay[i] = float.Parse(defaultPacksPerDay[i]);
		}

		drawnCardIndex = new int[houseCreator.houseCardNumberIndex.Length];
		cardsPerDayTracker = null;
		cardsPerDayTracker = new int[numberOfDays, packSelection.Length];
		fixedPacksPerDay = new string[packsPerDay.Length];
		for (int i = 0; i < packsPerDay.Length; i++)
		{
			fixedPacksPerDay[i] = packsPerDay[i].ToString();
		}

		houseCreator.ResetCardIndex();
		cardManager.InitializeDuplicateStarCounter();
		starCounter = 0;
		cardWeightManager.ShiftStepIterationCounterInitializer();

		endNum = new int[packsPerDay.Length];
		packDistribution = null;
		packDistribution = new bool[numberOfDays];
		defaultCardRarities = new float[setCardRarities.Length];
		cardsOfRarityInPack = new int[5];
		numberOfPacksForTheDay = new int[5];

		weight = cardWeightManager.GetWeightPerRoomArray();
		for (int i = 0; i < weight.Length; i++)
		{
			houseCreator.GetRoomsInThisHouse()[i].weight = weight[i];
		}

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
		if (!runCompleted)
		{
			int runCounter = 1;
			csvCreator.CreateHeader();
			for (int i = 0; i < numberOfSimulations; i++)
			{
				InitializeValues();
				AllocatePacksForTheDay();
			}
			++runCounter;
			csvCreator.CreateCSVFile();
			runCompleted = true;
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
					/*Debug.Log("cardsPerDayTracker[" + j + ", 0]: " + cardsPerDayTracker[j, 0]);
					Debug.Log("cardsPerDayTracker[" + j + ", 1]: " + cardsPerDayTracker[j, 1]);
					Debug.Log("cardsPerDayTracker[" + j + ", 2]: " + cardsPerDayTracker[j, 2]);
					Debug.Log("cardsPerDayTracker[" + j + ", 3]: " + cardsPerDayTracker[j, 3]);
					Debug.Log("cardsPerDayTracker[" + j + ", 4]: " + cardsPerDayTracker[j, 4]);*/
				}
			}
		}

		for (int j = 0; j < numberOfDays; j++)
		{
			cardWeightManager.SetCurrentNumberOfDays(j + 1);
			totalPacksThisDay = 0;

			for (int k = 0; k < packSelection.Length; k++)
			{
				totalPacksThisDay += cardsPerDayTracker[j, k];
				//Debug.Log("cardsPerDayTracker[" + j + ", " + k + "]: " + cardsPerDayTracker[j, k]);
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
		csvCreator.CreateCSVString(drawnCardIndex);
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

				/*Debug.Log("cardsOfRarityInPack[0]: " + cardsOfRarityInPack[0]);
				Debug.Log("cardsOfRarityInPack[1]: " + cardsOfRarityInPack[1]);
				Debug.Log("cardsOfRarityInPack[2]: " + cardsOfRarityInPack[2]);
				Debug.Log("cardsOfRarityInPack[3]: " + cardsOfRarityInPack[3]);
				Debug.Log("cardsOfRarityInPack[4]: " + cardsOfRarityInPack[4]);*/

				DrawChancePerMinimumCardRarity(i);
				rarityOfCurrentlyDrawnCard = cardDrawer.DrawARarity();
				getIndexOfDrawnCard = cardManager.DrawCardFromIndexBasedOnRarity(rarityOfCurrentlyDrawnCard);
				drawnCardIndex[getIndexOfDrawnCard] += 1;
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

		/*Debug.Log("setCardRarities[0]: " + setCardRarities[0]);
		Debug.Log("setCardRarities[1]: " + setCardRarities[1]);
		Debug.Log("setCardRarities[2]: " + setCardRarities[2]);
		Debug.Log("setCardRarities[3]: " + setCardRarities[3]);
		Debug.Log("setCardRarities[4]: " + setCardRarities[4]);*/

	}

	private void GetCardValues()
	{
		for (int i = 0; i < setCardRarities.Length; i++)
		{
			setCardRarities[i] = defaultCardRarities[i];
		}
	}
}