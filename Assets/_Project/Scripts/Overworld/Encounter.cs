using UnityEngine;

public static class Encounter
{
    public static MonsterData leader;
    public static int enemyCount;
    public static Vector3 playerPosition;
    public static Quaternion playerRotation;
    public enum ReturnReason { None, Victory, Defeat }
    public static ReturnReason returnReason;
    public static float aggroResumeTime;
}
