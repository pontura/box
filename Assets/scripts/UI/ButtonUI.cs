using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonUI : MonoBehaviour
{
    int id;
    System.Action<int> OnClick;

    public void Init(System.Action<int> OnClick, int id = 0)
    {
        this.id = id;
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => { OnClick(id); });
    }
}
