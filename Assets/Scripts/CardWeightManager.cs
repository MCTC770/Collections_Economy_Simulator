using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardWeightManager : MonoBehaviour {

	[SerializeField] bool weightPerRoom = true;
	[SerializeField] bool weightPerCard;
	[Space(12)]
	[SerializeField] float[] weightPerRoomArray;
	[SerializeField] float[] weightPerCardArray;
	[Space(12)]
	[SerializeField] bool weightChangeByDays = true;
	[SerializeField] bool weightChangeByProgress;
	[Space(12)]
	[SerializeField] int shiftSequenceInDays;
	[SerializeField] int shiftSteps;
	[SerializeField] int shiftCardProgressSteps;
	[SerializeField] int shiftRoomProgressSteps;
	[Space(12)]
	[SerializeField] bool timeLocksEnabled;
	[SerializeField] int[] daysUntilRoomUnlock;

	int currentNumberOfDays;
	int shiftSequenceCounter;
	int dayOfLastShift = 0;
	int initialCollectedCardsCounter;
	int shiftStepIterationCounter = 1;
	int countRoomsCompleted = 0;
	int cardIndexCounter = 0;
	bool shiftStepIterationCounterInitialized = false;
	string[] setWeightPerRoomArray;
	string[] setWeightPerCardArray;
	GlobalCardDrawHandler globalCardDrawHandler;
	HouseCreator houseCreator;

	// Use this for initialization
	void Start () {
		houseCreator = FindObjectOfType<HouseCreator>();
		globalCardDrawHandler = FindObjectOfType<GlobalCardDrawHandler>();
		globalCardDrawHandler.SetWeight(weightPerRoomArray);

		setWeightPerRoomArray = new string[weightPerRoomArray.Length];
		for (int i = 0; i < weightPerRoomArray.Length; i++)
		{
			setWeightPerRoomArray[i] = weightPerRoomArray[i].ToString();
		}

		if (weightPerRoom == true)
		{
			weightPerCard = false;
		}
		else
		{
			timeLocksEnabled = false;
		}

		if (weightChangeByDays == true)
		{
			weightChangeByProgress = false;
		}

	}

	public void ShiftStepIterationCounterInitializer()
	{
		shiftStepIterationCounterInitialized = false;
	}

	public void SetCurrentNumberOfDays(int currentDayCount)
	{
		currentNumberOfDays = currentDayCount;
		ShiftSequenceHandler();
	}

	public float[] GetWeightPerRoomArray()
	{
		return weightPerRoomArray;
	}

	public float[] GetWeightPerCardArray()
	{
		return weightPerCardArray;
	}

	public void SetLengthOfWeightPerCardArray(int[] indexArray)
	{
		InitializeSetWeightPerCardArray();

		if (indexArray.Length != weightPerCardArray.Length)
		{
			weightPerCardArray = new float[indexArray.Length];
			for (int i = 0; i < weightPerCardArray.Length; i++)
			{
				if (setWeightPerCardArray.Length <= weightPerCardArray.Length && i < setWeightPerCardArray.Length)
				{
					weightPerCardArray[i] = float.Parse(setWeightPerCardArray[i]);
				}
				else if (setWeightPerCardArray.Length > weightPerCardArray.Length)
				{
					weightPerCardArray[i] = float.Parse(setWeightPerCardArray[i]);
				}
				else
				{
					weightPerCardArray[i] = 0;
				}
			}
		}
	}

	private void InitializeSetWeightPerCardArray()
	{
		setWeightPerCardArray = new string[weightPerCardArray.Length];
		for (int i = 0; i < weightPerCardArray.Length; i++)
		{
			setWeightPerCardArray[i] = weightPerCardArray[i].ToString();
		}
	}

	public void SetLengthOfWeightPerRoomArray(int indexArray)
	{
		InitializeSetWeightPerRoomArray();

		if (indexArray != weightPerRoomArray.Length)
		{
			weightPerRoomArray = new float[indexArray];
			for (int i = 0; i < weightPerRoomArray.Length; i++)
			{
				print(i);
				if (setWeightPerRoomArray.Length <= weightPerRoomArray.Length && i < setWeightPerRoomArray.Length)
				{
					weightPerRoomArray[i] = float.Parse(setWeightPerRoomArray[i]);
				}
				else if (setWeightPerRoomArray.Length > weightPerRoomArray.Length)
				{
					weightPerRoomArray[i] = float.Parse(setWeightPerRoomArray[i]);
				}
				else
				{
					weightPerRoomArray[i] = 0;
				}
			}
		}
	}

	private void InitializeSetWeightPerRoomArray()
	{
		setWeightPerRoomArray = new string[weightPerRoomArray.Length];
		for (int i = 0; i < weightPerRoomArray.Length; i++)
		{
			setWeightPerRoomArray[i] = weightPerRoomArray[i].ToString();
		}
	}

	void ShiftSequenceHandler()
	{
		if (weightChangeByDays)
		{
			shiftSequenceCounter += 1;
			//print("shiftSequenceInDays: " + shiftSequenceInDays + " shiftSequenceCounter: " + shiftSequenceCounter);
			if (shiftSequenceInDays <= shiftSequenceCounter && dayOfLastShift != currentNumberOfDays)
			{
				shiftSequenceCounter -= shiftSequenceInDays;
				dayOfLastShift = currentNumberOfDays;
				if (weightPerRoom)
				{
					CalculateWeightChanceByDayPerRoom();
				}
				else if (weightPerCard)
				{
					CalculateWeightChanceByDayPerCard();
				}
				TimeLockRooms();
			}
		}
		else if (weightChangeByProgress)
		{
			initialCollectedCardsCounter = 0;
			if (!shiftStepIterationCounterInitialized)
			{
				shiftStepIterationCounter = 1;
				shiftStepIterationCounterInitialized = true;
			}
			if (weightPerRoom)
			{
				CalculateWeightChanceByProgressPerRoom();
			}
			else if (weightPerCard)
			{
				CalculateWeightChanceByProgressPerCard();
			}
			TimeLockRooms();
		}
	}

	private void TimeLockRooms()
	{
		if (timeLocksEnabled)
		{
			string[] temporaryWeightPerRoomArrayStore = new string[weightPerRoomArray.Length];
			for (int i = 0; i < weightPerRoomArray.Length; i++)
			{
				temporaryWeightPerRoomArrayStore[i] = weightPerRoomArray[i].ToString();
			}

			for (int i = 0; i < daysUntilRoomUnlock.Length; i++)
			{
				if (daysUntilRoomUnlock[i] >= currentNumberOfDays)
				{
					weightPerRoomArray[i] = 0;
				}
			}

			cardIndexCounter = 0;
			for (int i = 0; i < houseCreator.GetRoomsInThisHouse().Length; i++)
			{
				for (int j = 0; j < houseCreator.GetRoomsInThisHouse()[i].weightOfCardsInRoom.Length; j++)
				{
					houseCreator.houseCardWeightIndex[cardIndexCounter] = weightPerRoomArray[i];
					cardIndexCounter += 1;
				}
			}

			globalCardDrawHandler.SetWeight(weightPerRoomArray);

			for (int i = 0; i < weightPerRoomArray.Length; i++)
			{
				weightPerRoomArray[i] = float.Parse(temporaryWeightPerRoomArrayStore[i]);
			}
		}
	}

	private void CalculateWeightChanceByDayPerRoom()
	{
		shiftSequenceCounter = 0;
		InitializeSetWeightPerRoomArray();
		weightPerRoomArray = new float[weightPerRoomArray.Length];
		for (int i = 0; i < weightPerRoomArray.Length; i++)
		{
			int shiftCalculator = i - shiftSteps;

			if (shiftCalculator < 0)
			{
				shiftCalculator += weightPerRoomArray.Length;
			}
			weightPerRoomArray[i] = float.Parse(setWeightPerRoomArray[shiftCalculator]);
			//print("currentNumberOfDays: " + currentNumberOfDays + " shiftCalculator: " + shiftCalculator + " weightPerRoomArray[" + i + "]: " + weightPerRoomArray[i]);
		}
		//print("---");
	}

	private void CalculateWeightChanceByDayPerCard()
	{
		shiftSequenceCounter = 0;
		InitializeSetWeightPerCardArray();
		weightPerCardArray = new float[weightPerCardArray.Length];
		for (int i = 0; i < weightPerCardArray.Length; i++)
		{
			int shiftCalculator = i - shiftSteps;

			if (shiftCalculator < 0)
			{
				shiftCalculator += weightPerCardArray.Length;
			}
			weightPerCardArray[i] = float.Parse(setWeightPerCardArray[shiftCalculator]);
			//print("currentNumberOfDays: " + currentNumberOfDays + " shiftCalculator: " + shiftCalculator + " weightPerCardArray[" + i + "]: " + weightPerCardArray[i]);
		}
		houseCreator.houseCardWeightIndex = weightPerCardArray;
		//print("---");
	}

	private void CalculateWeightChanceByProgressPerCard()
	{
		for (int i = 0; i < houseCreator.houseCardIsCollectedIndex.Length; i++)
		{
			if (houseCreator.houseCardIsCollectedIndex[i] == true)
			{
				initialCollectedCardsCounter += 1;
			}
		}
		//print("initialCollectedCardsCounter: " + initialCollectedCardsCounter + " shiftCardProgressSteps * shiftStepIterationCounter: " + shiftCardProgressSteps * shiftStepIterationCounter);
		if (initialCollectedCardsCounter >= shiftCardProgressSteps * shiftStepIterationCounter)
		{
			shiftStepIterationCounter += 1;
			InitializeSetWeightPerCardArray();
			for (int i = 0; i < weightPerCardArray.Length; i++)
			{
				int shiftCalculator = i - shiftSteps;
				if (shiftCalculator < 0)
				{
					shiftCalculator += weightPerCardArray.Length;
				}
				weightPerCardArray[i] = float.Parse(setWeightPerCardArray[shiftCalculator]);
				//print("currentNumberOfDays: " + currentNumberOfDays + " shiftStepIterationCounter: " + (shiftStepIterationCounter - 1) + " initialCollectedCardsCounter: " + initialCollectedCardsCounter + " shiftCalculator: " + shiftCalculator + " weightPerCardArray[" + i + "]: " + weightPerCardArray[i]);
			}
			//print("---");
		}
	}

	private void CalculateWeightChanceByProgressPerRoom()
	{
		InitializeSetWeightPerRoomArray();
		//weightPerRoomArray = new float[weightPerRoomArray.Length];
		int previousRoomsCompleted = countRoomsCompleted;
		int[] roomCompletionTracker = new int[houseCreator.GetRoomsInThisHouse().Length];

		for (int i = 0; i < roomCompletionTracker.Length; i++)
		{
			roomCompletionTracker[i] = 0;
		}

		for (int i = 0; i < houseCreator.GetRoomsInThisHouse().Length; i++)
		{
			int totalCardsCollectedInRoom = 0;
			for (int j = 0; j < houseCreator.GetRoomsInThisHouse()[i].indexNumber.Length; j++)
			{
				int[] collectedCardsPerRoomCounter = new int[houseCreator.GetRoomsInThisHouse()[i].indexNumber.Length];
				int currentIndex = houseCreator.GetRoomsInThisHouse()[i].indexNumber[j];
				if (houseCreator.houseCardIsCollectedIndex[currentIndex - 1])
				{
					collectedCardsPerRoomCounter[j] = 1;
				}
				//print(j + " " + collectedCardsPerRoomCounter[j]);
				for (int k = 0; k < collectedCardsPerRoomCounter.Length; k++)
				{
					totalCardsCollectedInRoom += collectedCardsPerRoomCounter[k];
				}
				//print("totalCardsCollectedInRoom: " + totalCardsCollectedInRoom);
			}
			if (totalCardsCollectedInRoom >= houseCreator.GetRoomsInThisHouse()[i].indexNumber.Length)
			{
				if (roomCompletionTracker[i] == 0)
				{
					roomCompletionTracker[i] += 1;
				}
			}
			//print("previousRoomsCompleted: " + previousRoomsCompleted + " countRoomsCompleted: " + countRoomsCompleted);
		}

		countRoomsCompleted = 0;

		for (int i = 0; i < roomCompletionTracker.Length; i++)
		{
			countRoomsCompleted += roomCompletionTracker[i];
		}

		for (int i = 0; i < weightPerRoomArray.Length; i++)
		{
			if (countRoomsCompleted > previousRoomsCompleted)
			{
				int shiftCalculator = i - shiftSteps;
				if (shiftCalculator < 0)
				{
					shiftCalculator += weightPerRoomArray.Length;
				}
				//print("setWeightPerRoomArray[" + shiftCalculator + "]: " + setWeightPerRoomArray[shiftCalculator]);
				weightPerRoomArray[i] = float.Parse(setWeightPerRoomArray[shiftCalculator]);
				//print("currentNumberOfDays: " + currentNumberOfDays + " shiftCalculator: " + shiftCalculator + " weightPerRoomArray[" + i + "]: " + weightPerRoomArray[i]);
			}
		}
		//print("---");
	}
}
