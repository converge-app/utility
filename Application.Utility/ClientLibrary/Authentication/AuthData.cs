namespace Application.Utility.ClientLibrary.Authentication
{
    public class AuthRegisterData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthRegisteredData
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class AuthData
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticatedData
    {
        public string Id { get; set; }
        public string Token { get; set; }
    }

}