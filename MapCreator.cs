using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    [Header("Tile Position_z")]
    public float Position_z = -2; 

    [Header("Tile_Prefab")]
    public GameObject Prefab;  // 타일 프리팹들
    [Header("Tile End")]
    [SerializeField]
    private GameObject EnD_Tile; //타일의 끝을 알려주는 타일 
    [SerializeField]
    private GameObject First_Tile;//더 뒤로 못가게 해주는 타일

    private float EndTile_x = 0; 
    private float EndTile_y = 0;

    [SerializeField]
    float End_tile_pos_x;
    [SerializeField]
    float End_tile_pos_y;
    [SerializeField]
    float first_tile_pos_x;
    [SerializeField]
    float first_tile_pos_y;




    [Header("Tile Total_Size:")]
    [SerializeField]
    private int Size = 0;

    [Header("Tile Size:")]
    [SerializeField]
    private int[] Tile_Size = null;//타일 하나하나 개수 
    [Header("Tile_Position:")]

    [SerializeField]
    public float[] position_x = null; //일반 타일 위치 


    [SerializeField]
    public float[] position_y = null;
    //



    void Start()
    {
        Create();
    }

    void Create()
    {
        for (int k = 0; k < Size; k++)//1번째 타일 ~SIZE 
        {
            for (int j = 0; j < Tile_Size[k]; j++)
            {
                if (j == 0)
                {
                    GameObject end_1 = Instantiate(First_Tile) as GameObject;
                    First_Tile.transform.position = new Vector3(position_x[k]+first_tile_pos_x, position_y[k]+first_tile_pos_y, 0.5f);
                    //First_Tile.transform.position = new Vector3(position_x[k]/*+5f*/+ 8f, position_y[k]+0.5f, 0.5f); ->stage1
                    //Debug.Log("dd" + position_x[k]);
                }
                GameObject obj = Instantiate(Prefab) as GameObject;
              
               
                position_x[k] = position_x[k] + 3.2f;
                Prefab.transform.position = new Vector3(position_x[k], position_y[k], 0.5f);


                if (j >= Tile_Size[k] - 1)
                {
                    EndTile_x = position_x[k] + End_tile_pos_x;
                    EndTile_y = position_y[k] + End_tile_pos_y;
                    /*
                    EndTile_x = position_x[k] + 5; ->stage1 script
                    EndTile_y = position_y[k] + 2.0f;*/
                    GameObject end = Instantiate(EnD_Tile) as GameObject;
                    EnD_Tile.transform.position = new Vector3(EndTile_x, EndTile_y, 0.5f); 

                }
            }



        }


    }


}
