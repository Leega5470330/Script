using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;


public class MusicStateManager : MonoBehaviour//stage 관리(노래에 따른 현재 진행상황)
{
    //UI 관리 부분 
    // public GameObject MainUI; 
    // public Text Teeet;

    // public Image StageImage;



    PlayerData player5;


    public static List<GameObject> MainUIList = new List<GameObject>();

    public TextMesh MusicName; //이름Text
    public static List<TextMesh> MusicNameList = new List<TextMesh>();



    public static List<Image> Tile_Image = new List<Image>();

    public static List<float> Position_x = new List<float>(); //노래 정보 창 위치 

    public int[] BossSucess = null;

    public int Stage1BossSucess = 0;

    float NamePosition_x = -2.3f;
    float MainPosition_x = 0;


    public GameObject Canvas;
    public List<GameObject> CanvasList = new List<GameObject>();

    GameObject[] CanvasUI = new GameObject[9];
    GameObject[] CanvasTile = new GameObject[9];
    GameObject[] CanvasMonster = new GameObject[3];

   

    public GameObject[] Deco = new GameObject[8];//타일이랑 플레이어 배치
    [SerializeField]
    
    GameObject Tile;

    public int[] Boss = { 0, 0, 0, 0, 0, 0, 0 };
    public int[] Score = { 0, 0, 0, 0, 0, 0, 0 };
    public float[] Time = { 0, 0, 0, 0, 0, 0, 0 };
    public bool[] Lock = { false, true, true, true, true, true, };
    public int[] hit = { 0, 0, 0, 0, 0, 0, 0 };

    //-------------------------------------------------------------------------------------
    //노래 정보 입력 부분 
    


    void LoadPlayerDataFromJson()//json 출력
    {
        string path = Path.Combine(Application.persistentDataPath + "/playerData.json");
        string jsonData = File.ReadAllText(path);
        player5= JsonUtility.FromJson<PlayerData>(jsonData);


    }


    public class MusicStage_  //노래 정보 적는 구조체 
    {

        public int progress;
        public string name;
        public bool Lock;
        public int Score;
        public float Position_x;
        public float time;
        public int boss;
        public int hit;
        public static string Scenename;
        public string tilename;
        public string open;
        public string monster;


        public MusicStage_(int _progress, string _name, bool _Lock, int _Score, int _boss, int _hit, float _time, string _tilename, string _open,string _monster)
        {

            name = _name;
            progress = _progress;
            Lock = _Lock;
            Score = _Score;
            boss = _boss;
            hit = _hit;
            time = _time;
            tilename = _tilename;
            open = _open;
            monster = _monster;

        }






    }

    List<MusicStage_> MusicList = new List<MusicStage_>(); //리스트 
                                                           //List<MusicStage_> MusicList = null;



    // Start is called before the first frame update
    void Start() //노래 정보 입력 부분  //Json으로 옮겨주기 
    {
        LoadPlayerDataFromJson();




        // DontDestroyOnLoad(Canvas);
        //Screen.SetResolution(Screen.width, (Screen.width * 16) / 9, true); //호면 해상도 유지 
        Screen.sleepTimeout = SleepTimeout.NeverSleep; //게임실행중 화면이 꺼지지않게 해줌
                                                       // LoadBoss1DataFromJson();

        MusicList.Add(new MusicStage_(0, "MEADOW", player5.Lock[0], player5.Score[0], player5.Boss[0], player5.hit[0], player5.time[0], "Stage1_tile","","S1_Mon1")); //첫번째곡에 대한 정보 
        MusicList.Add(new MusicStage_(1, "DESERT", false, player5.Score[1], player5.Boss[1], player5.hit[1], player5.time[1], "Stage2_tile","", "S2_Mon2")); //두번짹 곡에 대한 정보 
        MusicList.Add(new MusicStage_(2, "JUNGLE", player5.Lock[2], player5.Score[2], player5.Boss[2], player5.hit[2], player5.time[2], "Stage3_tile","", "S3_Mon"));//세번째곡
        MusicList.Add(new MusicStage_(3, "CEMETRY", player5.Lock[3], player5.Score[3], player5.Boss[3], player5.hit[3], player5.time[3], "Stage4_tile","", "S4_Mon"));//네번째곡
        MusicList.Add(new MusicStage_(4, "Retro", player5.Lock[4], player5.Score[4], player5.Boss[4], player5.hit[4], player5.time[4], "Stage5_tile","", "S5_Mon"));//다섯번째곡
        MusicList.Add(new MusicStage_(5, "SNOW", player5.Lock[5], player5.Score[5], player5.Boss[5], player5.hit[5], player5.time[5], "Stage6_tile","", "S6_Mon"));//여섯번째곡
        MusicList.Add(new MusicStage_(6, "NIGHT", player5.Lock[6], player5.Score[6], player5.Boss[6], player5.hit[6], player5.time[6], "Stage7_tile","", "S7_Mon"));//일곱번째곡




        //Debug.Log("C" + CanvasList.Count);

        //Teeet.text = ""+CanvasList.Count;

        //  UICreat_();
        //Debug.Log(MusicList.Contains);


        UITest();
       // input_array();

    }

   

    void UITest()
    {

        for (int i = 0; i <= MusicList.Count - 1; i++)
        {

            GameObject Main = Instantiate(Canvas, new Vector3(MainPosition_x, 0.13f, -8.1f), Quaternion.identity);
            CanvasList.Add(Main); //Canvas 복사
            //Debug.Log("여기 까진 작동");
            //Input_Imfomation(i); //정보 기입하기 

            //tile_Position_x[i] = MainPosition_x - 5;


            Input_Imfomation(i);
            MainPosition_x += 20;




        }
    }

