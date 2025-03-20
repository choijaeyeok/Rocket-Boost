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
    [SerializeField] AudioClip mainEngineSFX;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem rightThrustParticles;
    [SerializeField] ParticleSystem leftThrustParticles;
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
        if(thrust.IsPressed())//IsPressed()�� thrust��� InputAction��
                              //���� ���� �ִ��� Ȯ���ϴ� �ż���
        {
            StartThrusting();
        }
        else
        {
            StopThrusting   ();
        }
    }
    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
        //Vector3.up�� ���� ������Ʈ�� Y�� ��, ���ӿ�����Ʈ�� �������� ����� ���ǰ� �޶���
        //transform.up�� ���� ������Ʈ�� �׳� ��, ���� ������Ʈ�� �������� ���� �׳� ���ΰ�
        //Time.deltaTime: ���� ������ �ƴ�, ���� �������� ������ �������� �̵��̳� �ִϸ��̼� ���� ���� �� ���.
        //Time.fixedDeltaTime: ���� ����, ���� ��� �� �����̳� �ӵ� ���� �������� ��Ҹ� �ٷ� �� ���.

        if (!audioSource.isPlaying)//������� ��ġ�� �ʰ� ������� ������ �������� ������� ������ �ϴ� �ڵ� (������� �ϳ��� ������)
        {

            audioSource.PlayOneShot(mainEngineSFX);//�� ���� �ϳ��� ��� ����?���ο� �Ҹ� ��� �� ���� �Ҹ� �ߴ�(�������) �� Play()
                                                   //�� ���� �ϳ��� ��� ����?���� �Ҹ� ���ļ� ��� ����(ȿ���� EX �ѼҸ�) �� PlayOneShot()
        }
        if (!mainEngineParticles.isPlaying) //�̹� ���� ���� ��� �ٽ� �������� �����Ƿ� ���� ����ȭ�� �ڿ������� �ִϸ��̼� ������ ����
        {
            mainEngineParticles.Play();
        }
    }
    private void StopThrusting()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }
    private void ProcessRotation()
    {
        float rotaitioninput = rotation.ReadValue<float>();//ReadValue<float>()�� rotation�� ���� � �Է��� �ް�
                                                           //�ִ��� float ������ �д� �Լ�, ����: -1 ������ +1
        
        if(rotaitioninput < 0)
        {
            RotateRight();
        }
        else if (rotaitioninput > 0)
        {
            RotateLeft();

        }
        else // �Է��� ���� ��
        {
            StopRotating();
        }
    }
    private void RotateRight()
    {
        ApplyRotation(rotationStrength); // ��Ʈ�Ѱ� . �� ������ ���ο� �ż��� ���� ����
                                         //���� ��� ����rotationStrength�� �Ű������� �޴´� �׷� float rotationThisFrame �Ű������� �װ��� �̾�޴´�.
                                         //���� rotationStrength�� float rotationThisFrame�� ������� ������ �������. why? �ϳ��� �ż��忡 ���� ��ݵ����� ���� �̸��� ���� �ż��带 ������ ���ϴϱ�
                                         //�̸���  ApplyRotation()�� ������ ���� �뵵�� �ٸ��ϱ� �ٵ� ������ �ż���� ApplyRotation() �̰� �ϳ����̴� �������.
        if (!rightThrustParticles.isPlaying)
        {
            leftThrustParticles.Stop();
            rightThrustParticles.Play();
        }
    }
    private void RotateLeft()
    {
        ApplyRotation(-rotationStrength); //���� ��� ����rotationStrength�� -�� ���� �Ű�������
                                          //�޴´� �׷�float rotationThisFrame �Ű������� �װ��� �̾�޴´�.
        if (!leftThrustParticles.isPlaying)
        {
            rightThrustParticles.Stop();
            leftThrustParticles.Play();
        }
    }
    private void StopRotating()
    {
        leftThrustParticles.Stop();
        rightThrustParticles.Stop();
    }
    private void ApplyRotation(float rotationThisFrame)//rotationThisFrame�� ������ ������, ����� ���� ������ ��...
    {
        rb.freezeRotation = true; // ����ȸ��(ex �ε��� �� ȸ��) ���� �ڵ�
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime); //������ rotationStrength�� ��ȣ�� rotationThisFrame������ �߾��� ������
                                                                                     //rotationStrength���� rotationThisFrame������ �޾ұ� ������ rotationThisFrame���� �ٲ�����
        rb.freezeRotation = false; //  ����ȸ�� ���� �ڵ�
    }

}


