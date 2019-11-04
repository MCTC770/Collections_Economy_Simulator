using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pack Config")]
public class PackCreator : ScriptableObject {

	float totalNumberOfCardsInPack;
	[SerializeField] int numberOf1StarCards;
	[SerializeField] int numberOf2StarCards;
	[SerializeField] int numberOf3StarCards;
	[SerializeField] int numberOf4StarCards;
	[SerializeField] int numberOf5StarCards;
	[SerializeField] DrawCards cardDrawer;
	float one;
	float two;
	float three;
	float four;
	float five;
	int currentDraw;

	// Use this for initialization
	void Start () {

		one = 63;
		two = 20;
		three = 10;
		four = 5;
		five = 2;

		totalNumberOfCardsInPack = numberOf1StarCards + numberOf2StarCards + numberOf3StarCards + numberOf4StarCards + numberOf5StarCards;
	}
	
	// Update is called once per frame
	void Update ()
	{
		GetCardValues();
		if (numberOf1StarCards > 0)
		{
			cardDrawer.SetCardRarities(one, two, three, four, five);
			currentDraw = cardDrawer.DrawARarity();
			numberOf1StarCards -= 1;
		}
		if (numberOf2StarCards > 0)
		{
			one = 0;
			cardDrawer.SetCardRarities(one, two, three, four, five);
			currentDraw = cardDrawer.DrawARarity();
			numberOf2StarCards -= 1;
		}
		if (numberOf3StarCards > 0)
		{
			one = 0;
			two = 0;
			cardDrawer.SetCardRarities(one, two, three, four, five);
			currentDraw = cardDrawer.DrawARarity();
			numberOf3StarCards -= 1;
		}
		if (numberOf4StarCards > 0)
		{
			one = 0;
			two = 0;
			three = 0;
			cardDrawer.SetCardRarities(one, two, three, four, five);
			currentDraw = cardDrawer.DrawARarity();
			numberOf4StarCards -= 1;
		}
		if (numberOf5StarCards > 0)
		{
			one = 0;
			two = 0;
			three = 0;
			four = 0;
			cardDrawer.SetCardRarities(one, two, three, four, five);
			currentDraw = cardDrawer.DrawARarity();
			numberOf5StarCards -= 1;
		}
		Debug.Log(currentDraw);
	}

	private void GetCardValues()
	{
		one = 63;
		two = 20;
		three = 10;
		four = 5;
		five = 2;
	}
}
