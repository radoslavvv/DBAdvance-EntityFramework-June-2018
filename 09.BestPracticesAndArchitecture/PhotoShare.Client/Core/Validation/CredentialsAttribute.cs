using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoShare.Client.Core.Validation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CredentialsAttribute : Attribute
    {
        private bool mustBeLogedIn;

        public CredentialsAttribute(bool mustBeLogedIn)
        {
            this.mustBeLogedIn = mustBeLogedIn;

            if (mustBeLogedIn)
            {
                if(Session.User == null)
                {
                    throw new ArgumentException("Invalid Credentials!");
                }
            }
            else
            {
                if(Session.User != null)
                {
                    throw new InvalidOperationException("Invalid Credentials");
                }
            }
        }
    }
}
