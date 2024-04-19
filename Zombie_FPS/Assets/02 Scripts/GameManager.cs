using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글톤 패턴으로 GameManager 인스턴스 작성 : 다른 스크립트에서 가져다 쓰려고 이렇게 작성 함
    private static GameManager instance;
    public static GameManager Instance()
    {
        return instance;
    } // 근데 이렇게 한 번 더 감싸는 이유는 이 인스턴스를 통해 값을 받아가야하는데 안에 값을 바꾸지는 못하도록 보호하기 위해

    public bool isGameOver = false;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        
    }
}
