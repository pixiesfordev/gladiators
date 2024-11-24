using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSky : MonoBehaviour{
	
	[SerializeField]
	private float rotationSpeed = 0.2f;

	//Rotate the object
    void Update(){
	    transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
    }
}
