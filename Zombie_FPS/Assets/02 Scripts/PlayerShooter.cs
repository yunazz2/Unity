// �� ��� ������ �� ��ũ��Ʈ
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Gun gun;

    public Transform gunPivot;          // ���� ������Ʈ�� ���� ���� ��ġ
    public Transform leftHandMount;     // �÷��̾��� �޼� ��ġ
    public Transform rightHandMount;    // �÷��̾��� ������ ��ġ

    public PlayerInput playerInput;     // �Է� ���� �޾Ƽ� �� �߻� ���� Ȯ��
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

    // ź�� UI
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
