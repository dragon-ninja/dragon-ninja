using TMPro;
using UnityEngine;

public class VillageUI : MonoBehaviour
{
	public GameObject buttonStageSelect;

	public TextMeshProUGUI textStageSelect;

	public void settingStageSelectButton()
	{
		buttonStageSelect.SetActive(value: true);
		int num = Singleton<DataManager>.Instance.selectLevel;
		textStageSelect.text = num.ToString();
	}

	public void onStageSelect()
	{
		Singleton<SoundManager>.Instance.playSound("uiClick");
		int nowLevel = Singleton<DataManager>.Instance.selectLevel;
		Singleton<UIControlManager>.Instance.onStageLevelSelectUI(delegate
		{
			int num = Singleton<DataManager>.Instance.selectLevel;
			if (nowLevel != num)
			{
				textStageSelect.text = num.ToString();
				Singleton<DataManager>.Instance.saveDataAsync();
			}
		});
	}
}
