using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float LevelLoadDelay = 2f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision other)//물체와 부딪히면 OnCollisionEnter() 메서드가 호출
    {
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
        
        GetComponent<Movement>().enabled = false; //내가 작성한 Movement 코드를 비활성화시켜 Finish에 도착하면 못 움직임
        Invoke("LoadNextLevel", LevelLoadDelay);//Finish에 도착 후 LevelLoadDelay가 2초니깐 2초뒤에 LoadNextLevel()매서드(다음 단계) 호출
    }

    void StartCrashSequcence() //StartCrashSequcence() 컨트롤 . 눌러서 매서드 생성 클릭하면 나타남
    {
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
        SceneManager.LoadScene(currentScene); // SceneManager.LoadScene(0)괄호의 0은 인덱스를 의미함
                                                                                        // 인덱스란? 요약하면 씬의 순서임 0은 첫번째 1은 두번째.....
    }
}

