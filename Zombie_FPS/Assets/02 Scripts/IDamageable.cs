using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    // 모든 플레이어들과 적들이 쓸 인터페이스
    
    // 총을 머리에 맞았는데 발에서 피가 나면 안되니까 맞은 위치를 저장하려고
    // 그리고 총을 앞에서 맞았으면 피가 뒤로 튀어야하는데 앞으로 튀면 안되니까 그런 것도 정해주려고
    void onDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);


}
