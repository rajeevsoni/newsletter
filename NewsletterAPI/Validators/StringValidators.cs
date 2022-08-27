namespace NewsletterAPI.Validators
{
    public static class StringValidators
    {
        public static bool IsValidEmail(this string email)
        {
            if(string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            return true;
        }
    }
}
