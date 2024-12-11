﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Tankz_1
{
    interface IDrawable
    {
        bool Enabled { get; }
        DrawLayer Layer { get; }
        void Draw();
    }
}