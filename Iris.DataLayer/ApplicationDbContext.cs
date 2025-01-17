﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using EFSecondLevelCache;
using Iris.DomainClasses;
using Iris.DomainClasses.Configurations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Iris.DataLayer
{
    public class ApplicationDbContext :
        IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>,
        IUnitOfWork
    {

        public DbSet<Post> Posts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }
        public DbSet<ProductDiscount> ProductDiscounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<SlideShowImage> SlideShowImages { get; set; }
        public DbSet<SiteOption> SiteOptions { get; set; }
        public DbSet<Factor> Factors { get; set; }
        public DbSet<FactorProduct> FactorProducts { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemType> ItemTypes { get; set; }
        //--
        public virtual DbSet<UserFavoritePost> UserFavoritePosts { get; set; }
        public virtual DbSet<UserFavoriteProduct> UserFavoriteProducts { get; set; }
        //public virtual DbSet<ProductQuestionAnswer> ProductQuestionAnswers { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<PropertyType> PropertyTypes { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        public virtual DbSet<ProductProperty> ProductProperty { get; set; }

        public virtual DbSet<BidHistory> BidHistory { get; set; }

        public virtual DbSet<AuctionItem> AuctionItem { get; set; }

        public virtual DbSet<CoinFactor> CoinFactor { get; set; }

        /// <summary>
        /// It looks for a connection string named connectionString1 in the web.config file.
        /// </summary>
        public ApplicationDbContext()
            : base("ApplicationDbContext")
        {
            //this.Database.Log = data => System.Diagnostics.Debug.WriteLine(data);
        }

        /// <summary>
        /// To change the connection string at runtime. See the SmObjectFactory class for more info.
        /// </summary>
        //public ApplicationDbContext(string connectionString)
        //    : base(connectionString)
        //{
        //    //Note: defaultConnectionFactory in the web.config file should be set.
        //}

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Configurations.Add(new ProductConfig());
            builder.Configurations.Add(new ProductPriceConfig());
            builder.Configurations.Add(new ProductDiscountConfig());
            builder.Configurations.Add(new PostConfig());
            builder.Configurations.Add(new CategoryConfig());
            builder.Configurations.Add(new PostCategoryConfig());
            builder.Configurations.Add(new PoductImageConfig());
            builder.Configurations.Add(new SiteOptionConfig());
            builder.Configurations.Add(new SlideShowConfig());
            builder.Configurations.Add(new FactorConfig());
            builder.Configurations.Add(new ItemTypeConfig());
            builder.Configurations.Add(new ItemConfig());
            //builder.Configurations.Add(new ProductQuestionAnswerConfig());
            builder.Configurations.Add(new PropertyTypeConfig());
            builder.Configurations.Add(new ColorConfig());
            builder.Configurations.Add(new CommentConfig());
            builder.Configurations.Add(new PropertyConfig());
            builder.Configurations.Add(new ProductPropertyConfig());
            builder.Configurations.Add(new BidHistoryConfig());
            builder.Configurations.Add(new CoinFactorConfig());


            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<CustomRole>().ToTable("Roles");
            builder.Entity<CustomUserClaim>().ToTable("UserClaims");
            builder.Entity<CustomUserRole>().ToTable("UserRoles");
            builder.Entity<CustomUserLogin>().ToTable("UserLogins");
            builder.Entity<ItemType>().ToTable("ItemType");
            //builder.Entity<Comment>().Property(q=> q.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);


        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public int SaveAllChanges(bool invalidateCacheDependencies)
        {
            return SaveChanges(invalidateCacheDependencies);
        }

        public async Task<int> SaveAllChangesAsync(bool invalidateCacheDependencies)
        {
            return await SaveChangesAsync(invalidateCacheDependencies);
        }

        public int SaveChanges(bool invalidateCacheDependencies)
        {
            var changedEntityNames = this.GetChangedEntityNames();
            var result = base.SaveChanges();
            if (invalidateCacheDependencies)
            {
                new EFCacheServiceProvider().InvalidateCacheDependencies(changedEntityNames);
            }
            return result;
        }

        public async Task<int> SaveChangesAsync(bool invalidateCacheDependencies)
        {
            var changedEntityNames = this.GetChangedEntityNames();
            var result = await base.SaveChangesAsync();
            if (invalidateCacheDependencies)
            {
                new EFCacheServiceProvider().InvalidateCacheDependencies(changedEntityNames);
            }
            return result;
        }


        public void MarkAsAdded<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Added;
        }
        public void MarkAsDeleted<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Deleted;
        }

        public IEnumerable<TEntity> AddThisRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            return ((DbSet<TEntity>)this.Set<TEntity>()).AddRange(entities);
        }

        public void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
        }

        public IList<T> GetRows<T>(string sql, params object[] parameters) where T : class
        {
            return Database.SqlQuery<T>(sql, parameters).ToList();
        }

        public void ForceDatabaseInitialize()
        {
            this.Database.Initialize(force: true);
        }

    }
}