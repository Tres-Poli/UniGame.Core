﻿using System;

namespace UniModules.UniGame.Core.Runtime.DataStructure
{
    [Serializable]
    public class SerializableStringsMap : 
        SerializableDictionary<string,string>
    {
        public SerializableStringsMap(int capacity) : base(capacity)
        {
            
        }
    }
}
