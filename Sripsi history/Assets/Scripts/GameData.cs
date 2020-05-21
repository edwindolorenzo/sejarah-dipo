using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int[] level;
    public bool[] levelClear;
    public int[] idChalange;
    public bool[] chalangeClear;
    public string[] chalangeDescription;
    public float[] miniGameScore;
    public int[] idMiniGame;
    public bool[] openMiniGame;
    public string[] miniGameDescription;

    public GameData(List<Stage> stages, List<MiniGame> miniGames)
    {
        List<int> levelList = new List<int>();
        List<bool> levelClearList = new List<bool>();
        List<int> idChalangeList = new List<int>();
        List<bool> chalangeClearList = new List<bool>();
        List<string> chalangeDescriptionList = new List<string>();
        List<float> miniGameScoreList = new List<float>();
        List<int> idMiniGameList = new List<int>();
        List<bool> openMiniGameList = new List<bool>();
        List<string> miniGameDescriptionList = new List<string>();

        foreach (Stage stage in stages)
        {
            levelList.Add(stage.Level);
            levelClearList.Add(stage.Clear);
            foreach (Chalange chalange in stage.Chalanges)
            {
                idChalangeList.Add(chalange.IdChalange);
                chalangeClearList.Add(chalange.Clear);
                chalangeDescriptionList.Add(chalange.NameChalange);
                Debug.Log("nama chalange = " + chalange.NameChalange);
            }
        }
        foreach(MiniGame miniGame in miniGames)
        {
            miniGameScoreList.Add(miniGame.Highscore);
            idMiniGameList.Add(miniGame.Id);
            openMiniGameList.Add(miniGame.Opened);
            miniGameDescriptionList.Add(miniGame.Description);
        };
        level = levelList.ToArray();
        levelClear = levelClearList.ToArray();
        idChalange = idChalangeList.ToArray();
        chalangeClear = chalangeClearList.ToArray();
        chalangeDescription = chalangeDescriptionList.ToArray();
        miniGameScore = miniGameScoreList.ToArray();
        idMiniGame = idMiniGameList.ToArray();
        openMiniGame = openMiniGameList.ToArray();
        miniGameDescription = miniGameDescriptionList.ToArray();
    }
}
