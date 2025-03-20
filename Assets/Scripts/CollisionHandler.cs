using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float LevelLoadDelay = 2f;
    [SerializeField] AudioClip successSFX;// 변수 이름을 바꿀 때 같은 변수를 전부 한 번에 변경하는 단축키는 F2 또는 Ctrl + R, R
    [SerializeField] AudioClip crashSFX;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
    
    AudioSource audioSource;

    bool isControllable = true;
    bool isCollidable = true;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }
    private void Update()
    {
        RespondToDebugKeys();

    }
    void RespondToDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)//wasPressedThisFrame는 한번만 누른걸로 해줌
                                                                                          //isPressed는 계속 눌린걸로 취급됨
        {
            LoadNextLevel();
        }
        if (Keyboard.current.cKey.wasPressedThisFrame)//c키가 눌렸는지 확인하는 함수
        {
            isCollidable = !isCollidable;
            Debug.Log("C key was pressed");
        }
    }
    private void OnCollisionEnter(Collision other)//물체와 부딪히면 OnCollisionEnter() 메서드가 호출
    {
        if (!isControllable || !isCollidable) { return; }//isControllable 변수가 false일 때 OnCollisionEnter 함수의 실행을 즉시 중단하는 역할
                                                         //if문 안에서 return;이 실행되면, OnCollisionEnter 함수는 실행 중단. 즉, 그 아래 코드들이 실행되지 않는다
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Everything is looking good");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequcence();// 컨트롤 . 눌러서 매서드 생성 클릭 //물체와 부딪히면 StartCrashSequcence() 호출
                                      //Invoke("ReloadLevel", 2f) = ReloadLevel()을 OnCollisionEnter() 내부에 선언하면 안 됨
                                      //ReloadLevel()이 OnCollisionEnter 함수 안에서만 존재하는 함수(지역 함수) 가 됨.
                                      //하지만 Invoke("ReloadLevel", 2f); 는 클래스 전체에서 ReloadLevel()을 찾아야 하는데, 못 찾음.
                                      //그래서 "클래스 멤버가 아니므로 Invoke가 찾지 못함" 이라는 에러가 발생함.
                break;
        }
    }

    void StartSuccessSequence()
    {
        isControllable = false; //  if (!isControllable) { return; }는 isControllable = false;일 때만 실행되니깐 isControllable = false;가 필요함  
                                //로켓의 상태를 명시적으로 멈추게 할 수 있어 충돌 후 소리 중복 재생을 방지할 수 있습니다.
                                //   GetComponent<Movement>().enabled = false;는 로켓이 더 이상 움직이지 않게 되지만, 충돌 이벤트가 계속 감지됨
        audioSource.Stop(); //이거 안쓰면 부딪히거나 Finish에 도착해도 부스터소리가 계속남
        audioSource.PlayOneShot(successSFX);
        successParticles.Play();
        GetComponent<Movement>().enabled = false; //내가 작성한 Movement 코드를 비활성화시켜 Finish에 도착하면 못 움직임
        Invoke("LoadNextLevel", LevelLoadDelay);//Finish에 도착 후 LevelLoadDelay가 2초니깐 2초뒤에 LoadNextLevel()매서드(다음 단계) 호출
    }

    void StartCrashSequcence() //StartCrashSequcence() 컨트롤 . 눌러서 매서드 생성 클릭하면 나타남
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSFX);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;// 내가 작성한 Movement 코드를 비활성화시켜 물체와 부딪힌 후 못 움직임
        Invoke("ReloadLevel", LevelLoadDelay);// 부딪힌 후 LevelLoadDelay가 2초니깐 2초뒤에 ReloadLevel()매서드(재시작) 호출                                      
    }

    void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex; //현재 씬
        int nextScene = currentScene + 1; //  다음 씬
        if(nextScene == SceneManager.sceneCountInBuildSettings)// SceneManager.sceneCountInBuildSettings는 전체 씬이 몇개인지 알려줌
                                                                //if (nextScene == SceneManager.sceneCountInBuildSettings)의 해석 = 다음 씬이 전체 씬의 개수와 정확하다면?
        {
            nextScene = 0; // 첫 씬으로 리셋
        }
                SceneManager.LoadScene(nextScene); // 위에  if문의 부정이라면 다음씬으로 넘어감 한마디로 인덱스0 1 2 012반복
    }
        
        
        
    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex; //현재 활성화된 씬의 인덱스 번호를 가져옴
                                                                                //buildIndex를 사용하면 씬의 이름을 직접 지정하지 않고, 인덱스 번호로 씬을 로드하거나 관리할 수 있다
        SceneManager.LoadScene(currentScene); // SceneManager.LoadScene(0)괄호의 0은 인덱스를 의미함 인덱스란? 요약하면 씬의 순서임 0은 첫번째 1은 두번째.....
                                                                                    // 새로운 씬 생성 방법: 원래있던 씬을 복사하거나 (컨트롤 D) 마우스 우클릭 create -> scene들어가면됨
                                                                                    // 인덱스 설정 방법: 왼쪽상단 file -> Build Profiles -> scenes List에서 바꾸면됨

    }
}

