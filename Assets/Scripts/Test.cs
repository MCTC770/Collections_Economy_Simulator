using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	[SerializeField] float one;
	[SerializeField] float two;
	[SerializeField] float three;
	[SerializeField] float four;
	[SerializeField] float five;
	CardIndex[] cardIndex;
	//CardIndex[] cardIndexArray;
	HouseCreator houseCreator;
	string json;

	private void Start()
	{
		houseCreator = FindObjectOfType<HouseCreator>();
		cardIndex = new CardIndex[houseCreator.houseCardNumberIndex.Length];

		for (int i = 0; i < houseCreator.houseCardNumberIndex.Length; i++)
		{
			cardIndex[i] = new CardIndex();

			cardIndex[i].indexNum = houseCreator.houseCardNumberIndex[i];
			cardIndex[i].rarity = houseCreator.houseCardRarityIndex[i];
			cardIndex[i].isColleted = houseCreator.houseCardIsCollectedIndex[i];

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
		print(Application.dataPath);
	}

	// Use this for initialization
	public void CardRarityModifier () {
		FindObjectOfType<DrawCards>().SetCardRarities(one, two, three, four, five);
		FindObjectOfType<DrawCards>().DrawARarity();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
