using Managers;
using UnityEngine;

namespace Player
{
     public class PlayerStash_resources : MonoBehaviour
     {
          #region Awake Singleton
          //-------------------------------------------------------------
          public static PlayerStash_resources Instance;
          void Awake()
          {
               if (Instance == null) Instance = this;
               else gameObject.SetActive(false);
          
               AwakeJob();
          }
          //-------------------------------------------------------------
          #endregion
     
          [SerializeField] ResourceData playerResources;
          const string SAVE_KEY = "PLAYER_RESOURCES";

          public int Gold => playerResources.gold;
          public int Gems => playerResources.gems;
          public int Silver => playerResources.silver;
     
          void AwakeJob() => LoadData();
          void LoadData()
          {
               playerResources = PlayerPrefs.HasKey(SAVE_KEY)
                    ? JsonUtility.FromJson<ResourceData>(PlayerPrefs.GetString(SAVE_KEY))
                    : new ResourceData();
               playerResources.silver = 0;
          }

          void SaveData() => PlayerPrefs.SetString(SAVE_KEY, JsonUtility.ToJson(playerResources));
     
     
          public void AddGold(int value) => GoldChange(value);
          public void RemoveGold(int value) => GoldChange(-value);
          void GoldChange(int value)
          {
               playerResources.gold += value;
               if (playerResources.gold < 0) playerResources.gold = 0;
               SaveData();
               ResourcesChangedEvent();
          }
     
     
          public void AddGem(int value) => GemChange(value);
          public void RemoveGem(int value) => GemChange(-value);
          void GemChange(int value)
          {
               playerResources.gems += value;
               if (playerResources.gems < 0) playerResources.gems = 0;
               SaveData();
               ResourcesChangedEvent();
          }

            
          public void AddSilver(int value) => SilverChange(value);
          public void RemoveSilver(int value) => SilverChange(-value);
          void SilverChange(int value)
          {
               playerResources.silver += value;
               if (playerResources.silver < 0) playerResources.silver = 0;
               SaveData();
               ResourcesChangedEvent();
          }
          
          void ResourcesChangedEvent() => EventManager.Instance.ResourcesChanged();
     }
}