using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CharacterController : MonoBehaviour 
{
	Animator animator;
	public Queue<ActionInfo> actionQueue;
	public ActionInfo nextAction;
	public NavMeshAgent navAgent;
	public bool actionStarted = false;
	public bool activityFinished = false;

	// Use this for initialization
	void Start() 
	{	
		animator = GetComponentInChildren<Animator>();
		animator.SetBool("actionToWalk", true);
        
        actionQueue = new Queue<ActionInfo>();
		actionQueue.Enqueue(new ActionInfo("Open door", 1, new Vector3(3, 1, 0), "Door"));
		actionQueue.Enqueue(new ActionInfo("Lie down", 2, new Vector3(-4, 0, 0), "Cube"));
       
		if (actionQueue.Count > 0) {
			nextAction = actionQueue.Dequeue();
		}

		navAgent = GetComponent<NavMeshAgent>();
		navAgent.destination = nextAction.location;
        
    }
	
	// Update is called once per frame
	void Update() 
	{
		if (nextAction != null) {
			if (Vector3.Distance(transform.position, nextAction.location) < 1 && !actionStarted) {
				actionStarted = true;
				animator.SetInteger("nextAction", nextAction.id);
			}
		}

	}
	
	void OnGUI()
	{
		GUI.Label (new Rect (10, 5,1000, 20), transform.position.ToString());
	}
	
}
