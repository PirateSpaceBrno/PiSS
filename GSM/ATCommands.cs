namespace PiSS
{
    public class ATCommands
    {
        public const string CZ_Prefix = "+420";
        public const string Response = "AT\r";
        public const string About = "ATI\r";
        public const string AcceptCall = "ATA\r";
        public const string HangUpCall = "ATH\r";

        public static string CallNumber(string phoneNumber)
        {
            return (phoneNumber.StartsWith("+") ? $"ATD{phoneNumber}\r" : $"ATD{CZ_Prefix}{phoneNumber}\r");
        }

        public const string EOF = "\r";
    }
}
