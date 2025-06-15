using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Gladiators.Main {
    public class Props_Attribute : IProps {
        public PropsType Type { get; set; }
        public string Name { get; set; }
        public string SpriteName { get; set; }
    }
}