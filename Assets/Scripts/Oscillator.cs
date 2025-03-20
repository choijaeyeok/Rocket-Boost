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
        startPosition = transform.position;//transform�� ������Ʈ�� ������ �ִ� ��ġ, ȸ��, ũ�� ������ ��� �ִ� ������Ʈ
        endPosition = startPosition + movementVector;//movementVector�� ������Ʈ�� ������ ����� ũ�⸦ ����
                                                     // ���������� 0.2,0 �̰� movementVector�� 0,8,0�̸� endPosition�� 0,10,0�̴�
    }
    void Update()
    {
        movementFactor = Mathf.PingPong(Time.time * speed, 1f);//0~1 ���̿��� �ݺ� 0->1->0->1, Time.time�� ������ ���۵� �� ����� �ð�
        transform.position = Vector3.Lerp(startPosition, endPosition, movementFactor); //movementFactor�� 0~1 ������ ������, ���� �󸶳� �̵��ߴ��� ������ ����
    }
}
