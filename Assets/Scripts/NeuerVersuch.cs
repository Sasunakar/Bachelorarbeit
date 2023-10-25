using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuerVersuch : MonoBehaviour
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
            yield return new WaitForSeconds(Random.Range(7,13));
            anim.SetTrigger("niesen");
            ps.Play();
            m_audio.Play();
            

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
