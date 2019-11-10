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
		CreateJson();
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

	void CreateJson()
	{
		{
			cardIndex = new CardIndex[houseCardNumberIndex.Length];

			for (int i = 0; i < houseCardNumberIndex.Length; i++)
			{
				cardIndex[i] = new CardIndex();

				cardIndex[i].indexNum = houseCardNumberIndex[i];
				cardIndex[i].rarity = houseCardRarityIndex[i];
				cardIndex[i].isColleted = houseCardIsCollectedIndex[i];

				if (json == null)
				{
					json = "{\"index\":{";
					json += "\"" + i + "\"" + ":";
					json += JsonUtility.ToJson(cardIndex[i]);
				}
				else
				{
					json += ",\"" + i + "\":" + JsonUtility.ToJson(cardIndex[i]);
				}

			}

			json += "}}";

			System.IO.File.WriteAllText(Application.dataPath + "/CardInfo.json", json);
			//print(Application.dataPath);
		}
	}

}
