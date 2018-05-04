using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Game.Base;
using Game.Base.Data;
using Game.Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Game.Data
{
    /// <summary>
    /// Object context
    /// </summary>
    public class GameObjectContext : DbContext, IDbContext
    {
        #region Ctor
        public GameObjectContext(IDataProvider provider): base()
        {
            DataProvider = provider;
        }

        #endregion

        #region Utilities

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //((IObjectContextAdapter) this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
            
            if (!optionsBuilder.IsConfigured)
                DataProvider?.Build(optionsBuilder).UseLazyLoadingProxies();

            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(GameEntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Attach an entity to the context or return an already attached entity (if it was already attached)
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Attached entity</returns>
        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
        {
            //little hack here until Entity Framework really supports stored procedures
            //otherwise, navigation properties of loaded entities are not loaded until an entity is attached to the context
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                //attach new entity
                Set<TEntity>().Attach(entity);
                return entity;
            }

            //entity is already loaded
            return alreadyAttached;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create database script
        /// </summary>
        /// <returns>SQL to generate database</returns>
        public string CreateDatabaseScript()
        {
            return this.CreateDatabaseScript();
        }

        /// <summary>
        /// Get DbSet
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>DbSet</returns>
        public new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }
        
        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Entities</returns>
        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            //add parameters to command
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        //output parameter
                        commandText += " output";
                    }
                }
            }

            var result = this.SqlQuery<TEntity>(commandText, parameters).ToList();

            //performance hack applied as described here - http://www.gamecommerce.com/boards/t/25483/fix-very-important-speed-improvement.aspx
            bool acd = this.AutoDetectChangesEnabled;
            try
            {
                this.AutoDetectChangesEnabled = false;

                for (int i = 0; i < result.Count; i++)
                    result[i] = AttachEntityToContext(result[i]);
            }
            finally
            {
                this.AutoDetectChangesEnabled = acd;
            }

            return result;
        }

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.SqlQuery<TElement>(sql, parameters);
        }
    
        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="doNotEnsureTransaction">false - the transaction creation is not ensured; true - the transaction creation is ensured.</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                //store previous timeout
                previousTimeout = base.Database.GetCommandTimeout();
                base.Database.SetCommandTimeout(timeout);
            }

            sql = sql.Replace("{", "{{").Replace("}", "}}");

            var result = 0;
            if (doNotEnsureTransaction)
            {
                result = base.Database.ExecuteSqlCommand(new RawSqlString(sql), parameters);
            }
            else
            {
                try
                {
                    base.Database.BeginTransaction();
                    result = base.Database.ExecuteSqlCommand(new RawSqlString(sql), parameters);
                    base.Database.CommitTransaction();
                }
                catch(DbException ex)
                {
                    base.Database.RollbackTransaction();

                    throw ex;
                }
            }
            
            return result;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether proxy creation setting is enabled (used in EF)
        /// </summary>
        public virtual bool ProxyCreationEnabled
        {
            get
            {
                return this.ProxyCreationEnabled;
            }
            set
            {
                this.ProxyCreationEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether auto detect changes setting is enabled (used in EF)
        /// </summary>
        public virtual bool AutoDetectChangesEnabled
        {
            get
            {
                return this.AutoDetectChangesEnabled;
            }
            set
            {
                this.AutoDetectChangesEnabled = value;
            }
        }

        /// <summary>
        /// 返回正在使用的DataProvider
        /// </summary>
        public IDataProvider DataProvider { get; }

        #endregion
    }
}