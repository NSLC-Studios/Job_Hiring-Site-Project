using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.PortableExecutable;
using Microsoft.EntityFrameworkCore;

namespace JobHiringAPI.Persistence
{
    public class JobDatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<CV> CVs { get; set; }
        public DbSet<Area> Areas { get; set; }
        //public DbSet<Education> Educations { get; set; }
        public DbSet<Company> Companies { get; set; }
        //public DbSet<Branch> Branches { get; set; }
        public DbSet<Job> Jobs { get; set; }
        //public DbSet<Rating> Rating { get; set; }
        //public DbSet<PreviuosEmployment> PreviuosEmployments { get; set; }
        public DbSet<Request> Requests { get; set; }
        //public DbSet<AreaCollection> AreaCollections { get; set; }
        public JobDatabaseContext(DbContextOptions<JobDatabaseContext> options) : base(options) { }
    }

    [Index(nameof(UserName), IsUnique = true)]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        [Required]
        public required string UserName { get; set; }
        public string ?FirstName { get; set; }
        public string ?LastName { get; set; }
        public string ?Email { get; set; }
        public string ?Phone { get; set; }
        public string ?About { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        public string Role { get; set; } = "User";
        public List<CV> CV { get; set; }
        //public List<Education> Education { get; set; }
        public List<Request> Request { get; set; }
        //public List<PreviuosEmployment> PreviuosEmployment { get; set; }
        //public List<AreaCollection> AreaCollection { get; set; }
        // public int AreaCollectionId { get; set; }
        public List<Company> Company { get; set; }
        public List<Area> Area { get; set; }

        //public List<Branch> Branch { get; set; }
    }

    public class CV
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CVID { get; set; }
        public string ?Summary { get; set; }
        public User User { get; set; }
        public int UserID { get; set; }
        public Area Area { get; set; }
        public int ?AreaID { get; set; }
    }

    public class Area
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AreaID { get; set; }
        [Required]
        //public int HolderID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        //[Required]
        //public required string HolderType { get; set; }
        [Required]
        public required string Country { get; set; }
        [Required]
        public required string County { get; set; }
        [Required]
        public required string City { get; set; }
        [Required]
        public required string PostalCode { get; set; }
        [Required]
        public required string Address { get; set; }
    }

    /*
    public class Education
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EducationID {  get; set; }
        [Required]
        public string Institute { get; set; }
        [Required]
        public int Span { get; set; }
        [Required]
        public string Faculty { get; set; }
        [Required]
        public string Graduation { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public int AreaID { get; set; }
        public Area Area { get; set; }
    }
    */

    [Index(nameof(CompanyName), IsUnique = true)]
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyID { get; set; }
        [Required]
        public required string CompanyName { get; set; }
        public string ?CompanyEmail { get; set; }
        public string ?CompanyPhone { get; set; }
        public string ?Description { get; set; }
        [Required]
        public int OwnerID { get; set; }
        [ForeignKey(nameof(OwnerID))]
        public User User { get; set; }
        public int ?AreaID { get; set; }
        public Area Area { get; set; }
        //public List<Area> Areas { get; set; }
        //public List<Rating> Ratings { get; set; }
        //public List<Branch> Branch { get; set; }
        //public List<AreaCollection> AreaCollection { get; set; }
        // public int AreaCollectionId { get; set; }
    }

    /*
    public class Branch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BranchID { get; set; }
        [Required]
        public string BranchName { get; set; }
        public string? BranchEmail { get; set; }
        public string? BranchPhone { get; set; }
        public int ManagerID { get { return User.UserID; } set { value = User.UserID; } }
        public User User { get; set; }
        public int AreaID { get; set; }
        public Area Area { get; set; }
        public int CompanyID { get; set; }
        public Company Company { get; set; }
        public List<Job> Job { get; set; }
        public List<AreaCollection> AreaCollection { get; set; }
    }
    */

    public class Job
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobID { get; set; }
        public int Pay { get; set; }
        public string ?WorkTime { get; set; }
        public string ?Description { get; set; }
        public string ?Language { get; set; }
        public int CompanyID { get; set; }
        public Company Company { get; set; }
        public int ?AreaID { get; set; }
        public Area Area { get; set; }
        public List<Request> Request { get; set; }
    }

    /*
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RatingID { get; set; }
        public int FRating { get; set; }
        public string Feedback { get; set; }
        public bool Anonymous { get; set; }
        public int CompanyID { get; set; }
        public Company Company { get; set; }
        public int FeedbackUserID { get { return User.UserID; } set { value = User.UserID; } }
        public User User { get; set; }
    }
    */

    /*
    public class PreviuosEmployment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrevEmploymentID { get; set; }
        public string Provider { get; set; }
        public string Description { get; set; }
        public string Position { get; set; }
        public int Stay { get; set; }
        public DateTimeOffset Hired { get; set; }
        public DateTimeOffset Resigned { get; set; }
        public int ProviderID { get { return Company.CompanyID; } set { value = Company.CompanyID; } }
        public Company Company { set; get; }
        public int UserID { get; set; }
        public User User { get; set; }
    }
    */

    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestID { get; set; }
        [Required]
        public required string Status { get; set; }
        public string ?Comment { get; set; }
        public string Response { get; set; } = "No response yet! Check back later!";
        [Required]
        public required int JobID { get; set; }
        public Job Job { set; get; }
        [Required]
        public required int UserID { get; set; }
        public User User { get; set; }
        [Required]
        public  required int CVID { get; set; }
        public CV CV { get; set; }
        //public int CVIDID { get; set;
        //public int CVIDIDID { get;set; }
        //public int CVIDIDIDID { get; set; }
        /*  Fox This           
        *[~>         <~]
        *[  >-=====-<  ]
        *[ <         > ]
        *[<« O ___ O »>]
        *<«~~«< T >»~~»>
        * <« <     > »> 
        *  <« <   > »>> 
        */
    }

    /*
    public class AreaCollection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AreaCollectionId { get; set; }
        public int HolderID { get; set; }
        public string HolderType { get; set; } // = "User";
        public int AreaID { get; set; }
        public Area Area { get; set; }
    }
    */
}
