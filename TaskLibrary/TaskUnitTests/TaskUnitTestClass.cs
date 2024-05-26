using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TaskLibrary;

namespace TaskUnitTests
{
    [TestClass]
    public class TaskUnitTestClass
    {
        [TestMethod]
        public void SetValidTaskName()
        {
            // Arrange
            string name = "Finish report";
            string assigner = "John Doe";
            string expectedName = name;
            string expectedAssigner = assigner;

            // Act
            Task task = new Task(name, assigner);

            // Assert
            Assert.AreEqual(expectedName, task.NumeTask);
            Assert.AreEqual(expectedAssigner, task.NumeAssigner);
            Assert.AreEqual(DateTime.Now.ToString("dd-MM-yyyy"), task.GetDataAsignarii); // Formatted date
            Assert.AreEqual(0.0, task.OreLogate);
            Assert.AreEqual("", task.DescriereTask);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SetInvalidTaskName()
        {
            // Arrange
            string name = "Name"; // Less than 5 characters
            string assigner = "John Doe";

            // Act
            new Task(name, assigner); // Expect exception
        }

        [TestMethod]
        public void SetValidTaskNewName()
        {
            // Arrange
            Task task = new Task("Old name", "John Doe");
            string newName = "New report";

            // Act
            task.NumeTask = newName;

            // Assert
            Assert.AreEqual(newName, task.NumeTask);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SetInvalidTaskNewName()
        {
            // Arrange
            Task task = new Task("Valid name", "John Doe");
            string shortName = "Name"; // Less than 5 characters

            // Act
            task.NumeTask = shortName; // Expect exception
        }

        [TestMethod]
        public void SetValidLoggedHours()
        {
            // Arrange
            Task task = new Task("Finish report", "John Doe");
            double validHours = 2.5;

            // Act
            task.OreLogate = validHours;

            // Assert
            Assert.AreEqual(validHours, task.OreLogate);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SetNegativeLoggedHours()
        {
            // Arrange
            Task task = new Task("Finish report", "John Doe");
            double negativeHours = -1.0;

            // Act
            task.OreLogate = negativeHours; // Expect exception
        }

        [TestMethod]
        public void SetNewAsigneerValidName()
        {
            // Arrange
            Task task = new Task("Finish report", "Old assigner");
            string newAssigner = "Jane Doe";

            // Act
            task.NumeAssigner = newAssigner;

            // Assert
            Assert.AreEqual(newAssigner, task.NumeAssigner);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SetNewAsigneerInvalidName()
        {
            // Arrange
            Task task = new Task("Finish report", "Old assigner");
            string shortName = "Name"; // Less than 5 characters

            // Act
            task.NumeAssigner = shortName; // Expect exception
        }

        [TestMethod]
        public void SetValidTaskDescription()
        {
            // Arrange
            Task task = new Task("Finish report", "John Doe");
            string description = "Complete the weekly sales analysis.";

            // Act
            task.DescriereTask = description;

            // Assert
            Assert.AreEqual(description, task.DescriereTask);
        }

        [TestMethod]
        public void GetTaskDate()
        {
            // Arrange
            Task task = new Task("Finish report", "John Doe");

            // Act
            string formattedDate = task.GetDataAsignarii;

            Assert.AreEqual(DateTime.Now.ToString("dd-MM-yyyy"), formattedDate);
        }

        [TestMethod]
        public void SetTaskDate()
        {
            // Arrange
            Task task = new Task("Valid name", "Valid Assigner");

            task.DataAsignariiAsDateTime = DateTime.Today;

            Assert.AreEqual(DateTime.Now.ToString("dd-MM-yyyy"), task.GetDataAsignarii);
        }
    }
}
