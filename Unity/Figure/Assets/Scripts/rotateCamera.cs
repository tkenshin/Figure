using UnityEngine;
using System.Collections;

public class rotateCamera : MonoBehaviour
{
	private const float minCameraAngleX = 310.0f;
	private const float maxCameraAngleX = 20.0f;
	private const float swipeSpeed = 30.0f;
	private const float autoRotateSpeed = 20.0f;

	private Vector3 baseMousePos;
	private bool isMouseDown = false;

	public bool isAutoRotation = true;
	public bool cameraMode = false;

	//public void autoRotation()
	//{
	//	if (isAutoRotation)
	//	{
	//		isAutoRotation = false;

	//	}
	//	else if (!isAutoRotation)
	//	{
	//		isAutoRotation = true;

	//	}

	//}

	//public void modeChange()
	//{
	//	if (cameraMode)
	//	{
	//		cameraMode = false;

	//	}
	//	else if (!cameraMode)
	//	{
	//		cameraMode = true;

	//	}
	//}

	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (isAutoRotation)
		{
			float angleY = transform.eulerAngles.y - Time.deltaTime * autoRotateSpeed;
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, angleY, 0);

		}

		if (cameraMode)
		{

			if ((Input.touchCount == 1 && !isMouseDown) || Input.GetMouseButtonDown(0))
			{
				//			Debug.Log ("touch");
				baseMousePos = Input.mousePosition;
				isMouseDown = true;

			}

			if (Input.GetMouseButtonUp(0))
			{
				isMouseDown = false;

			}

			if (isMouseDown)
			{
				//			Debug.Log ("True");
				Vector3 mousePos = Input.mousePosition;
				Vector3 distanceMousePos = (mousePos - baseMousePos);
				//			Debug.Log ("eulerAngle.x = " + transform.eulerAngles.x + "\n" + "eulerAngle.y = " + transform.eulerAngles.y); 
				float angleX = transform.eulerAngles.x - distanceMousePos.y * swipeSpeed * 0.01f;
				float angleY = transform.eulerAngles.y + distanceMousePos.x * swipeSpeed * 0.01f;
				//			Debug.Log ("angleX = "	 + angleX + "\n" + "angleY = " + angleY);

				if ((angleX >= -10.0f && angleX <= maxCameraAngleX) || (angleX >= minCameraAngleX && angleX <= 370.0f))
				{
					transform.eulerAngles = new Vector3(angleX, angleY, 0);

				}
				else {
					transform.eulerAngles = new Vector3(transform.eulerAngles.x, angleY, 0);

				}
				baseMousePos = mousePos;

			}

		}
	}
}
