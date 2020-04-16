using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioClip arrow;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip fireball;
    [SerializeField] AudioClip gameover;
    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip level;
    [SerializeField] AudioClip newGame;
    [SerializeField] AudioClip rock;
    [SerializeField] AudioClip towerBuilt;

    public AudioClip Arrow
    {
        get
        {
            return arrow;
        }
    }
    public AudioClip Death
    {
        get
        {
            return death;
        }
    }
    public AudioClip Fireball
    {
        get
        {
            return fireball;
        }
    }
    public AudioClip GameOver
    {
        get
        {
            return gameover;
        }
    }
    public AudioClip Hit
    {
        get
        {
            return hit;
        }
    }
    public AudioClip Level
    {
        get
        {
            return level;
        }
    }
    public AudioClip NewGame
    {
        get
        {
            return newGame;
        }
    }
    public AudioClip Rock
    {
        get
        {
            return rock;
        }
    }
    public AudioClip TowerBuilt
    {
        get
        {
            return towerBuilt;
        }
    }
}
