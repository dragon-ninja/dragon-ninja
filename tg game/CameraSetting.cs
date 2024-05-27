using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
	private enum CameraMoveState
	{
		STATE_LEFT,
		STATE_RIGHT
	}

	public float gameWidth;

	public float gameHeight;

	public int unitSize;

	public Camera mainCamera;

	public float playerMaxDistance;

	private float cameraWidthSize;

	private float cameraHeightSize;

	private float standardXpos;

	private float oldPer;

	private bool moveState;

	private bool changeStandardState;

	private float changePosition;

	private CameraMoveState cameraMoveState = CameraMoveState.STATE_RIGHT;

	private void Start()
	{
		float num = gameWidth / (float)unitSize;
		float num2 = gameHeight / (float)unitSize;
		float num3 = 0f;
		float num4 = gameWidth / gameHeight;
		float num5 = (float)Screen.width / (float)Screen.height;
		num3 = ((!(num4 <= num5)) ? (num / num5) : num2);
		mainCamera.orthographicSize = num3 / 2f;
		Vector3 vector = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
		cameraWidthSize = vector.x;
		cameraHeightSize = vector.y;
	}

	public void setPlayerXpos(float x, float startWidthSize)
	{
		standardXpos = x;
		base.transform.position = new Vector3(startWidthSize, 0f, -10f);
	}

	public void setMoveState(float direction)
	{
		moveState = true;
		CameraMoveState cameraMoveState = (direction > 0f) ? CameraMoveState.STATE_RIGHT : CameraMoveState.STATE_LEFT;
		if (cameraMoveState != this.cameraMoveState)
		{
			this.cameraMoveState = cameraMoveState;
			standardXpos *= -1f;
			changeStandardState = true;
			changePosition = 0f;
		}
	}

	public void moveCamera(float startWidthSize, float endWidthSize, Vector3 playerPosition)
	{
		
		Vector3 position = base.transform.position;
		Vector3 vector = position + new Vector3(standardXpos, 0f, 0f);
		float num = Mathf.Abs(playerPosition.x - vector.x);
		float num2 = (!(vector.x > playerPosition.x)) ? 1 : (-1);
		float num3 = Mathf.Round(num * (num * 0.01f) * 100f) * 0.01f;
		Vector3 vector2 = position + new Vector3(num * num3 * num2, 0f, 0f);
		if (vector2.x - cameraWidthSize < startWidthSize)
		{
			vector2.x = startWidthSize + cameraWidthSize;
		}
		else if (vector2.x + cameraWidthSize > endWidthSize)
		{
			vector2.x = endWidthSize - cameraWidthSize;
		}
		base.transform.position = vector2;
	}

	public void stopCameraMove()
	{
		moveState = false;
	}

	public float getCameraWidth()
	{
		return cameraWidthSize;
	}

	public float getCameraHeight()
	{
		return cameraHeightSize;
	}

	public void onDieAnimation()
	{
		base.transform.DOMove(new Vector3(-2.5f, 0f, 0f), 1f).SetRelative(isRelative: true);
		StartCoroutine(zoomCamera());
	}

	private IEnumerator zoomCamera()
	{
		while (true)
		{
			float orthographicSize = mainCamera.orthographicSize;
			mainCamera.orthographicSize = orthographicSize - Time.deltaTime * 25f;
			if (!(mainCamera.orthographicSize <= 4f))
			{
				yield return null;
				continue;
			}
			break;
		}
	}
}