    void Input_Imfomation(int k) //정보 기입하기 
    {
        for (int u = 0; u < 6; u++)
        {
            CanvasTile[u] = (CanvasList[k].transform.GetChild(3)).transform.GetChild(u).gameObject; //타일 이미지 
        }


        
         for (int n = 0; n < 2; n++)
         {       
             CanvasMonster[n] = (CanvasList[k].transform.GetChild(4)).transform.GetChild(n).gameObject;

         }

        GameObject Canvasbackground = (CanvasList[k].transform.GetChild(5)).gameObject; //광고 창 
        GameObject CanvasLock = (CanvasList[k].transform.GetChild(2)).transform.GetChild(1).gameObject; //자물쇠 버튼 
        GameObject CanvasBoss = (CanvasList[k].transform.GetChild(5)).transform.GetChild(5).gameObject; //boss_Text
        GameObject CanvasADS = (CanvasList[k].transform.GetChild(5)).transform.GetChild(4).gameObject; //버튼
        GameObject CanvasBtn = (CanvasList[k].transform.GetChild(5)).transform.GetChild(3).gameObject; //창닫기 버튼 
        GameObject Notice = (CanvasList[k].transform.GetChild(5)).transform.GetChild(6).gameObject; //알림 느낌표 

        GameObject Notic_open= (CanvasList[k].transform.GetChild(2)).transform.GetChild(3).gameObject; //언제 게임이 업데이트 되는지 
        TextMeshProUGUI Notice_Open_Text = Notic_open.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI Boss_Text = CanvasBoss.GetComponent<TextMeshProUGUI>();
        if (MusicList[k].Lock == false)
        {
            Canvasbackground.SetActive(false);
        }
        if (k > 0)
        {
            if (MusicList[k - 1].boss < 3)
            {
                Notice.SetActive(true);
                CanvasADS.SetActive(false); //악보가 3개 모이지 않으면 광고 버튼이 보이지않는다.
            }
            else if (MusicList[k - 1].boss >= 3)
            {
                Notice.SetActive(false);
                CanvasADS.SetActive(true); //악보가 3개 모이면 광고 버튼이 보인다.
            }
            Boss_Text.text = "" + MusicList[k - 1].boss + " / 3";
        }
        Button Close_button = CanvasBtn.GetComponent<Button>();
        Close_button.onClick.AddListener(() =>
        {
            if (Canvasbackground.activeSelf == true)
            {
                Canvasbackground.SetActive(false);
            }
        });
        Button Lock_button = CanvasLock.GetComponent<Button>();
        Lock_button.onClick.AddListener(() => { Canvasbackground.SetActive(true); });




        //Debug.Log("canvas" + CanvasTile[0]);


        for (int s = 0; s < 6; s++)
        {

            CanvasUI[s] = (CanvasList[k].transform.GetChild(1)).transform.GetChild(s).gameObject; //MusicName 


           // Debug.Log("CanvasUI" + CanvasUI.Length);
        }

        for (int j = 0; j < 6; j++)
        {
            TextMeshProUGUI BossNum = CanvasUI[1].GetComponent<TextMeshProUGUI>();
            BossNum.text = MusicList[k].boss + "/3";


            TextMeshProUGUI Name2 = CanvasUI[5].GetComponent<TextMeshProUGUI>();
            Name2.text = MusicList[k].name; //노래 제목 출력



            TextMeshProUGUI Hit = CanvasUI[2].GetComponent<TextMeshProUGUI>(); //hit 
            Hit.text = "Combo:"+MusicList[k].hit  ;
           
           
            TextMeshProUGUI GameTime = CanvasUI[3].GetComponent<TextMeshProUGUI>();
            Image TileImage1 = CanvasTile[j].GetComponent<Image>();//타일 이미지 변경 스크립트
            
            GameTime.text = "Time:" + MusicList[k].time.ToString("N2");
            
            //  Debug.Log("MusicList[k].tilename");

            TextMeshProUGUI Score_text = CanvasUI[0].GetComponent<TextMeshProUGUI>(); 
            Score_text.text = "Score:" + MusicList[k].Score;
            if (MusicList[k].Lock == false) //잠겨있는 상태가 아닐때 
            {
                CanvasTile[j].SetActive(true);
                //StageImage2.sprite = Resources.Load<Sprite>("Image/" + MusicList[k].Image);
                TileImage1.sprite = Resources.Load<Sprite>("Image/" + MusicList[k].tilename);
                CanvasMonster[k].SetActive(true);
                for(int a = 0; a < 2; a++)
                {
                    if (k == a)
                    {
                        CanvasMonster[k].SetActive(true);
                    }
                }
              


            }

            if (MusicList[k].Lock == true)//잠겨있는 상태일때
            {
                CanvasTile[j].SetActive(true);
                // StageImage2.sprite = Resources.Load<Sprite>("Image/Lock");
                
                TileImage1.sprite = Resources.Load<Sprite>("Image/Stage0_tile");

                CanvasList[k].transform.GetChild(2).gameObject.SetActive(true);
                
                Notice_Open_Text.text = "Lock";
                
                
                for (int a = 0; a < 2; a++)
                {
                    CanvasMonster[a].SetActive(false);
                }
                



            }
         

           


            Button Button_Array = CanvasUI[4].GetComponent<Button>();
            Button_Array.onClick.AddListener(() => {
                if (PlayerPrefs.GetInt("Heart_Count") >= 1)
                {
                    Loading1.LoadScene(MusicList[k].progress);

                    
                }
            });

        }
    }
}
 




