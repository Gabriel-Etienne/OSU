using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct Partition
{
    public AudioSource song;
    public List<NoteToSpawn> notes;
}
