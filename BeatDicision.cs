using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeatDicision : MonoBehaviour //Beat 판정
{
    [Header("BeatDicision Standard")]//판정기준
    public static float Stand_M;//기준-0.1
    public static float Stand_P;//기준+0.1
    [Header("BeatHandle")]
    [SerializeField]
    private float DicisionGuage; //판정을 위한 handle 게이지 

    [SerializeField]
    private Image BeatColor; //뒤에 판정 
    [Header("BeatDicision Text")]
    [SerializeField]
    private TextMeshProUGUI BDT; //판정 텍스트

    [Header("BeatDicision")] //Player스크립트에서 참조 
    public static bool Go ; // 전진이나 방어 등의 버튼의 능력 사용가능 (Player)
    public static bool No ;//전진,방어 불가능 (Player)

    [Header("ComboDicision")]
    public static bool Combo_true ;  //콤보 쌓임. (Combo Script)
    public static bool Combo_false; //콤보 0으로 만든다. (Combo Script)

    public static bool MonsterBeat;         //몬스터의 한박을 나타냄
    public static bool ObsBeat; // 가시A 비트
    public static bool ObsBeat2; // 가시B 비트
    public static bool FallObsBeat; // 낙석 비트
    public static bool FallObsBeat2; // 낙석 떨어지는 비트

    [Header("BeatDicision State")]
    public bool P_M = false;//perfect Miss 판정 

    [Header("BeatHandle")]
    public static bool State_1 ;// L->R /R->L 넘어가는 상황
    public static bool State_2 ; //R->L/ L->R 넘어가는 상황


    [Header("BeatEffect")]
    public ParticleSystem Perfect_Effect;
    // public GameObject Effect_Position;

    [Header("Wating_Miss")] //대기 할때 Miss안뜨게 
    public static bool Miss_Wating = true;

    public bool state;//박자 입력했는지 
    private void Awake()
    {
        
        Miss_Wating = true;
        Go = false;
        No = false;
        Combo_true = false; 
        Combo_false = false;
        MonsterBeat=false;
        ObsBeat = false;
        ObsBeat2 = false;
        FallObsBeat = false;
        FallObsBeat2 = false;
        P_M = false;
        State_1 = true;
        State_2 = false;

        state = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

        //Debug.Log("Stand_P" + Stand_P);
        // text3.text = "Stage1:" + State_1 + "/ Stage2:" + State_2;
        if (BeatSetting.first == false && BeatSetting.Handle == false && BeatSetting.Ga >= Stand_M && BeatSetting.Ga <= Stand_P && Button1.DicisionButton == true && State_1 == true && state == false & P_M == false&&GameMannager.Count_State!=true)
        {
            //Debug.Log("퍼펙");
            // text2.text = "State1_perfect";
            Perfect();
            P_M = true;
            state = true;
           
          


        }//퍼펙트
        else if (BeatSetting.first == false && BeatSetting.Handle == true && BeatSetting.Ga >= Stand_M && BeatSetting.Ga <= Stand_P && Button1.DicisionButton == true && State_2 == true && P_M == false && state == false && GameMannager.Count_State != true)
        {
          
            Perfect();
        
            P_M = true;
            state = true;
          
        }
        else if (BeatSetting.first == false && BeatSetting.Handle == false && Button1.DicisionButton == true && P_M == false && state == false&&State_1 == true && (BeatSetting.Ga > Stand_P && BeatSetting.Ga <= BeatSetting.Limit_t) && GameMannager.Count_State != true)
        {
         //   Debug.Log("MissState1" + Button1.DicisionButton + "first" + BeatSetting.first + "State_2" + State_2 + "state_1" + State_1 + "state" + state + "P_M" + P_M);
            //text2.text = "State1  입력 Miss";
            Miss();
            Miss_Wating = false; //Miss  
            state = true;
            P_M = true;


        }
        else if (BeatSetting.first == false && BeatSetting.Handle == true && Button1.DicisionButton == true && P_M == false && state == false&& State_2 == true && (BeatSetting.Ga >= 0 && BeatSetting.Ga < Stand_M) && GameMannager.Count_State != true)
        {
           // Debug.Log("MissState2" + Button1.DicisionButton + "first" + BeatSetting.first + "State_2" + State_2 + "state_1" + State_1 + "state" + state + "P_M" + P_M);
            Miss();
            Miss_Wating = false; //Miss  
            state = true;
            P_M = true;
        }

        else if (BeatSetting.first == true && Button1.DicisionButton == true)
        {
          //  Debug.Log("first true" + Button1.DicisionButton + "first" + BeatSetting.first + "State_2" + State_2 + "state_1" + State_1 + "state" + state + "P_M" + P_M);
            //text2.text = "첫박 Miss";
            Miss();
            Miss_Wating = false;
            state = true;
            P_M = true;

        }
        else if (BeatSetting.first == true && Button1.DicisionButton==false) //첫박이고 입력안했을때 반의 반박자
        {
         //   Debug.Log("first true none" + Button1.DicisionButton + "first" + BeatSetting.first + "State_2" + State_2 + "state_1" + State_1 + "state" + state + "P_M" + P_M);
            state = false;
            State_1 = true;
            P_M = false;
        }
        else if (BeatSetting.first == false && BeatSetting.Handle == false && BeatSetting.Ga < Stand_M && Button1.DicisionButton == false && State_1 == true && state == false && GameMannager.Count_State != true)
        {
          //  Debug.Log("R->L안쳐서 Miss" + Button1.DicisionButton + "first" + BeatSetting.first + "State_2" + State_2 + "state_1" + State_1 + "state" + state + "P_M" + P_M);
            //text2.text = "R->L 안쳐서 Miss";
            Miss();
            Miss_Wating = true; //Miss떠도 대기콤보 유지  
            Button1.DicisionButton = false;
            State_1 = false;
            State_2 = true;
            P_M = false;
            MonsterBeat = true;
            ObsBeat = true;
            ObsBeat2 = true;
            FallObsBeat = true;
            FallObsBeat2 = true;

        }

        else if (BeatSetting.first == false && BeatSetting.Handle == false && BeatSetting.Ga < Stand_M && Button1.DicisionButton == true && State_1 == true && P_M == true && state == true && GameMannager.Count_State != true)
        {
            
            Button1.DicisionButton = false;
            State_1 = false;
            State_2 = true;
            P_M = false;
            state = false;
            
        }


        else if (BeatSetting.first == false && BeatSetting.Handle == true && BeatSetting.Ga > Stand_P && Button1.DicisionButton == false && State_2 == true && state == false && GameMannager.Count_State != true)
        {
            Miss();
            Miss_Wating = true; //Miss떠도 대기콤보 유지  

            // Debug.Log("L->R안쳐서 Miss" + Button1.DicisionButton + "first" + BeatSetting.first + "State_2" + State_2 + "state_1" + State_1 + "state" + state + "P_M" + P_M);
            //text2.text = "R->L 안쳐서 Miss";
            // text2.text = "L->R안쳐서 Miss";
            Button1.DicisionButton = false;
            State_1 = true;
            State_2 = false;
            P_M = false;
            MonsterBeat = true;
            ObsBeat = true;
            ObsBeat2 = true;
            FallObsBeat = true;
            FallObsBeat2 = true;
           

        }

        else if (BeatSetting.first == false && BeatSetting.Handle == true && BeatSetting.Ga > Stand_P && Button1.DicisionButton == true && State_2 == true && P_M == true && state == true && GameMannager.Count_State != true)
        {
            Button1.DicisionButton = false;
            State_1 = true;
            State_2 = false;
            P_M = false;
            state = false;
            
        }




    }


    void Dicision_Text() //Text출력하는 함수 
    {
        BDT.gameObject.SetActive(false);
    }

    void Perfect()
    {
        if (Perfect_Effect.isPlaying)
        {
            Perfect_Effect.Stop();
            Perfect_Effect.Play();
        }
        else if (Perfect_Effect.isStopped)
        {
            Perfect_Effect.Play();
        }
       
        //Instantiate(Perfect_Effect,Effect_Position.transform.position,Quaternion.identity);
        //Debug.Log("Beat" + GameMannager.Count_State);
        if (GameMannager.StartDicision != true)
        {
            BDT.gameObject.SetActive(true);

            BDT.color = new Color(0.0f, 0.0f, 0.8f);
            BDT.text = "Perfect";
            Invoke("Dicision_Text", 0.4f);
            Combo_true = true;
            BeatColor.GetComponent<Image>().color = new Color(0, 0.1f, 1f, 0.5f);

        }

        No = false;
        Go = true;
        //Debug.Log("여긴 BeatDicision Attack" +Button1.Attack + ", Monster" + MonsterScr.PlaState2 + ", GO" + Go);
        if (MonsterScr.Monsterlife>1||MonsterScr2.Monsterlife>1||MonsterScr3.Monsterlife>1) //몬스터 라이프가 1 이상일때 
        {
            MonsterBeat = true;
            ObsBeat = true;
            ObsBeat2 = true;
            FallObsBeat = true;
            FallObsBeat2 = true;
        }
        if (Button1.Attack == true && (MonsterScr.Monsterlife == 1 || MonsterScr2.Monsterlife == 1 || MonsterScr3.Monsterlife == 1)) //몬스터 라이프가 1일때 && 플레이어가 공격 눌렀을때 
        {
            MonsterBeat = false;
            ObsBeat = false;
            ObsBeat2 = false;
            FallObsBeat = false;
            FallObsBeat2 = false;
        }
        if (Button1.Attack == false && (MonsterScr.Monsterlife == 1 || MonsterScr2.Monsterlife == 1 || MonsterScr3.Monsterlife == 1)) //몬스터 라이프가 1일때 && 플레이어가 공격을 누르지 않았을때 
        {
            MonsterBeat = true;
            ObsBeat = true;
            ObsBeat2 = true;
            FallObsBeat = true;
            FallObsBeat2 = true;
        }      
    }
  
    void Miss()
    {
        if (GameMannager.Count_State!= true)
        {
            //MissSound.instance.PlaySound(); // 맞는 사운드
            BDT.gameObject.SetActive(true);
            BDT.color = new Color(0.8f, 0.8f, 0.8f);
           
            if (Miss_Wating == false) //대기해서 틀린게 아닐 때 
            {
                BDT.text = "Miss";
                Invoke("Dicision_Text", 0.4f);
                Combo_false = true;
                if (Combo.Score_all >= 80)
                {
                    Combo.Score_part = -80; //점수
                    Combo.Score_all += Combo.Score_part; //전체 점수    부분*배율                    
                }
                if (Combo.Score_all < 80)
                {                 
                    Combo.Score_all = 0;                   
                }            
            }
            else if(Miss_Wating==true)
            {
                BDT.gameObject.SetActive(false);
                Combo_false = false;
            }
            
            BeatColor.GetComponent<Image>().color = new Color(0.8f, 0.1f, 0.1f, 0.8f);
        }
        Go = false;
        No = true;
        
        MonsterBeat = true;
        ObsBeat = true;
        ObsBeat2 = true;
        FallObsBeat = true;
        FallObsBeat2 = true;
        //  if(MonsterScr.Monsterlife == 1 || MonsterScr2.Monsterlife == 1 || MonsterScr3.Monsterlife == 1))
    }
}