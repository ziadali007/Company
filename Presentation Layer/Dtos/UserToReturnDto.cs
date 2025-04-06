namespace Presentation_Layer.Dtos
{
    public class UserToReturnDto
    {
        public String Id { get; set; }

        public String UserName { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }


        public String Email { get; set; }


        public IEnumerable<string>? Roles { get; set; }
    }
}
