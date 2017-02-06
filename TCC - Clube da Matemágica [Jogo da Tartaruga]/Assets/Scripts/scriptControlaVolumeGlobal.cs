using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptControlaVolumeGlobal : MonoBehaviour {

    // Variável Global de Som
    public float volumeGlobal = 1.0F;

    public void aumentaVolumeGlobal()
    {
        volumeGlobal = Mathf.Min(volumeGlobal + 0.15F, 1.0F);
        AudioListener.volume = volumeGlobal;
    }
    public void diminuiVolumeGlobal()
    {
        volumeGlobal = Mathf.Min(volumeGlobal - 0.15F, 1.0F);
        AudioListener.volume = volumeGlobal;
    }
}
