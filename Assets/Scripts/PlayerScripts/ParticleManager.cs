using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticleType { Landing, DoubleJump, Running }
public class ParticleManager : MonoBehaviour
{
    [SerializeField] private GameObject DoubleJumpDustParticles;
    [SerializeField] private GameObject landOnGroundDustParticle;
    [SerializeField] private ParticleSystem RunningParticle;

    private void Start()
    {
        RunningParticle.Stop();
    }

    public void PlayDoubleJumpParticle()
    {
        Vector3 landOnGroundDustPartilePosition = new Vector3(transform.localPosition.x + 0.1f, transform.position.y + 0.1f, transform.position.z);
        Quaternion landOnGroundDustParticleRotation = Quaternion.Euler(90, 0, 0);
        GameObject DoubleJumpParticles = Instantiate(DoubleJumpDustParticles, landOnGroundDustPartilePosition, landOnGroundDustParticleRotation);
        SetRemoveParticles(DoubleJumpParticles);
    }
    public void PlayLandOnGroundParticle()
    {
        Vector3 landOnGroundDustPartilePosition = new Vector3(transform.localPosition.x + 0.1f, transform.position.y + 0.1f, transform.position.z);
        Quaternion landOnGroundDustParticleRotation = Quaternion.Euler(90, 0, 0);
        GameObject LandingParticles = Instantiate(landOnGroundDustParticle, landOnGroundDustPartilePosition, landOnGroundDustParticleRotation);
        SetRemoveParticles(LandingParticles);
    }
    public void PlayerRunningParticle()
    {
        RunningParticle.Play();
    }

    private void SetRemoveParticles(GameObject obj)
    {
        StartCoroutine(RemoveParticles(obj));
    }
    IEnumerator RemoveParticles(GameObject obj)
    {
        yield return new WaitForSeconds(1);
        Destroy(obj.gameObject);
    }
}
