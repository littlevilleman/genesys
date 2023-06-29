using System;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    //private List<GameDataProfile> gameDataProfiles = new();
    //private int currentProfile;

    private GameDataProfile gameDataProfile;
    public GameDataProfile GameDataProfile => gameDataProfile;

    public void SaveDataProfile()
    {
        string profileDataJson = JsonUtility.ToJson(gameDataProfile);
        PlayerPrefs.SetString("gameDataProfile", profileDataJson);
    }

    public void LoadDataProfile()
    {
        string gameDataJson = PlayerPrefs.GetString("gameDataProfile");

        if (gameDataJson != string.Empty)
        {
            gameDataProfile = JsonUtility.FromJson<GameDataProfile>(gameDataJson);
        }

        EventBus.Send(new EventLoadDataProfile() { GameDataProfile = gameDataProfile });
    }
}

public class EventLoadDataProfile : EventContext
{
    public GameDataProfile GameDataProfile;
} 


public class GameDataProfile
{
    public string Name;
    public DateTime StartTime;
    public int PlayedTime;
    public int CheckPoint;

    //public List<CharacterData> characterData;

    // Unlock characters
    // Unlock cards
}