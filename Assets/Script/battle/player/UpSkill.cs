using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpSkill : MonoBehaviour
{
    public UpLevel ul;
    public int index = 0;

    public void up() {
        ul.selectSkill(index);

     /*   if (ul.selectSkillList[index].level >= 5 || ul.selectSkillList[index].skillForm == "SuperSkill") { 
            //消耗超武进化道具
        }


        UpLevel.player.addSkill(ul.selectSkillList[index]);

        //if(DungeonManager.upLevelNum == 0)
        //ul.gameObject.SetActive(false);

        ul.gameObject.SetActive(false);

        if (DataManager.Get().userData.towerData.awaitUpgrade > 0) { 
            ul.gameObject.SetActive(true);
            DataManager.Get().userData.towerData.awaitUpgrade--;
            if(DataManager.Get().userData.towerData.awaitUpgrade == 0)
                ul.gameObject.SetActive(false);
        }

        Player.levelUpIng = false;

        Time.timeScale = GameSceneManage.nowTimeScale;*/
    }
}
