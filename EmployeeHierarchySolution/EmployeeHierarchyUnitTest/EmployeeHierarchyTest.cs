using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmployeeHierarchy;

namespace EmployeeHierarchyUnitTest
{
    [TestClass]
    public class EmployeeHierarchyTest
    {
        bool expectedResult;
        bool actualResult;
        static string csvFile = "Employee1,,\"1000\"" + Environment.NewLine +
                                "Employee2,Employee1,\"800\"" + Environment.NewLine +
                                "Employee3,Employee1,\"500\"" + Environment.NewLine +
                                "Employee5,Employee1,\"500\"" + Environment.NewLine +
                                "Employee4,Employee2,\"500\"" + Environment.NewLine +

                                //Uncomment below line to test invalid integer
                                //"Employee7,Employee3,\"500abc\"" + Environment.NewLine +

                                //Uncomment below 2 lines to test employee does not report to more than one manager
                                //"Employee7,Employee3,\"500\"" + Environment.NewLine +
                                //"Employee7,Employee2,\"500\"" + Environment.NewLine +

                                //Uncomment below line to test more than one CEO
                                //"Employee7,,\"1000\"" + Environment.NewLine +

                                //Uncomment below two line to test circular reference
                                //"Employee7,Employee8,\"1000\"" + Environment.NewLine +
                                //"Employee8,Employee7,\"1000\"" + Environment.NewLine +

                                //Uncomment below line to test if managers are also employees
                                //"Employee9,Employee8,\"1000\"" + Environment.NewLine +

                                "Employee6,Employee2,\"500\"" + Environment.NewLine;

        Employees emp = new Employees(csvFile);

        [TestMethod]
        public void Test_checkSalariesValidIntegers()
        {
            expectedResult = false;
            actualResult = emp.checkSalariesValidIntegers();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void Test_checkIfEmployeeReportsToManyManagers()
        {
            expectedResult = false;
            actualResult = emp.checkIfEmployeeReportsToManyManagers();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void Test_OnlyOneCEOExists()
        {
            expectedResult = false;
            actualResult = emp.checkOnlyOneCEOExists();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void Test_checkCircularReferenceExists()
        {
            expectedResult = false;
            actualResult = emp.checkCircularReferenceExists();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void Test_checkManagersAreEmployees()
        {
            expectedResult = true;
            actualResult = emp.checkManagersAreEmployees();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void Test_getBudgetSalary()
        {
            long expectedValue = 3800;
            long actualValue = emp.getBudgetSalary("employee1");
            Assert.AreEqual(expectedValue, actualValue);
        }

    }
}
