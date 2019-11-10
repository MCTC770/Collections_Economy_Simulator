using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CsvCreator : MonoBehaviour {

	[SerializeField] string fileNameTotalLog;
	[SerializeField] string fileNameDailyLog;
	[SerializeField] bool totalLog = false;
	[SerializeField] bool dailyLog = false;

	string csvTotal;
	string csvDaily;

	int currentDay = 0;
	int runCounter = 0;
	int simulationCounter = 0;
	int duplicateStarCounter = 0;

	HouseCreator housecreator;
	GlobalCardDrawHandler globalCardDrawHandler;

	// Use this for initialization
	void Start () {
		housecreator = FindObjectOfType<HouseCreator>();
		globalCardDrawHandler = FindObjectOfType<GlobalCardDrawHandler>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InitializeCSV()
	{
		csvTotal = null;
	}

	public void CreateHeader()
	{
		if (totalLog)
		{
			csvTotal += "Simulation,";
			csvTotal += "Days,";
			csvTotal += "Duplicate Stars,";
			csvTotal += "Total Cards,";
			for (int i = 0; i < housecreator.houseCardIsCollectedIndex.Length; i++)
			{
				csvTotal += "Card" + (i + 1);
				if (i < housecreator.houseCardIsCollectedIndex.Length - 1)
				{
					csvTotal += ",";
				}
			}
		}
		if (dailyLog)
		{
			csvDaily += "Simulation,";
			csvDaily += "Days,";
			csvDaily += "Duplicate Stars,";
			csvDaily += "Daily Cards,";
			for (int i = 0; i < housecreator.houseCardIsCollectedIndex.Length; i++)
			{
				csvDaily += "Card" + (i + 1);
				if (i < housecreator.houseCardIsCollectedIndex.Length - 1)
				{
					csvDaily += ",";
				}
			}
		}
	}

	public void CreateCSVString(int[] drawnCardIndex, bool daily)
	{
		if (totalLog && !daily)
		{
			CreateTotalLog(drawnCardIndex);
		}
		if (dailyLog && daily)
		{
			CreateDailyLog(drawnCardIndex);
		}
	}

	private void CreateTotalLog(int[] drawnCardIndex)
	{
		csvTotal += System.Environment.NewLine;

		// Tracks number of simulations
		++runCounter;
		csvTotal += runCounter + ",";

		// Number of days
		csvTotal += globalCardDrawHandler.GetNumberOfDays() + ",";

		// Duplicate star counter
		csvTotal += duplicateStarCounter + ",";

		// Total cards drawn
		int totalDrawnCards = 0;
		for (int i = 0; i < drawnCardIndex.Length; i++)
		{
			totalDrawnCards += drawnCardIndex[i];
		}
		csvTotal += totalDrawnCards + ",";

		// Tracks collected cards
		for (int i = 0; i < drawnCardIndex.Length; i++)
		{
			csvTotal += drawnCardIndex[i].ToString();
			if (i < drawnCardIndex.Length - 1)
			{
				csvTotal += ",";
			}
		}
	}

	private void CreateDailyLog(int[] drawnCardIndex)
	{
		csvDaily += System.Environment.NewLine;

		// Tracks number of simulations
		csvDaily += simulationCounter + ",";

		// Number of days
		csvDaily += currentDay + ",";

		// Duplicate star counter
		csvDaily += duplicateStarCounter + ",";

		// Total cards drawn
		int totalDrawnCards = 0;
		for (int i = 0; i < drawnCardIndex.Length; i++)
		{
			totalDrawnCards += drawnCardIndex[i];
		}
		csvDaily += totalDrawnCards + ",";

		// Tracks collected cards
		for (int i = 0; i < drawnCardIndex.Length; i++)
		{
			csvDaily += drawnCardIndex[i].ToString();
			if (i < drawnCardIndex.Length - 1)
			{
				csvDaily += ",";
			}
		}
	}

	public void CreateCSVFile()
	{
		if (totalLog)
		{
			System.IO.File.WriteAllText(Application.dataPath + "/" + fileNameTotalLog + ".csv", csvTotal);
		}
		if (dailyLog)
		{
			System.IO.File.WriteAllText(Application.dataPath + "/" + fileNameDailyLog + ".csv", csvDaily);
		}
	}

	public void SetDuplicateStarCounter(int stars)
	{
		duplicateStarCounter = stars;
	}

	public void SetCurrentDay(int currentDayInput)
	{
		currentDay = currentDayInput;
	}

	public void SetSimulationCounter(int currentSimulation)
	{
		simulationCounter = currentSimulation;
	}
}
