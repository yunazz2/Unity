// 총 쏘는 행위를 할 스크립트
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Gun gun;

    public Transform gunPivot;          // 게임 오브젝트인 총이 있을 위치
    public Transform leftHandMount;     // 플레이어의 왼손 위치
    public Transform rightHandMount;    // 플레이어의 오른손 위치

    public PlayerInput playerInput;     // 입력 값을 받아서 총 발사 유무 확인
    public Animator playerAnimator;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        gun.gameObject.SetActive(true);
    }

    void Update()
    {
        if(playerInput.fire)
        {
            gun.Fire();
        }
        else if(playerInput.reload)
        {
            if(gun.Reload())
            {
                playerAnimator.SetTrigger("Reload");
            }
        }

        UpdateUI();
    }

    // 탄알 UI
    public void UpdateUI()
    {
        if (gun != null)
        {
            UIManager.Instance().UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
        }
    }

    public void OnAnimatorIK(int layerIndex)
    {
        gunPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }
}
