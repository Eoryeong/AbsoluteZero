using UnityEngine;

public class IKControl : MonoBehaviour
{
    public Animator anim;
    public Transform leftHandMount;
    public Transform rightHandMount;

    private void OnAnimatorIK(int layerIndex)
    {
        // 0..1 범위의 가중치 값을 설정하여 IK가 조준 할 시작 위치와 목표 위치 사이의 거리를 결정합니다. 
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        //위치 자체는 SetIKPosition을 사용하여 별도로 설정됩니다 .
        anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }
}