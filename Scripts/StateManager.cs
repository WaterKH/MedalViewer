using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {

	public enum States { Viewer, Presenter };
	public States State;

	// Use this for initialization
	void Start () 
	{
		State = States.Viewer;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
