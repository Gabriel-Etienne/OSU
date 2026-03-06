using UnityEngine;

[System.Serializable]
public struct NoteToSpawn
{
    public NoteToSpawn(float newTimeForPerfect, Vector2 newPosition, Color newColor, int newNb)
    {
        timeForPerfect =  newTimeForPerfect;
        position = newPosition;
        color = newColor;
        nb = newNb;
    }
    
    public float timeForPerfect;
    public Vector2 position;
    public Color color;
    public int nb;
}


[System.Serializable]
public struct PosAndTime
{
    public PosAndTime(float newTimeForPerfect, Vector2 newPosition)
    {
        time =  newTimeForPerfect;
        position = newPosition;
    }
    
    public float time;
    public Vector2 position;
}
