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
    private void OnEnable()//Ȱ��ȭ�� �� �ؾ� �� �۾��� ó���ϴ� �� ���
    {
        thrust.Enable(); // Enable()�� thrust�� InputAction�� Ȱ��ȭ���ִ� �ż���
        rotation.Enable();
    }
    private void FixedUpdate()//��������ó���� ���� �����
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed())//IsPressed()�� thrust��� InputAction��
                               //���� ���� �ִ��� Ȯ���ϴ� �ż���
        {
            rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
            //Vector3.up�� ���� ������Ʈ�� Y�� ��, ���ӿ�����Ʈ�� �������� ����� ���ǰ� �޶���
            //transform.up�� ���� ������Ʈ�� �׳� ��, ���� ������Ʈ�� �������� ���� �׳� ���ΰ�
            //Time.deltaTime: ���� ������ �ƴ�, ���� �������� ������ �������� �̵��̳� �ִϸ��̼� ���� ���� �� ���.
            //Time.fixedDeltaTime: ���� ����, ���� ��� �� �����̳� �ӵ� ���� �������� ��Ҹ� �ٷ� �� ���.
       
            if (!audioSource.isPlaying)//������� ��ġ�� �ʰ� ������� ������ �������� ������� ������ �ϴ� �ڵ� (������� �ϳ��� ������)
            {
               
                audioSource.PlayOneShot(mainEngine);// �������ó�� ��� ������ �Ҹ� �� Play()
                                                                                          //ȿ����ó�� ������ ���� �� �ݺ��� �Ҹ� �� PlayOneShot()
            }
        
        }
        else
        {
            audioSource.Stop();
        }
    } 
    private void ProcessRotation()
    {
        float rotaitioninput = rotation.ReadValue<float>();//ReadValue<float>()�� rotation�� ���� � �Է��� �ް�
                                                           //�ִ��� float ������ �д� �Լ�, ����: -1 ������ +1
        Debug.Log("here is our rotation value: " + rotaitioninput);
        if(rotaitioninput < 0)
        {
            ApplyRotation(rotationStrength); // ��Ʈ�Ѱ� . �� ������ ���ο� �ż��� ���� ����
            //���� ��� ����rotationStrength�� �Ű������� �޴´� �׷� float rotationThisFrame �Ű������� �װ��� �̾�޴´�.
           //���� rotationStrength�� float rotationThisFrame�� ������� ������ �������. why? �ϳ��� �ż��忡 ���� ��ݵ����� ���� �̸��� ���� �ż��带 ������ ���ϴϱ�
           //�̸���  ApplyRotation()�� ������ ���� �뵵�� �ٸ��ϱ� �ٵ� ������ �ż���� ApplyRotation() �̰� �ϳ����̴� �������.
        }
        else if (rotaitioninput > 0)                                  
        {
            ApplyRotation(-rotationStrength); //���� ��� ����rotationStrength�� -�� ���� �Ű�������
                                                                            //�޴´� �׷�float rotationThisFrame �Ű������� �װ��� �̾�޴´�.

        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // ����ȸ��(ex �ε��� �� ȸ��) ���� �ڵ�
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime); //������ rotationStrength�� ��ȣ�� rotationThisFrame������ �߾��� ������
                                                                                     //rotationStrength���� rotationThisFrame������ �޾ұ� ������ rotationThisFrame���� �ٲ�����
        rb.freezeRotation = false; //  ����ȸ�� ���� �ڵ�
    }

}


