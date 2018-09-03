using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using StudentDataService.Model;
using Dapper;

namespace StudentDataService
{
    /// <summary>
    /// Summary description for StudentWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class StudentWebService : System.Web.Services.WebService
    {

        #region users

        private string Encrypt(string data)
        {
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(data);
            bytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(bytes);
            string hashed = System.Text.Encoding.ASCII.GetString(bytes);
            return hashed;
        }
        [WebMethod]
        public bool CreateUser(string user, string pass)
        {
            var query = "INSERT INTO users(userName, userPass) VALUES (@un, @up);";
            var param = new { un = user, up = Encrypt(pass) };

            using (var db = Model.Database.GetConnection().OpenAndReturn())
            using (var transaction = db.BeginTransaction())
            {
                try
                {
                    db.Execute(query, param, transaction);
                    transaction.Commit();
                    return true;
                }
                catch
                {

                    transaction.Rollback();
                    return false;
                }
            }
        }
        [WebMethod]
        public bool CreateDeveloperAccount()
        {
            if (CreateUser("dev", "dev"))
            {
                var userIdQuery = "(SELECT userId FROM users WHERE userName = 'dev')";
                var query = $"INSERT INTO user_roles (user, role) VALUES ({userIdQuery}, 1)";
                Model.Database.GetConnection().Execute(query);
                return true;
            }
            else
            {
                return false;
            }
        }
        [WebMethod]
        public bool ValidateUserCredentials(string user, string pass)
        {
            try
            {
                var query = "SELECT COUNT(*) FROM users WHERE userName = @un AND userPass = @up";
                var param = new { un = user, up = Encrypt(pass) };
                var results = (long)Model.Database.GetConnection().ExecuteScalar(query, param);
                return results == 1;
            }
            catch
            {
                return false;
            }
        }
        [WebMethod]
        public List<Model.User> GetAllUsers()
        {
            try
            {
                using (var db = Model.Database.GetConnection())
                {
                    var query = "SELECT * FROM users";
                    return db.Query<Model.User>(query).ToList();
                }
            }
            catch
            {
                return new List<Model.User>();
            }
        }
        #endregion users

        #region roles
        [WebMethod]
        public List<Model.role> GetAllRoles()
        {
            try
            {
                using (var db = Model.Database.GetConnection())
                {
                    var query = "SELECT * FROM roles";
                    return db.Query<Model.role>(query).ToList();
                }
            }
            catch
            {
                return new List<Model.role>();
            }
        }
        [WebMethod]
        public bool AddRoleToUser(String userId, String roleId)
        {
            using (var db = Model.Database.GetConnection().OpenAndReturn())
            using (var transaction = db.BeginTransaction())
            {
                try
                {
                    var query = "INSERT INTO user_roles (user, role) VALUES (@userId, @roleId)";
                    var param = new { userId, roleId };
                    db.Execute(query, param, transaction);
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        [WebMethod]
        public List<Model.UserRole> GetRolesForUser(string userId)
        {
            try
            {
                using (var db = Model.Database.GetConnection())
                {
                    var query = "SELECT * FROM UserRoleData WHERE user = @userId";
                    var param = new { userId };
                    return db.Query<Model.UserRole>(query, param).ToList();
                }
            }
            catch
            {
                return new List<Model.UserRole>();
            }
        }

        //     [WebMethod]
        //      public string HelloWorld()
        //      {
        //         return "Hello World";
        //     }
        #endregion roles
    }
}

