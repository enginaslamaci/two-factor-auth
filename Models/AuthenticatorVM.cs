namespace LoginWith2FA.Models
{
    public class AuthenticatorVM
    {
        public string Account { get; set; }
        public string ManualEntryKey { get; set; }
        public string QrCodeSetupImageUrl { get; set; }
        public string UserInputCode { get; set; }
    }
}
