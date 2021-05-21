using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsManager : MonoBehaviour
{
    [System.Serializable]
    public class Reward
    {
        public string id;
        public bool once;
        public int nukes;
        public int traps;
        public int xp;
    }

    public static RewardsManager instance;
    private void Awake()
    {
        instance = this;
    }


    public List<Reward> rewards;

    public void RewardUser(string rewardID)
    {
        string collectedRewards = PlayerPrefs.GetString("collected_rewards");
        foreach (Reward reward in rewards)
        {
            if (rewardID.Equals(reward.id))
            {
                if (reward.once)
                {
                    if (!collectedRewards.Contains(reward.id))
                    {
                        collectedRewards += "_" + reward.id;
                        PlayerPrefs.SetString("collected_rewards", collectedRewards);
                    }
                    else
                    {
                        return;
                    }
                }
                RewardDialog.instance.SetNukeNumber(reward.nukes);
                RewardDialog.instance.SetTrapsNumber(reward.traps);
                RewardDialog.instance.SetXpNumber(reward.xp);
                UiManager.instance.OpenWindow(RewardDialog.instance);
                PlayerPrefsController.instance.AddNukes(reward.nukes);
                PlayerPrefsController.instance.AddTraps(reward.traps);
                PlayerPrefsController.instance.AddXp(reward.xp);

                break;
            }
        }
    }
}
