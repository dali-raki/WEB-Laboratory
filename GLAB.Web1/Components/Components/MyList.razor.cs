﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace GLAB.Web1.Components.Components
{
    public partial class MyList<T> : ComponentBase
    {
        [Parameter] public List<T> Items { get; set; }


    }
}
