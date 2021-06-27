using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movimiento : MonoBehaviour
{
	[SerializeField]
	Transform _destination;

	NavMeshAgent _navMeshAgent;

	// Use this for initialization
	void Start ()
	{
		transform.position = _destination.transform.position;
		_navMeshAgent = this.GetComponent<NavMeshAgent>();
	}
	void Update (){
		SetDestination();
	}

	private void SetDestination()
	{
		if(_destination != null)
		{
			Vector3 targetVector = _destination.transform.position;
			_navMeshAgent.SetDestination(targetVector);
		}
	}
}