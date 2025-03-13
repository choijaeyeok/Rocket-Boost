using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustStrength = 100f;
    [SerializeField] float rotationStrength = 100f;
    [SerializeField] AudioClip mainEngine;
    Rigidbody rb;
    AudioSource audioSource;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()//활성화될 때 해야 할 작업을 처리하는 데 사용
    {
        thrust.Enable(); // Enable()는 thrust의 InputAction을 활성화해주는 매서드
        rotation.Enable();
    }
    private void FixedUpdate()//물리연산처리를 위해 사용함
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed())//IsPressed()는 thrust라는 InputAction이
                               //현재 눌려 있는지 확인하는 매서드
        {
            rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
            //Vector3.up는 게임 오브젝트의 Y축 위, 게임오브젝트가 기울어지면 위라는 정의가 달라짐
            //transform.up은 게임 오브젝트의 그냥 위, 게임 오브젝트가 기울어지던 말던 그냥 위로감
            //Time.deltaTime: 물리 연산이 아닌, 게임 로직에서 프레임 독립적인 이동이나 애니메이션 등을 만들 때 사용.
            //Time.fixedDeltaTime: 물리 연산, 예를 들어 힘 적용이나 속도 같은 물리적인 요소를 다룰 때 사용.
       
            if (!audioSource.isPlaying)//오디오가 겹치지 않게 오디오가 나오지 않을때만 오디오가 나오게 하는 코드 (오디오가 하나만 나오게)
            {
               
                audioSource.PlayOneShot(mainEngine);// 배경음악처럼 계속 유지될 소리 → Play()
                                                                                          //효과음처럼 빠르게 여러 번 반복될 소리 → PlayOneShot()
            }
        
        }
        else
        {
            audioSource.Stop();
        }
    } 
    private void ProcessRotation()
    {
        float rotaitioninput = rotation.ReadValue<float>();//ReadValue<float>()는 rotation이 현재 어떤 입력을 받고
                                                           //있는지 float 값으로 읽는 함수, 왼쪽: -1 오른쪽 +1
        Debug.Log("here is our rotation value: " + rotaitioninput);
        if(rotaitioninput < 0)
        {
            ApplyRotation(rotationStrength); // 컨트롤과 . 을 누르면 새로운 매서드 생성 가능
            //위에 썼던 변수rotationStrength을 매개변수로 받는다 그럼 float rotationThisFrame 매개변수가 그값을 이어받는다.
           //변수 rotationStrength와 float rotationThisFrame를 사용하지 않으면 오류뜬다. why? 하나의 매서드에 서로 상반되지만 같은 이름을 가진 매서드를 대입을 못하니깐
           //이름이  ApplyRotation()로 같지만 서로 용도가 다르니깐 근데 대입할 매서드는 ApplyRotation() 이거 하나뿐이니 오류뜬다.
        }
        else if (rotaitioninput > 0)                                  
        {
            ApplyRotation(-rotationStrength); //위에 썼던 변수rotationStrength에 -를 붙혀 매개변수로
                                                                            //받는다 그럼float rotationThisFrame 매개변수가 그값을 이어받는다.

        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // 물리회전(ex 부딪힐 때 회전) 막는 코드
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime); //원래는 rotationStrength가 괄호안 rotationThisFrame역할을 했었음 하지만
                                                                                     //rotationStrength값을 rotationThisFrame값으로 받았기 때문에 rotationThisFrame으로 바꿔써야함
        rb.freezeRotation = false; //  물리회전 가능 코드
    }

}


