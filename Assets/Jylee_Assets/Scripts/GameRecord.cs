using UnityEngine;

public enum GameRecordEvent
{
    SurvivedTime,
    TraveledDistance,
    SleepTime,
    EatFood,
    DrinkWater,
    GunFire,
    ShootHit,
    SuccessHunt,
    Attacked,
    Test
}

public class GameRecode : MonoBehaviour
{
    public static GameRecode instance;

    public float totalSurvivedTime;
    public float totalTraveledDistance;
    public float totalSleepTime;
    public float totalEatFood;
    public float totalDrinkWater;
    public float totalGunFire;
    public float totalShootHit;
    public float totalSuccessHunt;
    public float totalAttacked;
    public float test;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void AddRecord(GameRecordEvent recordType, float amount = 1f)
    {
        switch (recordType)
        {
            case GameRecordEvent.SurvivedTime:
                totalSurvivedTime += amount;
                break;
            case GameRecordEvent.TraveledDistance:
                totalTraveledDistance += amount;
                break;
            case GameRecordEvent.SleepTime:
                totalSleepTime += amount;
                break;
            case GameRecordEvent.EatFood:
                totalEatFood += amount;
                break;
            case GameRecordEvent.DrinkWater:
                totalDrinkWater += amount;
                break;
            case GameRecordEvent.GunFire:
                totalGunFire += amount;
                break;
            case GameRecordEvent.ShootHit:
                totalShootHit += amount;
                break;
            case GameRecordEvent.SuccessHunt:
                totalSuccessHunt += amount;
                break;
            case GameRecordEvent.Attacked:
                totalAttacked += amount;
                break;
            case GameRecordEvent.Test:
                test += amount;
                break;
        }
    }

}
