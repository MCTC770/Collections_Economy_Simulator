using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CsvCreator : MonoBehaviour
{

	[SerializeField] string fileNameTotalLog;
	[SerializeField] string fileNameDailyLog;
	[SerializeField] string fileNamesettingsLog;
	[Space (12)]
	[SerializeField] bool totalLog = false;
	[SerializeField] bool dailyLog = false;
	[SerializeField] bool settingsLog = false;
	[Space(12)]
	[SerializeField] bool roomCompletionInfo = false;
	[SerializeField] bool perCardInfo = false;

	string csvTotal;
	string csvDaily;
	string currentTime;

	int currentDay = 0;
	int runCounter = 0;
	int simulationCounter = 0;
	int duplicateStarCounter = 0;

	int[] roomCompletionTracker;
	int[] roomCompletionDay;

	HouseCreator houseCreator;
	GlobalCardDrawHandler globalCardDrawHandler;
	CardWeightManager cardWeightManager;

	// Use this for initialization
	void Start()
	{
		currentTime = System.DateTime.Now.ToString();
		currentTime = currentTime.Replace("/", "-");
		currentTime = currentTime.Replace(":", "-");

		houseCreator = FindObjectOfType<HouseCreator>();
		globalCardDrawHandler = FindObjectOfType<GlobalCardDrawHandler>();
		cardWeightManager = FindObjectOfType<CardWeightManager>();

		roomCompletionTracker = new int[houseCreator.GetRoomsInThisHouse().Length];
		roomCompletionDay = new int[houseCreator.GetRoomsInThisHouse().Length];

		for (int i = 0; i < roomCompletionTracker.Length; i++)
		{
			roomCompletionTracker[i] = 0;
			roomCompletionDay[i] = 0;
		}

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void InitializeCSV()
	{
		csvTotal = null;
	}

	public void CreateHeader()
	{
		if (settingsLog)
		{
			CreateSettingsLog();
		}
		if (totalLog)
		{
			csvTotal += "Simulation,";
			csvTotal += "Days,";
			csvTotal += "Duplicate Stars,";
			csvTotal += "Total Cards,";
			csvTotal += "Avg Cards/Day,";
			csvTotal += "Unique Cards";

			if (roomCompletionInfo)
			{
				csvTotal += ",";

				for (int i = 0; i < houseCreator.GetRoomsInThisHouse().Length; i++)
				{
					csvTotal += "Room " + (i + 1) + " Day";
					if (i < houseCreator.GetRoomsInThisHouse().Length - 1)
					{
						csvTotal += ",";
					}
				}
			}

			if (perCardInfo)
			{
				csvTotal += ",";

				for (int i = 0; i < houseCreator.houseCardIsCollectedIndex.Length; i++)
				{
					csvTotal += "Card " + (i + 1);
					if (i < houseCreator.houseCardIsCollectedIndex.Length - 1)
					{
						csvTotal += ",";
					}
				}
			}
		}
		if (dailyLog)
		{
			csvDaily += "Simulation,";
			csvDaily += "Days,";
			csvDaily += "Duplicate Stars,";
			csvDaily += "Daily Cards";

			if (roomCompletionInfo)
			{
				csvDaily += ",";

				for (int i = 0; i < houseCreator.GetRoomsInThisHouse().Length; i++)
				{
					csvDaily += "Room " + (i + 1) + " Day";
					if (i < houseCreator.GetRoomsInThisHouse().Length - 1)
					{
						csvDaily += ",";
					}
				}
			}


			if (perCardInfo)
			{
				csvDaily += ",";

				for (int i = 0; i < houseCreator.houseCardIsCollectedIndex.Length; i++)
				{
					csvDaily += "Card" + (i + 1) + ",";
				}
				for (int i = 0; i < houseCreator.houseCardIsCollectedIndex.Length; i++)
				{
					csvDaily += "Card" + (i + 1) + " Daily";
					if (i < houseCreator.houseCardIsCollectedIndex.Length - 1)
					{
						csvDaily += ",";
					}
				}
			}
		}
	}

	public void CreateCSVString(int[] drawnCardIndexTotal, int[] drawnCardIndexDaily, bool daily)
	{
		if (totalLog && !daily)
		{
			CreateTotalLog(drawnCardIndexTotal);
		}
		if (dailyLog && daily)
		{
			CreateDailyLog(drawnCardIndexTotal, drawnCardIndexDaily);
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

		// Average cards drawn
		csvTotal += ((float)totalDrawnCards / (float)globalCardDrawHandler.GetNumberOfDays()) + ",";

		// Total unique cards collected
		int totalUniqueCardsDrawn = 0;
		for (int i = 0; i < drawnCardIndex.Length; i++)
		{
			if (drawnCardIndex[i] != 0)
			{
				++totalUniqueCardsDrawn;
			}
		}
		csvTotal += totalUniqueCardsDrawn;

		if (roomCompletionInfo)
		{
			// Room completion day
			csvTotal += ",";

			for (int i = 0; i < houseCreator.GetRoomsInThisHouse().Length; i++)
			{
				csvTotal += roomCompletionDay[i];
				if (i < houseCreator.GetRoomsInThisHouse().Length - 1)
				{
					csvTotal += ",";
				}
			}
		}

		if (perCardInfo)
		{
			csvTotal += ",";

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
	}

	private void CreateDailyLog(int[] drawnCardIndexTotal, int[] drawnCardIndexDaily)
	{
		csvDaily += System.Environment.NewLine;

		// Tracks number of simulations
		csvDaily += simulationCounter + ",";

		// Number of days
		csvDaily += currentDay + ",";

		// Duplicate star counter
		csvDaily += duplicateStarCounter + ",";

		// Daily cards drawn
		int totalDrawnCards = 0;
		for (int i = 0; i < drawnCardIndexDaily.Length; i++)
		{
			totalDrawnCards += drawnCardIndexDaily[i];
		}
		csvDaily += totalDrawnCards;

		if (roomCompletionInfo)
		{
			csvDaily += ",";

			// Room completion day
			for (int i = 0; i < houseCreator.GetRoomsInThisHouse().Length; i++)
			{
				csvDaily += roomCompletionDay[i];
				if (i < houseCreator.GetRoomsInThisHouse().Length - 1)
				{
					csvDaily += ",";
				}
			}
		}

		if (perCardInfo)
		{
			csvDaily += ",";

			// Tracks collected cards accumulated
			for (int i = 0; i < drawnCardIndexTotal.Length; i++)
			{
				csvDaily += drawnCardIndexTotal[i].ToString() + ",";
			}

			// Tracks collected cards daily
			for (int i = 0; i < drawnCardIndexDaily.Length; i++)
			{
				csvDaily += drawnCardIndexDaily[i].ToString();
				if (i < drawnCardIndexDaily.Length - 1)
				{
					csvDaily += ",";
				}
			}
		}
	}

	public void TrackRoomCompletion()
	{
		for (int i = 0; i < houseCreator.GetRoomsInThisHouse().Length; i++)
		{
			int totalCardsCollectedInRoom = 0;
			for (int j = 0; j < houseCreator.GetRoomsInThisHouse()[i].indexNumber.Length; j++)
			{
				int[] collectedCardsPerRoomCounter = new int[houseCreator.GetRoomsInThisHouse()[i].indexNumber.Length];
				int currentIndex = houseCreator.GetRoomsInThisHouse()[i].indexNumber[j];
				if (houseCreator.houseCardIsCollectedIndex[currentIndex - 1])
				{
					collectedCardsPerRoomCounter[j] = 1;
				}
				//print(j + " " + collectedCardsPerRoomCounter[j]);
				for (int k = 0; k < collectedCardsPerRoomCounter.Length; k++)
				{
					totalCardsCollectedInRoom += collectedCardsPerRoomCounter[k];
				}
				//print("totalCardsCollectedInRoom: " + totalCardsCollectedInRoom);
			}
			if (totalCardsCollectedInRoom >= houseCreator.GetRoomsInThisHouse()[i].indexNumber.Length)
			{
				if (roomCompletionTracker[i] == 0)
				{
					roomCompletionTracker[i] += 1;
					roomCompletionDay[i] = currentDay;
					//print("roomCompletionTracker[" + i + "]: " + roomCompletionTracker[i]);
				}
			}
			//print("previousRoomsCompleted: " + previousRoomsCompleted + " countRoomsCompleted: " + countRoomsCompleted);
		}

		/*int countRoomsCompleted = 0;

		for (int i = 0; i < roomCompletionTracker.Length; i++)
		{
			countRoomsCompleted += roomCompletionTracker[i];
			//print("countRoomsCompleted: " + countRoomsCompleted + " roomCompletionTracker["+i+"]: " + roomCompletionTracker[i]);
		}
		//print("countRoomsCompleted After: " + countRoomsCompleted);*/
	}

	public void CreateCSVFile()
	{
		if (totalLog)
		{
			System.IO.File.WriteAllText(Application.dataPath + "/CSVs/" + fileNameTotalLog + " - " + currentTime + ".csv", csvTotal);
		}
		if (dailyLog)
		{
			System.IO.File.WriteAllText(Application.dataPath + "/CSVs/" + fileNameDailyLog + " - " + currentTime + ".csv", csvDaily);
		}
		Debug.Log("Simulation(s) complete!");
	}

	public void CreateSettingsLog()
	{
		string logTXT;

		logTXT = globalCardDrawHandler.GetSettingsValues();
		logTXT += System.Environment.NewLine;
		logTXT += System.Environment.NewLine;
		logTXT += System.Environment.NewLine;
		logTXT += cardWeightManager.GetSettingsValues();
		logTXT += System.Environment.NewLine;
		logTXT += System.Environment.NewLine;
		logTXT += System.Environment.NewLine;
		logTXT += globalCardDrawHandler.GetPackValues();
		logTXT += System.Environment.NewLine;
		logTXT += System.Environment.NewLine;
		logTXT += System.Environment.NewLine;
		logTXT += houseCreator.GetRoomValues();

		System.IO.File.WriteAllText(Application.dataPath + "/CSVs/" + fileNamesettingsLog + " - " + currentTime + ".txt", logTXT);
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