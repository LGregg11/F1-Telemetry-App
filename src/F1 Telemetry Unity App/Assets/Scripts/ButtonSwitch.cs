using UnityEngine;
using UnityEngine.UI;

public class ButtonSwitch : MonoBehaviour
{
    private bool isOn = false;

    public Button onButton;
    public Button offButton;

    public void SwitchClick()
    {
        isOn = !isOn;
        onButton.gameObject.SetActive(!isOn);
        offButton.gameObject.SetActive(isOn);
    }
}
