using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    private Animator anim;
    private ParticleSystem ps;
    AudioSource m_audio;
    // Start is called before the first frame update
    IEnumerator Start()
    {
         string name = gameObject.name;
         anim = GetComponent<Animator>();
         ps = GameObject.Find(name+"PS").GetComponent<ParticleSystem>();
         m_audio = GetComponent<AudioSource>();
 

         while (true)
        {
            yield return new WaitForSeconds(Random.Range(10,15));
            anim.SetInteger("IdleIndex", Random.Range(0,6));
            anim.SetTrigger("Idle");
            if (anim.GetInteger("IdleIndex") == 4)
            {
                ps.Play();
                m_audio.Play();
            }
            

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idles.niesen"))
        //{
        //   m_audio.Play();
        //   ps.Play();
            
        //}
        
    }
}
