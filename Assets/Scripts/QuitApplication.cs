using UnityEngine;
using UnityEngine.InputSystem;

public class QuitApplication : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.escapeKey.isPressed)
        {
            Debug.Log("pressed esc");
            Application.Quit();//유니티에서 애플리케이션을 종료하는 함수
        }
    }
}
  