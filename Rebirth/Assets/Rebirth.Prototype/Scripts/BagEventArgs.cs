using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebirth.Prototype
{
    public class BagEventArgs : EventArgs
    {
        public BagEventArgs(Item item)
        {
            Item = item;
        }

        public Item Item;
    }
}