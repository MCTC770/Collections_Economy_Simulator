using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CsvCreator : MonoBehaviour {

	[SerializeField] string fileName;
	string csv;

	int runCounter = 0;

	HouseCreator housecreator;

	// Use this for initialization
	void Start () {
		housecreator = FindObjectOfType<HouseCreator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InitializeCSV()
	{
		csv = null;
	}

	public void CreateHeader()
	{
		csv += "Simulation,";
		for (int i = 0; i < housecreator.houseCardIsCollectedIndex.Length; i++)
		{
			csv += "Card" + (i + 1);
			if (i < housecreator.houseCardIsCollectedIndex.Length - 1)
			{
				csv += ",";
			}
		}
	}

	public void CreateCSVString(int[] drawnCardIndex)
	{
		csv += System.Environment.NewLine;

		++runCounter;
		csv += +runCounter + ",";

		for (int i = 0; i < drawnCardIndex.Length; i++)
		{
			csv += drawnCardIndex[i].ToString();
			if (i < drawnCardIndex.Length - 1)
			{
				csv += ",";
			}
		}
	}

	public void CreateCSVFile()
	{
		System.IO.File.WriteAllText(Application.dataPath + "/" + fileName + ".csv", csv);
	}
}
