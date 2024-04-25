using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Coin : MonoBehaviourPun, IItem
{
    public int score = 200; // 코인 아이템은 원래 돈이지만 우리는 점수 값 올리는 데에 씀

    public void Use(GameObject target)
    {
        GameManager.Instance().AddScore(score); // 점수 올리고
        
        //Destroy(gameObject);    // 사용했으니까 게임 오브젝트 삭제

        PhotonNetwork.Destroy(gameObject);
    }
}
