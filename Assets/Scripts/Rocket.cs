using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Rocket : MonoBehaviour
{
    //TODO create an array of available levels to traverse. 
    //TODO create an var for current level.  
    private enum State { Alive, Dying, Transcending }
    private State state = State.Alive; 
    private Rigidbody rigidBody;
    private AudioSource audioSource;
    [SerializeField] private float rcsThrust = 250f; 
    [SerializeField] private float mainThrust = 25f;
    [SerializeField] private float levelLoadDelay = 2f; 
    [SerializeField] private AudioClip thruster;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip success;
    [SerializeField] private ParticleSystem thrustParticle;
    [SerializeField] private ParticleSystem deathParticle;
    [SerializeField] private ParticleSystem successParticle;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>(); 
    }
    
    void FixedUpdate()
    {
        if (state == State.Alive)
        {
            CheckThrust();
            CheckRotate();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // Do nothing. 
                break;
            case "Finish":
                TranscendLevel(); 
                break;
            default:
                PlayerDied(); 
                break; 
        }   
    }

    private void TranscendLevel()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticle.Play(); 
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void PlayerDied()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticle.Play(); 
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void CheckThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // Thrust
            rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(thruster);
            }
            thrustParticle.Play(); 
        }
        else
        {
            audioSource.Stop();
            thrustParticle.Stop(); 
        }
    }

    private void CheckRotate()
    {
        rigidBody.freezeRotation = true; 
        if (Input.GetKey(KeyCode.A)) {
            // Rotate Left
            transform.Rotate(Vector3.forward * (rcsThrust * Time.deltaTime)); 
        }
        if (Input.GetKey(KeyCode.D))
        {
            // Rotate Right
            transform.Rotate(-Vector3.forward * (rcsThrust * Time.deltaTime)); 
        }
        rigidBody.freezeRotation = false; 
    }

    
}
