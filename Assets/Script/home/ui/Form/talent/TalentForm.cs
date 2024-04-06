using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentForm : BaseUIForm
{
    //����
   /* public Sprite line_unlock;
    public Sprite line_lock;
    //horizontal ����
    public Sprite line_unlock_h;
    public Sprite line_lock_h;*/

    TalentInfoPanel talentInfoPanel;


    public List<TalentData> talentList;
    public List<TalentData> super_talentList;

    public List<TalentLevelSlot> levelSlotList;
    public List<TalentSlot> talentSlotList;
    public List<TalentSlot> super_talentSlotList;
    //public List<TalentLevelSlot> talentLevelSlotList;

    public override void Awake()
    {
        base.Awake();
        ui_type.ui_FormType = UIformType.Normal;
        ui_type.ui_ShowType = UIformShowMode.HideOther;
        ui_type.IsClearStack = false;

        talentInfoPanel = 
            UIFrameUtil.FindChildNode(this.transform, "TalentInfoPanel").GetComponent<TalentInfoPanel>();
        talentInfoPanel.form = this;

        //������ʾ��Ϣ�¼�
        MessageMgr.AddMsgListener("TalentInfoShow", p =>
        {
            TalentInfoShow(p.Key,int.Parse(p.Value.ToString()));
        });

        Transform talent_node = 
            UIFrameUtil.FindChildNode(this.transform, "Content_talent");
        Transform talent_super_node = 
            UIFrameUtil.FindChildNode(this.transform, "Content_talent_super");
        Transform talent_level_node = 
            UIFrameUtil.FindChildNode(this.transform, "Content_talent_level");

      




        //�ȼ��ڵ�
        levelSlotList = new List<TalentLevelSlot>();
        for (int i = 0; i < talent_level_node.childCount; i++) {
            TalentLevelSlot ts = talent_level_node.GetChild(i).GetComponent<TalentLevelSlot>();
            levelSlotList.Add(ts);
        }
        levelSlotList[0].Refresh(true);


        //��ͨ�ڵ�
        talentList = new List<TalentData>();
        super_talentList = new List<TalentData>();
        int index = 0;
        talentSlotList = new List<TalentSlot>();
       
        for (int i = 0;i< talent_node.childCount; i++) {
            Transform items = talent_node.GetChild(i);
            
            for (int j = 0; j < items.childCount; j++)
            {
                talentList.Add(new TalentData(
                    TalentFactory.Get().talentList[index].id));
                TalentSlot ts = items.GetChild(j).GetComponent<TalentSlot>();
                ts.talent = TalentFactory.Get().talentList[index];
                ts.index = index++;

                if (i == talent_node.childCount-1 && j == items.childCount-1)
                    ts.endFlag = true;

/*              if (DataManager.Get().userData.talentList!=null && DataManager.Get().userData.talentList.Contains(ts.talent.id))
                    ts.Refresh(true);
                elseDataManager.Get().userData.talentList
                    ts.Refresh(false);*/
                
                talentSlotList.Add(ts);
            }
        }
        //����ڵ�
        index = 0;
        super_talentSlotList = new List<TalentSlot>();
        for (int i = 0; i < talent_super_node.childCount; i++)
        {
            super_talentList.Add(new TalentData(
                   TalentFactory.Get().superTalentList[index].id));

            TalentSlot ts = talent_super_node.GetChild(i).GetComponent<TalentSlot>();
            ts.talent = TalentFactory.Get().superTalentList[index];
            ts.index = index++;

            if (i == talent_super_node.childCount - 1)
                ts.endFlag = true;

            /*if (DataManager.Get().userData.talentList != null && DataManager.Get().userData.talentList.Contains(ts.talent.id))
                ts.Refresh(true);
            else
                ts.Refresh(false);*/
            super_talentSlotList.Add(ts);   
        }

        Refresh();
    }

    public override void Hide() {
        base.Hide();
        talentInfoPanel.Hide();
    }

    void init() {
        //�����5�ı���   �ó��ȼ��͹ؼ��츳��ռλ����  ���в���ʾ�Ļ�����,������Ҫռλ
        /*int num = (talentList.Count - 2) / 5;

        int level = 0;
        //��¼�ȼ��仯�ڵ�
        List<int> levelList = new List<int>();
        for (int i=0; i < talentList.Count; i++) {
            if (talentList[i].level != 0 && talentList[i].level!= level) { 
                level = talentList[i].level;
                levelList.Add(i);
            }
        }*/
      
    }

    async void Refresh() {
        //��ȡDataManager.userData.talentList
        await DataManager.Get().refreshRoleAttributeStr();
       



        //�ȼ�����ʾ
        for (int i = 0; i < levelSlotList.Count && i< DataManager.Get().roleAttrData.nowLevel; i++)
        {
            levelSlotList[i].Refresh(true);
        }

        //�츳
        for (int j = 0; j < talentSlotList.Count; j++)
        {
            if(DataManager.Get().roleAttrData.talentList.Contains(talentSlotList[j].talent.id))
                talentSlotList[j].Refresh(true);
            else
                talentSlotList[j].Refresh(false);
        }
        //�����츳
        for (int j = 0; j < super_talentSlotList.Count; j++)
        {
            if (DataManager.Get().roleAttrData.talentList.Contains(super_talentSlotList[j].talent.id))
                super_talentSlotList[j].Refresh(true);
            else
                super_talentSlotList[j].Refresh(false);
        }

    }


    //��ǰ�������츳����
    public int nowIndex;
    //��ǰ�츳����һ���Ƿ����
    public bool nowUnlockFlag;

    void TalentInfoShow(string talentType, int index) {
        nowIndex = index;

        if (talentType != null && talentType.IndexOf("super") != -1)
        {
            talentInfoPanel.Show(super_talentSlotList[nowIndex].talent);
        }
        else {
            talentInfoPanel.Show(talentSlotList[nowIndex].talent);
        }
    }

    public async void unlockTalent(Talent t) {
        //�����츳������
       /* DataManager.Get().userData.gold -= t.expend.num;

        if (DataManager.Get().userData.talentList == null)
            DataManager.Get().userData.talentList = new List<string>();

        if (!DataManager.Get().userData.talentList.Contains(t.id))
            DataManager.Get().userData.talentList.Add(t.id);

        DataManager.Get().save();*/
        Debug.Log("T:"+ DataManager.Get().roleAttrData.talentList.Count);
        Debug.Log("t.id:" + t.id);
        
        //�����츳
        LockTalentPost post = new LockTalentPost();
        post.talentId = t.id;
        string json = JsonConvert.SerializeObject(post);
        string str = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/talent/unlock", json, DataManager.Get().getHeader());


        //ˢ��ui
        /*if (t.talentType == "common")
        {
            talentSlotList[nowIndex].Refresh(true);
        }
        else {
            super_talentSlotList[nowIndex].Refresh(true);
        }*/
        //Ӧ�����Ե���ɫ

        //ˢ�¶�����Դ
        MessageMgr.SendMsg("UpMenuRefresh", new MsgKV(null,null));

        Refresh();
    }
}

//����������
public class LockTalentPost {
    public string talentId;
}