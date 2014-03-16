using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Runner
{
    public class ShaderInfo
    {
        public Shader Shader;
        public float Distance;
        private UnityEngine.Object shader;

        public ShaderInfo(Shader shader)
        {
            Shader = shader;
        }

        public ShaderInfo()
        {

        }

        public ShaderInfo(Shader shader, float distance)
        {
            Shader = shader;
            Distance = distance;
        }
    }
}
