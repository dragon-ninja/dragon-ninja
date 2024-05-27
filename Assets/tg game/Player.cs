using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
	[Header("INDEXS")]
	private float moveSpeed;

	private bool knockBackState;

	private float moveMinPos;

	private float moveMaxPos;

	private float lastMoveDirection;

	private Vector3 oldPosition;

	[Header("OBJECTS")]
	public Transform imageObjectTransform;

	public SpriteRenderer spriteHelmet;

	public SpriteRenderer spriteArmor;

	public SpriteRenderer spriteWeapon;

	public SpriteRenderer spriteHorse;

	public SpriteRenderer spriteShadow;

	[Header("SKILL IMAGES")]
	public SpriteRenderer spriteSkillHelmet;

	public SpriteRenderer spriteSkillArmor;

	public SpriteRenderer spriteSkillWeapon;

	public SpriteRenderer spriteSkillHorse;

	[Header("PARTICLES")]
	public ParticleSystem healing;

	public ParticleSystem activePower;

	public ParticleSystem activeShield;

	public ParticleSystem activeSpeed;

	[Header("EFFECTS")]
	public SpriteRenderer effectPower;

	public SpriteRenderer effectShield;

	public SpriteRenderer effectSpeed;

	private Sequence effectPowerSequence;

	private Sequence effectShieldSequence;

	private Sequence effectSpeedSequence;

	private Transform oldEffect;

	private ActiveCallback callPowerEnded;

	private ActiveCallback callShieldEnded;

	private ActiveCallback callSpeedEnded;

	private float timeActivePower;

	private float timeActiveShield;

	private float timeActiveSpeed;

	private Sequence sequence;

	private bool moveJumpActionState;

	public float getLastMoveDirection()
	{
		return lastMoveDirection;
	}

	public void settingSpeed(float s)
	{
		moveSpeed = s;
	}

	public Vector3 getBodyPosition()
	{
		return spriteArmor.transform.position;
	}

	public void settingImage(int helmet, int armor, int weapon, int horse, bool action = true)
	{
		settingHorse(horse);
		settingArmor(armor);
		settingHelmet(helmet);
		settingWeapon(weapon);
		if (action && sequence == null)
		{
			sequence = DOTween.Sequence();
			sequence.Append(spriteHelmet.transform.DOLocalMoveY(-0.05f, 1f).SetRelative(isRelative: true));
			sequence.Append(spriteHelmet.transform.DOLocalMoveY(0.05f, 1f).SetRelative(isRelative: true));
			sequence.SetEase(Ease.InOutExpo);
			sequence.SetLoops(-1);
			sequence.Play();
		}
		resetSkillSprites();
	}

	public void settingHelmet(int index)
	{
		Sprite sprite = Singleton<AssetManager>.Instance.LoadSprite("Equipment/Helmet/" + index.ToString());
		spriteHelmet.sprite = sprite;
	}

	public void settingArmor(int index)
	{
		Sprite sprite = Singleton<AssetManager>.Instance.LoadSprite("Equipment/Armor/" + index.ToString());
		spriteArmor.sprite = sprite;
	}

	public void settingWeapon(int index)
	{
		Sprite sprite = Singleton<AssetManager>.Instance.LoadSprite("Equipment/Weapon/" + index.ToString());
		spriteWeapon.sprite = sprite;
	}

	public void settingHorse(int index)
	{
		Sprite sprite = Singleton<AssetManager>.Instance.LoadSprite("Equipment/Horse/" + index.ToString());
		spriteHorse.sprite = sprite;
	}

	public void settingMoveSize(float min, float max)
	{
		moveMinPos = min;
		moveMaxPos = max;
	}

	public void onParticleHealing()
	{
		healing.gameObject.SetActive(value: true);
		healing.Play();
	}

	public void onActivePowerUp(ActiveCallback callback, float time)
	{
		callPowerEnded = callback;
		timeActivePower = time;
		activePower.gameObject.SetActive(value: true);
		activePower.Play();
		if (effectPowerSequence == null)
		{
			effectPowerSequence = DOTween.Sequence();
		}
		onEffectIcon(effectPower, effectPowerSequence);
	}

	public void onActiveShieldUp(ActiveCallback callback, float time)
	{
		callShieldEnded = callback;
		timeActiveShield = time;
		activeShield.gameObject.SetActive(value: true);
		activeShield.Play();
		if (effectShieldSequence == null)
		{
			effectShieldSequence = DOTween.Sequence();
		}
		onEffectIcon(effectShield, effectShieldSequence);
	}

	public void onActiveSpeedUp(ActiveCallback callback, float time)
	{
		callSpeedEnded = callback;
		timeActiveSpeed = time;
		activeSpeed.gameObject.SetActive(value: true);
		activeSpeed.Play();
		if (effectSpeedSequence == null)
		{
			effectSpeedSequence = DOTween.Sequence();
		}
		onEffectIcon(effectSpeed, effectSpeedSequence);
	}

	public void offActivePowerUp()
	{
		activePower.gameObject.SetActive(value: false);
	}

	public void offActiveShieldUp()
	{
		activeShield.gameObject.SetActive(value: false);
	}

	public void offActiveSpeedUp()
	{
		activeSpeed.gameObject.SetActive(value: false);
	}

	private void Update()
	{
		updateActiveTimer(ref callPowerEnded, ref timeActivePower);
		updateActiveTimer(ref callShieldEnded, ref timeActiveShield);
		updateActiveTimer(ref callSpeedEnded, ref timeActiveSpeed);
	}

	private void updateActiveTimer(ref ActiveCallback callback, ref float time)
	{
		if (callback != null)
		{
			time -= Time.deltaTime;
			if (time <= 0f)
			{
				callback();
				callback = null;
				time = 0f;
			}
		}
	}

	private void onEffectIcon(SpriteRenderer spriteRenderer, Sequence sequence)
	{
		Transform transform = spriteRenderer.transform;
		transform.localScale = new Vector3(0f, 0f, 0f);
		spriteRenderer.color = Color.white;
		transform.DOKill();
		spriteRenderer.DOKill();
		sequence.Kill();
		sequence.Append(transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f).SetEase(Ease.OutBack));
		sequence.Append(spriteRenderer.DOFade(0f, 1f).SetDelay(1f));
		sequence.Play();
		if (oldEffect != null)
		{
			Vector3 localPosition = oldEffect.localPosition;
			oldEffect.localPosition = new Vector3(localPosition.x, localPosition.y, -6f);
		}
		Vector3 localPosition2 = transform.localPosition;
		transform.localPosition = new Vector3(localPosition2.x, localPosition2.y, -7f);
		oldEffect = transform;
	}

	private void flipImage(float direction)
	{
		if (direction < 0f)
		{
			if (imageObjectTransform.localScale.x > 0f)
			{
				imageObjectTransform.localScale = new Vector3(-1f, 1f, 1f);
			}
		}
		else if (imageObjectTransform.localScale.x < 0f)
		{
			imageObjectTransform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	public void move(float direction)
	{
		if (!knockBackState)
		{
			lastMoveDirection = direction;
			Vector3 position = base.transform.position;
			position.x += direction * moveSpeed * Time.deltaTime;
			if (position.x < moveMinPos)
			{
				position.x = moveMinPos;
			}
			else if (position.x > moveMaxPos)
			{
				position.x = moveMaxPos;
			}
			oldPosition = base.transform.position;
			base.transform.position = position;
			flipImage(direction);
			if (!moveJumpActionState)
			{
				moveJumpActionState = true;
				imageObjectTransform.DOLocalJump(new Vector3(0f, 0f, 0f), 0.2f, 1, 0.2f).SetRelative(isRelative: true).SetLoops(-1);
			}
		}
	}

	public void stop()
	{
		imageObjectTransform.DOKill();
		imageObjectTransform.localPosition = new Vector3(0f, 0f, 0f);
		moveJumpActionState = false;
	}

	public void knockBack()
	{
		knockBackState = true;
		base.transform.DOMove(new Vector3((0f - lastMoveDirection) * moveSpeed * 0.05f, 0f, 0f), 0.1f).SetRelative(isRelative: true).OnComplete(delegate
		{
			knockBackState = false;
		});
	}

	public void onDieAnimation()
	{
		stop();
		offActivePowerUp();
		offActiveShieldUp();
		offActiveSpeedUp();
		effectPower.gameObject.SetActive(value: false);
		effectShield.gameObject.SetActive(value: false);
		effectSpeed.gameObject.SetActive(value: false);
		float duration = 2f;
		sequence.Kill();
		spriteHorse.transform.DOMove(new Vector3(-1f, 0.3f, 0f), duration).SetRelative(isRelative: true);
		spriteHorse.transform.DORotateQuaternion(Quaternion.Euler(0f, 0f, 45f), duration).SetRelative(isRelative: true);
		spriteArmor.transform.DOMove(new Vector3(-3f, 0.3f, 0f), duration).SetRelative(isRelative: true);
		spriteArmor.transform.DORotateQuaternion(Quaternion.Euler(0f, 0f, 50f), duration).SetRelative(isRelative: true);
		spriteHelmet.transform.DOMove(new Vector3(-3.35f, 0.1f, 0f), duration).SetRelative(isRelative: true);
		spriteWeapon.transform.DOMove(new Vector3(-2f, 0.1f, 0f), duration).SetRelative(isRelative: true);
		spriteWeapon.transform.DORotateQuaternion(Quaternion.Euler(0f, 0f, 30f), duration).SetRelative(isRelative: true);
		spriteShadow.transform.DOMove(new Vector3(-1.5f, 0f, 0f), duration).SetRelative(isRelative: true);
	}

	public Vector3 getOldPosition()
	{
		return oldPosition;
	}

	public void resetSkillSprites()
	{
		if (!(spriteSkillHelmet == null))
		{
			spriteSkillHelmet.sprite = null;
			spriteSkillArmor.sprite = null;
			spriteSkillWeapon.sprite = null;
			spriteSkillHorse.sprite = null;
			spriteSkillHelmet.color = new Color(1f, 1f, 1f, 0f);
			spriteSkillArmor.color = new Color(1f, 1f, 1f, 0f);
			spriteSkillWeapon.color = new Color(1f, 1f, 1f, 0f);
			spriteSkillHorse.color = new Color(1f, 1f, 1f, 0f);
		}
	}

	public SpriteRenderer getSkillSpriteHead()
	{
		return spriteSkillHelmet;
	}

	public SpriteRenderer getSkillSpriteArmor()
	{
		return spriteSkillArmor;
	}

	public SpriteRenderer getSkillSpriteWeapon()
	{
		return spriteSkillWeapon;
	}

	public SpriteRenderer getSkillSpriteHorse()
	{
		return spriteSkillHorse;
	}
}
