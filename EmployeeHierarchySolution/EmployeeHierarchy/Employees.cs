using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using System.IO;


namespace EmployeeHierarchy
{
    public class Employees
    {
        List<Staff> personnel = new List<Staff>();
        bool result = false;
        int total = 0;
        public Employees(string csvString)
        {
            try
            {
                TextFieldParser parser = new TextFieldParser(new StringReader(csvString.ToLower()));
                parser.HasFieldsEnclosedInQuotes = true;
                parser.SetDelimiters(",");

                while (!parser.EndOfData)
                {
                    string[] details = parser.ReadFields();

                    personnel.Add(new Staff
                    {
                        employeeID = details[0],
                        mangerID = details[1],
                        salary = details[2].Trim()
                    });
                }

                parser.Close();

                checkSalariesValidIntegers();
                checkIfEmployeeReportsToManyManagers();
                checkOnlyOneCEOExists();
                checkCircularReferenceExists();
                checkManagersAreEmployees();

            }
            catch (Exception ex)
            {}

        }
        //check salaries are valid integers
        public bool checkSalariesValidIntegers()
        {
            Int32 salary;
            try
            {
                return personnel.Where(i => !int.TryParse(i.salary, out salary)).Any();
            }
            catch(Exception ex)
            {
                return true;
            }
        }

        //check that managers are also employees
        public bool checkManagersAreEmployees()
        {
            try
            {
              return  personnel.Select(i => i.mangerID).ToList().Except(personnel.Select(x => x.employeeID).ToList()).Count() == 1;
              
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //check that one employees does not report to more than one manager
        public bool checkIfEmployeeReportsToManyManagers()
        {
            try
            {
                return personnel.GroupBy(x => x.employeeID).Where(g => g.Count() > 1).Any();
            }
            catch (Exception e)
            {
                return false;
            }
        }

        // Check if only one CEO exists
        public bool checkOnlyOneCEOExists()
        {
            try
            {
                return personnel.Where(x => x.mangerID == string.Empty || x.mangerID == null).Count() > 1;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        //check for circular reference
        public bool checkCircularReferenceExists()
        {
            result = false;
            try
            {
                foreach (var managerId in personnel.Select(i => i.mangerID).Distinct().ToList())
                {
                    foreach (var employeeId in personnel.Where(x => x.mangerID == managerId).Select(i => i.employeeID).Distinct().ToList())
                    {
                        if (personnel.Where(j => j.mangerID == employeeId).Select(j => j.employeeID).ToList().Contains(managerId)) return result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return result;
        }

        
        public long getBudgetSalary(string employeeId)
        {
            return recursiveFunction(employeeId.ToLower());
        }

        // recursive function to get subordinate employees
        private int recursiveFunction(string empId)
        {
            
            total += personnel.Where(i => i.mangerID == empId || i.employeeID == empId).Sum(i => int.Parse(i.salary));
            if (personnel.Where(i => i.mangerID == empId).Count() == 0) return total;
            foreach (var item1 in personnel.Where(i => i.mangerID == empId).ToList())
            {
                foreach (var item2 in personnel.Where(l => l.mangerID == item1.employeeID).ToList())
                {
                    recursiveFunction(item2.employeeID);
                }
            }
            return total;
        }

    }
}
