using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioClipRefsSO : ScriptableObject
{
    public List<AudioClip> chop;
    public List<AudioClip> deliveryFail;
    public List<AudioClip> deliverySuccess;
    public List<AudioClip> footStep;
    public List<AudioClip> objectDrop;
    public List<AudioClip> objectPicked;
    public AudioClip stoveSizzle;
    public List<AudioClip> trash;
    public List<AudioClip> warning;
}
