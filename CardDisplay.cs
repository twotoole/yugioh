using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    Renderer _renderer;
    Material[] _materials;

    // public Material cardArt;

    // Start is called before the first frame update
    void Awake()
    {
        _renderer = gameObject.GetComponent<MeshRenderer> ();
        setCardImage();
    }

   public void setCardImage()
    {
        _materials = _renderer.materials;
        _materials[2] = card.cardImage;
        _renderer.materials = _materials;
    }

}
