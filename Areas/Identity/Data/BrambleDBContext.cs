using Bramble.Models.DataModels.ApiModels.MusicBrainzModels;
using Bramble.Models.GenericModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations.Schema;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace BrambleWeb.Data;

public class BrambleDBContextSystemData : System.Data.Entity.DbContext
{
	public BrambleDBContextSystemData()
	{
	}
}

public class BrambleDBContext : DbContext
{
	private readonly string _connectionString;
    private readonly IConfiguration _configuration;

	public BrambleDBContext(DbContextOptions<BrambleDBContext> options, IConfiguration configuration)
	: base(options)
	{
		_configuration = configuration;
		_connectionString = _configuration.GetConnectionString("BrambleDB");
	}
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer(_connectionString);
	}
	protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("BrambleDB");
        builder.Entity<UserModel>().ToTable("BRAMBLE_USERS");
		builder.Entity<RoleModel>().ToTable("BRAMBLE_ROLES");
		builder.Entity<ClaimModel>().ToTable("BRAMBLE_IDENTITY_CLAIMS");
		builder.Entity<IdentityUserClaim<string>>().ToTable("BRAMBLE_IDENTITY_CLAIMS");
		builder.Entity<IdentityUserClaim<string>>().HasKey(p => new { p.Id });
		base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
    [Table("BRAMBLE_USERS", Schema = "BrambleDB")]
    public class BrambleUserStore : UserStore<UserModel>
    {
        public BrambleUserStore(BrambleDBContext dBContext) : base(dBContext)
        {

        }

    }

	[Table("BRAMBLE_ROLES", Schema = "BrambleDB")]
	public class BrambleRoleStore : RoleStore<RoleModel, BrambleDBContext>
    {
		public BrambleRoleStore(BrambleDBContext dBContext) : base(dBContext)
		{

		}
	}


	[Table("BRAMBLE_IDENTITY_CLAIMS", Schema = "BrambleDB")]
	public class BrambleClaimsFactory : UserClaimsPrincipalFactory<UserModel>
    {
		public BrambleClaimsFactory(UserManager<UserModel> userManager, IOptions<IdentityOptions> optionsAccessor) : 
			base(userManager, optionsAccessor)
		{

		}
	}
    public DbSet<UserModel> BRAMBLE_USERS { get; set; }
    public DbSet<ClaimModel> BRAMBLE_IDENTITY_CLAIMS { get; set; }
	public DbSet<RoleModel> BRAMBLE_ROLES { get; set; }
	public DbSet<GenreItem> META_BRAINZ_GENRES { get; set; }

}
