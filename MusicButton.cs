using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MusicButton : MonoBehaviour
{

    // public GameObject CameraPo;
    public static int Current_Count; //현재 있는 씬 위치 
    public static bool N_BButton ; //다음 넘어가는 버튼 눌렀는지 

    public GameObject Player;
    public Animator ani;

    public Image Next_Effect; //다음 버튼 눌렀을때 반응 (버튼 이미지)
    public Image Before_Effect; // 이전 버튼 눌렀을때 반응 (버튼 크기가 작아지는) 
    static float Current_Position; // 현재 카메라 위치
    static float Move_Position; //움직여야하는 위치

    public float Velocity_x ; //넘어가는 속도
  
    public float SmoothTime ; //넘어가는 시간


    float MovePosition_X;  //움직이는 동작

    [SerializeField]
    private AudioSource Sound_Button;

    private int[] Current_Count_Position = {0,20,40,60,80,100,120};

 
    void Awake()//초기화
    {
       
        Time.timeScale = 1;
        Current_Count = 1;
        MovePosition_X = 0;
        Current_Position = 0f;
        Velocity_x = 2.0f;
        SmoothTime = 0.2f;
        N_BButton = true;
        Move_Position = 0f;



    }

    private void Start()
    {
        
    }

    // Start is called before the first frame update
    private void Update()
    {
        
        MovePosition_X = Mathf.SmoothDamp(this.transform.position.x, Move_Position, ref Velocity_x, SmoothTime); //카메라 부드럽게 이동 
        this.transform.position = new Vector3(MovePosition_X, this.transform.position.y, this.transform.position.z); //카메라 이동 

    }
    public void NextButton() //다음으로 넘어가는 버튼
    {
        Sound_Button.Play();

        ani.SetTrigger("Miss");
        Player.transform.Translate(0, 1f, 0);


        Next_Effect.transform.localScale = new Vector3(Next_Effect.transform.localScale.x, Next_Effect.transform.localScale.y - 0.03f, Next_Effect.transform.localScale.z);
        N_BButton = true;
        if (Current_Count < 8) //다음으로 넘어갈 수 있는 페이지가 있을때 
        {

            Current_Position = CurrentPosition(Current_Count);
            Current_Count =Current_Count+1;
            Move_Position = Current_Position+20;
        


        }

        if (Current_Count >= 8)//끝페이지에서 다음버튼 눌렀을때 
        {
            Current_Count = 1;
            Current_Position = gameObject.transform.position.x;
            Move_Position = 0;
            //CameraPo.transform.position = new Vector3(0, CameraPo.transform.position.y, CameraPo.transform.position.z);
          //  Debug.Log("ddd/" + Current_Count);
        }


    }
    public void Next_UP() //다음 버튼 눌렀다 뗄 때 다시 커지는 모션
    {
        Player.transform.Translate(0, -1f, 0);

        ani.SetTrigger("Idle"); // 멈춤 애니메이션
                                //S ani.SetBool("Jump",false);
        Next_Effect.transform.localScale = new Vector3(Next_Effect.transform.localScale.x, Next_Effect.transform.localScale.y + 0.03f, Next_Effect.transform.localScale.z);
    }
    public void BeforeButton(){//전으로 
        Sound_Button.Play();
        ani.SetTrigger("Miss");
        Player.transform.Translate(0, 1f, 0);
        //    Debug.Log("Velocity" + Velocity_x);
        //   Debug.Log("Move Position" + MovePosition_X);
        N_BButton = true;
        Before_Effect.transform.localScale = new Vector3(Before_Effect.transform.localScale.x, Before_Effect.transform.localScale.y - 0.03f, Before_Effect.transform.localScale.z);
        if (Current_Count>0) //첫번째 페이지 이상일때 
        {
            Current_Position = CurrentPosition(Current_Count);
            Current_Count--;
          
            Move_Position = Current_Position - 20;
         
         //   Debug.Log("ddd--" + Current_Count);
        }
        if (Current_Count <= 0) //첫페이지에서 이전 버튼을 눌렀을 때
        {
           
            Current_Count = 7;
            Current_Position = gameObject.transform.position.x;
            Move_Position = 120;
           
          //  Debug.Log("ddd|" + Current_Count);
        }

       

    }

    public void Before_Up()
    {
        ani.SetTrigger("Idle"); // 멈춤 애니메이션
                                //S ani.SetBool("Jump",false);
        Player.transform.Translate(0, -1f, 0);
        Before_Effect.transform.localScale = new Vector3(Before_Effect.transform.localScale.x, Before_Effect.transform.localScale.y + 0.03f, Before_Effect.transform.localScale.z);
    }
    public int CurrentPosition(int a) //카메라 위치값 제대로 해주기  
    {
        return Current_Count_Position[a - 1];
    }


}
