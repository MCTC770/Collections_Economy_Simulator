using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseCreator : MonoBehaviour {

	[SerializeField] RoomCreator[] roomsInThisHouse;
	int totalIndexCounter = 0;

	public int[] houseCardNumberIndex;
	public int[] houseCardRarityIndex;
	public float[] houseCardWeightIndex;
	public bool[] houseCardIsCollectedIndex;

	//public string cardInfoStore;

	CardIndex[] cardIndex;
	CardWeightManager cardWeightManager;
	string json;
	bool checkedWeightManagerArrays = false;

	// Use this for initialization
	void Start ()
	{
		cardWeightManager = FindObjectOfType<CardWeightManager>();
		houseCardNumberIndex = new int[houseCardRarityIndex.Length];

		CreateCardIndex();
	}

	public RoomCreator[] GetRoomsInThisHouse()
	{
		return roomsInThisHouse;
	}

	public void ResetCardIndex()
	{
		for (int i = 0; i < houseCardIsCollectedIndex.Length; i++)
		{
			houseCardIsCollectedIndex[i] = false;
		}
	}

	private void CreateCardIndex()
	{
		for (int i = 0; i < roomsInThisHouse.Length; i++)
		{
			roomsInThisHouse[i].SetIndexValue(totalIndexCounter);
			roomsInThisHouse[i].InitializeRoomValues();
			roomsInThisHouse[i].GenerateCardIndex();
			totalIndexCounter = roomsInThisHouse[i].GetIndexValue();
		}

		houseCardNumberIndex = new int[totalIndexCounter];
		houseCardRarityIndex = new int[totalIndexCounter];
		houseCardIsCollectedIndex = new bool[totalIndexCounter];
		houseCardWeightIndex = new float[totalIndexCounter];

		int h = 0;

		for (int j = 0; j < roomsInThisHouse.Length; j++)
		{
			for (int i = 0; i < roomsInThisHouse[j].indexNumber.Length; i++)
			{
				houseCardNumberIndex[h] = roomsInThisHouse[j].indexNumber[i];
				houseCardRarityIndex[h] = roomsInThisHouse[j].rarity[i];
				houseCardIsCollectedIndex[h] = roomsInThisHouse[j].isCollected[i];
				houseCardWeightIndex[h] = roomsInThisHouse[j].weightOfCardsInRoom[i];
				h++;
			}
		}
		h = 0;

		if (!checkedWeightManagerArrays)
		{
			cardWeightManager.SetLengthOfWeightPerCardArray(houseCardNumberIndex);
			cardWeightManager.SetLengthOfWeightPerRoomArray(indexArray: roomsInThisHouse.Length);
			checkedWeightManagerArrays = true;
		}
	}

	public void SetHouseCardWeightIndex()
	{
		int h = 0;
		for (int j = 0; j < roomsInThisHouse.Length; j++)
		{
			for (int i = 0; i < roomsInThisHouse[j].indexNumber.Length; i++)
			{
				houseCardWeightIndex[h] = roomsInThisHouse[j].weightOfCardsInRoom[i];
				h++;
			}
		}
		h = 0;
	}

	public string GetRoomValues()
	{
		string returnedTXT;

		returnedTXT = "Rooms:";
		returnedTXT += System.Environment.NewLine;
		returnedTXT += System.Environment.NewLine;

		for (int i = 0; i < roomsInThisHouse.Length; i++)
		{
			returnedTXT += "Room[" + i + "]: " + roomsInThisHouse[i].name;
			returnedTXT += System.Environment.NewLine;
			returnedTXT += "oneStarCardsInRoom: " + roomsInThisHouse[i].oneStarCardsInRoom;
			returnedTXT += System.Environment.NewLine;
			returnedTXT += "twoStarCardsInRoom: " + roomsInThisHouse[i].twoStarCardsInRoom;
			returnedTXT += System.Environment.NewLine;
			returnedTXT += "threeStarCardsInRoom: " + roomsInThisHouse[i].threeStarCardsInRoom;
			returnedTXT += System.Environment.NewLine;
			returnedTXT += "fourStarCardsInRoom: " + roomsInThisHouse[i].fourStarCardsInRoom;
			returnedTXT += System.Environment.NewLine;
			returnedTXT += "fiveStarCardsInRoom: " + roomsInThisHouse[i].fiveStarCardsInRoom;
			returnedTXT += System.Environment.NewLine;
			returnedTXT += "index:";
			returnedTXT += System.Environment.NewLine;

			for (int j = 0; j < roomsInThisHouse[i].indexNumber.Length; j++)
			{
				returnedTXT += "indexNumber[" + j + "]: " + roomsInThisHouse[i].indexNumber[j] + " rarity:[" + j + "]: " + roomsInThisHouse[i].rarity[j];
				returnedTXT += System.Environment.NewLine;
			}
		}

		return returnedTXT;
	}
}
