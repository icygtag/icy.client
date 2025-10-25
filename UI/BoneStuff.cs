using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MalachiTemp.Utilities
{
    internal class BoneStuff
    {
        // put these near the top of Movement (class-level)
        private static readonly Dictionary<VRRig, List<LineRenderer>> _casualBoneESP = new Dictionary<VRRig, List<LineRenderer>>();
        private static readonly int[] _casualBones = {
    4, 3, 5, 4, 19, 18, 20, 19, 3, 18, 21, 20, 22, 21, 25, 21, 29, 21, 31, 29, 27, 25, 24, 22, 6, 5, 7, 6, 10, 6, 14, 6, 16, 14, 12, 10, 9, 7
};
        private static Material _casualLineMaterial;



        public static void GetIndex()
        {
            //hi
        }
    }
}