using PhotoShare.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoShare.Client.Core
{
    public static class Session
    {
        private static User user;

        public static User User
        {
            get => user;
            set => user = value;
        }
    }
}
