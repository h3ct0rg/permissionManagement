using AutoMapper;
using Moq;
using PermissionManagement.Model;
using PermissionManagement.Repositories.UnitOfWork;
using PermissionManagement.Repository.PermissionRepository;
using PermissionManagement.Services;
using PermissionManagement.ViewModels;

namespace PermissionManagementTest.PermissionServiceTest
{
    public class PermissionServiceTest
    {

        private readonly Mock<IPermissionRepository> _permissionRepositoryMock = new Mock<IPermissionRepository>();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<Serilog.ILogger> _loggerMock = new Mock<Serilog.ILogger>();
        private PermissionService? permissionService;
        private Mock<IMapper> _mapperMock = new Mock<IMapper>();

        [Fact]
        public async Task AddPermissionAsync_WhenValidPermissionProvided_ShouldAddPermission()
        {
            // Arrange
            var permissionRepositoryMock = new Mock<IPermissionRepository>();
            permissionRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<permissionModel>())).Returns(Task.CompletedTask);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<permissionModel>(It.IsAny<PermissionViewModel>())).Returns((PermissionViewModel pvm) =>
            {
                return new permissionModel();
            });

            mapperMock.Setup(mapper => mapper.Map<PermissionViewModel>(It.IsAny<permissionModel>())).Returns((permissionModel pvm) =>
            {
                return new PermissionViewModel();
            });

            var loggerMock = new Mock<Serilog.ILogger>();


            permissionService = new PermissionService(permissionRepositoryMock.Object, unitOfWorkMock.Object, mapperMock.Object, loggerMock.Object);

            var permissionViewModel = new PermissionViewModel
            {
                Id = Guid.NewGuid(),
                IdEmployee = Guid.NewGuid(),
                IdPermissionType = Guid.NewGuid(),
                createdDate = DateTime.Now,
                updatedDate = DateTime.Now
            };

            // Act
            var result = await permissionService.addPermissionAsync(permissionViewModel);

            // Assert
            Assert.NotNull(result);
            // Add more assertions as needed
        }

        [Fact]
        public async Task DeletePermission_WhenPermissionExists_ShouldReturnTrue()
        {
            // Arrange
            var idPermission = Guid.NewGuid();
            permissionService = new PermissionService(_permissionRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);

            var permissionToDelete = new permissionModel { Id = idPermission };
            _permissionRepositoryMock.Setup(repo => repo.getByIDAsync(It.IsAny<Guid>())).ReturnsAsync(permissionToDelete);

            // Act
            var result = await permissionService.deletePermission(idPermission);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeletePermission_WhenPermissionDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var idPermission = Guid.NewGuid();
            permissionService = new PermissionService(_permissionRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
            _permissionRepositoryMock.Setup(repo => repo.getByIDAsync(idPermission)).ReturnsAsync((permissionModel)null);

            // Act
            var result = await permissionService.deletePermission(idPermission);

            // Assert
            Assert.False(result);
        }

        // Write similar test methods for other scenarios

        // Example test for GetPermissionById
        [Fact]
        public async Task GetPermissionById_ShouldReturnPermission()
        {
            // Arrange
            var idPermission = Guid.NewGuid();
            var permission = new permissionModel { Id = idPermission };
            permissionService = new PermissionService(_permissionRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
            _permissionRepositoryMock.Setup(repo => repo.getByIDAsync(idPermission)).ReturnsAsync(permission);

            // Act
            var result = await permissionService.getPermissionById(idPermission);

            // Assert
            Assert.Equal(permission, result);
        }

        [Fact]
        public async Task GetPermissionList_ShouldReturnListOfPermissions()
        {
            // Arrange
            var expectedPermissions = new List<permissionModel>(); // Set up expected permissions
            permissionService = new PermissionService(_permissionRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
            _permissionRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(expectedPermissions);

            // Act
            var result = await permissionService.getPermissionList();

            // Assert
            Assert.Equal(expectedPermissions, result);
        }

        [Fact]
        public async Task GetPermissionByEmployeeId_ShouldReturnListOfPermissionsForEmployee()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var expectedPermissions = new List<permissionModel>();
            permissionService = new PermissionService(_permissionRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
            _permissionRepositoryMock.Setup(repo => repo.GetAllPermissionByEmployeeId(employeeId)).ReturnsAsync(expectedPermissions);

            // Act
            var result = await permissionService.getPermissionByEmployeeId(employeeId);

            // Assert
            Assert.Equal(expectedPermissions, result);
        }

        [Fact]
        public async Task UpdatePermission_WhenPermissionExists_ShouldReturnTrue()
        {
            // Arrange
            var id = Guid.NewGuid();
            var permissionToUpdate = new permissionModel { Id = id };
            permissionService = new PermissionService(_permissionRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
            _permissionRepositoryMock.Setup(repo => repo.getByIDAsync(id)).ReturnsAsync(permissionToUpdate);

            // Act
            var result = await permissionService.updatePermission(id, permissionToUpdate);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdatePermission_WhenPermissionDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var id = Guid.NewGuid();
            permissionService = new PermissionService(_permissionRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
            _permissionRepositoryMock.Setup(repo => repo.getByIDAsync(id)).ReturnsAsync((permissionModel)null);

            // Act
            var result = await permissionService.updatePermission(id, new permissionModel());

            // Assert
            Assert.False(result);
        }

    }
}