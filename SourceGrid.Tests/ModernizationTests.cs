using System;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;
using DevAge.Data.SqlClient;
using NUnit.Framework;

namespace SourceGrid.Tests
{
    [TestFixture]
    public class ModernizationTests
    {
        [Test]
        public void SqlCommandBuilder_ReturnsMicrosoftDataSqlClientCommands()
        {
            SqlCommandBuilder builder = CreateCommandBuilder();

            Assert.AreEqual(typeof(Microsoft.Data.SqlClient.SqlCommand), builder.GetInsertCommand().GetType());
            Assert.AreEqual(typeof(Microsoft.Data.SqlClient.SqlCommand), builder.GetUpdateCommand().GetType());
            Assert.AreEqual(typeof(Microsoft.Data.SqlClient.SqlCommand), builder.GetDeleteCommand().GetType());
        }

        [Test]
        public void SqlCommandBuilder_CreatesInsertCommandWithAllColumns()
        {
            var command = CreateCommandBuilder().GetInsertCommand();

            Assert.AreEqual("INSERT INTO [People] ([Id],[Name]) VALUES (@p0,@p1)", command.CommandText);
            Assert.AreEqual(2, command.Parameters.Count);
            Assert.AreEqual(SqlDbType.Int, command.Parameters[0].SqlDbType);
            Assert.AreEqual("Id", command.Parameters[0].SourceColumn);
            Assert.AreEqual(SqlDbType.VarChar, command.Parameters[1].SqlDbType);
            Assert.AreEqual("Name", command.Parameters[1].SourceColumn);
        }

        [Test]
        public void SqlCommandBuilder_CreatesUpdateCommandWithPrimaryKeyPredicate()
        {
            var command = CreateCommandBuilder().GetUpdateCommand();

            Assert.AreEqual("UPDATE [People] SET [Name] = @p1 WHERE [Id] = @p0", command.CommandText);
            Assert.AreEqual(2, command.Parameters.Count);
        }

        [Test]
        public void SqlCommandBuilder_CreatesDeleteCommandWithPrimaryKeyPredicate()
        {
            var command = CreateCommandBuilder().GetDeleteCommand();

            Assert.AreEqual("DELETE [People] WHERE [Id] = @p0", command.CommandText);
            Assert.AreEqual(2, command.Parameters.Count);
        }

        [Test]
        public void DesFactoryMigration_PreservesRoundTripBehavior()
        {
            const string clearText = "SourceGrid 5.0";
            string encrypted = global::DevAge.Security.Cryptography.Utilities.DES.EncryptString(clearText, "12345678");

            Assert.AreEqual(clearText, global::DevAge.Security.Cryptography.Utilities.DES.DecryptString(encrypted, "12345678"));
        }

        [Test]
        public void Sha1FactoryMigration_PreservesHashFormat()
        {
            Assert.AreEqual(
                "W6ph5Mm5Pz8GgiULbPgzG37mj9g=",
                global::DevAge.Security.Cryptography.Utilities.SHA1.HashPassword("password"));
        }

        [TestCase(typeof(global::DevAge.DevAgeApplicationException))]
        [TestCase(typeof(global::DevAge.TypeNotSupportedException))]
        [TestCase(typeof(global::DevAge.UnrecognizedCommandLineParametersException))]
        [TestCase(typeof(global::DevAge.ConversionErrorException))]
        [TestCase(typeof(global::DevAge.Configuration.ConfigurationException))]
        [TestCase(typeof(global::DevAge.Data.BinaryDataSetInvalidException))]
        [TestCase(typeof(global::DevAge.Data.BinaryDataSetVersionException))]
        [TestCase(typeof(global::DevAge.IO.InvalidDataException))]
        [TestCase(typeof(global::DevAge.IO.TypeNotSupportedException))]
        [TestCase(typeof(global::DevAge.Patterns.ActivityCanceledException))]
        [TestCase(typeof(global::DevAge.Patterns.ActivityStatusNotValidException))]
        [TestCase(typeof(global::DevAge.Patterns.TimeOutActivityException))]
        [TestCase(typeof(global::DevAge.Patterns.SubActivityException))]
        [TestCase(typeof(global::DevAge.Text.FixedLength.InvalidFieldLengthException))]
        [TestCase(typeof(global::DevAge.Text.FixedLength.ValueNotValidLengthException))]
        [TestCase(typeof(global::DevAge.Text.FixedLength.ValueNotSupportedException))]
        [TestCase(typeof(global::DevAge.Text.FixedLength.TypeNotSupportedException))]
        [TestCase(typeof(global::DevAge.Text.FixedLength.RegExException))]
        [TestCase(typeof(global::DevAge.Text.FixedLength.FieldParseException))]
        [TestCase(typeof(global::DevAge.Text.FixedLength.FieldStringConvertException))]
        [TestCase(typeof(global::DevAge.Text.FixedLength.FieldNotDefinedException))]
        [TestCase(typeof(global::DevAge.Text.FixedLength.FailedPropertySetFieldException))]
        [TestCase(typeof(global::DevAge.Text.FixedLength.FailedPropertyGetFieldException))]
        [TestCase(typeof(SourceGridException))]
        [TestCase(typeof(EditingCellException))]
        [TestCase(typeof(EndEditingException))]
        public void ModernExceptions_DoNotExposeFormatterSerialization(Type exceptionType)
        {
            ConstructorInfo constructor = exceptionType.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new[] { typeof(SerializationInfo), typeof(StreamingContext) },
                null);

            Assert.AreEqual(null, constructor);
            Assert.AreEqual(0, exceptionType.GetCustomAttributes(typeof(SerializableAttribute), false).Length);
        }

        private static SqlCommandBuilder CreateCommandBuilder()
        {
            DataTable table = new DataTable("People");
            DataColumn id = table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.PrimaryKey = new[] { id };
            return new SqlCommandBuilder(table);
        }
    }
}
