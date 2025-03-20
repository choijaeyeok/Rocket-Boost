using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] float speed;
    
    Vector3 startPosition;
    Vector3 endPosition;
    float movementFactor;
    void Start()
    {
        startPosition = transform.position;//transform는 오브젝트가 가지고 있는 위치, 회전, 크기 정보를 담고 있는 컴포넌트
        endPosition = startPosition + movementVector;//movementVector는 오브젝트가 움직일 방향과 크기를 결정
                                                     // 시작지점이 0.2,0 이고 movementVector가 0,8,0이면 endPosition은 0,10,0이다
    }
    void Update()
    {
        movementFactor = Mathf.PingPong(Time.time * speed, 1f);//0~1 사이에서 반복 0->1->0->1, Time.time는 게임이 시작된 후 경과한 시간
        transform.position = Vector3.Lerp(startPosition, endPosition, movementFactor); //movementFactor는 0~1 사이의 값으로, 현재 얼마나 이동했는지 비율을 결정
    }
}
