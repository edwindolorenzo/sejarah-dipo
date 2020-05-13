using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    List<MiniGame> miniGames = new List<MiniGame>();
    private List<Stage> stages = new List<Stage>();

    public static GameManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        LoadData();
    }

    public List<Stage> AllStage ()
    {
        return stages;
    }

    public Stage SelectStage(int level)
    {        
        Stage stage = stages[level-1];
        return stage;
    }

    public List<MiniGame> AllMiniGame()
    {
        return miniGames;
    }

    public MiniGame SelectMiniGame(int id)
    {
        foreach(MiniGame miniGame in miniGames)
        {
            if (miniGame.Id == id)
                return miniGame;
        }
        return null;
    }

    public void UpdateStage(Stage stageUpdated, int level, List<MiniGame> listMiniGames)
    {
        int indexStage = stages.FindIndex(x => x.Level == level);
        stages[indexStage] = stageUpdated;
        miniGames = listMiniGames;
        SaveData();
    }

    public void UpdateMiniGame(MiniGame miniGame)
    {
        int indexMiniGame = miniGames.FindIndex(x => x.Id == miniGame.Id);
        miniGames[indexMiniGame] = miniGame;
        SaveData();
    }

    public void SaveData()
    {
        SaveSystem.SaveData(stages, miniGames);
    }

    public void LoadData()
    {
        GameData data = SaveSystem.LoadData();

        if (data != null)
        {
            stages.Clear();
            for(int i = 0; i< data.level.Length; i++)
            {
                int stageLevel = data.level[i];
                bool levelClear = data.levelClear[i];
                int chalangeNumber = i * 2 + i;
                Chalange chalange1 = new Chalange(data.idChalange[chalangeNumber], data.chalangeClear[chalangeNumber]);
                Chalange chalange2 = new Chalange(data.idChalange[chalangeNumber+1], data.chalangeClear[chalangeNumber+1]);
                Chalange chalange3 = new Chalange(data.idChalange[chalangeNumber+2], data.chalangeClear[chalangeNumber+2]);
                stages.Add(new Stage(stageLevel, new List<Chalange> { chalange1, chalange2, chalange3 }, levelClear));
            }
            miniGames.Clear();
            for(int i = 0; i < data.idMiniGame.Length; i++)
            {
                float miniGameScore = data.miniGameScore[i];
                int idMiniGame = data.idMiniGame[i];
                bool openMiniGame = data.openMiniGame[i];
                miniGames.Add(new MiniGame(idMiniGame, miniGameScore, openMiniGame));
            }
        }
        else {
            miniGames = new List<MiniGame>()
            {
                new MiniGame(1),
                new MiniGame(2)
            };
            stages.Add(new Stage(1, new List<Chalange> { new Chalange(1), new Chalange(2), new Chalange(3) }));
            stages.Add(new Stage(2, new List<Chalange> { new Chalange(1), new Chalange(2), new Chalange(4) }));
            stages.Add(new Stage(3, new List<Chalange> { new Chalange(1), new Chalange(2), new Chalange(3) }));
            stages.Add(new Stage(4, new List<Chalange> { new Chalange(1), new Chalange(2), new Chalange(3) }));
            stages.Add(new Stage(5, new List<Chalange> { new Chalange(2), new Chalange(3), new Chalange(4) }));
        }
    }
}
