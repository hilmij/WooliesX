namespace WooliesXFunctionTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WooliesXFunctionApp.Service;

    [TestClass]
    public class UserServiceTest
    {
        [TestMethod]
        public void TestGetUser()
        {
            var user = new UserService().GetUser();
            Assert.IsTrue(user.Name.Length > 0);
            Assert.AreSame(user.Name, "Hilmi Jauffer");
            Assert.IsTrue(user.Token.Length > 0);
        }
    }
}
