﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using map2agbgui.Models.Main;

namespace map2agbgui
{ 

    public partial class App : Application
    {

        public static MainModel MainViewModel { get; set; }

        public App()
        {
           
        }

    }
}
