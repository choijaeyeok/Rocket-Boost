using UnityEngine;
using UnityEngine.InputSystem;

public class QuitApplication : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.escapeKey.isPressed)
        {
            Debug.Log("pressed esc");
            Application.Quit();//����Ƽ���� ���ø����̼��� �����ϴ� �Լ�
        }
    }
}
  