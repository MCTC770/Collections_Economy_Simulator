using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardWeightManager : MonoBehaviour {

	[SerializeField] bool weightPerHouse = true;
	[SerializeField] bool weightPerCard;
	[Space(12)]
	[SerializeField] float[] weightPerHouseArray;
	[SerializeField] float[] weightPerCardArray;
	[Space(12)]
	[SerializeField] bool weightChangeByTime = true;
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
	string[] setWeightPerHouseArray;
	GlobalCardDrawHandler globalCardDrawHandler;

	// Use this for initialization
	void Start () {
		globalCardDrawHandler = FindObjectOfType<GlobalCardDrawHandler>();
		globalCardDrawHandler.SetWeight(weightPerHouseArray);

		setWeightPerHouseArray = new string[weightPerHouseArray.Length];
		for (int i = 0; i < weightPerHouseArray.Length; i++)
		{
			setWeightPerHouseArray[i] = weightPerHouseArray[i].ToString();
		}

		if (weightPerHouse == true)
		{
			weightPerCard = false;
		}
		else
		{
			weightPerCard = true;
		}

		if (weightChangeByTime == true)
		{
			weightChangeByProgress = false;
		}
		else
		{
			weightChangeByProgress = true;
		}

	}

	public void SetCurrentNumberOfDays(int currentDayCount)
	{
		currentNumberOfDays = currentDayCount;
	}

	public void TimeLockRooms()
	{
		if (timeLocksEnabled)
		{
			for (int i = 0; i < weightPerHouseArray.Length; i++)
			{
				weightPerHouseArray[i] = int.Parse(setWeightPerHouseArray[i]);
			}

			for (int i = 0; i < daysUntilRoomUnlock.Length; i++)
			{
				if (daysUntilRoomUnlock[i] >= currentNumberOfDays)
				{
					weightPerHouseArray[i] = 0;
				}
			}

			globalCardDrawHandler.SetWeight(weightPerHouseArray);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
