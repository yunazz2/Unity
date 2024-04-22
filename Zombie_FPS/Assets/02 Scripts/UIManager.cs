using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // ΩÃ±€≈Ê ∆–≈œ
    private static UIManager instance;
    public static UIManager Instance()
    {
        return instance;
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        
    }

    public void UpdateAmmoText(int magAmmo, int ammoRemain)
    {

    }
}
