﻿using UnityEngine;
using System.Collections;


public class UIObject : MonoBehaviour 
{
    protected Player _ownerPlayer;

    public virtual void BindPlayer(Player p)
    {
        this._ownerPlayer = p;
    }

    protected bool isInit = false;

    public virtual void Init() {
        
    }

    public virtual void Clear() {
        
    }

    public virtual void SetParentPanelDepth( int depth ) { 
    
    }
}