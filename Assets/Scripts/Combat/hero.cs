using UnityEngine;

public class hero : MonoBehaviour
{
    [SerializeField]
    private GameObject selfGO;
    
    public unit getSelfUnit()
    {
        return selfGO.GetComponent<unit>();
    }

    public virtual int skill1(actionStep playerTSU)
    {
        Debug.Log("Skill 1 de base.");
        return 0;
    }
}
