using Unity.VisualScripting;
using UnityEngine;


public class PlayerColliderManager : MonoBehaviour
{
    private CoinManager coinManager;

    private AudioSource audioSource;
    public AudioClip CoinUpSound;
    public AudioClip HitSound;

    public GameManager gameManager;

    
   

    void Start(){
        coinManager = FindObjectOfType<CoinManager>();
        audioSource=GetComponent<AudioSource>();

        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Sola"))
        {
           Destroy(collision.gameObject); // Deactivate the obstacle when it collides with the player
           coinManager.AddPoint(1);
           audioSource.PlayOneShot(CoinUpSound);
        }

        
         if (collision.gameObject.CompareTag("Hit"))
        {
          
          
           audioSource.PlayOneShot(HitSound);
           gameManager.GameOver();
        }
    }
}
