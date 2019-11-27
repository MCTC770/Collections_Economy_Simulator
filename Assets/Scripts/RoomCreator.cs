using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pack Config")]
public class RoomCreator : ScriptableObject {

	public float weight;
	public int oneStarCardsInRoom;
	public int twoStarCardsInRoom;
	public int threeStarCardsInRoom;
	public int fourStarCardsInRoom;
	public int fiveStarCardsInRoom;
	public int totalCardsInRoom;

	[Space(12)]

	//public bool roomCompled;

	[Space(12)]

	public int[] indexNumber;
	public int[] rarity;
	public bool[] isCollected;
	public float[] weightOfCardsInRoom;

	int OneStarCardsLeft;
	int TwoStarCardsLeft;
	int ThreeStarCardsLeft;
	int FourStarCardsLeft;
	int FiveStarCardsLeft;

	int cardIndexCounter;
	//int cardsCollectedCounter;

	// Use this for initialization
	void Start ()
	{
		InitializeRoomValues();
	}

	public void InitializeRoomValues()
	{
		totalCardsInRoom = oneStarCardsInRoom + twoStarCardsInRoom + threeStarCardsInRoom + fourStarCardsInRoom + fiveStarCardsInRoom;

		OneStarCardsLeft = oneStarCardsInRoom;
		TwoStarCardsLeft = twoStarCardsInRoom;
		ThreeStarCardsLeft = threeStarCardsInRoom;
		FourStarCardsLeft = fourStarCardsInRoom;
		FiveStarCardsLeft = fiveStarCardsInRoom;

		indexNumber = new int[totalCardsInRoom];
		rarity = new int[totalCardsInRoom];
		isCollected = new bool[totalCardsInRoom];
		weightOfCardsInRoom = new float[totalCardsInRoom];
	}

	public void SetIndexValue(int indexCounterReceived)
	{
		cardIndexCounter = indexCounterReceived;
	}

	public int GetIndexValue()
	{
		return cardIndexCounter;
	}

	public void GenerateCardIndex()
	{
		for (int i = 0; i < totalCardsInRoom; i++)
		{
			cardIndexCounter += 1;
			indexNumber[i] = cardIndexCounter;

			if (OneStarCardsLeft > 0)
			{
				rarity[i] = 1;
				OneStarCardsLeft -= 1;
			}
			else if (TwoStarCardsLeft > 0)
			{
				rarity[i] = 2;
				TwoStarCardsLeft -= 1;
			}
			else if (ThreeStarCardsLeft > 0)
			{
				rarity[i] = 3;
				ThreeStarCardsLeft -= 1;
			}
			else if (FourStarCardsLeft > 0)
			{
				rarity[i] = 4;
				FourStarCardsLeft -= 1;
			}
			else if (FiveStarCardsLeft > 0)
			{
				rarity[i] = 5;
				FiveStarCardsLeft -= 1;
			}

			isCollected[i] = false;
			weightOfCardsInRoom[i] = weight;
		}
	}

	public void SetWeightOfCardsInRoom()
	{
		for (int i = 0; i < weightOfCardsInRoom.Length; i++)
		{
			weightOfCardsInRoom[i] = weight;
		}
	}

	/*public void CheckForRoomCompletion()
	{
		roomCompled = false;
		for (int i = 0; i < isCollected.Length; i++)
		{
			if (isCollected[i] == true)
			{
				cardsCollectedCounter += 1;
			}
			if (cardsCollectedCounter == isCollected.Length)
			{
				roomCompled = true;
			}
			Debug.Log("cardsCollectedCounter: " + cardsCollectedCounter + " isCollected.Length: " + isCollected.Length);
		}
	}*/
}
