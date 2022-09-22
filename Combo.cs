using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Combo : MonoBehaviour
{
    [Header("Text")]
    [SerializeField]
    private TextMeshProUGUI Combo_UI;      // 플레이어 머리 위 콤보 표시
    [SerializeField]
    private TextMeshProUGUI Combo_Text_UI;//콤보만 적혀있는 text
    [SerializeField]
    private GameObject Combo_Object;//콤보+NumberText 
    [SerializeField]
    private TextMeshProUGUI Score_Text_UI;//총 스코어 적혀 있음 
    [SerializeField]
    private TextMeshProUGUI Fever_Text_UI;//피버 텍스ㅡ  
    [SerializeField]
    private TextMeshProUGUI Fever_Text_UI2;//피버 사이드 텍스트
    [SerializeField]
    private TextMeshProUGUI Magni_Text_UI2;//배율 텍스트

    [Header("effect")]
    [SerializeField]
    private Image Skill_effect;           //Skill이 있따는 것을 알려주는 effect
    [SerializeField]
    private ParticleSystem Combo_effect;       //Combo에 대한 effect 
    [SerializeField]
    private ParticleSystem Fever_effect;       //Fever에 대한 effect 


    [Header("Pora")]
    [SerializeField]
    private GameObject Player_Position;
    Vector3 EndPos;//도착점
    Vector3 StartPos;//시작점

    [Header("Score")]
    [SerializeField]
    public static float Score_Magni=1;//점수 배율
    public static float Score_all=0; //전체점수 
    public static float Score_part = 0;//상황에 따른 점수 (100점 130점등등) 
    public static int MaxCombo = 0;//콤보 최고치 


    [Header("Fever")]
    [SerializeField]
    private int Fever_Combo=0;
    public static bool fever=false;//피버 타임인지 


    private bool Combo_check = true;




    Vector3 dir;

    public static int Count_Combo;
    public static int Count_Combo_Skill ;//스킬을 사용하기 위한 콤보 100 이 모였는지 알아보는 숫자.. 

    public static int Count_Skill ; //구동가능한 스킬 횟수 (몬스터를 만나면 자동 작동)
    float UI_Scale; //콤보 UI 크기

    float time; //Fade out
    float time2;
    float _fadeTime ;
   

    void Awake()//초기화
    {

        MaxCombo = 0;
        Fever_Combo = 0;
        Combo_check = true;
        fever = false;
        Score_part = 0;
        Score_all = 0;
        Score_Magni = 1;
        time = 0;
        time2 = 0;
        _fadeTime = 0.6f;

        UI_Scale =1;
        Count_Combo = 0;
        Count_Combo_Skill = 0;
        Count_Skill = 0;
        Skill_effect.fillAmount = 0;
       
    }
    void Start()
    {
        Combo_Object.SetActive(false);

    }
    void Max_Combo()//최고 콤보 담는곳 
    {
        if (MaxCombo < Count_Combo)//MaxCombo보다 Count_Combo가 클때
        {
            MaxCombo = Count_Combo;
        }
    }
    void Count_Zero()
    {
        if ((MonsterScr.PlaState2 == false && (Player.Mns2State < 2) && MonsterScr2.PlaState2 == false )) //몬스터가 폭탄몬스터 일때 
        {

            if (Button1.Defense == true)
            {
                BeatDicision.Combo_false = true;
                BeatDicision.Combo_true = false;
                Count_Combo = 0;
                Combo_check = false;

            }

        }
        if ((Player.long_State_UP == false && Player.long_State_Down == false && Player.State_Down == false && Player.State_UP == false)) //사다리 없을때 사다리버튼 누르면 
        {
            if (Button1.Ladder == true)
            {
                BeatDicision.Combo_false = true;
                BeatDicision.Combo_true = false;
                Count_Combo = 0;
                Combo_check = false;
             
            }
        }
        if ((Player.long_State_UP == true || Player.long_State_Down == true || Player.State_Down == true || Player.State_UP == true) && Player.NoGo == true) //사다리 있고 끝타일일때 공격이나 어택 누를 경우
        {

            if (Button1.Attack == true)
            {
                BeatDicision.Combo_false = true;
                BeatDicision.Combo_true = false;
                //  Combo_Object.SetActive(false);
                Count_Combo = 0;
                Combo_check = false;
               
            }
        }
    }

    void Update()
    {
        Max_Combo();
        if (Score_all < 0)
        {
            Score_all = 0;
        }
        Score_Text_UI.text = ""+Mathf.Round(Score_all);
        Count_Zero();
        Combo_Effect_Create();
        Fever_Count();
        fill_Skill();
        Score_ma_Count();
       
        Combo_Object.transform.Translate(dir * Time.deltaTime);//하늘로 솟음
    
        if (time < _fadeTime) //Fade Out
        {
            Combo_UI.color = new Color(1, 1, 1, (1f - time /5));
            Combo_Text_UI.color = new Color(1, 1, 1, (1f - time / 5));

            Combo_Object.transform.localScale = new Vector3(UI_Scale, UI_Scale, UI_Scale) * (UI_Scale + time);
       

        }
        else
        {
            time = 0;

            Combo_Object.SetActive(false);
        }
        time += Time.deltaTime;


        Combo_UI.text = "" + Count_Combo;


        if (BeatDicision.Combo_true == true && GameMannager.Out_Time == true && Player.NoGo == false && Player.Combo_NO ==false) //게임진행 타이머가 작동할때만 콤보가 작동하게 한다.(out_Time=true)
        {
            time = 0;
            Combo_UI.color = new Color(1, 1, 1, 1); //Fade Out
            Combo_Text_UI.color = new Color(1, 1, 1, 1); //Fade Out
                                                         //  Combo_UI.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y+1, Combo_UI.transform.position.z);//Combo UI 시작 위치 
            Combo_Object.transform.position = new Vector3(Player_Position.transform.position.x+1, Player_Position.transform.position.y-1, Player_Position.transform.position.z);//Combo UI 시작 위치 

            //날라가는 랜덤 값 
            dir = new Vector3(Random.Range(Player_Position.transform.position.x -2, Player_Position.transform.position.x +3), Random.Range(Player_Position.transform.position.y - 80, Player_Position.transform.position.y + 300), -1).normalized;
           // Debug.Log("dir" + dir.x +"dir.y"+dir.y);

       
           Combo_Object.transform.localScale = new Vector3(UI_Scale, UI_Scale, UI_Scale);

            if (Count_Combo >=0 ) //
            {
                Combo_Object.SetActive(true);

            }
            
           
            //콤보 값 올려줌 //스킬값 올려줌 
            Count_Combo += 1;
            Count_Combo_Skill += 1;
            Fever_Combo += 1;
            BeatDicision.Combo_true = false;

            //ComboText_Set();          
        }
        if (BeatDicision.Combo_false== true && GameMannager.Out_Time == true) //대기 상태가 아닌 miss 
        {
            Fever_Combo = 0;
            Count_Combo = 0;
            Count_Combo_Skill = 0;
            BeatDicision.Combo_false = false;
            BeatDicision.Miss_Wating = true;
        }
        if(BeatDicision.Combo_false=false&& GameMannager.Out_Time == true) //웨이팅 상황일때 
        {
            Debug.Log("잘 찍히고 있음");
            BeatDicision.Combo_false = false;
            BeatDicision.Miss_Wating = true;
        }

        if (Count_Combo_Skill >= 9) //콤보 90이 가득차면 스킬 사용 가능 (10번 연속 성공하면)
        {  // 90으로 해야 비트에 맞춰서 앞에 몬스터가 있을때 콤보 100되면서 스킬씀, 100으로 하면 101때 공격함
            Count_Skill +=1;
            Count_Combo_Skill = 0;
        }
      
    }
    void fill_Skill()//콤보가 쌓임과 동시에 스킬을 쓸 수 있는지 
    {
        if (Count_Combo_Skill >= 1)
        {
            Skill_effect.fillAmount = Count_Combo_Skill * 0.1f + 0.1f;  //0.1f는 9번으로 Max를 채우기 위함
        }
        if (Count_Combo_Skill < 1)
        {
            Skill_effect.fillAmount = 0;
        }
        

    }

    void ComboText_Set() //콤보 UI 뜨고 끄게 만드는 것 (일정 시간이 지나면 콤보UI가 사라지는 것)
    {
        Combo_UI.gameObject.SetActive(true);

        if (Combo_UI.gameObject.activeSelf == true)
        {
            Invoke("ComboText_Delete", 0.4f); //현재 UI가 작동 된 후 1초 뒤에 꺼짐 
        }

    }
    void ComboText_Delete() //UI를 끄는 함수 ComboText_Set의 Invoke에 들어갈 함수임
    {
        Combo_UI.gameObject.SetActive(false);
    }

    void Combo_Effect_Create() //콤보가 쌓이면 불꽃 && 콤보 개수에 따라 점수배율 
    {
        ParticleSystem.MainModule particleMain = Combo_effect.main;

        if (Count_Combo >= 20&&fever==false) //콤보가 20이상/ 피버가 아니어야 작동
        {
            Combo_effect.gameObject.SetActive(true);
            if (Count_Combo >= 20 && Count_Combo < 30)//콤보가 20개 이상일때 파란색
            {
                
                particleMain.startColor = new Color(0, 0, 1f);
            }
            else if (Count_Combo >= 30 && Count_Combo < 40) //콤보가 30개 이상일때 초록색
            {
               
                particleMain.startColor = new Color(0, 1f, 0);
            }
            else if (Count_Combo >= 40 && Count_Combo < 50) //콤보가 40개 이상일때 노란색 
            {
               
                particleMain.startColor = new Color(0.2f, 0.2f, 0);
            }
            else if (Count_Combo >= 50 && Count_Combo < 100) //콤보가 50개 이상일때 주황색 
            {
              
                particleMain.startColor = new Color(0.6f, 0.2f, 0.2f);
            }
            else if (Count_Combo >= 100)//콤보가 100개 이상일때 빨간색
            {
               
                particleMain.startColor = new Color(1f, 0f, 0);
            }
            

        }
        else if (Count_Combo > 20)
        {
            Combo_effect.gameObject.SetActive(false);
        }

    }
    void Fever_Text()
    {
        Fever_Text_UI.gameObject.SetActive(false);
    }

    void Fever_Count()
    {
        if (Fever_Combo >= 40)
        {
            Fever_effect.Play();
            Fever_Text_UI.gameObject.SetActive(true);
            Invoke("Fever_Text", 0.7f);
            Invoke("Fever_Cut",10f);
            Fever_Text_UI2.gameObject.SetActive(true);
            fever = true;//피버 상태일때 fever를 true로 만들어줘서 불꽃이 작동 되지 않게 한다.
            Fever_Combo = 0;
        }
    }

    void Fever_Cut()
    {
        Fever_Text_UI2.gameObject.SetActive(false);
        Fever_effect.Stop();
        fever = false;
    }

    void Score_ma_Count()//콤보 개수에 따른 점수배율
    {
        Magni_Text_UI2.text = "X" + Score_Magni;
       
        if (Count_Combo <= 20) {
            Score_Magni = 1;
            Magni_Text_UI2.gameObject.SetActive(false);
           
            
        }
        else if (Count_Combo > 20 && Count_Combo <=30&&fever!=true)//콤보가 20개 이상일때 파란색
        {
            Score_Magni = 1.15f;
            Magni_Text_UI2.gameObject.SetActive(true);

        }
        else if (Count_Combo > 30 && Count_Combo <= 50 && fever != true) //콤보가 30개 이상일때 초록색
        {
            Score_Magni = 1.3f;
           
        }
        else if (Count_Combo > 50 && Count_Combo <= 70 && fever != true) //콤보가 50개 이상일때 노란색 
        {
            Score_Magni = 1.6f;
           
        }
        else if (Count_Combo > 70 && Count_Combo <=90 && fever != true) //콤보가 70개 이상일때 주황색 
        {
            Score_Magni = 2.0f;
            
        }
        else if (Count_Combo >90 && fever != true)//콤보가 90개 이상일때 빨간색
        {
            Score_Magni = 3.0f;
            
        }
        else if (fever==true)//콤보가 90개 이상일때 빨간색
        {
            Score_Magni = 5.0f;

        }
    }
   

}
