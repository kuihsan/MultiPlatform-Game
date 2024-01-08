using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Linq;
using System.IO;

namespace DragonArts.Common {
    
    public class ScriptableItem : ScriptableObject {
        [HideInInspector]
        public string uid;
        [HideInInspector]
        public string label;
        [HideInInspector]
        public string description;
    }
}
