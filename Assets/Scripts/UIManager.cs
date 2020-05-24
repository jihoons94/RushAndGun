using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public void Btn_DirectionChange()
    {
        
        if (!GameManager.Instance)
        {
            Debug.LogError("GameManager is null");
            return;
        }

        if (!GameManager.Instance.PlayerChatacter)
        {
            Debug.LogError("CharcterController is null");
            return;
        }

        GameManager.Instance.PlayerChatacter.Change_CharacterDirByToggle();
    }

}
