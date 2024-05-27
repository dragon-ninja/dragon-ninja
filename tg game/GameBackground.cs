using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class GameBackground : MonoBehaviour
{
	private List<BackgroundItem> listBackgroundItems = new List<BackgroundItem>();

	private List<BackgroundSubItem> listBackgroundSubItems = new List<BackgroundSubItem>();

	private List<BackgroundObject> listBackgroundObjects = new List<BackgroundObject>();

	private List<SpriteRenderer> listAllSprites = new List<SpriteRenderer>();

	private List<BackgroundLight> listLightSprites = new List<BackgroundLight>();

	private CustomLoadManager customLoadManager;

	private float maxY;

	public void settingBackgroundImages(int stage, GameScene gameScene, CameraSetting cameraComponent)
	{
		customLoadManager = Singleton<CustomLoadManager>.Instance;
		maxY = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y;
		switch (stage)
		{
		case -1:
			settingStage7_village();
			break;
		case 0:
			settingStage0(cameraComponent);
			break;
		case 1:
			settingStage1();
			break;
		case 2:
			settingStage2();
			break;
		case 3:
			settingStage3(gameScene);
			break;
		case 4:
			settingStage4(cameraComponent);
			break;
		case 5:
			settingStage5();
			break;
		case 6:
			settingStage6(cameraComponent);
			break;
		case 7:
			settingStage7();
			break;
		case 8:
			settingStage8();
			break;
		}
	}

	public void settingStage0(CameraSetting cameraComponent)
	{
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(0), 1f, 300f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(1), 0.95f, 205f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(2), 0.75f, 204f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(3), 0.35f, 203f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(4), 0f, 202f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(5), 0f, 100f, t: false));
		float cameraHeight = cameraComponent.getCameraHeight();
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(6), SubItem.SUB_SCROLL_HOLD, new Vector3(0f, 0f, 250f), new float[3]
		{
			0.5f,
			cameraHeight * 0.66f,
			cameraHeight
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(7), SubItem.SUB_SCROLL_HOLD, new Vector3(0f, 0f, 250f), new float[3]
		{
			0.5f,
			cameraHeight * 0.66f,
			cameraHeight
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(8), SubItem.SUB_SCROLL_HOLD, new Vector3(0f, 0f, 250f), new float[3]
		{
			0.5f,
			cameraHeight * 0.66f,
			cameraHeight
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(9), SubItem.SUB_SCROLL_HOLD, new Vector3(0f, 0f, 250f), new float[3]
		{
			0.5f,
			cameraHeight * 0.66f,
			cameraHeight
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(10), SubItem.SUB_LIGHT, new Vector3(-3.58f, maxY, 150f), new float[1]
		{
			3f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(11), SubItem.SUB_LIGHT, new Vector3(-2.52f, maxY, 150f), new float[1]
		{
			22f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(12), SubItem.SUB_LIGHT, new Vector3(0.02f, maxY, 150f), new float[1]
		{
			5f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(13), SubItem.SUB_LIGHT, new Vector3(0.59f, maxY, 150f), new float[1]
		{
			3.5f
		}));
	}

	public void settingStage1()
	{
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(0), 1f, 300f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(1), 0.75f, 205f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(2), 0.5f, 204f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(3), 0.4f, 203f, t: true));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(4), 0f, 202f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(5), 0.25f, 202f, t: true));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(6), 0f, 100f, t: false));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(7), SubItem.SUB_LIGHT, new Vector3(-3.58f, maxY, 150f), new float[1]
		{
			3f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(8), SubItem.SUB_LIGHT, new Vector3(-2.52f, maxY, 150f), new float[1]
		{
			22f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(9), SubItem.SUB_LIGHT, new Vector3(0.02f, maxY, 150f), new float[1]
		{
			5f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(10), SubItem.SUB_LIGHT, new Vector3(0.59f, maxY, 150f), new float[1]
		{
			3.5f
		}));
	}

	public void settingStage2()
	{
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(0), 1f, 300f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(1), 0.8f, 206f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(2), 0.75f, 205f, t: true));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(3), 0.75f, 205f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(4), 0.5f, 204f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(5), 0.25f, 203f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(6), 0f, 202f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(7), 0f, 202f, t: true));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(8), 0f, 100f, t: false));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(9), SubItem.SUB_LIGHT, new Vector3(-3.58f, maxY, 150f), new float[1]
		{
			3f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(10), SubItem.SUB_LIGHT, new Vector3(-2.52f, maxY, 150f), new float[1]
		{
			22f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(11), SubItem.SUB_LIGHT, new Vector3(0.02f, maxY, 150f), new float[1]
		{
			5f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(12), SubItem.SUB_LIGHT, new Vector3(0.59f, maxY, 150f), new float[1]
		{
			3.5f
		}));
	}

	public void settingStage3(GameScene gameScene)
	{
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(0), 1f, 300f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(1), 0.75f, 206f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(2), 0.5f, 205f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(3), 0.25f, 204f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(4), 0.75f, 203f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(5), 0.5f, 202f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(6), 0f, 100f, t: false));
		Sprite sprite = customLoadManager.LoadSpriteBackground(7);
		float num = sprite.rect.height / sprite.pixelsPerUnit;
		addSubItem(new BackgroundSubItem(sprite, SubItem.SUB_SCROLL, new Vector3(0f, 0f, 199f), new float[3]
		{
			0.5f,
			gameScene.startHeight,
			gameScene.startHeight + 0.01f
		}));
		Sprite sprite2 = customLoadManager.LoadSpriteBackground(8);
		float num2 = sprite2.rect.height / sprite2.pixelsPerUnit;
		addSubItem(new BackgroundSubItem(sprite2, SubItem.SUB_SCROLL, new Vector3(0f, 0f, 200f), new float[3]
		{
			0.2f,
			gameScene.startHeight,
			gameScene.startHeight + 0.01f
		}));
		Sprite sprite3 = customLoadManager.LoadSpriteBackground(9);
		float num3 = sprite3.rect.height / sprite3.pixelsPerUnit;
		addSubItem(new BackgroundSubItem(sprite3, SubItem.SUB_SCROLL, new Vector3(0f, 0f, 201f), new float[3]
		{
			0.3f,
			gameScene.startHeight + 2.9f,
			gameScene.startHeight + 3f
		}));
		Sprite sprite4 = customLoadManager.LoadSpriteBackground(10);
		float num4 = sprite4.rect.height / sprite4.pixelsPerUnit;
		addSubItem(new BackgroundSubItem(sprite4, SubItem.SUB_SCROLL, new Vector3(0f, 0f, 202f), new float[3]
		{
			0.4f,
			gameScene.startHeight + 2.9f,
			gameScene.startHeight + 3f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(11), SubItem.SUB_LIGHT, new Vector3(-3.58f, maxY, 150f), new float[1]
		{
			3f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(12), SubItem.SUB_LIGHT, new Vector3(-2.52f, maxY, 150f), new float[1]
		{
			22f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(13), SubItem.SUB_LIGHT, new Vector3(0.02f, maxY, 150f), new float[1]
		{
			5f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(14), SubItem.SUB_LIGHT, new Vector3(0.59f, maxY, 150f), new float[1]
		{
			3.5f
		}));
	}

	public void settingStage4(CameraSetting cameraComponent)
	{
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(0), 1f, 300f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(1), 0.8f, 207f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(2), 0.75f, 206f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(3), 0.5f, 205f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(4), 0.4f, 203f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(5), 0f, 202f, t: true));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(6), 0.25f, 201f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(7), 0f, 200f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(8), 0f, 100f, t: false));
		float cameraHeight = cameraComponent.getCameraHeight();
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(9), SubItem.SUB_SCROLL_HOLD, new Vector3(0f, 0f, 204f), new float[3]
		{
			0.4f,
			cameraHeight * 0.56f,
			cameraHeight * 0.9f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(10), SubItem.SUB_SCROLL_HOLD, new Vector3(0f, 0f, 204f), new float[3]
		{
			0.3f,
			cameraHeight * 0.56f,
			cameraHeight * 0.9f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(11), SubItem.SUB_LIGHT, new Vector3(-3.58f, maxY, 150f), new float[1]
		{
			3f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(12), SubItem.SUB_LIGHT, new Vector3(-2.52f, maxY, 150f), new float[1]
		{
			22f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(13), SubItem.SUB_LIGHT, new Vector3(0.02f, maxY, 150f), new float[1]
		{
			5f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(14), SubItem.SUB_LIGHT, new Vector3(0.59f, maxY, 150f), new float[1]
		{
			3.5f
		}));
	}

	public void settingStage5()
	{
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(0), 1f, 300f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(1), 0.75f, 205f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(2), 0.5f, 204f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(3), 0.4f, 203f, t: true));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(4), 0f, 202f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(5), 0.25f, 202f, t: true));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(6), 0f, 202f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(7), 0f, 100f, t: false));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(8), SubItem.SUB_LIGHT, new Vector3(-3.58f, maxY, 150f), new float[1]
		{
			3f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(9), SubItem.SUB_LIGHT, new Vector3(-2.52f, maxY, 150f), new float[1]
		{
			22f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(10), SubItem.SUB_LIGHT, new Vector3(0.02f, maxY, 150f), new float[1]
		{
			5f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(11), SubItem.SUB_LIGHT, new Vector3(0.59f, maxY, 150f), new float[1]
		{
			3.5f
		}));
	}

	public void settingStage6(CameraSetting cameraComponent)
	{
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(0), 1f, 300f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(1), 0.98f, 209f, t: false, ItemAction.ACTION_FADE, 3.5f));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(2), 0.96f, 208f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(3), 0.85f, 207f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(4), 0.6f, 206f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(5), 0.5f, 205f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(6), 0.4f, 204f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(7), 0.4f, 203f, t: false, ItemAction.ACTION_FADE, 1f));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(8), 0f, 202f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(9), 0f, 201f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(10), 0f, 100f, t: false));
		float cameraHeight = cameraComponent.getCameraHeight();
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(11), SubItem.SUB_SCROLL_HOLD, new Vector3(0f, 0f, 250f), new float[3]
		{
			0.5f,
			cameraHeight * 0.66f,
			cameraHeight
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(12), SubItem.SUB_SCROLL_HOLD, new Vector3(0f, 0f, 250f), new float[3]
		{
			0.5f,
			cameraHeight * 0.66f,
			cameraHeight
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(13), SubItem.SUB_SCROLL_HOLD, new Vector3(0f, 0f, 250f), new float[3]
		{
			0.5f,
			cameraHeight * 0.66f,
			cameraHeight
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(14), SubItem.SUB_SCROLL_HOLD, new Vector3(0f, 0f, 250f), new float[3]
		{
			0.5f,
			cameraHeight * 0.66f,
			cameraHeight
		}));
	}

	public void settingStage7()
	{
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(0), 1f, 300f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(1), 0.98f, 209f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(2), 0.75f, 204f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(3), 0.5f, 203f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(4), 0.25f, 202f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(5), 0f, 100f, t: false));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(6), SubItem.SUB_ROTATE, new Vector3(-2f, 3f, 208f), new float[1]
		{
			20f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(7), SubItem.SUB_ROTATE, new Vector3(-2f, 3f, 207f), new float[1]
		{
			10f
		}));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(8), SubItem.SUB_ROTATE, new Vector3(-2f, 3f, 206f), new float[1]));
		addSubItem(new BackgroundSubItem(customLoadManager.LoadSpriteBackground(9), SubItem.SUB_ROTATE, new Vector3(-2f, 3f, 206f), new float[1]));
	}

	public void settingStage7_village()
	{
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(14), 1f, 300f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(15), 0.98f, 209f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(16), 0.75f, 204f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(17), 0.5f, 203f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(18), 0.25f, 202f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(19), 0f, 100f, t: false));
	}

	public void settingStage8()
	{
		GameObject data = Singleton<AssetManager>.Instance.LoadObject("Effect/Background/firesword");
		List<BackgroundItemParticle> particles = new List<BackgroundItemParticle>
		{
			new BackgroundItemParticle(data, new Vector3(-1.33f, 3.54f, -1f)),
			new BackgroundItemParticle(data, new Vector3(1.92f, 3.54f, -1f))
		};
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(0), 0.75f, 300f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(1), 0.75f, 209f, t: false, ItemAction.ACTION_NOT, 0f, particles));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(2), 0.55f, 204f, t: true));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(3), 0.5f, 202f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(4), 1f, 201f, t: false));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(5), 1f, 201f, t: true));
		addItem(new BackgroundItem(customLoadManager.LoadSpriteBackground(6), 0f, 100f, t: false));
	}

	public void addItem(BackgroundItem item)
	{
		listBackgroundItems.Add(item);
	}

	public void addSubItem(BackgroundSubItem item)
	{
		listBackgroundSubItems.Add(item);
	}

	public void createBackground(float startWidthSize, float mapWidthSize, float startHeight, Transform cameraTransform)
	{
		float num = startWidthSize * 2f;
		float num2 = num;
		int count = listBackgroundItems.Count;
		BackgroundObject item = default(BackgroundObject);
		for (int i = 0; i < count; i++)
		{
			BackgroundItem backgroundItem = listBackgroundItems[i];
			List<BackgroundItemParticle> listParticles = backgroundItem.listParticles;
			int num3 = (listParticles != null) ? listParticles.Count : 0;
			float num4 = backgroundItem.image.rect.width / backgroundItem.image.pixelsPerUnit * 2.5f;
			float y = startHeight;
			if (backgroundItem.top)
			{
				y = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y;
			}
			for (int j = 0; j < 100; j++)
			{
				if (num2 >= mapWidthSize)
				{
					break;
				}
				GameObject gameObject = new GameObject();
				SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
				spriteRenderer.sortingLayerName = "Background";
				spriteRenderer.sprite = backgroundItem.image;
				if (listParticles != null)
				{
					for (int k = 0; k < num3; k++)
					{
						GameObject gameObject2 = UnityEngine.Object.Instantiate(listParticles[k].obj);
						gameObject2.name = "ParticleObject";
						gameObject2.transform.parent = gameObject.transform;
						gameObject2.transform.localPosition = listParticles[k].position;
					}
				}
				gameObject.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
				if (backgroundItem.action == ItemAction.ACTION_FADE)
				{
					Sequence sequence = DOTween.Sequence();
					sequence.Append(spriteRenderer.DOFade(0f, backgroundItem.actionSubData / 2f));
					sequence.Append(spriteRenderer.DOFade(1f, backgroundItem.actionSubData / 2f));
					sequence.SetLoops(-1);
					sequence.Play();
				}
				if (backgroundItem.movePercent >= 0.99f)
				{
					gameObject.transform.parent = cameraTransform;
					gameObject.transform.localPosition = new Vector3(0f, y, backgroundItem.zorder - cameraTransform.position.z);
					listAllSprites.Add(spriteRenderer);
					break;
				}
				gameObject.transform.parent = base.transform;
				gameObject.transform.position = new Vector3(num2 + num4 / 2f, y, backgroundItem.zorder);
				item.gameObject = gameObject;
				item.movePercent = backgroundItem.movePercent;
				item.oldPosition = gameObject.transform.position;
				num2 += num4;
				listAllSprites.Add(spriteRenderer);
				listBackgroundObjects.Add(item);
			}
			num2 = num;
		}
	}

	public void createBackgroundSub(float cameraSizeWidth, float cameraSizeHeight, Transform cameraTransform)
	{
		int count = listBackgroundSubItems.Count;
		for (int i = 0; i < count; i++)
		{
			BackgroundSubItem backgroundSubItem = listBackgroundSubItems[i];
			if (backgroundSubItem.state == SubItem.SUB_SCROLL || backgroundSubItem.state == SubItem.SUB_SCROLL_HOLD)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(Singleton<AssetManager>.Instance.LoadObject("Objects/Background/BackgroundScroll"));
				bool flag = backgroundSubItem.state == SubItem.SUB_SCROLL_HOLD;
				if (flag)
				{
					gameObject.transform.parent = cameraTransform;
				}
				else
				{
					gameObject.transform.parent = base.transform;
				}
				gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
				gameObject.GetComponent<BackgroundScroll>().initObject(backgroundSubItem.image, backgroundSubItem.supData[0], new Vector3(UnityEngine.Random.Range((0f - cameraSizeWidth) / 2f, cameraSizeWidth * 3f), UnityEngine.Random.Range(backgroundSubItem.supData[1], backgroundSubItem.supData[2]), 50f), flag, "Background");
				Vector3 position = gameObject.transform.position;
				gameObject.transform.position = new Vector3(position.x, position.y, backgroundSubItem.position.z);
				listAllSprites.Add(gameObject.GetComponent<SpriteRenderer>());
			}
			else if (backgroundSubItem.state == SubItem.SUB_LIGHT)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(Singleton<AssetManager>.Instance.LoadObject("Objects/Background/BackgroundLight"));
				gameObject2.transform.parent = cameraTransform;
				gameObject2.transform.localScale = new Vector3(2f, 2.5f, 2f);
				gameObject2.transform.position = backgroundSubItem.position;
				BackgroundLight component = gameObject2.GetComponent<BackgroundLight>();
				component.initObject(backgroundSubItem.image, backgroundSubItem.supData[0], "Background");
				listLightSprites.Add(component);
			}
			else if (backgroundSubItem.state == SubItem.SUB_ROTATE)
			{
				GameObject gameObject3 = UnityEngine.Object.Instantiate(Singleton<AssetManager>.Instance.LoadObject("Objects/Background/BackgroundRotate"));
				gameObject3.transform.parent = cameraTransform;
				gameObject3.transform.localScale = new Vector3(2f, 2f, 2f);
				gameObject3.transform.position = backgroundSubItem.position;
				gameObject3.GetComponent<BackgroundRotate>().initObject(backgroundSubItem.image, backgroundSubItem.supData[0], "Background");
				listAllSprites.Add(gameObject3.GetComponent<SpriteRenderer>());
			}
		}
	}

	public void moveBackground(Vector3 cameraPosition)
	{
		int count = listBackgroundObjects.Count;
		for (int i = 0; i < count; i++)
		{
			BackgroundObject backgroundObject = listBackgroundObjects[i];
			if (!(backgroundObject.movePercent >= 0.99f))
			{
				Vector3 position = backgroundObject.oldPosition + new Vector3(cameraPosition.x * backgroundObject.movePercent, 0f, 0f);
				backgroundObject.gameObject.transform.position = position;
			}
		}
	}

	public void moveChange(float x)
	{
		int count = listBackgroundObjects.Count;
		for (int i = 0; i < count; i++)
		{
			BackgroundObject backgroundObject = listBackgroundObjects[i];
			if (!(backgroundObject.movePercent >= 0.99f))
			{
				Vector3 position = backgroundObject.gameObject.transform.position;
				backgroundObject.gameObject.transform.DOMove(new Vector3(position.x + x * backgroundObject.movePercent, position.y, position.z), 0.3f).SetEase(Ease.InExpo);
			}
		}
	}

	public void allSpritesFade(float endValue, float time)
	{
		int count = listAllSprites.Count;
		for (int i = 0; i < count; i++)
		{
			listAllSprites[i].DOKill();
			listAllSprites[i].DOFade(endValue, time);
		}
		count = listLightSprites.Count;
		for (int j = 0; j < count; j++)
		{
			listLightSprites[j].fadeAction(endValue, time);
		}
	}
}
