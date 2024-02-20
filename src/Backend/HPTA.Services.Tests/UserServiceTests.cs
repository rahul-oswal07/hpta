using AutoMapper;
using DevCentralClient.Contracts;
using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;
using Moq;

namespace HPTA.Services.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IDevCentralClientService> _mockDevCentralClientService = new();
        private readonly Mock<IIdentityService> _mockIdentityService = new();
        private readonly Mock<IUserRepository> _mockUserRepository = new();
        private readonly Mock<ITeamRepository> _mockTeamRepository = new();
        private readonly Mock<IUserTeamRepository> _mockUserTeamRepository = new();
        private readonly Mock<IMapper> _mockMapper = new();

        private UserService CreateUserService()
        {
            return new UserService(
                _mockDevCentralClientService.Object,
                _mockUserRepository.Object,
                _mockTeamRepository.Object,
                _mockUserTeamRepository.Object,
                _mockMapper.Object);
        }

        [Fact]
        public async Task ImportFromDevCentral_ExternalUser_CreatesUser()
        {
            // Arrange
            string email = "external@example.com";
            string azureAdUserId = "unique-azure-ad-user-id";
            _mockIdentityService.Setup(x => x.GetEmail()).Returns(email);
            _mockIdentityService.Setup(x => x.GetId()).Returns(azureAdUserId);
            _mockDevCentralClientService.Setup(x => x.GetTeamsInfo(email)).ReturnsAsync(new List<DevCentralTeamsResponse>());

            var userService = CreateUserService();

            // Act
            await userService.GetCustomClaims(email);

            // Assert
            //_mockUserRepository.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
            _mockUserRepository.Verify(x => x.SaveAsync(), Times.Once);
        }
    }
}