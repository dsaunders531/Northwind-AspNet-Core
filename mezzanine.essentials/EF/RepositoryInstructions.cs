//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;

//namespace mezzanine.EF
//{
//    #region "Step 1 - Create Models"
//    [Table("TestResults")]
//    public class TestResult
//    {
//        [Key]
//        [Required()]
//        [Editable(allowEdit: false)]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        [Display(Name = "Id")]
//        public long Id { get; set; }

//        [Required]
//        [Display(Name = "Test Result Name")]
//        [DataType(DataType.Text)]
//        [MinLength(1, ErrorMessage = "The name is too short!")]
//        [MaxLength(256, ErrorMessage = "The name is too long!")]
//        public string Name { get; set; }

//        [Required]
//        [Display(Name = "Test Date")]
//        [DataType(DataType.Date)]
//        public DateTime TestDate { get; set; }

//        [Required]
//        [Display(Name = "Tester Name")]
//        [DataType(DataType.Text)]
//        public string UserName { get; set; }

//        [Required]
//        [Display(Name = "Food parcel id")]
//        public long FoodParcelId { get; set; }

//        public FoodParcel FoodParcel { get; set; }

//        public List<TestResultItem> TestResultItems { get; set; }
//    }

//    [Table("TestResultItems")]
//    public class TestResultItem
//    {
//        [Key]
//        [Required()]
//        [Editable(allowEdit: false)]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        [Display(Name = "Id")]
//        public long Id { get; set; }

//        [Required]
//        [Editable(allowEdit: false)]
//        [Display(Name = "Test result id")]
//        public long TestResultId { get; set; }

//        public TestResult TestResult { get; set; }

//        [Required]
//        [Editable(allowEdit: false)]
//        [Display(Name = "Food item id")]
//        public long FoodItemId { get; set; }

//        public FoodItem FoodItem { get; set; }

//        [Required]
//        [Editable(allowEdit: false)]
//        [Display(Name = "Category id")]
//        public long CategoryId { get; set; }

//        public LOV Category { get; set; }

//        [Required]
//        [DataType(DataType.Text)]
//        [Display(Name = "Food Item Rating")]
//        [Range(minimum: 1, maximum: 5)]
//        public byte Result { get; set; }
//    }
//    #endregion

//    #region "Step 2 - Create application context"
//    /// <summary>
//    /// The database for the application.
//    /// </summary>
//    public class ApplicationDbContext : DbContext
//    {
//        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

//        public DbSet<TestResult> TestResults { get; set; }

//    }
//    #endregion

//    #region "Step 3 - Create repositories"
//    /// <summary>
//    /// The generic interface for EF repositories
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    public interface IRepository<T>
//        {
//            void Save();
//            void Create(T item);
//            void Update(T item);
//            void Delete(T item);
//            IQueryable<T> GetAll { get; }
//            T Get(long id);
//        }

//    /// <summary>
//    /// The base repository class. All repositories need  to inherit from this and implement IRepository T 
//    /// </summary>
//    public abstract class Repository
//    {
//        public ApplicationDbContext Context { get; set; } = null;

//        public Repository(ApplicationDbContext context)
//        {
//            this.Context = context;
//        }
//    }

//    public class TestResultRepository : Repository, IRepository<TestResult>
//    {
//        public TestResultRepository(ApplicationDbContext context) : base(context) { }

//        public IQueryable<TestResult> GetAll
//        {
//            get
//            {
//                // Note the inclues for related items .ThenInclude adds the subitems
//                // You must add the subitems to .Update as attachRange otherwise you 
//                // will get errors on save because EF will try to make new rows for the related items
//                return this.Context.TestResults.Include(t => t.TestResultItems).ThenInclude(c => c.Category)
//                                               .Include(t => t.TestResultItems).ThenInclude(f => f.FoodItem);
//            }
//        }

//        public TestResult Get(long id)
//        {
//            return (from TestResult t in this.GetAll where t.Id == id select t).FirstOrDefault();
//        }

//        public void Create(TestResult item)
//        {
//            this.Context.TestResults.Add(item);
//        }

//        public void Delete(TestResult item)
//        {
//            this.Context.TestResults.Remove(item);
//        }

//        public void Save()
//        {
//            this.Context.SaveChanges();
//        }

//        public void Update(TestResult item)
//        {
//            // Ignore the .ThenInclude() items
//            this.Context.AttachRange(item.TestResultItems.Select(i => i.FoodItem));
//            this.Context.AttachRange(item.TestResultItems.Select(c => c.Category));

//            this.Context.TestResults.Update(item);
//        }
//    }
//    #endregion

//    #region "Step 4 - Add to startup"
//    /* Add the context and each repository to Startup.ConfigureServices
//     *  services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(this.AppConfiguration.ConnectionStrings.Content)); // Application data
//     *  services.AddTransient<IRepository<FoodParcel>, FoodParcelRepository>();
//     *  
//     * 
//     */
//    #endregion

//    #region "Step 5 - Create and update database"
//    /*
//     * You should have configured a connection string and made it available in the application startup.
//     * 
//     * After making any model changes run on the package manager console:
//     * Add-Migration -Context myDbContect SomeName
//     * 
//     * You can roll back migrations using Remove-Migration -Context myDbContext
//     * Sometimes it is worth removing all your local migrations after doing development work so only 1 new one is commited.
//     * 
//     * Run Update-Database -Context myDbContext to build or update the database to the latest format.
//     * 
//     * To automatically create database and apply all the migrations: 
//     * in Startup.Configure
//     * 
//     *       // https://stackoverflow.com/questions/42355481/auto-create-database-in-entity-framework-core
//     *       using (IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
//     *       {
//     *           AuthenticationDbContext authenticationDbContext = serviceScope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();
//     *           authenticationDbContext.Database.Migrate();
//     *
//     *           // repeat for other DbContexts
//     *           
//     *       }
//     */
//    #endregion
        //#region "Step 6 - Extend the functionality of the repository using extension methods instead of adding to the repository
        //public static class TestResultRepositoryExtensions
        //{
        //    public static TestResult GetByUser(this IRepository<TestResult> me, long foodParcelId, string username)
        //    {
        //        return (from TestResult t in me.GetAll where t.FoodParcelId == foodParcelId && t.UserName == username select t).FirstOrDefault();
        //    }
        //}
        //#endregion
//}
