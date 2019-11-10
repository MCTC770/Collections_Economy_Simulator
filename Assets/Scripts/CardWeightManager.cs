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

	public void TimeLockRooms()
	{
		if (timeLocksEnabled)
		{
			for (int i = 0; i < weightPerRoomArray.Length; i++)
			{
				weightPerRoomArray[i] = int.Parse(setWeightPerRoomArray[i]);
			}

			for (int i = 0; i < daysUntilRoomUnlock.Length; i++)
			{
				if (daysUntilRoomUnlock[i] >= currentNumberOfDays)
				{
					weightPerRoomArray[i] = 0;
				}
			}
			globalCardDrawHandler.SetWeight(weightPerRoomArray);
		}
	}

	void ShiftSequenceHandler()
	{
		if (weightChangeByDays)
		{
			shiftSequenceCounter += 1;
			print("shiftSequenceInDays: " + shiftSequenceInDays + " shiftSequenceCounter: " + shiftSequenceCounter);
			if (shiftSequenceInDays <= shiftSequenceCounter && dayOfLastShift != currentNumberOfDays)
			{
				shiftSequenceCounter -= shiftSequenceInDays;
				dayOfLastShift = currentNumberOfDays;
				if (weightPerRoom)
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
					print("---");
				}
				else if (weightPerCard)
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
						print("currentNumberOfDays: " + currentNumberOfDays + " shiftCalculator: " + shiftCalculator + " weightPerCardArray[" + i + "]: " + weightPerCardArray[i]);
					}
					houseCreator.houseCardWeightIndex = weightPerCardArray;
					print("---");
				}
				TimeLockRooms();
			}
		}

	}

	// Update is called once per frame
	void Update () {
		
	}
}
