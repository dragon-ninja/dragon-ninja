using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightScene : MonoBehaviour
{
	[Header("OBJECTS")]
	public GameObject mainCamera;

	public GameObject gameScene;

	public GameObject player;

	public GameObject skillKnight;

	public GameObject thunder;

	public GameObject knight;

	public GameObject endCircle;

	public List<GameObject> listLights;

	[Header("IMAGES")]
	public List<Sprite> listThunderSprites;

	public List<Sprite> listKnightSprites;

	[Header("DATAS")]
	public Material spriteWhiteMaterial;

	private GameScene gameSceneComponent;

	private Player playerComponent;

	private SkillKnight skillKnightComponent;

	private SpriteRenderer thunderRenderer;

	private SpriteRenderer knightRenderer;

	private SpriteRenderer endCircleRenderer;

	private float knightXpos;

	private bool opningStart;

	private bool opningEnded;

	private void Start()
	{
		gameSceneComponent = gameScene.GetComponent<GameScene>();
		playerComponent = player.GetComponent<Player>();
		skillKnightComponent = skillKnight.GetComponent<SkillKnight>();
		thunderRenderer = thunder.GetComponent<SpriteRenderer>();
		knightRenderer = knight.GetComponent<SpriteRenderer>();
		endCircleRenderer = endCircle.GetComponent<SpriteRenderer>();
		knightXpos = knight.transform.position.x - 0.5f;
		StartCoroutine(updatePlayerMoveCheck());
	}

	private void Update()
	{
	}

	private IEnumerator updatePlayerMoveCheck()
	{
		while (true)
		{
			if (player.transform.position.x > 65f)
			{
				Input.ResetInputAxes();
				playerComponent.stop();
				if (!opningStart)
				{
					startOpning();
				}
			}
			if (!opningEnded)
			{
				yield return null;
				continue;
			}
			break;
		}
	}

	private void startOpning()
	{
		opningStart = true;
		StartCoroutine(updateOpning());
	}

	private IEnumerator updateOpning()
	{
		gameSceneComponent.setCameraMoveVillage(state: true);
		gameSceneComponent.getTouchEvent().setEnabled(s: false);
		Vector3 position = mainCamera.transform.position;
		mainCamera.transform.DOMove(new Vector3(knightXpos, position.y, position.z), 0.5f);
		yield return new WaitForSeconds(0.8f);
		Singleton<SoundManager>.Instance.playSound("knight_thunder");
		float frameTime = 0.08f;
		thunderRenderer.sprite = listThunderSprites[0];
		yield return new WaitForSeconds(frameTime);
		thunderRenderer.sprite = listThunderSprites[1];
		knightRenderer.sprite = listKnightSprites[1];
		yield return new WaitForSeconds(frameTime);
		mainCamera.transform.DOShakePosition(1f, 1f, 40);
		thunderRenderer.sprite = listThunderSprites[2];
		yield return new WaitForSeconds(frameTime);
		thunderRenderer.sprite = listThunderSprites[3];
		knightRenderer.sprite = listKnightSprites[2];
		yield return new WaitForSeconds(frameTime);
		thunderRenderer.sprite = listThunderSprites[4];
		knightRenderer.sprite = listKnightSprites[1];
		yield return new WaitForSeconds(frameTime);
		thunderRenderer.sprite = listThunderSprites[5];
		knightRenderer.sprite = listKnightSprites[2];
		yield return new WaitForSeconds(frameTime);
		thunderRenderer.sprite = listThunderSprites[6];
		knightRenderer.sprite = listKnightSprites[0];
		yield return new WaitForSeconds(frameTime);
		thunderRenderer.sprite = listThunderSprites[7];
		yield return new WaitForSeconds(frameTime);
		thunderRenderer.sprite = listThunderSprites[8];
		yield return new WaitForSeconds(frameTime);
		thunder.SetActive(value: false);
		yield return new WaitForSeconds(1f);
		Material material = knightRenderer.material;
		knightRenderer.material = spriteWhiteMaterial;
		float amountValue = 0f;
		int count = listLights.Count;
		for (int i = 0; i < count; i++)
		{
			listLights[i].transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetDelay(0.5f + (float)i * 0.2f);
		}
		while (true)
		{
			knightRenderer.material.SetFloat("_FlashAmount", amountValue);
			amountValue += Time.deltaTime * 0.4f;
			if (amountValue > 1f)
			{
				break;
			}
			yield return null;
		}
		knightRenderer.material.SetFloat("_FlashAmount", 1f);
		yield return new WaitForSeconds(0.5f);
		Singleton<SoundManager>.Instance.playSound("portalOpenStart");
		endCircle.transform.DOScale(new Vector3(50f, 50f, 50f), 0.25f);
		yield return new WaitForSeconds(0.5f);
		for (int j = 0; j < count; j++)
		{
			listLights[j].SetActive(value: false);
		}
		knight.SetActive(value: false);
		skillKnightComponent.settingEnemy(gameSceneComponent.initKnightEnemy());
		skillKnightComponent.startSkill(1.8f);
		endCircleRenderer.DOFade(0f, 0.5f);
		yield return new WaitForSeconds(0.8f);
		Singleton<SoundManager>.Instance.playBGM("knight_bgm");
		gameSceneComponent.setCameraMoveVillage(state: false);
		gameSceneComponent.getTouchEvent().setEnabled(s: true);
		opningEnded = true;
	}
}
