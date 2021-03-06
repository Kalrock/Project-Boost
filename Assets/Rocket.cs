﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;

    int currentLevel;
    int maxLevel;

    [SerializeField] float rcsThrust = 1000f;
    [SerializeField] float rcsRotation = 200f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] float mainEngineVolume = 1f;
    [SerializeField] AudioClip death;
    [SerializeField] float deathVolume = 1f;
    [SerializeField] AudioClip success;
    [SerializeField] float successVolume = 1f;

    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem successParticle;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    bool isCollisions = true;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        maxLevel = SceneManager.sceneCountInBuildSettings;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }

        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
	}

    private void RespondToDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKey(KeyCode.C))
        {
            if (isCollisions)
            {
                isCollisions = false;
            }
            else
            {
                isCollisions = true;
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive) { return; }
        if (!isCollisions) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        successParticle.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(success, successVolume);
        Invoke("LoadNextLevel", 1f); // todo parameterise time
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        deathParticle.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(death, deathVolume);
        Invoke("LoadLevel", 1f); // todo parameterise time
    }

    private void LoadNextLevel()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
        }
        else
        {
            currentLevel = 0;
        }
        SceneManager.LoadScene(currentLevel);
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
    {
        float thrustThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust(thrustThisFrame);
        }
        else
        {
            mainEngineParticle.Stop();
            audioSource.Stop();
        }
    }

    private void ApplyThrust(float thrustThisFrame)
    {
        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
        if (!audioSource.isPlaying)
        {
            mainEngineParticle.Play();
            audioSource.PlayOneShot(mainEngine, mainEngineVolume);
        }
    }

    private void RespondToRotateInput()
    {
        rigidBody.angularVelocity = Vector3.zero; // remove rotation due to physics

        float rotationThisFrame = rcsRotation * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
    }
}
