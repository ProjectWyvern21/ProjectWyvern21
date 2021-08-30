using UnityEngine;

public class PageMenu : MonoBehaviour
{
    public GameObject PageUI; 
    public void ClosePage()
    {
        Cursor.visible = false;
        Destroy(PageUI);
    }
}
