using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyPrefab;

    public FMODUnity.StudioEventEmitter music;

    public beat beatMarker;

    int score;

    int scoreMult;

    float heat;

    public int maxMult;

    public float heatPerMult;

    public float heatMult;

    public float heatlostfreq;

    public float bpm;

    public float onBeatInterval;

    float beat_time;

    float freq;

    float heat_time;

    void Start()
    {
        heat_time = heatlostfreq;
        scoreMult = 1;
        heat = 0;
        score = 0;

        float bps = bpm / 60f;
        freq = 1 / bps;
        beat_time = 0;
    }
    void Update()
    {
        if (heat_time <= 0)
        {
            AddHeat(-scoreMult *heatMult * heatPerMult * 0.01f);
            heat_time = heatlostfreq;
        }

        heat_time -= Time.deltaTime;

        float beatprc = beat_time / freq;

        beatMarker.setScale(beatprc);

        beat_time += Time.deltaTime;

        if (beat_time >= freq)
            beat_time = 0;
    }
    public void AddHeat(float amount)
    {
        heat += amount;
        if (heat > scoreMult * heatMult * heatPerMult)
        {
            if (scoreMult < maxMult)
            {
                scoreMult++;
                music.SetParameter("Intensity", scoreMult - 1);
            }
            else
            {
                heat = score * heatMult * heatPerMult;
            }
        }
        if (heat <= 0)
        {
            heat = 0;
            if (scoreMult > 1)
            {
                scoreMult--;
                music.SetParameter("Intensity", scoreMult - 1);
                heat = scoreMult * heatMult * heatPerMult;
            }
        }
    }
    public void AddScore(int points)
    {
        score += points * scoreMult;
    }
    public GameObject getPlayer()
    {
        return player;
    }
    public int getScore()
    {
        return score;
    }

    public int getScoreMult()
    {
        return scoreMult;
    }

    public bool onBeat()
    {
        float b = beat_time / freq;

        return (b <= 1 && b >= 1 - onBeatInterval/2.0 || b >= 0 && b <= onBeatInterval/2.0);
    }
}
