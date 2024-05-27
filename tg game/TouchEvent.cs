using UnityEngine;
using UnityEngine.EventSystems;

public class TouchEvent
{
	private touchDelegate touchBegan;

	private touchDelegate touchMoved;

	private touchDelegate touchEnded;

	private TouchPhase touchState;

	private Vector2 previousPosition = Vector2.zero;

	private bool touchEnabled = true;

	public void setEnabled(bool s)
	{
		touchEnabled = s;
	}

	public void setTouchBegan(touchDelegate began)
	{
		touchBegan = began;
	}

	public void setTouchMoved(touchDelegate moved)
	{
		touchMoved = moved;
	}

	public void setTouchEnded(touchDelegate ended)
	{
		touchEnded = ended;
	}

	public void touchUpdate()
	{
		if (touchEnabled)
		{
			touchMouse();
		}
	}

	private void touchMouse()
	{
		touchState = TouchPhase.Canceled;
		if (Input.GetMouseButtonDown(0))
		{
			//Debug.Log("touchMouse 单击");
			touchState = TouchPhase.Began;
		}
		else if (Input.GetMouseButton(0))
		{
			touchState = TouchPhase.Moved;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			touchState = TouchPhase.Ended;
		}
		//UCODESHOP TODO
		if (!EventSystem.current.IsPointerOverGameObject() && touchState != TouchPhase.Canceled)
		{
			//Debug.Log("检测到touchMouse 单击");
			touchHitCheck(UnityEngine.Input.mousePosition);
		}
	}

	private void touchDevice()
	{
		if (UnityEngine.Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(0))
		{
			touchState = UnityEngine.Input.GetTouch(0).phase;
			Debug.Log("编辑器下执行否");
			touchHitCheck(UnityEngine.Input.GetTouch(0).position);
		}
	}

	private void touchHitCheck(Vector2 pos)
	{
		Vector2 vector = (Vector2)Camera.main.ScreenToWorldPoint(pos);
		Ray ray = Camera.main.ScreenPointToRay(pos);
		Ray2D ray2D = new Ray2D(ray.origin, ray.direction);
		RaycastHit2D[] hits = Physics2D.RaycastAll(ray2D.origin, ray2D.direction, 999999f);
		switch (touchState)
		{
		case TouchPhase.Began:
			if (touchBegan != null)
			{
				touchBegan(hits, getTouchPositionObject(pos));
			}
			break;
		case TouchPhase.Moved:
		case TouchPhase.Stationary:
			if (touchMoved != null)
			{
				touchMoved(hits, getTouchPositionObject(pos));
			}
			break;
		case TouchPhase.Ended:
		case TouchPhase.Canceled:
			if (touchEnded != null)
			{
				touchEnded(hits, getTouchPositionObject(pos));
			}
			break;
		}
		previousPosition = pos;
	}

	private TouchPosition getTouchPositionObject(Vector2 pos)
	{
		return new TouchPosition
		{
			position = pos,
			previousPosition = previousPosition
		};
	}
}
