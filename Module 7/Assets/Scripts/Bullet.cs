using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public GameObject explosion;
	public HealthComponent h;
	public int damage = 10;
	
	void OnCollisionEnter(Collision col)
    {
    	GameObject e = Instantiate(explosion, this.transform.position, Quaternion.identity);
		HealthComponent h = col.gameObject.GetComponent<HealthComponent>();
		
		if (isValidTarget(col))
			h.TakeDamage(damage);
		if (col.gameObject.tag == "Enemy")
		{
			col.gameObject.GetComponent<Animator>().SetInteger("hp", h.curHealth);
		}
		
    	Destroy(e,1.5f);
    	Destroy(this.gameObject);
    }

	bool isValidTarget(Collision col)
	{
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "Enemy")
			return true;
		else
			return false;
	}
}
