using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EffectPlayFeedback : Feedback
{
    [SerializeField] List<ParticleSystem> particles;
    [SerializeField] List<VisualEffect> effects;

    public override void CreateFeedBack()
    {
        if(particles != null)
        {
            particles.ForEach(p => p.Play());
        }    
        if(effects != null)
        {
            effects.ForEach(p => p.Play());
        }
    }
    
    public override void FinishFeedBack()
    {
        if (particles != null)
        {
            particles.ForEach(p => p.Simulate(0));
        }
        if (effects != null)
        {
            effects.ForEach(e => e.Stop());
        }
    }
}
