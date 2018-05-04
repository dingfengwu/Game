using Game.Base;
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
    /// SQL Serevr Compact provider
    /// </summary>
    public class SqliteDataProvider : IDataProvider
    {
        #region 字段

        private DataSettings _settings;

        #endregion

        #region 构造函数

        public SqliteDataProvider(DataSettings settings)
        {
            this._settings = settings;
        }

        #endregion

        /// <summary>
        /// Initialize database
        /// </summary>
        public virtual List<string> InitDatabase()
        {
            var customCommands = new List<string>();
            customCommands.AddRange(ParseCommands(CommonHelper.MapPath("~/App_Data/Install/SqlServer.tables.sql"), false));

            return customCommands;
        }

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

        /// <summary>
        /// A value indicating whether this data provider supports stored procedures
        /// </summary>
        public virtual bool StoredProceduredSupported
        {
            get { return false; }
        }

        /// <summary>
        /// A value indicating whether this data provider supports backup
        /// </summary>
        public bool BackupSupported
        {
            get { return false; }
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
            return 0; //HASHBYTES functions is missing in SQL CE
        }


        public DbContextOptionsBuilder Build(DbContextOptionsBuilder optionBuilder)
        {
            return optionBuilder.UseSqlite(_settings.DataConnectionString);
        }
    }
}
