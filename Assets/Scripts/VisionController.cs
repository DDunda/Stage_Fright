using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionController : MonoBehaviour
{
	public Camera camera;
	public Transform cursor;
	public GameObject visionObject;

	void Update()
	{
		if(camera.gameObject.activeInHierarchy && camera.enabled)
		{
			visionObject.SetActive(true);
			Vector3 pos = camera.ScreenToWorldPoint(Input.mousePosition);
			pos.z = 0;
			cursor.position = pos;
		} else
		{
			visionObject.SetActive(false);
		}
	}
}
