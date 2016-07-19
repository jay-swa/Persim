﻿using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class StateSpaceManager : MonoBehaviour {
		public TimeSpan startTime;
		public List<DataRecord> dataset;
		List<StateSpace> stateSpaceHistory;

		// Use this for initialization
		void Start () {
			startTime = new TimeSpan(0, 8, 20, 0, 0);    // Simulation start time
			dataset = new List<DataRecord>();
			stateSpaceHistory = new List<StateSpace> ();
		}

		public void AddDataRecord(TimeSpan time, string objName, string status) {
			dataset.Add(new DataRecord(time, objName, status));
			print(time + ", " + objName + ", " + status);
		}

		public void PrintDataset() {
			foreach (DataRecord record in dataset) {
				record.Print(); 
			}
		}

		public void SaveData() {
			using (System.IO.StreamWriter file = 
				new System.IO.StreamWriter(@"Assets/Files/dataset.txt")) {
				foreach (DataRecord record in dataset) {
					file.WriteLine(record.ToString());
				}
			}
		}

		// update state space
		public void UpdateStateSpace(TimeSpan time, int objId, string newStatus) {			
			StateSpace newStateSpace = GetLatestStateSpace ();
			newStateSpace.UpdateObjectStatus (objId, newStatus);
		}

		// return the latest state space
		public StateSpace GetLatestStateSpace() {
			return stateSpaceHistory [stateSpaceHistory.Count];
		}			

		void OnApplicationQuit() {
			print("Printing dataset...");
			PrintDataset();
			SaveData();
			print("Data saved to file");
		}
	}
}