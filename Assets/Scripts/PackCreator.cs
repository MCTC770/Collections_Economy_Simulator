using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pack Config")]
public class PackCreator : ScriptableObject {

	int totalNumberOfCardsInPack;
	public int numberOf1StarCards;
	public int numberOf2StarCards;
	public int numberOf3StarCards;
	public int numberOf4StarCards;
	public int numberOf5StarCards;
	public DrawCards cardDrawer;
	
	void Start()
	{
		totalNumberOfCardsInPack = numberOf1StarCards + numberOf2StarCards + numberOf3StarCards + numberOf4StarCards + numberOf5StarCards;
	}

}
