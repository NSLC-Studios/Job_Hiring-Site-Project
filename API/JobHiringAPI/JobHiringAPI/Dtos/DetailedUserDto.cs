namespace JobHiringAPI.Dtos
{
    public class DetailedUserDto
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string About { get; set; }
        public bool Company { get; set; }
        public string ?Companies { get; set; }
        public string Role { get; set; }
    }
}