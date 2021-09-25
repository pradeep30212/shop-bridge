using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ShopBridge.Controllers;
using ShopBridge.Entities;
using ShopBridge.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopBridge.UnitTests
{
    public class InventoriesControllerTest
    {
        private Mock<IRepository<Inventory>> mockRepository;
        private InventoriesController _controller;
        
        [SetUp]
        public void Setup()
        {
            mockRepository = new Mock<IRepository<Inventory>>();
            _controller = new InventoriesController(mockRepository.Object);
        }

        [Test]
        public void ListInventries_WhenCallGetApi_ThenVerify()
        {
            // Arrange
            mockRepository.Setup(x => x.Get()).Returns(GetInventries());

            // Act
            var result = _controller.GetInventories();
            var actionResult = result.Result as OkObjectResult;
            // Assert
            Assert.AreEqual(actionResult.StatusCode, 200);
        }

        [Test]
        public void GetNoInventoryById_WhenCallGetApi_ThenVerify()
        {
            // Arrange
            mockRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(GetNullInventory);

            // Act
            var result = _controller.GetInventory(1);
            var actionResult = result.Result.Result as NotFoundResult;
            // Assert
            Assert.AreEqual(actionResult.StatusCode, 404);
        }

        [Test]
        public void GetInventoryById_WhenCallGetApi_ThenVerify()
        {
            // Arrange
            mockRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(GetCreatedInventory);

            // Act
            var result = _controller.GetInventory(1);
            var actionResult = result.Result.Value;
            
            // Assert
            Assert.AreEqual(actionResult.Name, "Inventory");
            Assert.AreEqual(actionResult.Description, "This is fake inventory");
            Assert.AreEqual(actionResult.Price, 400);
            Assert.AreEqual(actionResult.InventoryId, 1);
        }

        [Test]
        public void CreateInventory_WhenCallAddApi_ThenVerify()
        {
            // Arrange
            mockRepository.Setup(x=>x.Add(It.IsAny<Inventory>())).Returns(GetCreatedInventory());

            // Act
            var result = _controller.CreateInventory(new Inventory());

            var actionResult = result.Result.Result as CreatedAtActionResult;

            // Assert
            Assert.AreEqual(actionResult.StatusCode, 201);
        }

        [Test]
        public void CreateNullInventory_WhenCallAddApi_ThenVerify()
        {
            // Arrange
            
            // Act
            var result = _controller.CreateInventory(null);

            var actionResult = result.Result.Result as BadRequestResult;

            // Assert
            Assert.AreEqual(actionResult.StatusCode, 400);
        }

        [Test]
        public void UpdateInventory_WhenCallUpdateApi_ThenVerify()
        {
            // Arrange
            var inventory = GetCreatedInventory();
            mockRepository.Setup(x => x.Update(It.IsAny<Inventory>())).Returns(inventory);
            mockRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(inventory);

            // Act
            var result = _controller.UpdateInventory(inventory.Result.InventoryId, inventory.Result);

            var actionResult = result.Result.Value;

            // Assert
            Assert.AreEqual(actionResult.Name , "Inventory");
            Assert.AreEqual(actionResult.Description, "This is fake inventory");
            Assert.AreEqual(actionResult.Price, 400);
            Assert.AreEqual(actionResult.InventoryId, 1);
        }

        [Test]
        public void UpdateInventory_WhenCallUpdateApiWithDifferentInventoryId_ThenVerify()
        {
            // Arrange
            var inventory = GetCreatedInventory();
            mockRepository.Setup(x => x.Update(It.IsAny<Inventory>())).Returns(inventory);
            mockRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(inventory);

            // Act
            var result = _controller.UpdateInventory(2, inventory.Result);

            var actionResult = result.Result.Result as BadRequestObjectResult;

            // Assert
            Assert.AreEqual(actionResult.StatusCode, 400);
        }

        [Test]
        public void UpdateInventoryWhichDoesNotExist_WhenCallUpdateApi_ThenVerify()
        {
            // Arrange
            var inventory = GetCreatedInventory();
            mockRepository.Setup(x => x.Update(It.IsAny<Inventory>())).Returns(inventory);
            mockRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(GetNullInventory);

            // Act
            var result = _controller.UpdateInventory(inventory.Result.InventoryId, inventory.Result);

            var actionResult = result.Result.Result as NotFoundObjectResult;

            // Assert
            Assert.AreEqual(actionResult.StatusCode, 404);
        }

        [Test]
        public void DeleteInventory_WhenCallDeleteApi_ThenVerify()
        {
            // Arrange
            mockRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(GetCreatedInventory);
            mockRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(GetDeleteResult);

            // Act
            var result = _controller.DeleteInventory(1);

            var actionResult = result.Result as OkObjectResult;

            // Assert
            Assert.AreEqual(actionResult.StatusCode, 200);
        }

        [Test]
        public void DeleteSuchInventoryWhichDoesNotExist_WhenCallDeleteApi_ThenVerify()
        {
            // Arrange
            mockRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(GetNullInventory);
            mockRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(GetDeleteResult);
            
            // Act
            var result = _controller.DeleteInventory(1);

            var actionResult = result.Result as NotFoundObjectResult;

            // Assert
            Assert.AreEqual(actionResult.StatusCode, 404);
        }

        private async Task GetDeleteResult()
        {
           
        }

        private async Task<Inventory> GetCreatedInventory()
        {
            return await Task.Run(() => new Inventory
            {
                InventoryId = 1,
                Name = "Inventory",
                Price = 400,
                Description = "This is fake inventory"
            });
        }

        private async Task<Inventory> GetNullInventory()
        {
            Inventory inventory = null;
            return await Task.Run(() => inventory);
        }

        private async Task<IEnumerable<Inventory>> GetInventries()
        {
            return await Task.Run(() => new List<Inventory>
            {
             new Inventory{Name = "Pen drive", Price = 300},
             new Inventory{Name = "hard disck", Price = 11300},
             new Inventory{Name = "Ram", Price = 3000},
            });
        }
    }
}