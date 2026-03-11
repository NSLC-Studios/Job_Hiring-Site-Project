namespace JobHiringAPI.Dtos
{
    public class CreateRequestDto
    {
        public int ID { get; set; }
        public int CVID { get; set; }
        public int UserID { get; set; }
        //public string Status { get; set; }  
        public string ?Comment { get; set; }
    }
}