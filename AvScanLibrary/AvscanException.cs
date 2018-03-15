using System;

namespace AvScanLibrary
{
    public class AvscanException : Exception
    {
        public AvscanException(string message) : base(message)
        {

        }

        public AvscanException(Exception ex) 
        {

        }

        public override string Message => base.Message;
    }
}