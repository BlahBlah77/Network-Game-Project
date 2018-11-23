using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {

	// Sets up varible to track players position 
	public Transform player;

	// LateUpdate function, Camera above the player will follow their transformation and rotation
	void LateUpdate ()
	{
		Vector3 newPosition = player.position;
		newPosition.y = transform.position.y;
		transform.position = newPosition;

		// rotation
		transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
	}

}
