using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using SurveyPlatform.BLL.Exceptions;
using SurveyPlatform.BLL.Interfaces;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.BLL.Services;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DAL.Interfaces;
using SurveyPlatform.BLL.Helpers;

namespace SurveyPlatform.BLL.Tests
{
    public class PollServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPollRepository> _pollRepositoryMock;
        private readonly Mock<IOptionRepository> _optionRepositoryMock;
        private readonly Mock<JwtHelper> _jwtHelperMock;
        private readonly IMapper _mapper;
        private readonly IPollService _sut;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;

        public PollServiceTests()
        {
            var mapperConfig = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<PollModel, Poll>();
                    cfg.CreateMap<Poll, PollModel>();
                    cfg.CreateMap<UpdatePollModel, Poll>();
                    cfg.CreateMap<PollOption, PollOptionModel>();
                    cfg.CreateMap<PollResponse, PollResponseModel>();
                });
            _mapper = new Mapper(mapperConfig);
            _pollRepositoryMock = new Mock<IPollRepository>();
            _optionRepositoryMock = new Mock<IOptionRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _jwtHelperMock = new Mock<JwtHelper>();
            _sut = new PollService(
                _pollRepositoryMock.Object,
                _mapper,
                _httpContextAccessorMock.Object,
                _optionRepositoryMock.Object,
                _userRepositoryMock.Object,
                _jwtHelperMock.Object
            );
        }

        [Fact]
        public async Task GetPollByIdAsync_PollNotExistSend_ThrowEntityNotFoundException()
        {
            // Arrange
            var pollId = Guid.NewGuid();
            var message = $"Poll {pollId} not found";

            // Act
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.GetPollByIdAsync(pollId));
            // Assert
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public async Task GetPollByIdAsync_PollExistSend_ReturnsPollModel()
        {
            // Arrange
            var pollId = Guid.NewGuid();
            var poll = new Poll { Id = pollId };
            _pollRepositoryMock.Setup(t => t.GetPollByIdAsync(pollId)).ReturnsAsync(poll);
            var mappedPoll = _mapper.Map<PollModel>(poll);

            // Act
            var result = await _sut.GetPollByIdAsync(pollId);

            // Assert
            Assert.Equivalent(mappedPoll, result);
        }

        [Fact]
        public async Task GetAllPollsAsync_NoPollsExistSend_ReturnsEmptyListOfPollModels()
        {
            // Arrange
            var pollsList = new List<Poll>(); 
            var pollsListMapped = _mapper.Map<IEnumerable<PollModel>>(pollsList);
            _pollRepositoryMock.Setup(t => t.GetAllPollsAsync()).ReturnsAsync(pollsList);

            // Act
            var result = await _sut.GetAllPollsAsync();

            // Assert
            Assert.Empty(result);
            Assert.Empty(pollsListMapped);
        }

        [Fact]
        public async Task GetAllPollsAsync_PollsExistSend_ReturnsListOfPollModels()
        {
            // Arrange
            var pollsList = new List<Poll>()
            {
                new Poll { Id = Guid.NewGuid() },
                new Poll { Id = Guid.NewGuid() }
            };
            var pollsListMapped = _mapper.Map<IEnumerable<PollModel>>(pollsList);
            _pollRepositoryMock.Setup(t => t.GetAllPollsAsync()).ReturnsAsync(pollsList);

            // Act
            var result = await _sut.GetAllPollsAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.NotEmpty(pollsListMapped);
            Assert.Equal(result.First().Id, pollsListMapped.First().Id);
        }

        [Fact]
        public async Task CreatePollAsync_UserDoesNotExistSend_ReturnsCreatedPoll()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var message = $"User {userId} not found";
            var newPollModel = new PollModel
            {
                Title = "Test Poll",
                Description = "Test Description",
                AuthorID = userId
            };
            // Act
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.CreatePollAsync(newPollModel));
            // Assert
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public async Task CreatePollAsync_UserExistAndPollValidSend_ReturnsCreatedPoll()
        {
            var userId = Guid.NewGuid();
            // Arrange
            var pollModel = new PollModel
            {
                Title = "Test Poll",
                Description = "Test Description",
                AuthorID = userId
            };

            var pollEntity = new Poll
            {
                Id = Guid.NewGuid(),
                Title = "Test Poll",
                Description = "Test Description",
                AuthorID = userId,
                CreatedAt = DateTime.UtcNow
            };
            var user = new User()
            {
                Id = userId
            };
            _userRepositoryMock.Setup(t => t.GetUserByIdAsync(userId)).ReturnsAsync(user);
            _pollRepositoryMock.Setup(r => r.CreatePollAsync(It.IsAny<Poll>())).ReturnsAsync(pollEntity);

            // Act
            var result = await _sut.CreatePollAsync(pollModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pollModel.Title, result.Title);
            Assert.Equal(pollModel.Description, result.Description);
            _pollRepositoryMock.Verify(r => r.CreatePollAsync(It.IsAny<Poll>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePollAsync_PollNotExistSend_ThrowEntityNotFoundException()
        {
            // Arrange
            var pollId = Guid.NewGuid();
            var message = $"Poll {pollId} not found";
            var updatePollModel = new UpdatePollModel
            {
                Title = "Test Poll",
                Description = "Test Description"
            };
            // Act
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.UpdatePollAsync(pollId, updatePollModel));
            // Assert
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public async Task UpdatePollAsync_PollExistSend_ReturnsUpdatedPollModel()
        {
            // Arrange
            var pollId = Guid.NewGuid();
            var existPoll = new Poll
            {
                Id = pollId,
                Title = "Test Poll1",
                Description = "Test Description1"
            };
            var updatePollModel = new UpdatePollModel
            {
                Title = "Updated Test Poll",
                Description = "Updated Test Description"
            };
            var updatedPoll = new Poll
            {
                Id = pollId,
                Title = updatePollModel.Title,
                Description = updatePollModel.Description
            };
            var updatedPollModel = new PollModel
            {
                Id = pollId,
                Title = updatePollModel.Title,
                Description = updatePollModel.Description
            };

            _pollRepositoryMock.Setup(t => t.GetPollByIdAsync(pollId)).ReturnsAsync(existPoll);
            _pollRepositoryMock.Setup(t => t.UpdatePollAsync(It.IsAny<Poll>(), It.IsAny<Poll>())).ReturnsAsync(updatedPoll);

            // Act
            var result = await _sut.UpdatePollAsync(pollId, updatePollModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedPollModel.Title, result.Title);
            Assert.Equal(updatedPollModel.Description, result.Description);
            _pollRepositoryMock.Verify(t => t.UpdatePollAsync(It.IsAny<Poll>(), It.IsAny<Poll>()), Times.Once);
            _pollRepositoryMock.Verify(t => t.GetPollByIdAsync(pollId), Times.Once);
        }

        [Fact]
        public async Task DeletePollAsync_PollNotExistSend_ThrowEntityNotFoundException()
        {
            // Arrange
            var pollId = Guid.NewGuid();
            var message = $"Poll {pollId} not found";
            
            // Act
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.DeletePollAsync(pollId));
            
            // Assert
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public async Task DeletePollAsync_PollExistSend_DeletedPoll()
        {
            // Arrange
            var pollId = Guid.NewGuid();
            var existPoll = new Poll
            {
                Id = pollId
            };

            _pollRepositoryMock.Setup(t => t.GetPollByIdAsync(pollId)).ReturnsAsync(existPoll);
            _pollRepositoryMock.Setup(t => t.DeletePollAsync(pollId)).Returns(Task.CompletedTask);

            // Act
            await _sut.DeletePollAsync(pollId);

            // Assert
            _pollRepositoryMock.Verify(t => t.GetPollByIdAsync(pollId), Times.Once);
            _pollRepositoryMock.Verify(t => t.DeletePollAsync(pollId), Times.Once);
        }

        [Fact]
        public async Task AddPollResponseAsync_TokenNotFoundSend_ThrowUnauthorizedAccessException()
        {
            // Arrange
            var pollId = Guid.NewGuid();
            var message = $"UserID from token was not found";
            _httpContextAccessorMock.Setup(t => t.HttpContext).Returns(new DefaultHttpContext());
           // _jwtHelperMock.Setup(t => t.GetUserIdFromToken(It.IsAny<HttpContext>())).Returns(Guid.NewGuid());

            // Act
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _sut.AddPollResponseAsync(pollId, Guid.NewGuid()));

            // Assert
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public async Task AddPollResponseAsync_PollNotExistSend_ThrowEntityNotFoundException()
        {
            // Arrange
            var pollId = Guid.NewGuid();
            var message = $"Poll {pollId} not found";
            _httpContextAccessorMock.Setup(t => t.HttpContext).Returns(new DefaultHttpContext());
            _jwtHelperMock.Setup(t => t.GetUserIdFromToken(It.IsAny<HttpContext>())).Returns(Guid.NewGuid());

            // Act
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.AddPollResponseAsync(pollId, Guid.NewGuid()));

            // Assert
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public async Task AddPollResponseAsync_PollExistButOptionNotExistsSend_ThrowEntityNotFoundException()
        {
            // Arrange
            var existPoll = new Poll()
            {
                Id = Guid.NewGuid()
            };
            var optionId = Guid.NewGuid();
            var message = $"Option {optionId} not found";
            _httpContextAccessorMock.Setup(t => t.HttpContext).Returns(new DefaultHttpContext());
            _jwtHelperMock.Setup(t => t.GetUserIdFromToken(It.IsAny<HttpContext>())).Returns(Guid.NewGuid());
            _pollRepositoryMock.Setup(t => t.GetPollWithResponsesAsync(existPoll.Id)).ReturnsAsync(existPoll);
            // Act
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.AddPollResponseAsync(existPoll.Id, optionId));

            // Assert
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public async Task AddPollResponseAsync_PollExistAndOptionExistButUserNotFoundSend_ThrowEntityNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var existPoll = new Poll()
            {
                Id = Guid.NewGuid()
            };
            var optionId = Guid.NewGuid();
            var message = $"User {userId} not found";
            _httpContextAccessorMock.Setup(t => t.HttpContext).Returns(new DefaultHttpContext());
            _jwtHelperMock.Setup(t => t.GetUserIdFromToken(It.IsAny<HttpContext>())).Returns(userId);
            _pollRepositoryMock.Setup(t => t.GetPollWithResponsesAsync(existPoll.Id)).ReturnsAsync(existPoll);
            _optionRepositoryMock.Setup(t => t.GetOptionByIdAsync(optionId)).ReturnsAsync(new PollOption());
            _userRepositoryMock.Setup(t => t.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.AddPollResponseAsync(existPoll.Id, optionId));

            // Assert
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public async Task AddPollResponseAsync_PollExistsButOptionNotBelongToPollSend_ThrowEntityConflictException()
        {
            var userId = Guid.NewGuid();
            var pollId = Guid.NewGuid();
            var optionId = Guid.NewGuid();
            // Arrange
            var option = new PollOption()
            {
                Id = optionId,
                PollId = pollId
            };
            var existPoll = new Poll()
            {
                Id = pollId,
                Options = new List<PollOption> { new PollOption(), option }
            };
            var user = new User()
            {
                Id = userId
            };
            var message = $"Option ID[{optionId}] doesn't belong to Poll[{pollId}]";
            _httpContextAccessorMock.Setup(t => t.HttpContext).Returns(new DefaultHttpContext());
            _jwtHelperMock.Setup(t => t.GetUserIdFromToken(It.IsAny<HttpContext>())).Returns(userId);
            _pollRepositoryMock.Setup(t => t.GetPollWithResponsesAsync(existPoll.Id)).ReturnsAsync(existPoll);
            _optionRepositoryMock.Setup(t => t.GetOptionByIdAsync(optionId)).ReturnsAsync(new PollOption());
            _userRepositoryMock.Setup(t => t.GetUserByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var exception = await Assert.ThrowsAsync<EntityConflictException>(async () => await _sut.AddPollResponseAsync(existPoll.Id, optionId));

            // Assert
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public async Task AddPollResponseAsync_PollExistsAndOptionExistsButUserAlreadyRespondedSend_ThrowEntityConflictException()
        {
            var userId = Guid.NewGuid();
            var pollId = Guid.NewGuid();
            var optionId = Guid.NewGuid();
            // Arrange
            var option = new PollOption()
            {
                Id = optionId,
                PollId = pollId
            };
            var existPoll = new Poll()
            {
                Id = pollId,
                Options = new List<PollOption> { new PollOption(), option },
                Responses = new List<PollResponse> { new PollResponse { UserId = userId } }
            };
            var user = new User()
            {
                Id = userId
            };
            var message = $"User already responded to this poll";
            _httpContextAccessorMock.Setup(t => t.HttpContext).Returns(new DefaultHttpContext());
            _jwtHelperMock.Setup(t => t.GetUserIdFromToken(It.IsAny<HttpContext>())).Returns(userId);
            _pollRepositoryMock.Setup(t => t.GetPollWithResponsesAsync(existPoll.Id)).ReturnsAsync(existPoll);
            _optionRepositoryMock.Setup(t => t.GetOptionByIdAsync(optionId)).ReturnsAsync(option);
            _userRepositoryMock.Setup(t => t.GetUserByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var exception = await Assert.ThrowsAsync<EntityConflictException>(async () => await _sut.AddPollResponseAsync(existPoll.Id, optionId));

            // Assert
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public async Task AddPollResponseAsync_PollExistsAndOptionExistsSend_ReturnsPollModelWithResponses()
        {

            // Arrange
            var userId = Guid.NewGuid();
            var pollId = Guid.NewGuid();
            var optionId = Guid.NewGuid();
            var option = new PollOption()
            {
                Id = optionId,
                PollId = pollId
            };
            var existPoll = new Poll()
            {
                Id = pollId,
                Options = new List<PollOption> { new PollOption(), option },
                Responses = new List<PollResponse> { new PollResponse(), new PollResponse() }
            };
            var user = new User()
            {
                Id = userId
            };
            var pollResponse = new PollResponse
            {
                PollId = pollId,
                OptionId = optionId,
                UserId = userId
            };
            var respondedPoll = new Poll()
            {
                Id = pollId,
                Options = new List<PollOption> { new PollOption(), option },
                Responses = new List<PollResponse> { new PollResponse(), new PollResponse(), pollResponse }
            };
            _httpContextAccessorMock.Setup(t => t.HttpContext).Returns(new DefaultHttpContext());
            _jwtHelperMock.Setup(t => t.GetUserIdFromToken(It.IsAny<HttpContext>())).Returns(userId);
            _pollRepositoryMock.Setup(t => t.GetPollWithResponsesAsync(existPoll.Id)).ReturnsAsync(existPoll);
            _optionRepositoryMock.Setup(t => t.GetOptionByIdAsync(optionId)).ReturnsAsync(option);
            _userRepositoryMock.Setup(t => t.GetUserByIdAsync(userId)).ReturnsAsync(user);
            _pollRepositoryMock.Setup(t => t.AddPollResponseAsync(It.Is<PollResponse>(r =>
                r.PollId == pollResponse.PollId &&
                r.OptionId == pollResponse.OptionId &&
                r.UserId == pollResponse.UserId)))
                .ReturnsAsync(respondedPoll);
            var mappedPollModel = _mapper.Map<PollModel>(respondedPoll);

            // Act
            var result = await _sut.AddPollResponseAsync(pollId, optionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mappedPollModel.Title, result.Title);
            Assert.True(result.Responses.Count > existPoll.Responses.Count);
            _pollRepositoryMock.Verify(t => t.AddPollResponseAsync(It.Is<PollResponse>(r =>
                r.PollId == pollResponse.PollId &&
                r.OptionId == pollResponse.OptionId &&
                r.UserId == pollResponse.UserId)), Times.Once);
            _pollRepositoryMock.Verify(t => t.GetPollWithResponsesAsync(existPoll.Id), Times.Once);
            _optionRepositoryMock.Verify(t => t.GetOptionByIdAsync(optionId), Times.Once);
            _userRepositoryMock.Verify(t => t.GetUserByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetResponsesByPollIdAsync_PollNotExistSend_ThrowEntityNotFoundException()
        {
            // Arrange
            var pollId = Guid.NewGuid();
            var message = $"Poll {pollId} not found";

            // Act
            var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _sut.GetResponsesByPollIdAsync(pollId));
            // Assert
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public async Task GetResponsesByPollIdAsync_PollExistButResponsesIsEmptySend_ReturnsPollModel()
        {
            // Arrange
            var pollId = Guid.NewGuid();
            var poll = new Poll
            {
                Id = pollId,
                Responses = new List<PollResponse>()
            };
            _pollRepositoryMock.Setup(t => t.GetPollByIdAsync(pollId)).ReturnsAsync(poll);
            var mappedPoll = _mapper.Map<PollModel>(poll);

            // Act
            var result = await _sut.GetPollByIdAsync(pollId);

            // Assert
            Assert.Equivalent(mappedPoll, result);
        }

        [Fact]
        public async Task GetResponsesByPollIdAsync_PollExistAndHaveResponsesSend_ReturnsPollModel()
        {
            // Arrange
            var pollId = Guid.NewGuid();
            var poll = new Poll
            {
                Id = pollId,
                Responses = new List<PollResponse>() { new PollResponse(), new PollResponse() }
            };
            _pollRepositoryMock.Setup(t => t.GetPollByIdAsync(pollId)).ReturnsAsync(poll);
            var mappedPoll = _mapper.Map<PollModel>(poll);

            // Act
            var result = await _sut.GetPollByIdAsync(pollId);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Responses);
            Assert.Equivalent(mappedPoll, result);
            Assert.True(result.Responses.Count > 0);
        }

    }
}

