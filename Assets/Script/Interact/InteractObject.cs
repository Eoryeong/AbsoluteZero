using UnityEngine;

public enum InteractionType
{
    None,
    PickupItem,
    Door,
    Chest,
    Obstacle,
    Bed,
    AreaChange
}

public class InteractObject : MonoBehaviour
{
    public string objectName;
    public InteractionType objectType;

    private void Start()
    {
        InteractNameUpdate();
    }

    public void TryInteractObject()
    {
        switch (objectType)
        {
            case InteractionType.PickupItem:
                GetComponent<PickupItem>().TryPickupItem();
                break;
            case InteractionType.Obstacle:
                GetComponent<Obstacle>().TryObstacleBreak();
                break;
            case InteractionType.Bed:
                GetComponent<ObjectBed>().TryUseBed();
                break;
        }
    }

    public string InteractNameUpdate()
    {
        string name = objectName;
        if (objectType == InteractionType.PickupItem)
        {
            name = GetComponent<PickupItem>().data.itemName;
            objectName = name;
        }

        return name;
    }
}
