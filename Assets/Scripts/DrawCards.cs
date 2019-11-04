using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCards : MonoBehaviour {

	//[SerializeField] [Tooltip("Lowest to highest rarity. The higher the number, the higher the rarity.")] float[] RarityPerCard;
	public float[] RarityPerCard;
	[SerializeField] int cardDrawCount = 100;
	float[] accumulatedCardRarities;
	float raritiesTotal;
	float randomNumber;
	float OneStarChance;
	float drawnCard;
	int rarity;
	int j;

	private void Start()
	{
		for (int i = 0; i < RarityPerCard.Length; i++)
		{
			raritiesTotal += RarityPerCard[i];
		}
	}

	private void Update()
	{
		if (cardDrawCount > 0)
		{
			DrawARarity();
			cardDrawCount -= 1;
		} else
		{
			cardDrawCount = 0;
		}
	}

	public void SetCardRarities (float Card1Star, float Card2Stars, float Card3Stars, float Card4Stars, float Card5Stars)
	{
		RarityPerCard[0] = Card1Star;
		RarityPerCard[1] = Card2Stars;
		RarityPerCard[2] = Card3Stars;
		RarityPerCard[3] = Card4Stars;
		RarityPerCard[4] = Card5Stars;
	}

	// Use this for initialization
	public int DrawARarity () {

		raritiesTotal = 0;

		for (int i = 0; i < RarityPerCard.Length; i++)
		{
			raritiesTotal += RarityPerCard[i];
		}

		accumulatedCardRarities = new float[RarityPerCard.Length];
		randomNumber = Random.Range(0f, 1f);

		for (int i = 0; i < RarityPerCard.Length; i++)
		{
			if (i == 0)
			{
				accumulatedCardRarities[i] = RarityPerCard[i];
			} else
			{
				accumulatedCardRarities[i] = (RarityPerCard[i] + accumulatedCardRarities[i-1]);
			}
		}

		bool cardDrawn = false;
		j = 0;

		while (!cardDrawn)
		{
			if (randomNumber <= (accumulatedCardRarities[j] / raritiesTotal))
			{
				drawnCard = RarityPerCard[j];
				rarity = j + 1;
				cardDrawn = true;
			} else
			{
				j++;
				if (j >= RarityPerCard.Length)
				{
					j = 4;
					cardDrawn = true;
				}
			}
		}
		return rarity;
	}
}
