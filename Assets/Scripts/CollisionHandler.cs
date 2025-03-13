using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float LevelLoadDelay = 2f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;
    
    AudioSource audioSource;

    bool isControllable = true;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision other)//��ü�� �ε����� OnCollisionEnter() �޼��尡 ȣ��
    {
        if (!isControllable) { return; }//isControllable ������ false�� �� OnCollisionEnter �Լ��� ������ ��� �ߴ��ϴ� ����
                                        //if�� �ȿ��� return;�� ����Ǹ�, �� �Լ��� �ٷ� ��. ��, �� �Ʒ� �ڵ���� ������� �ʴ´�
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Everything is looking good");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequcence();// ��Ʈ�� . ������ �ż��� ���� Ŭ�� //��ü�� �ε����� StartCrashSequcence() ȣ��
                                      //Invoke("ReloadLevel", 2f) = ReloadLevel()�� OnCollisionEnter() ���ο� �����ϸ� �� ��
                                      //ReloadLevel()�� OnCollisionEnter �Լ� �ȿ����� �����ϴ� �Լ�(���� �Լ�) �� ��.
                                      //������ Invoke("ReloadLevel", 2f); �� Ŭ���� ��ü���� ReloadLevel()�� ã�ƾ� �ϴµ�, �� ã��.
                                      //�׷��� "Ŭ���� ����� �ƴϹǷ� Invoke�� ã�� ����" �̶�� ������ �߻���.
                break;
        }
    }

    void StartSuccessSequence()
    {
        isControllable = false; //  if (!isControllable) { return; }�� isControllable = false;�� ���� ����Ǵϱ� isControllable = false;�� �ʿ���  
                                //������ ���¸� ��������� ���߰� �� �� �־� �浹 �� �Ҹ� �ߺ� ����� ������ �� �ֽ��ϴ�.
                                //   GetComponent<Movement>().enabled = false;�� ������ �� �̻� �������� �ʰ� ������, �浹 �̺�Ʈ�� ��� ������
        audioSource.PlayOneShot(success);
        GetComponent<Movement>().enabled = false; //���� �ۼ��� Movement �ڵ带 ��Ȱ��ȭ���� Finish�� �����ϸ� �� ������
        Invoke("LoadNextLevel", LevelLoadDelay);//Finish�� ���� �� LevelLoadDelay�� 2�ʴϱ� 2�ʵڿ� LoadNextLevel()�ż���(���� �ܰ�) ȣ��
    }

    void StartCrashSequcence() //StartCrashSequcence() ��Ʈ�� . ������ �ż��� ���� Ŭ���ϸ� ��Ÿ��
    {
        isControllable = false;
        audioSource.PlayOneShot(crash);
        GetComponent<Movement>().enabled = false;// ���� �ۼ��� Movement �ڵ带 ��Ȱ��ȭ���� ��ü�� �ε��� �� �� ������
        Invoke("ReloadLevel", LevelLoadDelay);// �ε��� �� LevelLoadDelay�� 2�ʴϱ� 2�ʵڿ� ReloadLevel()�ż���(�����) ȣ��                                      
    }

    void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex; //���� ��
        int nextScene = currentScene + 1; //  ���� ��
        if(nextScene == SceneManager.sceneCountInBuildSettings)// SceneManager.sceneCountInBuildSettings�� ��ü ���� ����� �˷���
                                                                //if (nextScene == SceneManager.sceneCountInBuildSettings)�� �ؼ� = ���� ���� ��ü ���� ������ ��Ȯ�ϴٸ�?
        {
            nextScene = 0; // ù ������ ����
        }
                SceneManager.LoadScene(nextScene); // ����  if���� �����̶�� ���������� �Ѿ �Ѹ���� �ε���0 1 2 012�ݺ�
    }
        
        
        
    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex; //���� Ȱ��ȭ�� ���� �ε��� ��ȣ�� ������
                                                                                //buildIndex�� ����ϸ� ���� �̸��� ���� �������� �ʰ�, �ε��� ��ȣ�� ���� �ε��ϰų� ������ �� �ִ�
        SceneManager.LoadScene(currentScene); // SceneManager.LoadScene(0)��ȣ�� 0�� �ε����� �ǹ��� �ε�����? ����ϸ� ���� ������ 0�� ù��° 1�� �ι�°.....
                                                                                    // ���ο� �� ���� ���: �����ִ� ���� �����ϰų� (��Ʈ�� D) ���콺 ��Ŭ�� create -> scene�����
                                                                                    // �ε��� ���� ���: ���ʻ�� file -> Build Profiles -> scenes List���� �ٲٸ��

    }
}

