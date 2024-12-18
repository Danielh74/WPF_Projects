﻿using PhotoGallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGallery.CustomEventArgs
{
    public class LoginEventArgs : EventArgs
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public List<User> UserList { get; set;}
    }
}
