using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

// UI ��ư �̺�Ʈ ó��
public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button optionButton;
    public Button shopButton;

    private UnityAction action;

    private void Start()
    {
        // UnityAction�� ����� �̺�Ʈ ���� ���
        action = () => OnButtonClick(startButton.name);
        startButton.onClick.AddListener(action);

        // ���� �޼ҵ带 Ȱ���� �̺�Ʈ ���� ���
        optionButton.onClick.AddListener(delegate{OnButtonClick(optionButton.name);});

        // ���ٽ��� Ȱ���� �̺�Ʈ ���� ���
        shopButton.onClick.AddListener(() => OnButtonClick(shopButton.name));
    }
    public void OnButtonClick(string msg)
    {
        Debug.Log(msg);
    }
}
