﻿using Game.Base;
using Game.Base.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Game.Data
{
    /// <summary>
    /// SQL Server data provider
    /// </summary>
    public class SqlServerDataProvider : IDataProvider
    {
        #region 字段

        private DataSettings _settings;

        #endregion

        #region 构造函数

        public SqlServerDataProvider(DataSettings settings)
        {
            this._settings = settings;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Parse commands
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="throwExceptionIfNonExists">Throw exception if the file doesn't exist</param>
        /// <returns></returns>
        protected virtual string[] ParseCommands(string filePath, bool throwExceptionIfNonExists)
        {
            if (!File.Exists(filePath))
            {
                if (throwExceptionIfNonExists)
                    throw new ArgumentException($"Specified file doesn't exist - {filePath}");
                
                return new string[0];
            }

            var statements = new List<string>();
            using (var stream = File.OpenRead(filePath))
            using (var reader = new StreamReader(stream))
            {
                string statement;
                while ((statement = ReadNextStatementFromStream(reader)) != null)
                {
                    if (!string.IsNullOrWhiteSpace(statement))
                        statements.Add(statement);
                }
            }

            return statements.ToArray();
        }

        /// <summary>
        /// Read the next statement from stream
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <returns>String</returns>
        protected virtual string ReadNextStatementFromStream(StreamReader reader)
        {
            var sb = new StringBuilder();

            while (true)
            {
                var lineOfText = reader.ReadLine();
                if (lineOfText == null)
                {
                    if (sb.Length > 0)
                        return sb.ToString();
                    
                    return null;
                }

                if (lineOfText.TrimEnd().ToUpper() == "GO")
                    break;

                sb.Append(lineOfText + Environment.NewLine);
            }

            return sb.ToString();
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Initialize database
        /// </summary>
        public virtual List<string> InitDatabase()
        {
            var customCommands = new List<string>();
            customCommands.AddRange(ParseCommands(CommonHelper.MapPath("~/App_Data/Install/SqlServer.Tables.sql"), false));
            customCommands.AddRange(ParseCommands(CommonHelper.MapPath("~/App_Data/Install/SqlServer.Indexes.sql"), false));
            customCommands.AddRange(ParseCommands(CommonHelper.MapPath("~/App_Data/Install/SqlServer.StoredProcedures.sql"), false));

            return customCommands;
        }

        /// <summary>
        /// A value indicating whether this data provider supports stored procedures
        /// </summary>
        public virtual bool StoredProceduredSupported
        {
            get { return true; }
        }

        /// <summary>
        /// A value indicating whether this data provider supports backup
        /// </summary>
        public virtual bool BackupSupported
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a support database parameter object (used by stored procedures)
        /// </summary>
        /// <returns>Parameter</returns>
        public virtual DbParameter GetParameter()
        {
            return new SqlParameter();
        }

        /// <summary>
        /// Maximum length of the data for HASHBYTES functions
        /// returns 0 if HASHBYTES function is not supported
        /// </summary>
        /// <returns>Length of the data for HASHBYTES functions</returns>
        public int SupportedLengthOfBinaryHash()
        {
            return 8000; //for SQL Server 2008 and above HASHBYTES function has a limit of 8000 characters.
        }
        
        public DbContextOptionsBuilder Build(DbContextOptionsBuilder optionBuilder)
        {
            return optionBuilder.UseSqlServer(_settings.DataConnectionString,options=> {
                options.EnableRetryOnFailure(3);
                options.CommandTimeout(10);
            });
        }

        #endregion
    }
}
