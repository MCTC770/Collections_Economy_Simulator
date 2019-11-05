using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCardDrawHandler : MonoBehaviour {

	[SerializeField] float[] setCardRarities = new float[5];
	[SerializeField] PackCreator[] packsDrawn;

	DrawCards cardDrawer;
	CardManager cardManager;

	int[] cardsOfRarityInPack;
	float[] defaultCardRarities;

	int rarityOfCurrentlyDrawnCard;

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
		for (int i = 0; i < packsDrawn.Length; i++)
		{
			cardsOfRarityInPack[0] = packsDrawn[i].numberOf1StarCards;
			cardsOfRarityInPack[1] = packsDrawn[i].numberOf2StarCards;
			cardsOfRarityInPack[2] = packsDrawn[i].numberOf3StarCards;
			cardsOfRarityInPack[3] = packsDrawn[i].numberOf4StarCards;
			cardsOfRarityInPack[4] = packsDrawn[i].numberOf5StarCards;

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
