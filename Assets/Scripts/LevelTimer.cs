using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class LevelTimer : MonoBehaviour
{
    public static LevelTimer Instance;
    GameManager gm;
    TextMeshProUGUI score;
    Animator anim;
    Animator controller;
    int timeLeft = 145;
    float second = 1.0f;
    float multiplier = 1.0f;
    ColorState state = ColorState.Default;
    public bool tutorial;
    enum ColorState
    {
        Default,
        Green,
        Yellow,
        Red,
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gm = GameManager.Instance;
        score = GetComponent<TextMeshProUGUI>();
        anim = GetComponent<Animator>();
        controller = GetComponent<Animator>();
    }

    void Update()
    {
        // Update score
        score.text = timeLeft.ToString();

        // Color manager
        StateHandler();

        // Stop updating if players are holding hands
        if (timeLeft > 0 && !gm.playersTogether) Timer();
    }

    void Timer()
    {
        if (gm.tooFar && state != ColorState.Red)
        {
            // Insrease timer speed if players are too far from each other
            multiplier = 2.0f;
            controller.speed = 5.0f;
            state = ColorState.Yellow;
        }
        else if (state == ColorState.Yellow)
        {
            // Default speed
            multiplier = 1.0f;
            controller.speed = 1.0f;
            state = ColorState.Default;
        }

        if (tutorial) return;

        if (second >= 0f)
        {
            second -= Time.deltaTime * multiplier;
        }
        else
        {
            // Play pulse animation when subtracting time, then reset timer
            Pulse();
            timeLeft--;
            second = 1.0f;
        }
    }

    public IEnumerator SubtractTime()
    {
        // Turn text red
        state = ColorState.Red;
        score.color = Color.red;

        // Subtract seven points from timer
        for (int i = 0; i < 7; i++)
        {
            if (timeLeft > 0) timeLeft--;
            // Wait for 0.1 seconds before the next iteration
            yield return new WaitForSeconds(.1f);
        }

        // Turn text either yellow or magenta based on distance
        state = gm.tooFar ? ColorState.Yellow : ColorState.Default;
    }

    public IEnumerator AddTime()
    {
        // Turn text red
        state = ColorState.Green;
        score.color = Color.green;

        // Subtract seven points from timer
        for (int i = 0; i < 5; i++)
        {
            timeLeft++;
            // Wait for 0.1 seconds before the next iteration
            yield return new WaitForSeconds(.1f);
        }

        // Turn text either yellow or magenta based on distance
        state = gm.tooFar ? ColorState.Yellow : ColorState.Default;
    }

    void StateHandler()
    {
        if (state == ColorState.Red)
        {
            //score.color = Color.red;
        }
        else if (state == ColorState.Yellow)
        {
            score.color = Color.yellow;
        }
        else if (state == ColorState.Green)
        {
            //score.color = Color.green;
        }
        else
        {
            score.color = Color.magenta;
        }
    }

    void Pulse()
    {
        anim.SetTrigger("Tick");
    }
}
