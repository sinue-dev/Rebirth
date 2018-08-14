using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebirth.Prototype
{
    public class BagEventArgs : EventArgs
    {
        public BagEventArgs(ItemBase item)
        {
            Item = item;
        }

        public ItemBase Item;
    }
}