﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGallery
{
    public static class Projects
    {
        private static IProjectMeta[] list = [
            new Tic_Tac_Toe.Project(),
            new Memory_Game.Project(),
            new Pokedex.Project(),
            new PhotoGallery.Project(),
            new Uno_Game.Project(),
            new Pong.Project()
        ];
        private static IProjectMeta[] extraList = [
            new EmployeeManager.Project(),
            new JokeApp.Project()
        ];
        public static IProjectMeta[] List { get => list; }
        public static IProjectMeta[] ExtraList { get => extraList; }
    }
}
