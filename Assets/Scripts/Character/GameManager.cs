using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int PlayerCharacter_Index;

    public List<CharacterManager> Characters = new List<CharacterManager>();

    public Dictionary<CharacterManager, Vector3> CharacterPoint_d = new Dictionary<CharacterManager, Vector3>();

    private bool isPointerOverUI = false;

    Vector3? startPoint_touch = null;

    public Camera mainCamera;

    public CharacterManager PlayerChatacter
    {
        get
        {
            return Characters[PlayerCharacter_Index];
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        for (int i = 0; i < Characters.Count; i++)
        {
            CharacterPoint_d.Add(Characters[i], Characters[i].transform.position);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            CharacterPoint_d.Clear();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TouchDownEvent(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            TouchUpEvent(Input.mousePosition);
        }
    }

    protected virtual void TouchDownEvent(Vector3 mousePosition)
    {
        isPointerOverUI = IsPointerOverUIObject((Vector2)mousePosition);
        startPoint_touch = mousePosition;
    }

    protected virtual void TouchUpEvent(Vector3 mousePosition)
    {
        if (isPointerOverUI)
        {
            return;
        }

        Vector3 dragDir = Vector3.zero;

        if (startPoint_touch != null)
        {
            dragDir = (Vector3)((mousePosition) - startPoint_touch);
        }

        for (int i=0; i<Characters.Count; i++)
        {
            CharacterManager character = Characters[i];

            Vector3? resultDest = character.Get_MovePoint(dragDir);

            if(resultDest != null)
            {
                character.DestPoint = resultDest;
                CharacterPoint_d[character] = (Vector3)resultDest;
            }
        }
    }


    protected bool IsPointerOverUIObject(Vector2 touchPos)
    {
        PointerEventData eventDataCurrentPosition
            = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = touchPos;

        List<RaycastResult> results = new List<RaycastResult>();


        EventSystem.current
        .RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
}
