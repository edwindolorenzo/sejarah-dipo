using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    List<Chalange> chalanges = new List<Chalange>() {
        new Chalange(1,"Menyelesaikan permainan"),
        new Chalange(2,"Menyelesaikan permainan tanpa menggunakan kesempatan hidup"),
        new Chalange(3,"Menggunakan kesempatan hidup hanya satu kali"),
        new Chalange(4,"Tidak terkena serangan selama permainan"),
        new Chalange(5,"Mengalahkan semua prajurit")
        };
    List<MiniGame> miniGames = new List<MiniGame>()
    {
        new MiniGame(1),
        new MiniGame(2)
    };
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
        stages.Add(new Stage(1, new List<Chalange> { chalanges[0], chalanges[1], chalanges[3] }));
        stages.Add(new Stage(2, new List<Chalange> { chalanges[0], chalanges[1], chalanges[3] }));
        stages.Add(new Stage(3, new List<Chalange> { chalanges[0], chalanges[1], chalanges[2] }));
        stages.Add(new Stage(4, new List<Chalange> { chalanges[0], chalanges[1], chalanges[2] }));
        stages.Add(new Stage(5, new List<Chalange> { chalanges[1], chalanges[2], chalanges[3] }));
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

    public void UpdateStage(Stage stageUpdated, int level)
    {
        int indexStage = stages.FindIndex(x => x.Level == level);
        stages[indexStage] = stageUpdated;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
