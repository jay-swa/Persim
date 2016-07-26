//#define CONTEXTSIM
#undef CONTEXTSIM

using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class SimulationEngine : MonoBehaviour {
		AICharacterControl characterController;			// AI Character controller attached on the character
		StateSpaceManager stateSpaceManager;			// state space manager attached on the character
		ContextDrivenSimulation contextDrvSimulation;

		void Start() {
			// configure simulation
			SimulationEntity.ReadObjectXml();
			SimulationEntity.Actions = Utils.ReadActionXml();	// TODO ReadActionXml with static Actions
			SimulationEntity.Activities = Utils.ReadActivityXml();	// TODO ReadActivityXml with static Activities
			SimulationEntity.ReadContextXml();

			// TODO AICharacterControl is located on another character
			characterController = GameObject.Find("Ethan").GetComponent<AICharacterControl>();	

			// initialize state space
			// TODO StateSpaceManager is located on another GameObject
			stateSpaceManager = GameObject.Find("Camera").GetComponent<StateSpaceManager>();	
			stateSpaceManager.InitializeStateSpace();

			contextDrvSimulation = new ContextDrivenSimulation (characterController, stateSpaceManager);
			#if CONTEXTSIM
			RunSimulation ();
			#endif
		}

		void Update() { 
			#if CONTEXTSIM
			if (characterController.activityFinished) {
				characterController.activityFinished = false;
				TransitionContext ();
			}
			#endif
		}
			
		// run simulation loop: WITHIN a context
		void RunSimulation() {
			// Run context-driven simulation engine
			contextDrvSimulation.SelectContextActivities ();
			contextDrvSimulation.ScheduleContextActivities ();
			contextDrvSimulation.PerformContextActivity ();
			//contextDrvSimulation.EvaluateStateSpace ();			// moved into TransitionContext()
			//contextDrvSimulation.TransitToNextContext ();			// moved into TransitionContext()
		}

		// transition to the next context if available: BETWEEN contexts
		void TransitionContext() {
			print ("Now " + stateSpaceManager.StateSpaceHistory.Count + " state spaces has stored");
			contextDrvSimulation.EvaluateStateSpace ();
			contextDrvSimulation.TransitToNextContext ();
			if (!SimulationEntity.IsEnd ())
				RunSimulation ();
			else
				Debug.Log ("Simulation Ends");			
		}
	}
}

