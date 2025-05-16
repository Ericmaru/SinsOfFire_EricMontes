using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    private PolygonCollider2D _polygonCollider;
    private PlayerControl _playerControl;
    private float _cuchilloDamage = 0.35;

   void Awake()
   {
        _playerControl = gameObject<PlayerControl>;
   }
   
    void Start()
    {
        
    }
    
        void Update()
    {
        
    }
}
