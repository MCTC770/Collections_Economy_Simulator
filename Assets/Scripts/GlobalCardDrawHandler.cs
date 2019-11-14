using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCardDrawHandler : MonoBehaviour {

	[SerializeField] int numberOfSimulations;
	[SerializeField] int numberOfDays;
	[Space(12)]
	[SerializeField] bool enableRandomMissedDays;
	[SerializeField] int rangeOfMissedDaysMin;
	[SerializeField] int rangeOfMissedDaysMax;
	[Space(12)]
	[SerializeField] bool guaranteedRarityEnabled;
	[SerializeField] [Range(1, 5)] int guaranteedRarityAfterDays;
	[SerializeField] int dayOfGuaranteedRarity;
	[SerializeField] int[] cardsOfSelectedRarity;
	[Space(12)]
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
	int[] dailyDrawnCardIndex;

	int missedDays;
	int[] missedDaysPicked;

	int starCounter;
	int totalPacksThisDay;
	int packRarityTracker;
	int currentRarity;
	int rarityOfCurrentlyDrawnCard;
	int getIndexOfDrawnCard;
	int currentSimulation;
	int runCounter;
	bool runCompleted = false;
	bool overwriteRarity = false;
	bool forcedRarityDrawn = false;

	string[] fixedPacksPerDay;
	float[] packsPerDayLeft;
	float[] weight;
	int[] numberOfPacksForTheDay = new int[5];
	int[] numberOfPacksForTheDayInUse;
	string[] numberOfPacksForTheDayString;
	bool[] uniqueRandomMissedDays;
	int[,] cardsPerDayTracker;

	// Use this for initialization
	void Start ()
	{
		if (!enableRandomMissedDays)
		{
			rangeOfMissedDaysMin = 0;
			rangeOfMissedDaysMax = 0;
		}
		if (rangeOfMissedDaysMin > rangeOfMissedDaysMax)
		{
			rangeOfMissedDaysMax = rangeOfMissedDaysMin;
		}

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

	public void SetStarCounter(int stars)
	{
		starCounter = stars;
	}

	public void SetWeight(float[] getWeights)
	{
		weight = getWeights;
	}

	public int GetNumberOfDays()
	{
		return numberOfDays;
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
		dailyDrawnCardIndex = new int[houseCreator.houseCardNumberIndex.Length];
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
		cardWeightManager.SetInitialWeightArrayValues();
		cardWeightManager.ShiftStepIterationCounterInitializer();

		currentSimulation += 1;
		forcedRarityDrawn = false;
		overwriteRarity = false;
		endNum = new int[packsPerDay.Length];
		packDistribution = null;
		packDistribution = new bool[numberOfDays];
		defaultCardRarities = new float[setCardRarities.Length];
		cardsOfRarityInPack = new int[5];
		numberOfPacksForTheDay = new int[5];

		missedDays = Random.Range(rangeOfMissedDaysMin, rangeOfMissedDaysMax + 1);
		if(missedDays > 0 && enableRandomMissedDays)
		{
			missedDaysPicked = new int[missedDays];
			uniqueRandomMissedDays = new bool[numberOfDays];
			for (int i = 0; i < uniqueRandomMissedDays.Length; i++)
			{
				uniqueRandomMissedDays[i] = false;
			}
			for (int i = 0; i < missedDays; i++)
			{
				missedDaysPicked[i] = Random.Range(0, numberOfDays);
				while (uniqueRandomMissedDays[missedDaysPicked[i]])
				{
					missedDaysPicked[i] = Random.Range(0, numberOfDays);
				}
				uniqueRandomMissedDays[missedDaysPicked[i]] = true;
				//print("missedDaysPicked[" + i + "]: " + missedDaysPicked[i]);
			}
		}
		else
		{
			enableRandomMissedDays = false;
			//print("no days missed");
		}


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
			runCounter = 1;
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
				if (currentCardNum > 0.00001)
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
			if (guaranteedRarityEnabled && dayOfGuaranteedRarity == j + 1)
			{
				cardManager.CreateIndexPerCardRarity(guaranteedRarityAfterDays);
				cardsOfSelectedRarity = cardManager.GetCardsOfDrawnRarity();
				int cardsOfRarityCounter = 0;
				for (int i = 0; i < cardsOfSelectedRarity.Length; i++)
				{
					if (houseCreator.houseCardIsCollectedIndex[cardsOfSelectedRarity[i] - 1])
					{
						++cardsOfRarityCounter;
					}
					Debug.Log("j: " + j + " cardsOfSelectedRarity[" + i + "]: " + cardsOfSelectedRarity[i] + " cardsOfRarityCounter: " + cardsOfRarityCounter);
				}
				if (cardsOfRarityCounter == 0)
				{
					overwriteRarity = true;
				}
			}

			if (enableRandomMissedDays)
			{
				if (!uniqueRandomMissedDays[j])
				{
					PackDistributionPerDay(j);
				}
			} else
			{
				PackDistributionPerDay(j);
			}

			csvCreator.SetCurrentDay(j + 1);
			csvCreator.SetSimulationCounter(currentSimulation);
			csvCreator.CreateCSVString(drawnCardIndex, dailyDrawnCardIndex, true);
			dailyDrawnCardIndex = new int[dailyDrawnCardIndex.Length];
		}
		csvCreator.CreateCSVString(drawnCardIndex, dailyDrawnCardIndex, false);
	}

	private void PackDistributionPerDay(int j)
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

				if (overwriteRarity && !forcedRarityDrawn)
				{
					rarityOfCurrentlyDrawnCard = guaranteedRarityAfterDays;
					forcedRarityDrawn = true;
					overwriteRarity = false;
					print("rarity: " + rarityOfCurrentlyDrawnCard);
				}
				else
				{
					rarityOfCurrentlyDrawnCard = cardDrawer.DrawARarity();
				}

				getIndexOfDrawnCard = cardManager.DrawCardFromIndexBasedOnRarity(rarityOfCurrentlyDrawnCard);
				print("getIndexOfDrawnCard: " + getIndexOfDrawnCard);
				drawnCardIndex[getIndexOfDrawnCard] += 1;
				dailyDrawnCardIndex[getIndexOfDrawnCard] += 1;
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