using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// </Internal Documentation>
///  Source File Name: FloatingPlatform
///  Luka Ivicevic 101131244
///  Date Last Modified : December 19, 2020
/// </Internal Documentation>

[System.Serializable]
public class FloatingPlatform : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public bool isActive;
    public float platformTimer;
    public float threshold;

    public PlayerBehaviour player;

    [Header("Audio Clips")]
    //public AudioSource audioSource;
    public AudioSource shrinkAudio;
    public AudioSource resizeAudio;

    private Vector3 distance;
    //Added below vectors
    [Header("Platform Size Vectors")]
    private Vector3 scaleChange;
    private Vector3 initScale;
    private Vector3 currScale;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>();
        //Added
        initScale = new Vector3(1, 1, 1);
        scaleChange = new Vector3(-0.05f, -0.05f, -0.05f);

        shrinkAudio.GetComponent<AudioClip>();
        resizeAudio.GetComponent<AudioClip>();
        //
        platformTimer = 0.1f;
        platformTimer = 0;
        isActive = false;
        distance = end.position - start.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            platformTimer += Time.deltaTime;
            _Move();
        }
        else
        {
            if (Vector3.Distance(player.transform.position, start.position) <
                Vector3.Distance(player.transform.position, end.position))
            {
                if (!(Vector3.Distance(transform.position, start.position) < threshold))
                {
                    platformTimer += Time.deltaTime;
                    _Move();
                }
            }
            else
            {
                if (!(Vector3.Distance(transform.position, end.position) < threshold))
                {
                    platformTimer += Time.deltaTime;
                    _Move();
                }
            }
        }
    }

    private void _Move()
    {
        var distanceX = (distance.x > 0) ? start.position.x + Mathf.PingPong(platformTimer, distance.x) : start.position.x;
        var distanceY = (distance.y > 0) ? start.position.y + Mathf.PingPong(platformTimer, distance.y) : start.position.y;

        transform.position = new Vector3(distanceX, distanceY, 0.0f);
    }

    public void Reset()
    {
        transform.position = start.position;
        platformTimer = 0;
    }
    /// <summary>
    /// Added below Functions for final
    /// </summary>
    //For testing
    //private void ShrinkPlatform(Vector3 scale)
    //{

    //    transform.localScale += scaleChange;
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            resizeAudio.Stop();
            Debug.Log("Player hit floating platform");
            StopCoroutine(ResizeTime());
            StartCoroutine(ShrinkTime());
            //ShrinkPlatform(scaleChange);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        shrinkAudio.Stop();
        Debug.Log("Set platfrom to BASE");
        StopCoroutine(ShrinkTime());
        StartCoroutine(ResizeTime());
        //ShrinkPlatform(-scaleChange);
    }

    IEnumerator ShrinkTime()
    {

        transform.localScale += scaleChange;
        currScale = transform.localScale;

        shrinkAudio.Play();
        Debug.Log(currScale);
        yield return new WaitForSeconds(0.5f);
        shrinkAudio.Stop();
        StartCoroutine(ShrinkTime());
    }

    IEnumerator ResizeTime()
    {
        //if (currScale.x <= initScale.x)
        //{

        //    transform.localScale -= scaleChange;
        //}
        //else if (currScale.x >= initScale.x)
        //{

        //    StopCoroutine(ResizeTime());
        //}
        //transform.localScale -= scaleChange;
        Debug.Log(currScale);
        resizeAudio.Play();
        yield return new WaitForSeconds(0.5f);
        resizeAudio.Stop();
        if (currScale.x >= initScale.x)
        {

            StopCoroutine(ResizeTime());
        }
        else if (currScale.x < initScale.x)
        {

            transform.localScale -= scaleChange;
            StartCoroutine(ResizeTime());
        }

        StartCoroutine(ResizeTime());
    }
}

