using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetObject : MonoBehaviour
{
	public GameObject targetObject;

	public SpriteRenderer petImage;

	public SpriteRenderer shadow;

	public List<Sprite> listShadowImages = new List<Sprite>();

	private PetData petData;

	private float minDistance = 1f;

	private bool moveState;

	private bool moveActionState;

	private IEnumerator objectMoveCoroutine;

	private float timer;

	private Sequence sequence;

	public SpriteRenderer getPetImage()
	{
		return petImage;
	}

	public void settingPetObject(PetData data, GameObject target, bool inventory = false)
	{
		targetObject = target;
		petData = data;
		if (objectMoveCoroutine != null)
		{
			StopCoroutine(objectMoveCoroutine);
			objectMoveCoroutine = null;
		}
		if (sequence != null)
		{
			sequence.Kill();
			sequence = null;
		}
		if ((int)petData.imageIndex == 0)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		objectMoveCoroutine = objectMove();
		switch (petData.type)
		{
		case PetPositionType.TYPE_WALK:
			settingWark();
			break;
		case PetPositionType.TYPE_FLY:
			settingFly(inventory);
			break;
		}
		petImage.sprite = Singleton<AssetManager>.Instance.LoadSprite("Pets/" + data.imageIndex.ToString());
		int num = petData.imageIndex;
		if (num == 21)
		{
			shadow.sprite = listShadowImages[1];
		}
		else
		{
			shadow.sprite = listShadowImages[0];
		}
		if (targetObject != null)
		{
			base.transform.position = new Vector3(target.transform.position.x, -1.03f, 0f);
			StartCoroutine(objectMoveCoroutine);
		}
	}

	private void settingWark()
	{
		petImage.transform.localPosition = new Vector3(0f, 0f, 0f);
	}

	private void settingFly(bool inventory)
	{
		float y = 2f;
		if (inventory)
		{
			y = 1f;
		}
		petImage.transform.localPosition = new Vector3(0f, y, 0f);
		sequence = DOTween.Sequence();
		sequence.Append(petImage.transform.DOLocalMoveY(0.3f, 1f).SetRelative(isRelative: true));
		sequence.Append(petImage.transform.DOLocalMoveY(-0.3f, 1f).SetRelative(isRelative: true));
		sequence.SetLoops(-1);
		sequence.Play();
	}

	private IEnumerator objectMove()
	{
		while (true)
		{
			float num = Mathf.Abs(targetObject.transform.position.x - base.transform.position.x);
			float num2 = (targetObject.transform.position.x > base.transform.position.x) ? 1 : (-1);
			if (!petImage.flipX && num2 < 0f)
			{
				petImage.flipX = true;
			}
			else if (petImage.flipX && num2 > 0f)
			{
				petImage.flipX = false;
			}
			if (num > minDistance)
			{
				float num3 = Mathf.Round(num * (num * 0.01f) * 100f) * 0.01f;
				Vector3 position = base.transform.position + new Vector3(num * num3 * num2, 0f, 0f);
				base.transform.position = position;
				moveState = true;
			}
			else
			{
				moveState = false;
			}
			if (moveState)
			{
				if (!moveActionState && petData.type == PetPositionType.TYPE_WALK)
				{
					petImage.transform.DOKill();
					petImage.transform.DOLocalJump(new Vector3(0f, 0f, 0f), 0.212f, 1, 0.2f).SetRelative(isRelative: true).SetLoops(-1);
					moveActionState = true;
				}
			}
			else if (moveActionState && petData.type == PetPositionType.TYPE_WALK)
			{
				petImage.transform.DOKill();
				petImage.transform.localPosition = new Vector3(0f, 0f, 0f);
				moveActionState = false;
			}
			yield return null;
		}
	}
}
